using App_OficinaVirtual.ViewModels;

namespace App_OficinaVirtual.Views;

public partial class RegisterView : ContentPage
{
	public RegisterView(RegisterViewModel registerViewModel)
	{
		InitializeComponent();

        BindingContext = registerViewModel;
	}
}