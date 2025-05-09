using System.Diagnostics;
using App_OficinaVirtual.ViewModels;

namespace App_OficinaVirtual.Views;

public partial class AjustesView : ContentView
{
    public AjustesView(AjustesViewModel ajustesViewModel)
    {
        InitializeComponent();
        BindingContext = ajustesViewModel;
        
    }
}
