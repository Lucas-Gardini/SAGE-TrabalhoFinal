using SAGE.Extension;
using SAGE.Modules.Usuarios;
using SAGE.Pages;
using System.Globalization;
using System.Windows.Input;
using Themes = SAGE.Resources.Styles.Themes;

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

            ThemeManager.LoadCurrentTheme();

            if (ThemeManager.SelectedTheme == nameof(Themes.DarkTheme))
            {
                ChangeThemeBtn.ImageSource = "moon.png";
            }
            else
            {
                ChangeThemeBtn.ImageSource = "sun.png";
            }
                // Lê o arquivo de lembrar, caso exista e preenche o input de usuário
                if (File.Exists(UsuarioCaminho))
            {
                RememberMeCheckBox.IsChecked = true;
                UsernameEntry.Text = File.ReadAllText(UsuarioCaminho);
            }

            ThemeManager.LoadCurrentTheme();
            var linguaSalva = Translator.Instance.GetSavedCulture();

            if (linguaSalva != null)
            {
                Translator.Instance.Culture = new CultureInfo(linguaSalva);
                Translator.Instance.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Evento disparado quando o botão de autenticação é clicado.
        /// </summary>
        private async void AuthButton_Clicked(object sender, EventArgs e)
        {
            AuthButton.IsEnabled = false;
            AuthButton.Text = Translator.Instance["loading"];
            // Obtém o nome de usuário da entrada de texto
            var usuario = UsernameEntry.Text;
            // Obtém a senha da entrada de texto
            var senha = PasswordEntry.Text;

            // Verifica se o nome de usuário foi informado
            if (string.IsNullOrEmpty(usuario))
            {
                await DisplayAlert(Translator.Instance["error"], Translator.Instance["notInfoUser"], "OK");
                AuthButton.Text = Translator.Instance["enter"];
                AuthButton.IsEnabled = true;
                return;
            }

            // Verifica se a senha foi informada
            if (string.IsNullOrEmpty(senha))
            {
                await DisplayAlert(Translator.Instance["error"], Translator.Instance["notInfoPass"], "OK");
                AuthButton.Text = Translator.Instance["enter"];
                AuthButton.IsEnabled = true;
                return;
            }

            await ShowLoadingModalAsync();

            // Valida o usuário e a senha
            var usuarioAutenticado = _usuariosService.RealizarLogin(usuario, senha);

            await HideLoadingModalAsync();

            // Verifica se a autenticação foi bem-sucedida
            if (usuarioAutenticado.Nome != "")
            {
                // Se o usuário marcou para lembrar dele, salvamos seu usuário em um arquivo na pasta do projeto
                if (RememberMeCheckBox.IsChecked)
                    File.WriteAllText(UsuarioCaminho, usuario);
                else
                    File.Delete(UsuarioCaminho);

                await Navigation.PushAsync(new AppShell()); // Navega para a página contendo a navegação principal
            } else
            {
                await DisplayAlert(Translator.Instance["error"], Translator.Instance["invUserOrPass"], "OK");
                AuthButton.Text = Translator.Instance["enter"];
                AuthButton.IsEnabled = true;
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

        private void ChangeThemeBtn_Clicked(object sender, EventArgs e)
        {
            if (ThemeManager.SelectedTheme == nameof(Themes.LightTheme))
            {
                ThemeManager.SetTheme(nameof(Themes.DarkTheme));
                ChangeThemeBtn.ImageSource = "moon.png";
            }
            else
            {
                ThemeManager.SetTheme(nameof(Themes.LightTheme));
                ChangeThemeBtn.ImageSource = "sun.png";
            }
        }

        private void ChangeLangBtn_Clicked(object sender, EventArgs e)
        {
            if(ChangeLangBtn.Text == "pt-BR")
            {
                Translator.Instance.Culture = new CultureInfo("en-US");
                Translator.Instance.OnPropertyChanged();
            }
            else
            {
                Translator.Instance.Culture = new CultureInfo("pt-BR");
                Translator.Instance.OnPropertyChanged();
            }

            Translator.Instance.SaveCurrentCulture(Translator.Instance.Culture.Name);
        }

        private Task ShowLoadingModalAsync()
        {
            LoadingModal.IsVisible = true;
            return Task.CompletedTask;
        }

        private Task HideLoadingModalAsync()
        {
            LoadingModal.IsVisible = false;
            return Task.CompletedTask;
        }

        private async void OnCreateNewUser(object sender, EventArgs e)
        {
           await Navigation.PushAsync(new CriarEditarUsuariosPage());
        }

        private async void TrocarSenhaButton_Clicked(object sender, EventArgs e)
        {
            var senha = await DisplayAlert(Translator.Instance["changePass"], Translator.Instance["changePassAsk"], Translator.Instance["yes"], Translator.Instance[key: "no"]);
            if (senha)
            {
                var novaSenha = await DisplayPromptAsync(Translator.Instance["changePass"], Translator.Instance["changePassNew"], "OK", Translator.Instance["cancel"], Translator.Instance["newPass"], 16, Keyboard.Default, "");

                Usuario usuario = UsuariosService.GetUsuarioLogado();
                usuario.Senha = novaSenha;

                _usuariosService.UpdateOne(usuario);

                await DisplayAlert(Translator.Instance["passChanged"], Translator.Instance["changePassSuccess"], "OK");
            }
        }
    }
}
