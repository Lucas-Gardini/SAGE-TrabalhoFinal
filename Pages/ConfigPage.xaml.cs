using SAGE.Extension;
using System.Globalization;
using Themes = SAGE.Resources.Styles.Themes;

namespace SAGE.Pages;

public partial class ConfigPage : ContentPage
{
	public ConfigPage()
	{
		InitializeComponent();

        ChangeTheme.IsToggled = ThemeManager.SelectedTheme == nameof(Themes.DarkTheme);

        if(Translator.Instance.Culture.Name ==  "en-US")
        {
            rbEn.IsChecked = false;
            rbPt.IsChecked = true;
        }
        else
        {
            rbPt.IsChecked = false;
            rbEn.IsChecked = true;
        }
    }

    private void Atualizar_idioma()
    {

    }

    private void RbPt_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        Translator.Instance.Culture = new CultureInfo("en-US");
        Translator.Instance.OnPropertyChanged();
        MessagingCenter.Send(this, "LanguageChanged");
    }

    private void RbEn_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        Translator.Instance.Culture = new CultureInfo("");
        Translator.Instance.OnPropertyChanged();
        MessagingCenter.Send(this, "LanguageChanged");
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
}