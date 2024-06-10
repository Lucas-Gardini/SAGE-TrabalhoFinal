namespace SAGE
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());

            // Remove todos os elementos relacionados a NavigationBar
            NavigationPage.SetHasNavigationBar(MainPage, false);
        }
    }
}
