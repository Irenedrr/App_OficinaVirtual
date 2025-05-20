using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using App_OficinaVirtual.DTO;
using App_OficinaVirtual.Models;
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

    [ObservableProperty]
    private string email;

    [ObservableProperty]
    private string contrasena;

    [ObservableProperty]
    private ObservableCollection<UsuarioResponseDto> listaUsuarios;

    [ObservableProperty]
    private bool mostrarUsuariosPanel;

    [ObservableProperty]
    private bool mostrarAjustesPanel;


 



    public MainPageViewModel(UsuarioService usuarioService, AuthService authService)
    {
        _usuarioService = usuarioService;
        _authService = authService;
        EstadoConexion = "Conectado";
        CargarUsuarioAsync();
        
    }


    [RelayCommand]
    public async Task CargarUsuariosConectadosAsync()
    {
        try
        {
            var usuarios = await _usuarioService.LeerTodosAsync();
            if (usuarios == null || !usuarios.Any()) return;

            ListaUsuarios = new ObservableCollection<UsuarioResponseDto>(usuarios);
            MostrarUsuariosPanel = true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error cargando usuarios conectados: {ex.Message}");
        }
    }

    [RelayCommand]
    public void CerrarPanelUsuarios()
    {
        MostrarUsuariosPanel = false;
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

                Email = usuario.Email;
                Contrasena = usuario.Contrasena;
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


    [RelayCommand]
    private void CerrarPanelAjustes()
    {
        MostrarAjustesPanel = false;
    }

    [RelayCommand]
    private async Task CambiarFotoAsync()
    {
        try
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Selecciona una imagen de perfil",
                FileTypes = FilePickerFileType.Images
            });

            if (result != null)
            {
                FotoPerfil = result.FullPath; // Guarda la ruta absoluta de la imagen
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Error seleccionando imagen: " + ex.Message);
        }
    }

    [RelayCommand]
    private async Task GuardarCambiosAsync()
    {

        try
        {
            int id = Preferences.Get("usuario_id", -1);
            if (id == -1) return;

            var dto = new UsuarioUpdateDto
            {
                Nombre = NombreUsuario,
                Email = Email,
                Contrasena = Contrasena,
                ImagenUrl = FotoPerfil,
                
            };

            Debug.WriteLine("DTO a enviar:");
            Debug.WriteLine(JsonSerializer.Serialize(dto));

            var resultado = await _usuarioService.ActualizarAsync(id, dto);

            if (resultado != null)
            {
                Debug.WriteLine("Usuario actualizado correctamente");
                MostrarAjustesPanel = false;
                await ActualizarListaUsuariosSinMostrarPanel();

            }

            else
            {
                Debug.WriteLine("Error: la respuesta del servidor fue nula");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Error guardando cambios: " + ex.Message);
        }
    }

    private async Task ActualizarListaUsuariosSinMostrarPanel()
    {
        try
        {
            var usuarios = await _usuarioService.LeerTodosAsync();
            if (usuarios != null && usuarios.Any())
            {
                ListaUsuarios = new ObservableCollection<UsuarioResponseDto>(usuarios);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error actualizando lista de usuarios: {ex.Message}");
        }
    }



}
