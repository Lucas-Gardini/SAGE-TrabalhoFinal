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
            // Atualize o conte�do da p�gina aqui
            this.AtualizarTitulosAba();
        });

        if (UsuariosService.GetUsuarioLogado()?.IsAdmin == true)
			UsersTab.IsVisible = true;
    }

    private void AtualizarTitulosAba()
    {
        // Obt�m a cultura atual
        var culture = Translator.Instance.Culture ?? CultureInfo.CurrentCulture;
    }

}