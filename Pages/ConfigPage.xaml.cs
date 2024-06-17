using SAGE.Extension;
using SAGE.Modules.Usuarios;
using System.Globalization;
using Themes = SAGE.Resources.Styles.Themes;

namespace SAGE.Pages;

public partial class ConfigPage : ContentPage
{
    UsuariosService _usuariosService = new UsuariosService();

    public ConfigPage()
	{
		InitializeComponent();

        ChangeTheme.IsToggled = ThemeManager.SelectedTheme == nameof(Themes.DarkTheme);

        if (Translator.Instance.Culture.Name == "pt-BR")
        {
            rbPt.IsChecked = true;
        }
        else if (Translator.Instance.Culture.Name == "en-US")
        {
            rbEn.IsChecked = true;
        }
    }


    private void RbPt_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (Translator.Instance.Culture.Name != "pt-BR")
        {
            Translator.Instance.Culture = new CultureInfo("pt-BR");
            Translator.Instance.OnPropertyChanged();
            MessagingCenter.Send(this, "LanguageChanged");
        }
    }

    private void RbEn_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (Translator.Instance.Culture.Name != "en-US")
        {
            Translator.Instance.Culture = new CultureInfo("en-US");
            Translator.Instance.OnPropertyChanged();
            MessagingCenter.Send(this, "LanguageChanged");
        }
    }

    private void ChangeTheme_Toggled(object sender, ToggledEventArgs e)
    {
        if (e.Value)
        {
            ThemeManager.SetTheme(nameof(Themes.DarkTheme));
        }
        else
        {
            ThemeManager.SetTheme(nameof(Themes.LightTheme));
        }
    }

    private async void TrocarSenhaButton_Clicked(object sender, EventArgs e)
    {
        var senha = await DisplayAlert(Translator.Instance["changePass"], Translator.Instance["changePassAsk"], Translator.Instance["yes"], Translator.Instance[key: "no"]);
        if (senha)
        {
            var novaSenha = await DisplayPromptAsync(Translator.Instance["changePass"], Translator.Instance["changePassNew"], "OK", Translator.Instance["cancel"], Translator.Instance["newPass"], 16, Keyboard.Default, "");

            Usuario usuario = UsuariosService.GetUsuarioLogado();
            usuario.Senha = novaSenha;

            _usuariosService.UpdateOne(usuario);

            await DisplayAlert(Translator.Instance["passChanged"], Translator.Instance["changePassSuccess"], "OK");
        }
    }
}