namespace SAGE.Pages;

public partial class IndexPage : ContentPage
{
	public IndexPage()
	{
		InitializeComponent();

		// For�a a remo��o do bot�o de voltar
        NavigationPage.SetHasBackButton(this, false);
    }
}