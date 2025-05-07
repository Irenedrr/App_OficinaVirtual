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
            System.Diagnostics.Debug.WriteLine($"Error en la navegación inicial: {ex.Message}");
        }
    }


}
