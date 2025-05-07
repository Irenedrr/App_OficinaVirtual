using System.Collections.ObjectModel;
using System.Text.Json;
using System.Threading.Tasks;
using App_OficinaVirtual.DTO;
using App_OficinaVirtual.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;

namespace App_OficinaVirtual.ViewModels;

public partial class RegisterViewModel : ObservableObject
{
    private readonly UsuarioService _usuarioService;
    private readonly RolService _rolService;

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
            var usuarioDto = new UsuarioCreateDto
            {
                Nombre = Nombre,
                Email = Email,
                Contrasena = Contrasena,
                RolId = RolSeleccionado.Id,
                ImagenUrl = string.IsNullOrWhiteSpace(ImagenUrl) ? "default.png" : ImagenUrl,
                Estado = "conectado"
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
}
