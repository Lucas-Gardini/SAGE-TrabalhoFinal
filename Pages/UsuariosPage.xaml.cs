using SAGE.Modules.Usuarios;

namespace SAGE.Pages;

public partial class UsuariosPage : ContentPage
{

    // Serviço de usuários para validação e outras operações
    private readonly UsuariosService _usuariosService = new();

    List<Usuario> Usuarios { get; set; } = new();

    public UsuariosPage()
	{
		InitializeComponent();

		Startup();
	}

	private void Startup()
	{
        Usuarios = _usuariosService.GetMany();

        UsuariosCollectionView.ItemsSource = Usuarios;
    }
}