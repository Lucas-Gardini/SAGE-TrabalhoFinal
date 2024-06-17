using SAGE.Modules.Usuarios;

namespace SAGE.Pages;

public partial class LogoutPage : ContentPage
{
	public LogoutPage()
	{
		InitializeComponent();
		Logout();
    }
    private async void Logout()
    {
        // Limpar os dados do usu�rio logado
        UsuariosService.Logout();

        // Redirecionar para a p�gina de login
        await Navigation.PushAsync(new MainPage());

        // Se quiser remover todas as p�ginas da pilha de navega��o e deixar apenas a p�gina de login:
        Application.Current.MainPage = new NavigationPage(new MainPage());
    }

}