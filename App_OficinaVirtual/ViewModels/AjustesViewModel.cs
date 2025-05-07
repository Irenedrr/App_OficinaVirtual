using App_OficinaVirtual.DTO;
using App_OficinaVirtual.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace App_OficinaVirtual.ViewModels;

public partial class AjustesViewModel : ObservableObject
{
    private readonly UsuarioService _usuarioService;
    private readonly RolService _rolService;

    [ObservableProperty] private string nombre;
    [ObservableProperty] private string email;
    [ObservableProperty] private string password;
    [ObservableProperty] private string imagenUrl;
    [ObservableProperty] private int usuarioId;

    [ObservableProperty] private string mensajeError;
    [ObservableProperty] private bool hayError;

    [ObservableProperty]
    private ObservableCollection<RolResponseDto> roles = new();

    [ObservableProperty] 
    private RolResponseDto rolSeleccionado;

    public AjustesViewModel(UsuarioService usuarioService, RolService rolService)
    {
        _usuarioService = usuarioService;
        _rolService = rolService;
        CargarUsuarioAsync();
    }

    private async void CargarUsuarioAsync()
    {
        try
        {
            UsuarioId = Preferences.Get("usuario_id", -1);
            if (UsuarioId == -1)
            {
                MensajeError = "Usuario no autenticado.";
                HayError = true;
                return;
            }

            var usuario = await _usuarioService.LeerPorIdAsync(UsuarioId);
            if (usuario == null)
            {
                MensajeError = "No se pudo obtener la información del usuario.";
                HayError = true;
                return;
            }

            var roles = await _rolService.LeerTodosAsync();

            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                Roles.Clear();
                foreach (var rol in roles)
                    Roles.Add(rol);

                
                if (string.IsNullOrWhiteSpace(Nombre))
                    Nombre = usuario.Nombre;

                if (string.IsNullOrWhiteSpace(Email))
                    Email = usuario.Email;

                if (string.IsNullOrWhiteSpace(ImagenUrl))
                    ImagenUrl = usuario.ImagenUrl;

                if (RolSeleccionado == null)
                    RolSeleccionado = Roles.FirstOrDefault(r => r.Id == usuario.RolId);

                
                if (string.IsNullOrWhiteSpace(Password))
                    Password = string.Empty;
            });
        }
        catch (Exception ex)
        {
            
            MensajeError = "Error al cargar los datos.";
            HayError = true;
        }
    }


    [RelayCommand]
    private async Task CambiarFotoAsync()
    {
        try
        {
            var file = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Selecciona una imagen",
                FileTypes = FilePickerFileType.Images
            });

            if (file != null)
                ImagenUrl = file.FullPath;
        }
        catch (Exception ex)
        {
            
            MensajeError = "Error al seleccionar imagen.";
            HayError = true;
        }
    }

    [RelayCommand]
    private async Task GuardarAsync()
    {
        if (string.IsNullOrWhiteSpace(Nombre) ||
            string.IsNullOrWhiteSpace(Email) ||
            string.IsNullOrWhiteSpace(Password) ||
            RolSeleccionado == null)
        {
            MensajeError = "Todos los campos son obligatorios.";
            HayError = true;
            return;
        }

        var dto = new UsuarioUpdateDto
        {
            Nombre = Nombre,
            Email = Email,
            Contrasena = Password,
            RolId = RolSeleccionado.Id,
            ImagenUrl = ImagenUrl,
            Estado = "conectado"
        };

        var actualizado = await _usuarioService.ActualizarAsync(UsuarioId, dto);

        if (actualizado != null)
        {
            
            HayError = false;
            MensajeError = string.Empty;
        }
        else
        {
            MensajeError = "No se pudieron guardar los cambios.";
            HayError = true;
        }
    }
}

