using App_OficinaVirtual.ViewModels;

namespace App_OficinaVirtual.Views;

public partial class LoginView : ContentPage
{
	public LoginView(LoginViewModel loginViewModel)
	{
		InitializeComponent();
        BindingContext = loginViewModel;
    }
}