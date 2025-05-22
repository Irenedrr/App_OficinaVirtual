using App_OficinaVirtual.DTO;
using App_OficinaVirtual.Services;
using App_OficinaVirtual.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls;

namespace App_OficinaVirtual.Views
{
    public partial class MainPage : ContentPage
    {

        
        public MainPage(MainPageViewModel mainViewModel)
        {
            InitializeComponent();
            BindingContext = mainViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Shell.Current.FlyoutBehavior = FlyoutBehavior.Flyout;
        }

        private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (sender is CheckBox checkbox && checkbox.BindingContext is UsuarioResponseDto usuario)
            {
                var vm = BindingContext as MainPageViewModel;

                if (vm == null) return;

                if (e.Value)
                {
                    if (!vm.ParticipantesSeleccionados.Contains(usuario))
                        vm.ParticipantesSeleccionados.Add(usuario);
                }
                else
                {
                    if (vm.ParticipantesSeleccionados.Contains(usuario))
                        vm.ParticipantesSeleccionados.Remove(usuario);
                }
            }
        }
    }
}   
