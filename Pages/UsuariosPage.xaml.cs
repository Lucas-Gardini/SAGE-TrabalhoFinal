using SAGE.Modules.Usuarios;

namespace SAGE.Pages
{
    /// <summary>
    /// P�gina principal para gerenciar usu�rios.
    /// </summary>
    public partial class UsuariosPage : ContentPage
    {
        // Servi�o para opera��es com usu�rios
        private readonly UsuariosService _usuariosService = new();
        // Lista para armazenar os usu�rios exibidos
        List<Usuario> Usuarios { get; set; } = new();

        /// <summary>
        /// Construtor da p�gina de usu�rios.
        /// </summary>
        public UsuariosPage()
        {
            InitializeComponent();
            Startup(); // Inicializa a p�gina
        }

        /// <summary>
        /// M�todo de inicializa��o para carregar os usu�rios.
        /// </summary>
        private void Startup()
        {
            Usuarios = _usuariosService.GetMany(); // Obt�m a lista de usu�rios
            UsuariosCollectionView.ItemsSource = Usuarios; // Define a fonte de itens para o CollectionView
        }

        /// <summary>
        /// M�todo acionado quando o bot�o de edi��o � clicado.
        /// </summary>
        private void OnEditItem_Clicked(object sender, EventArgs e)
        {
            var menuItem = sender as Button;

            if (menuItem == null)
                return;

            var usuarioId = menuItem.CommandParameter;

            var modal = new CriarEditarUsuariosPage(Convert.ToInt32(usuarioId)); // Cria a p�gina modal para edi��o

            modal.AoFechar += Startup; // Associa o evento de fechamento para atualizar a lista de usu�rios

            Navigation.PushModalAsync(modal); // Abre a p�gina modal
        }

        /// <summary>
        /// M�todo acionado quando o bot�o de exclus�o � clicado.
        /// </summary>
        private void OnDeleteItem_Clicked(object sender, EventArgs e)
        {
            var menuItem = sender as Button;

            if (menuItem == null)
                return;

            var usuarioId = menuItem.CommandParameter;

            try
            {
                if (Convert.ToInt32(usuarioId) == UsuariosService.GetUsuarioLogado().Id) // Verifica se o usu�rio n�o est� tentando excluir a si mesmo
                    throw new Exception("N�o � poss�vel excluir o usu�rio logado.");

                _usuariosService.DeleteOne(_usuariosService.GetOne(u => u.Id == Convert.ToInt32(usuarioId))!); // Exclui o usu�rio
            } catch
            {
                DisplayAlert("Erro", "N�o foi poss�vel excluir o usu�rio", "OK"); // Exibe mensagem de erro
            }

            Startup(); // Atualiza a lista de usu�rios
        }

        /// <summary>
        /// M�todo acionado quando o bot�o flutuante � clicado.
        /// </summary>
        private void FabButton_Clicked(object sender, EventArgs e)
        {
            var modal = new CriarEditarUsuariosPage(); // Cria a p�gina modal para cria��o de um novo usu�rio

            modal.AoFechar += Startup; // Associa o evento de fechamento para atualizar a lista de usu�rios

            Navigation.PushModalAsync(modal); // Abre a p�gina modal
        }
    }
}
