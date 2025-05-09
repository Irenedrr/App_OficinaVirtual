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


     


    }
}   
