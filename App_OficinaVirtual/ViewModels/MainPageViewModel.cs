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
    private readonly EventoService _eventoService;
    private readonly AuthService _authService;
    private readonly MensajeService _mensajeService;

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
    public ObservableCollection<UsuarioResponseDto> listaUsuarios;

    [ObservableProperty]
    private bool mostrarUsuariosPanel;

    [ObservableProperty]
    private bool mostrarAjustesPanel;

    //propiedad panel evento
    [ObservableProperty]
    private bool mostrarEventosPanel;

    [ObservableProperty]
    private string tituloEvento;

    [ObservableProperty]
    private string descripcionEvento;

    [ObservableProperty]
    private DateTime fechaEvento = DateTime.Today;

    [ObservableProperty]
    private TimeSpan horaEvento = TimeSpan.Zero;

    [ObservableProperty]
    private string tipoEvento;

    [ObservableProperty]
    private ObservableCollection<EventoResponseDto> listaEventos;

    [ObservableProperty]
    private ObservableCollection<UsuarioResponseDto> participantesSeleccionados;

    //Panel chat

    [ObservableProperty]
    private bool mostrarChatPanel;

    [ObservableProperty]
    private UsuarioResponseDto usuarioSeleccionadoChat;

    [ObservableProperty]
    private ObservableCollection<MensajeResponseDto> mensajesChat;

    [ObservableProperty]
    private string mensajeNuevo;








    public MainPageViewModel(UsuarioService usuarioService, AuthService authService, EventoService eventoService, MensajeService mensajeService)
    {
        _usuarioService = usuarioService;
        _authService = authService;
        _eventoService = eventoService;
        _mensajeService = mensajeService;

        ListaUsuarios = new ObservableCollection<UsuarioResponseDto>();
        ParticipantesSeleccionados = new ObservableCollection<UsuarioResponseDto>();


        EstadoConexion = "Conectado";
        CargarUsuarioAsync();
        
    }

    //Caargar datos

    [RelayCommand]
    public async Task CargarUsuariosConectadosAsync()
    {
        try
        {
            var usuarios = await _usuarioService.LeerTodosAsync();
            if (usuarios == null || !usuarios.Any()) return;

            ListaUsuarios = new ObservableCollection<UsuarioResponseDto>(usuarios);
            AbrirPanelUnico("usuarios");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error cargando usuarios conectados: {ex.Message}");
        }
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
    public async Task CargarEventosAsync()
    {
        try
        {
            var eventos = await _eventoService.LeerTodosAsync();
            int usuarioId = Preferences.Get("usuario_id", -1);
            if (usuarioId == -1) return;

            var filtrados = eventos
                .Where(e => e.Participantes.Contains(usuarioId))
                .ToList();

            ListaEventos = new ObservableCollection<EventoResponseDto>(filtrados);
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Error cargando eventos: " + ex.Message);
        }
    }


    private async Task CargarMensajesAsync()
    {
        if (UsuarioSeleccionadoChat == null)
        {
            Debug.WriteLine("UsuarioSeleccionadoChat es null");
            return;
        }

        var todos = await _mensajeService.LeerTodosAsync();
        var miId = Preferences.Get("usuario_id", -1);

        var relevantes = todos
            .Where(m => (m.EmisorId == miId && m.ReceptorId == UsuarioSeleccionadoChat.Id) ||
                        (m.EmisorId == UsuarioSeleccionadoChat.Id && m.ReceptorId == miId))
            .OrderBy(m => m.Fecha)
            .ToList();

        Debug.WriteLine($"Mensajes encontrados: {relevantes.Count}");

        MensajesChat = new ObservableCollection<MensajeResponseDto>(relevantes);
    }






    //Cambiar foto de perfil

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

    //Guardar datos

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
                mostrarAjustesPanel = false;
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

    [RelayCommand]
    public async Task GuardarEventoAsync()
    {
        try
        {
            var id = Preferences.Get("usuario_id", -1);
            if (id == -1) return;

            var participantes = ListaUsuarios
                .Where(u => u.IsSeleccionado)
                .Select(u => u.Id)
                .ToList();

            var nuevoEvento = new EventoCreateDto
            {
                Titulo = TituloEvento,
                Descripcion = DescripcionEvento,
                FechaEvento = FechaEvento.Date + HoraEvento,
                Tipo = TipoEvento,
                CreadorId = id,
                Participantes = ListaUsuarios
                 .Where(u => u.IsSeleccionado)
                 .Select(u => u.Id)
                 .ToList()
            };





            var creado = await _eventoService.CrearAsync(nuevoEvento);

            if (creado != null)
            {
                Debug.WriteLine("Evento creado correctamente");
                await CargarEventosAsync();
                LimpiarFormularioEvento();
                
            }
            else
            {
                Debug.WriteLine("Error: evento no creado");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine("Error guardando evento: " + ex.Message);
        }
    }

    //limpiar datos
    private void LimpiarFormularioEvento()
    {
        TituloEvento = string.Empty;
        DescripcionEvento = string.Empty;
        TipoEvento = null;

        FechaEvento = DateTime.Today;         
        HoraEvento = TimeSpan.Zero;           

        ParticipantesSeleccionados = new ObservableCollection<UsuarioResponseDto>();

        foreach (var usuario in ListaUsuarios)
            usuario.IsSeleccionado = false;
    }

    [RelayCommand]
    public void MostrarPanelEventos()
    {
        AbrirPanelUnico("eventos");
    }

    [RelayCommand]
    public async Task MostrarPanelAjustes()
    {
       
        AbrirPanelUnico("ajustes");
    }




    //Actualizar datos

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


    //metodos de mensajes

    [RelayCommand]
    public async Task SeleccionarUsuarioChatAsync(UsuarioResponseDto usuario)
    {
        if (usuario == null)
        {
            Debug.WriteLine("⚠️ El parámetro 'usuario' llegó como null");
            return;
        }

        Debug.WriteLine($"✅ Usuario tocado: {usuario.Nombre}");

        UsuarioSeleccionadoChat = usuario;
        MensajeNuevo = string.Empty;
        AbrirPanelUnico("chat");
        await CargarMensajesAsync();
    }



    [RelayCommand]
    public async Task EnviarMensajeAsync()
    {
        if (string.IsNullOrWhiteSpace(MensajeNuevo) || UsuarioSeleccionadoChat == null)
            return;

        var emisorId = Preferences.Get("usuario_id", -1);
        if (emisorId == -1) return;

        var nuevoMensaje = new MensajeCreateDto
        {
            Contenido = MensajeNuevo,
            EmisorId = emisorId,
            ReceptorId = UsuarioSeleccionadoChat.Id,
            Fecha = DateTime.Now
        };

        var creado = await _mensajeService.CrearAsync(nuevoMensaje);
        if (creado != null)
        {
            MensajeNuevo = string.Empty;
            await CargarMensajesAsync();
        }
    }

    public void AbrirPanelUnico(string panel)
    {
        MostrarUsuariosPanel = false;
        MostrarAjustesPanel = false;
        MostrarEventosPanel = false;
        MostrarChatPanel = false;

        // Forzar notificación
        OnPropertyChanged(nameof(MostrarUsuariosPanel));
        OnPropertyChanged(nameof(MostrarAjustesPanel));
        OnPropertyChanged(nameof(MostrarEventosPanel));
        OnPropertyChanged(nameof(MostrarChatPanel));

        // Activar el que corresponde
        switch (panel)
        {
            case "usuarios":
                MostrarUsuariosPanel = true;
                break;
            case "ajustes":
                MostrarAjustesPanel = true;
                break;
            case "eventos":
                MostrarEventosPanel = true;
                break;
            case "chat":
                MostrarChatPanel = true;
                break;
        }
    }




    //cerrar paneles

    [RelayCommand]
    private async Task CerrarSesionAsync()
    {
        _authService.Logout();
        await Shell.Current.GoToAsync("//login", true);
    }

    [RelayCommand]
    public void CerrarPanelUsuarios()
    {
        MostrarUsuariosPanel = false;
    }

    [RelayCommand]
    private void CerrarPanelAjustes()
    {
        MostrarAjustesPanel = false;
        
    }

    [RelayCommand]
    public void CerrarPanelEventos()
    {
        MostrarEventosPanel = false;
        MostrarUsuariosPanel = false;
    }

    [RelayCommand]
    public void CerrarPanelChat()
    {
        MostrarChatPanel = false;
        MostrarUsuariosPanel = false;
    }


    public void CerrarTodosLosPaneles()
    {
        MostrarUsuariosPanel = false;
        MostrarAjustesPanel = false;
        MostrarEventosPanel = false;
        MostrarChatPanel = false;
    }


}
