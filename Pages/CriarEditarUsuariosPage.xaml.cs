using SAGE.Modules.Usuarios;
using Microsoft.Maui.Controls;

namespace SAGE.Pages
{
    /// <summary>
    /// P�gina para criar ou editar usu�rios.
    /// </summary>
    public partial class CriarEditarUsuariosPage : ContentPage
    {
        // Servi�o para gerenciamento de usu�rios
        private readonly UsuariosService _usuariosService = new();
        // Propriedade para armazenar o usu�rio atual
        private Usuario Usuario { get; set; }

        // Evento para notificar quando a p�gina deve ser fechada
        public event Action AoFechar;

        /// <summary>
        /// Construtor para criar uma nova p�gina de usu�rio.
        /// </summary>
        public CriarEditarUsuariosPage()
        {
            InitializeComponent();
            Usuario = new Usuario(); // Inicializa um novo usu�rio
            BindingContext = this;
            AoFechar = () => { }; // Define o evento de fechamento como um m�todo vazio
        }

        /// <summary>
        /// Construtor para editar um usu�rio existente.
        /// </summary>
        /// <param name="id">ID do usu�rio a ser editado.</param>
        public CriarEditarUsuariosPage(int id)
        {
            InitializeComponent();
            Usuario = _usuariosService.GetOne(u => u.Id == id)!; // Obt�m o usu�rio pelo ID
            NomeEntry.Text = Usuario.Nome;
            IsAdminSwitch.IsToggled = Usuario.IsAdmin;
            BindingContext = this;
            AoFechar = () => { }; // Define o evento de fechamento como um m�todo vazio
        }

        /// <summary>
        /// M�todo acionado quando o bot�o de fechar � clicado.
        /// </summary>
        private async void CloseButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync(); // Fecha a p�gina modal
        }

        /// <summary>
        /// M�todo acionado quando o bot�o de salvar � clicado.
        /// </summary>
        private async void OnSaveClicked(object sender, EventArgs e)
        {
            Usuario.Nome = NomeEntry.Text;
            Usuario.Senha = SenhaEntry.Text;
            Usuario.IsAdmin = IsAdminSwitch.IsToggled;

            if (string.IsNullOrWhiteSpace(Usuario.Nome))
            {
                await DisplayAlert("Erro", "Por favor, preencha todos os campos obrigat�rios.", "OK");
                return;
            }

            try
            {
                if (Usuario.Id == 0)
                {
                    if (string.IsNullOrWhiteSpace(Usuario.Senha))
                    {
                        await DisplayAlert("Erro", "Por favor, preencha todos os campos obrigat�rios.", "OK");
                        return;
                    }
                    _usuariosService.InsertOne(Usuario); // Adiciona um novo usu�rio
                } else
                {
                    if (!string.IsNullOrWhiteSpace(Usuario.Senha))
                        Usuario.Senha = BCrypt.Net.BCrypt.HashPassword(Usuario.Senha); // Hash da senha

                    _usuariosService.UpdateOne(Usuario); // Atualiza o usu�rio existente
                }
                await DisplayAlert("Sucesso", "Usu�rio salvo com sucesso!", "OK");
                AoFechar?.Invoke(); // Invoca o evento de fechamento
                await Navigation.PopModalAsync(); // Fecha a p�gina modal
            } catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Falha ao salvar usu�rio: {ex.Message}", "OK");
            }
        }
    }
}
