using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using App_OficinaVirtual.Services;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using System.Diagnostics;

namespace App_OficinaVirtual.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly AuthService _servicioAutenticacion;

    [ObservableProperty]
    private string correo = string.Empty;

    [ObservableProperty]
    private string contrasena = string.Empty;

    [ObservableProperty]
    private string mensajeError = string.Empty;

    [ObservableProperty]
    private bool hayError;

    public LoginViewModel(AuthService servicioAutenticacion)
    {
        _servicioAutenticacion = servicioAutenticacion;
    }

    [RelayCommand]
    private async Task IniciarSesionAsync()
    {
        Debug.WriteLine("El comando de iniciar sesión se ejecutó");

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
}


