namespace SAGE.Pages;

public partial class IndexPage : ContentPage
{
	public IndexPage()
	{
		InitializeComponent();

		// Força a remoção do botão de voltar
        NavigationPage.SetHasBackButton(this, false);
    }
}