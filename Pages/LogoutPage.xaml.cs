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
        // Fecha a aplica��o
        Environment.Exit(0);
    }
}