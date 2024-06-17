using SAGE.Extension; // Importa o namespace para TranslateExtension
using SAGE.Modules.Usuarios; // Importa o namespace para UsuariosService
using System.Globalization; // Importa o namespace para CultureInfo
using Themes = SAGE.Resources.Styles.Themes; // Importa o namespace para Themes e renomeia-o

namespace SAGE.Pages
{
    public partial class ConfigPage : ContentPage
    {
        UsuariosService _usuariosService = new UsuariosService(); // Instância do serviço de usuários

        // Construtor da página de configurações
        public ConfigPage()
        {
            InitializeComponent(); // Inicializa os componentes da página

            // Define o estado da CheckBox ChangeTheme com base no tema selecionado
            ChangeTheme.IsToggled = ThemeManager.SelectedTheme == nameof(Themes.DarkTheme);

            // Verifica a cultura selecionada e marca o RadioButton correspondente
            if (Translator.Instance.Culture.Name == "pt-BR")
            {
                rbPt.IsChecked = true;
            }
            else if (Translator.Instance.Culture.Name == "en-US")
            {
                rbEn.IsChecked = true;
            }
        }

        // Método chamado quando o RadioButton para Português é alterado
        private void RbPt_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (Translator.Instance.Culture.Name != "pt-BR")
            {
                // Define a cultura como Português e notifica a mudança
                Translator.Instance.Culture = new CultureInfo("pt-BR");
                Translator.Instance.OnPropertyChanged();
                MessagingCenter.Send(this, "LanguageChanged"); // Envio de mensagem sobre mudança de idioma
            }

            Translator.Instance.SaveCurrentCulture(Translator.Instance.Culture.Name); // Salva a cultura atual
        }

        // Método chamado quando o RadioButton para Inglês é alterado
        private void RbEn_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (Translator.Instance.Culture.Name != "en-US")
            {
                // Define a cultura como Inglês e notifica a mudança
                Translator.Instance.Culture = new CultureInfo("en-US");
                Translator.Instance.OnPropertyChanged();
                MessagingCenter.Send(this, "LanguageChanged"); // Envio de mensagem sobre mudança de idioma
            }

            Translator.Instance.SaveCurrentCulture(Translator.Instance.Culture.Name); // Salva a cultura atual
        }

        // Método chamado quando a CheckBox ChangeTheme é alternada
        private void ChangeTheme_Toggled(object sender, ToggledEventArgs e)
        {
            // Define o tema com base no estado da CheckBox
            if (e.Value)
            {
                ThemeManager.SetTheme(nameof(Themes.DarkTheme));
            }
            else
            {
                ThemeManager.SetTheme(nameof(Themes.LightTheme));
            }
        }

        // Método chamado quando o botão TrocarSenhaButton é clicado
        private async void TrocarSenhaButton_Clicked(object sender, EventArgs e)
        {
            // Exibe um alerta para confirmar a troca de senha
            var senha = await DisplayAlert(Translator.Instance["changePass"], Translator.Instance["changePassAsk"], Translator.Instance["yes"], Translator.Instance[key: "no"]);

            if (senha)
            {
                // Exibe um prompt para o usuário inserir a nova senha
                var novaSenha = await DisplayPromptAsync(Translator.Instance["changePass"], Translator.Instance["changePassNew"], "OK", Translator.Instance["cancel"], Translator.Instance["newPass"], 16, Keyboard.Default, "");

                // Obtém o usuário logado e atualiza a senha
                Usuario usuario = UsuariosService.GetUsuarioLogado();
                usuario.Senha = novaSenha;

                _usuariosService.UpdateOne(usuario); // Atualiza a senha no banco de dados

                await DisplayAlert(Translator.Instance["passChanged"], Translator.Instance["changePassSuccess"], "OK"); // Exibe um alerta de sucesso
            }
        }
    }
}
