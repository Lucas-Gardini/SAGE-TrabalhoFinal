using SAGE.Modules.Usuarios;

namespace SAGE
{
    public partial class MainPage : ContentPage
    {
        private static readonly UsuariosService _usuariosService = new ();

        public MainPage()
        {
            InitializeComponent();
        }

        private void AuthButton_Clicked(object sender, EventArgs e)
        {
            var usuarios = _usuariosService.GetMany();
            var b = 2;
        }
    }
}
