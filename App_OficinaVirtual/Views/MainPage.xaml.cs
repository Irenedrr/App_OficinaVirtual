using App_OficinaVirtual.Services;
using App_OficinaVirtual.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;

namespace App_OficinaVirtual.Views
{
    public partial class MainPage : ContentPage
    {

        private bool ajustesAbierto = false;
        private Grid? ajustesContainer;
        public MainPage(MainPageViewModel mainViewModel)
        {
            InitializeComponent();

            BindingContext = mainViewModel;
        }


        private async void AbrirAjustes_Clicked(object sender, EventArgs e)
        {
            if (!ajustesAbierto)
            {
                ajustesContainer = new Grid
                {
                    WidthRequest = 260,
                    TranslationX = -260,
                    BackgroundColor = Color.FromArgb("#E5B6FC"),
                    HorizontalOptions = LayoutOptions.Start,
                    ZIndex = 10
                };

                Grid.SetColumn(ajustesContainer, 1);

                
                var httpClient = new HttpClient();
                var jsonOptions = new System.Text.Json.JsonSerializerOptions(); 
                var usuarioService = new UsuarioService(httpClient, jsonOptions);
                var rolService = new RolService(httpClient, jsonOptions);
                var ajustesViewModel = new AjustesViewModel(usuarioService, rolService);

                var ajustesView = new AjustesView(ajustesViewModel);
                ajustesContainer.Children.Add(ajustesView);
                PanelContenedor.Children.Add(ajustesContainer);

                ajustesContainer.IsVisible = true;
                await ajustesContainer.TranslateTo(0, 0, 300, Easing.CubicOut);
                ajustesAbierto = true;
            }
            else
            {
                if (ajustesContainer != null)
                {
                    await ajustesContainer.TranslateTo(-260, 0, 300, Easing.CubicIn);
                    ajustesContainer.IsVisible = false;
                    PanelContenedor.Children.Remove(ajustesContainer);
                    ajustesAbierto = false;
                }
            }
        }


    }
}   
