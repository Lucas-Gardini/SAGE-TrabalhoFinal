using SAGE.Extension;
using System.Globalization;

namespace SAGE.Pages;

public partial class ConfigPage : ContentPage
{
	public ConfigPage()
	{
		InitializeComponent();
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
}