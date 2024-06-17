using SAGE.Extension; // Importa o namespace para TranslateExtension
using SAGE.Modules.Usuarios; // Importa o namespace para UsuariosService
using System.Globalization; // Importa o namespace para CultureInfo
using Themes = SAGE.Resources.Styles.Themes; // Importa o namespace para Themes e renomeia-o

namespace SAGE.Pages
{
    public partial class ConfigPage : ContentPage
    {
        UsuariosService _usuariosService = new UsuariosService(); // Inst�ncia do servi�o de usu�rios

        // Construtor da p�gina de configura��es
        public ConfigPage()
        {
            InitializeComponent(); // Inicializa os componentes da p�gina

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

        // M�todo chamado quando o RadioButton para Portugu�s � alterado
        private void RbPt_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (Translator.Instance.Culture.Name != "pt-BR")
            {
                // Define a cultura como Portugu�s e notifica a mudan�a
                Translator.Instance.Culture = new CultureInfo("pt-BR");
                Translator.Instance.OnPropertyChanged();
                MessagingCenter.Send(this, "LanguageChanged"); // Envio de mensagem sobre mudan�a de idioma
            }

            Translator.Instance.SaveCurrentCulture(Translator.Instance.Culture.Name); // Salva a cultura atual
        }

        // M�todo chamado quando o RadioButton para Ingl�s � alterado
        private void RbEn_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (Translator.Instance.Culture.Name != "en-US")
            {
                // Define a cultura como Ingl�s e notifica a mudan�a
                Translator.Instance.Culture = new CultureInfo("en-US");
                Translator.Instance.OnPropertyChanged();
                MessagingCenter.Send(this, "LanguageChanged"); // Envio de mensagem sobre mudan�a de idioma
            }

            Translator.Instance.SaveCurrentCulture(Translator.Instance.Culture.Name); // Salva a cultura atual
        }

        // M�todo chamado quando a CheckBox ChangeTheme � alternada
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

        // M�todo chamado quando o bot�o TrocarSenhaButton � clicado
        private async void TrocarSenhaButton_Clicked(object sender, EventArgs e)
        {
            // Exibe um alerta para confirmar a troca de senha
            var senha = await DisplayAlert(Translator.Instance["changePass"], Translator.Instance["changePassAsk"], Translator.Instance["yes"], Translator.Instance[key: "no"]);

            if (senha)
            {
                // Exibe um prompt para o usu�rio inserir a nova senha
                var novaSenha = await DisplayPromptAsync(Translator.Instance["changePass"], Translator.Instance["changePassNew"], "OK", Translator.Instance["cancel"], Translator.Instance["newPass"], 16, Keyboard.Default, "");

                // Obt�m o usu�rio logado e atualiza a senha
                Usuario usuario = UsuariosService.GetUsuarioLogado();
                usuario.Senha = novaSenha;

                _usuariosService.UpdateOne(usuario); // Atualiza a senha no banco de dados

                await DisplayAlert(Translator.Instance["passChanged"], Translator.Instance["changePassSuccess"], "OK"); // Exibe um alerta de sucesso
            }
        }
    }
}
