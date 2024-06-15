namespace SAGE.Pages;

public partial class LogoutPage : ContentPage
{
	public LogoutPage()
	{
		InitializeComponent();
		Logout();
    }

	private void Logout()
	{
        // Fecha a aplicação
        Environment.Exit(0);
    }
}