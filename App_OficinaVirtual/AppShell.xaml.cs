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



        Task.Run(async () => await MostrarPantallaInicial());
    }

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
            vm.MostrarAjustesPanel = true;
        }
    }



    private async Task MostrarPantallaInicial()
    {
        try
        {
            if (Preferences.ContainsKey("access_token"))
            {
                await GoToAsync("//home");
            }
            else
            {
                await GoToAsync("//login");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error en la navegación: {ex.Message}");
        }
    }


}
