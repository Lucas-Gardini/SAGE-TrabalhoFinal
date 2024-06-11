using SAGE.Modules.Usuarios;
using Microsoft.Maui.Controls;

namespace SAGE.Pages
{
    /// <summary>
    /// Página para criar ou editar usuários.
    /// </summary>
    public partial class CriarEditarUsuariosPage : ContentPage
    {
        // Serviço para gerenciamento de usuários
        private readonly UsuariosService _usuariosService = new();
        // Propriedade para armazenar o usuário atual
        private Usuario Usuario { get; set; }

        // Evento para notificar quando a página deve ser fechada
        public event Action AoFechar;

        /// <summary>
        /// Construtor para criar uma nova página de usuário.
        /// </summary>
        public CriarEditarUsuariosPage()
        {
            InitializeComponent();
            Usuario = new Usuario(); // Inicializa um novo usuário
            BindingContext = this;
            AoFechar = () => { }; // Define o evento de fechamento como um método vazio
        }

        /// <summary>
        /// Construtor para editar um usuário existente.
        /// </summary>
        /// <param name="id">ID do usuário a ser editado.</param>
        public CriarEditarUsuariosPage(int id)
        {
            InitializeComponent();
            Usuario = _usuariosService.GetOne(u => u.Id == id)!; // Obtém o usuário pelo ID
            NomeEntry.Text = Usuario.Nome;
            IsAdminSwitch.IsToggled = Usuario.IsAdmin;
            BindingContext = this;
            AoFechar = () => { }; // Define o evento de fechamento como um método vazio
        }

        /// <summary>
        /// Método acionado quando o botão de fechar é clicado.
        /// </summary>
        private async void CloseButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync(); // Fecha a página modal
        }

        /// <summary>
        /// Método acionado quando o botão de salvar é clicado.
        /// </summary>
        private async void OnSaveClicked(object sender, EventArgs e)
        {
            Usuario.Nome = NomeEntry.Text;
            Usuario.Senha = SenhaEntry.Text;
            Usuario.IsAdmin = IsAdminSwitch.IsToggled;

            if (string.IsNullOrWhiteSpace(Usuario.Nome))
            {
                await DisplayAlert("Erro", "Por favor, preencha todos os campos obrigatórios.", "OK");
                return;
            }

            try
            {
                if (Usuario.Id == 0)
                {
                    if (string.IsNullOrWhiteSpace(Usuario.Senha))
                    {
                        await DisplayAlert("Erro", "Por favor, preencha todos os campos obrigatórios.", "OK");
                        return;
                    }
                    _usuariosService.InsertOne(Usuario); // Adiciona um novo usuário
                } else
                {
                    if (!string.IsNullOrWhiteSpace(Usuario.Senha))
                        Usuario.Senha = BCrypt.Net.BCrypt.HashPassword(Usuario.Senha); // Hash da senha

                    _usuariosService.UpdateOne(Usuario); // Atualiza o usuário existente
                }
                await DisplayAlert("Sucesso", "Usuário salvo com sucesso!", "OK");
                AoFechar?.Invoke(); // Invoca o evento de fechamento
                await Navigation.PopModalAsync(); // Fecha a página modal
            } catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Falha ao salvar usuário: {ex.Message}", "OK");
            }
        }
    }
}
