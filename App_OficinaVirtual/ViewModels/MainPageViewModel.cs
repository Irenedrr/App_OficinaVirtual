using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App_OficinaVirtual.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace App_OficinaVirtual.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
    private readonly UsuarioService _usuarioService;
    private readonly AuthService _authService;

    [ObservableProperty]
    private string nombreUsuario;

    [ObservableProperty]
    private string estadoConexion;

    [ObservableProperty]
    private string fotoPerfil;

    public MainPageViewModel(UsuarioService usuarioService, AuthService authService)
    {
        _usuarioService = usuarioService;
        _authService = authService;
        EstadoConexion = "Online";
        CargarUsuarioAsync();
    }

    private async void CargarUsuarioAsync()
    {
        try
        {
            int usuarioId = Preferences.Get("usuario_id", -1); 

            if (usuarioId == -1) return;

            var usuario = await _usuarioService.LeerPorIdAsync(usuarioId);
            if (usuario != null)
            {
                NombreUsuario = usuario.Nombre;
                FotoPerfil = usuario.ImagenUrl;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error cargando el usuario: {ex.Message}");
        }
    }



    [RelayCommand]
    private async Task CerrarSesionAsync()
    {
        _authService.Logout();
        await Shell.Current.GoToAsync("//login", true);
    }
}
