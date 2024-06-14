using SAGE.Extension;
using System.Globalization;

namespace SAGE
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Translator.Instance.Culture = new CultureInfo("pt-BR");

            MainPage = new NavigationPage(new MainPage());

            // Remove todos os elementos relacionados a NavigationBar
            NavigationPage.SetHasNavigationBar(MainPage, false);
        }
    }
}
