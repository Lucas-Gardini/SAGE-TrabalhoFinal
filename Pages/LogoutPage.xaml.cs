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
        // Limpar os dados do usuário logado
        UsuariosService.Logout();

        // Redirecionar para a página de login
        await Navigation.PushAsync(new MainPage());

        // Se quiser remover todas as páginas da pilha de navegação e deixar apenas a página de login:
        Application.Current.MainPage = new NavigationPage(new MainPage());
    }

}