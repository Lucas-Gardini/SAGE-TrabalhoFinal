using SAGE.Modules.Usuarios;

namespace SAGE;

public partial class AppShell : Shell
{
	public AppShell()
	{
        InitializeComponent();

		if (UsuariosService.GetUsuarioLogado()?.IsAdmin == true)
			UsersTab.IsVisible = true;
	}
}