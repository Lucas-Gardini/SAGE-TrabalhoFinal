using SAGE.Modules.Usuarios;

namespace SAGE.Pages
{
    /// <summary>
    /// Página principal para gerenciar usuários.
    /// </summary>
    public partial class UsuariosPage : ContentPage
    {
        // Serviço para operações com usuários
        private readonly UsuariosService _usuariosService = new();
        // Lista para armazenar os usuários exibidos
        List<Usuario> Usuarios { get; set; } = new();

        /// <summary>
        /// Construtor da página de usuários.
        /// </summary>
        public UsuariosPage()
        {
            InitializeComponent();
            Startup(); // Inicializa a página
        }

        /// <summary>
        /// Método de inicialização para carregar os usuários.
        /// </summary>
        private void Startup()
        {
            Usuarios = _usuariosService.GetMany(); // Obtém a lista de usuários
            UsuariosCollectionView.ItemsSource = Usuarios; // Define a fonte de itens para o CollectionView
        }

        /// <summary>
        /// Método acionado quando o botão de edição é clicado.
        /// </summary>
        private void OnEditItem_Clicked(object sender, EventArgs e)
        {
            var menuItem = sender as Button;

            if (menuItem == null)
                return;

            var usuarioId = menuItem.CommandParameter;

            var modal = new CriarEditarUsuariosPage(Convert.ToInt32(usuarioId)); // Cria a página modal para edição

            modal.AoFechar += Startup; // Associa o evento de fechamento para atualizar a lista de usuários

            Navigation.PushModalAsync(modal); // Abre a página modal
        }

        /// <summary>
        /// Método acionado quando o botão de exclusão é clicado.
        /// </summary>
        private void OnDeleteItem_Clicked(object sender, EventArgs e)
        {
            var menuItem = sender as Button;

            if (menuItem == null)
                return;

            var usuarioId = menuItem.CommandParameter;

            try
            {
                if (Convert.ToInt32(usuarioId) == UsuariosService.GetUsuarioLogado().Id) // Verifica se o usuário não está tentando excluir a si mesmo
                    throw new Exception("Não é possível excluir o usuário logado.");

                _usuariosService.DeleteOne(_usuariosService.GetOne(u => u.Id == Convert.ToInt32(usuarioId))!); // Exclui o usuário
            } catch
            {
                DisplayAlert("Erro", "Não foi possível excluir o usuário", "OK"); // Exibe mensagem de erro
            }

            Startup(); // Atualiza a lista de usuários
        }

        /// <summary>
        /// Método acionado quando o botão flutuante é clicado.
        /// </summary>
        private void FabButton_Clicked(object sender, EventArgs e)
        {
            var modal = new CriarEditarUsuariosPage(); // Cria a página modal para criação de um novo usuário

            modal.AoFechar += Startup; // Associa o evento de fechamento para atualizar a lista de usuários

            Navigation.PushModalAsync(modal); // Abre a página modal
        }
    }
}
