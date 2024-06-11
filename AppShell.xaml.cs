using SAGE.Extension;
using SAGE.Modules.Usuarios;
using SAGE.Pages;
using System.Globalization;

namespace SAGE;

public partial class AppShell : Shell
{
	public AppShell()
	{
        InitializeComponent();

        MessagingCenter.Subscribe<ConfigPage>(this, "LanguageChanged", (sender) =>
        {
            // Atualize o conteúdo da página aqui
            this.AtualizarTitulosAba();
        });

        if (UsuariosService.GetUsuarioLogado()?.IsAdmin == true)
			UsersTab.IsVisible = true;
    }

    private void AtualizarTitulosAba()
    {
        // Obtém a cultura atual
        var culture = Translator.Instance.Culture ?? CultureInfo.CurrentCulture;
    }

}