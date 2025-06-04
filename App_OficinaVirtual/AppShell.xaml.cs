using App_OficinaVirtual.ViewModels;
using App_OficinaVirtual.Views;

namespace App_OficinaVirtual;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        
        Routing.RegisterRoute("login", typeof(LoginView));
        Routing.RegisterRoute("registro", typeof(RegisterView));
        Routing.RegisterRoute("home", typeof(MainPage));

        Navigating += OnShellNavigating;


        Task.Run(async () => await MostrarPantallaInicial());
    }

    private void OnShellNavigating(object sender, ShellNavigatingEventArgs e)
    {
        // Verificamos si se navega hacia "home"
        if (e.Target.Location.OriginalString.Contains("home", StringComparison.OrdinalIgnoreCase))
        {
            // Cierra el menú lateral
            Shell.Current.FlyoutIsPresented = false;

            // Cierra los paneles flotantes
            if (Shell.Current.CurrentPage is Views.MainPage mainPage &&
                mainPage.BindingContext is MainPageViewModel vm)
            {
                vm.CerrarTodosLosPaneles();
            }
        }
    }



    //Metodos para mostrar los paneles de la vista principal
    private async void OnMostrarUsuariosClicked(object sender, EventArgs e)
    {
        if (Current?.CurrentPage is Views.MainPage mainPage &&
            mainPage.BindingContext is MainPageViewModel vm)
        {
            vm.MostrarUsuariosPanel = true;
            await vm.CargarUsuariosConectadosAsync(); 
        }
    }

    private async void OnMostrarAjustesClicked(object sender, EventArgs e)
    {
        if (Current?.CurrentPage is Views.MainPage mainPage &&
            mainPage.BindingContext is MainPageViewModel vm)
        {
            vm.AbrirPanelUnico("ajustes");
        }
    }


    private async void OnMostrarEventosClicked(object sender, EventArgs e)
    {
        if (Current?.CurrentPage is Views.MainPage mainPage &&
            mainPage.BindingContext is MainPageViewModel vm)
        {
            await vm.CargarEventosAsync();
            await vm.CargarUsuariosConectadosAsync();
            vm.MostrarEventosPanel = true;
             
        }
    }

    private async void OnMostrarChatClicked(object sender, EventArgs e)
    {
        if (Current?.CurrentPage is Views.MainPage mainPage &&
            mainPage.BindingContext is MainPageViewModel vm)
        {
            await vm.CargarUsuariosConectadosAsync(); 
            vm.MostrarChatPanel = true;
        }
    }

    private async Task MostrarPantallaInicial()
    {
        try
        {
            if (Preferences.ContainsKey("access_token"))
            {
                await Shell.Current.GoToAsync("//home");
            }
            else
            {
                await Shell.Current.GoToAsync("//login");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error en la navegación: {ex.Message}");
        }
    }

}
