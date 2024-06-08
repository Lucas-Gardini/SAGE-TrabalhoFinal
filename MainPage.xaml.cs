using SAGE.Modules.Usuarios;
using SAGE.Pages;

namespace SAGE
{
    /// <summary>
    /// Representa a página principal do aplicativo.
    /// </summary>
    public partial class MainPage : ContentPage
    {
        // Caminho onde fica salvo o arquivo de lembrar usuário
        public readonly string UsuarioCaminho = Path.Combine(FileSystem.AppDataDirectory, "usuario.txt");

        // Serviço de usuários para validação e outras operações
        private readonly UsuariosService _usuariosService = new();

        /// <summary>
        /// Inicializa uma nova instância da classe <see cref="MainPage"/>.
        /// </summary>
        public MainPage()
        {
            InitializeComponent();

            // Lê o arquivo de lembrar, caso exista e preenche o input de usuário
            if (File.Exists(UsuarioCaminho))
                UsernameEntry.Text = File.ReadAllText(UsuarioCaminho);
        }

        /// <summary>
        /// Evento disparado quando o botão de autenticação é clicado.
        /// </summary>
        private async void AuthButton_Clicked(object sender, EventArgs e)
        {
            // Obtém o nome de usuário da entrada de texto
            var usuario = UsernameEntry.Text;
            // Obtém a senha da entrada de texto
            var senha = PasswordEntry.Text;

            // Verifica se o nome de usuário foi informado
            if (string.IsNullOrEmpty(usuario))
            {
                await DisplayAlert("Erro", "Usuário não informado", "OK");
                return;
            }

            // Verifica se a senha foi informada
            if (string.IsNullOrEmpty(senha))
            {
                await DisplayAlert("Erro", "Senha não informada", "OK");
                return;
            }

            // Valida o usuário e a senha
            var usuarioAutenticado = _usuariosService.RealizarLogin(usuario, senha);

            // Verifica se a autenticação foi bem-sucedida
            if (usuarioAutenticado.Nome != "")
            {
                // Se o usuário marcou para lembrar dele, salvamos seu usuário em um arquivo na pasta do projeto
                if (RememberMeCheckBox.IsChecked)
                    File.WriteAllText(UsuarioCaminho, usuario);
                else
                    File.Delete(UsuarioCaminho);

                await DisplayAlert("Sucesso", $"Bem vindo(a) de volta {usuarioAutenticado.Nome}", "OK");
                await Navigation.PushAsync(new IndexPage());
            } else
            {
                await DisplayAlert("Erro", "Usuário ou senha inválidos", "OK");
            }
        }

        /// <summary>
        /// Evento disparado quando o botão de limpeza é clicado, excluindo o arquivo do sqlite e fechando a aplicação.
        /// </summary>
        private void ClearDBButton_Clicked(object sender, EventArgs e)
        {
            // Exclui o arquivo do banco de dados
            File.Delete(_usuariosService.DbPath);
            // Fecha a aplicação
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
