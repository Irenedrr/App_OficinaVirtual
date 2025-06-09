using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using App_OficinaVirtual.Services;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using System.Diagnostics;
using System.Text.Json;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace App_OficinaVirtual.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly AuthService _servicioAutenticacion;
    private readonly UsuarioService _usuarioService;


    [ObservableProperty]
    private string correo = string.Empty;

    [ObservableProperty]
    private string contrasena = string.Empty;

    [ObservableProperty]
    private string mensajeError = string.Empty;

    [ObservableProperty]
    private bool hayError;

    public LoginViewModel(AuthService servicioAutenticacion, UsuarioService usuarioService)
    {
        _servicioAutenticacion = servicioAutenticacion;
        _usuarioService = usuarioService;

    }
    [RelayCommand]
    private async Task IniciarSesionAsync()
    {
        MensajeError = "";
        HayError = false;

        if (string.IsNullOrWhiteSpace(Correo) || string.IsNullOrWhiteSpace(Contrasena))
        {
            MensajeError = "Por favor, completa ambos campos.";
            HayError = true;
            return;
        }

        try
        {
            var exito = await _servicioAutenticacion.LoginAsync(Correo, Contrasena);
            if (exito)
            {
                Preferences.Default.Set("access_token", _servicioAutenticacion.AccessToken);

                int usuarioId = await _usuarioService.ObtenerIdPorCorreoAsync(Correo);

                if (usuarioId == -1)
                {
                    MensajeError = "No se pudo obtener el ID del usuario.";
                    HayError = true;
                    return;
                }

                Preferences.Default.Set("usuario_id", usuarioId);

                // 🔁 Configura datos del usuario activo en la API
                using var httpClient = new HttpClient();
                var backendUrl = $"http://{ObtenerIPLocal()}:8000";
                var endpoint = $"{backendUrl}/sessiones/login"; // <--- ¡importante! este endpoint es el correcto

                var usuarioActivo = new
                {
                    usuario_id = usuarioId,
                    nombre = await _usuarioService.ObtenerNombrePorId(usuarioId),
                    personaje = await _usuarioService.ObtenerPersonajePorId(usuarioId),
                    oficina = await _usuarioService.ObtenerOficinaPorId(usuarioId)
                };

                var json = JsonSerializer.Serialize(usuarioActivo);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(endpoint, content);
                if (!response.IsSuccessStatusCode)
                {
                    MensajeError = "No se pudo comunicar con la API.";
                    HayError = true;
                    return;
                }

                // ✅ Ir a la página con WebView (donde tienes el juego)
                await Shell.Current.GoToAsync("//home", true);
            }
            else
            {
                MensajeError = "Credenciales incorrectas.";
                HayError = true;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error en login: {ex.Message}");
            MensajeError = "Error al conectar con el servidor. Por favor, inténtalo de nuevo.";
            HayError = true;
        }
    }



    [RelayCommand]
    private void IrARegistro()
    {
        Debug.WriteLine("¡IrARegistroCommand se ejecutó!");
        Shell.Current.GoToAsync("registro");

    }

    public static string ObtenerIPLocal()
    {
        foreach (var netInterface in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (netInterface.OperationalStatus == OperationalStatus.Up &&
                netInterface.NetworkInterfaceType != NetworkInterfaceType.Loopback)
            {
                var ipProps = netInterface.GetIPProperties();

                foreach (var addr in ipProps.UnicastAddresses)
                {
                    if (addr.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return addr.Address.ToString();
                    }
                }
            }
        }

        return "127.0.0.1";
    }

}