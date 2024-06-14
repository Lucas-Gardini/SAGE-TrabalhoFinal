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
}