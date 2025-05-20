using System.Collections.ObjectModel;
using System.Text.Json;
using System.Threading.Tasks;
using App_OficinaVirtual.DTO;
using App_OficinaVirtual.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;
using App_OficinaVirtual.Models;
using Microsoft.Maui.Storage;

namespace App_OficinaVirtual.ViewModels;

public partial class RegisterViewModel : ObservableObject
{
    private readonly UsuarioService _usuarioService;
    private readonly RolService _rolService;

    [ObservableProperty]
    private OficinaVisual oficinaSeleccionada;

    public ObservableCollection<OficinaVisual> Oficinas { get; } = new()
    {
        new OficinaVisual { Nombre = "Comun", Imagen = "mapa_comun.png" },
        new OficinaVisual { Nombre = "Gaming", Imagen = "mapa_gaming.png" }

    };

    [ObservableProperty]
    private PersonajeVisual personajeSeleccionado;

    public ObservableCollection<PersonajeVisual> Personajes { get; } = new()
{
    new PersonajeVisual { Nombre = "Chica", Imagen = "chica.png" },
    new PersonajeVisual { Nombre = "Chico", Imagen = "chico.png" }
};




    public ObservableCollection<RolResponseDto> Roles { get; } = new();

    [ObservableProperty]
    private string nombre;

    [ObservableProperty]
    private string email;

    [ObservableProperty]
    private string contrasena;

    [ObservableProperty]
    private RolResponseDto rolSeleccionado;

    [ObservableProperty]
    private string imagenUrl;

    [ObservableProperty]
    private string estado = "conectado";

    [ObservableProperty]
    private string mensajeError;

    [ObservableProperty]
    private bool hayError;

    public RegisterViewModel(UsuarioService usuarioService, RolService rolService)
    {
        _usuarioService = usuarioService;
        _rolService = rolService;
        CargarRolesAsync();
    }

    private async void CargarRolesAsync()
    {
        try
        {
            var roles = await _rolService.LeerTodosAsync();
            Roles.Clear();
            foreach (var rol in roles)
            {
                Roles.Add(rol);
            }
        }
        catch
        {
            MensajeError = "Error al cargar los roles.";
            HayError = true;
        }
    }

    [RelayCommand]
    public async Task RegistrarUsuarioAsync()
    {
        if (string.IsNullOrWhiteSpace(Nombre) || string.IsNullOrWhiteSpace(Email) ||
            string.IsNullOrWhiteSpace(Contrasena) || RolSeleccionado == null)
        {
            MensajeError = "Por favor, completa todos los campos obligatorios.";
            HayError = true;
            return;
        }

        try
        {
            if (OficinaSeleccionada == null)
            {
                MensajeError = "Selecciona una oficina.";
                HayError = true;
                return;
            }

            var usuarioDto = new UsuarioCreateDto
            {
                Nombre = Nombre,
                Email = Email,
                Contrasena = Contrasena,
                RolId = RolSeleccionado.Id,
                ImagenUrl = string.IsNullOrWhiteSpace(ImagenUrl) ? "default.png" : ImagenUrl,
                Estado = "conectado",
                Oficina = OficinaSeleccionada.Nombre,
                Personaje = PersonajeSeleccionado.Nombre
            };


            var usuarioCreado = await _usuarioService.CrearAsync(usuarioDto);
            if (usuarioCreado != null)
            {
                await Shell.Current.GoToAsync("//login", true);
            }
            else
            {
                MensajeError = "Error al registrar el usuario. Por favor, inténtalo de nuevo.";
                HayError = true;
            }
        }
        catch (Exception ex)
        {
            
            MensajeError = "Error al conectar con el servidor. Por favor, inténtalo de nuevo.";
            HayError = true;
        }
    }

    [RelayCommand]
    public void IrALogin()
    {
        Shell.Current.GoToAsync("//login");
    }

    [RelayCommand]
    private async Task ElegirImagenAsync()
    {
        try
        {
            var resultado = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Selecciona una imagen",
                FileTypes = FilePickerFileType.Images
            });

            if (resultado != null)
            {
                // Almacena la ruta del archivo como URI para MAUI
                ImagenUrl = resultado.FullPath;
            }
        }
        catch (Exception)
        {
            MensajeError = "No se pudo seleccionar una imagen.";
            HayError = true;
        }
    }
}
