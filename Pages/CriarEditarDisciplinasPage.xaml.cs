using SAGE.Modules.Disciplinas; // Importa o namespace para Disciplina
using SAGE.Modules.Usuarios; // Importa o namespace para Usuario
using SAGE.Modules.Generic; // Importa o namespace para GenericService
using SAGE.Extension; // Importa o namespace para TranslateExtension

namespace SAGE.Pages
{
    public partial class CriarEditarDisciplinasPage : ContentPage
    {
        // Servi�o gen�rico para opera��es relacionadas a Disciplina
        private readonly GenericService<Disciplina> _genericService = new();
        // Objeto Disciplina para manipula��o na p�gina
        private Disciplina Disciplina { get; set; }

        // Servi�o gen�rico para opera��es relacionadas a Usuario
        private readonly GenericService<Usuario> _usuariosServiceGeneric = new();
        // Lista de Usuarios para manipula��o na p�gina
        List<Usuario> Usuarios { get; set; } = new();

        // Evento de fechar a p�gina
        public event Action AoFechar;

        // Construtor padr�o da p�gina para criar uma nova disciplina
        public CriarEditarDisciplinasPage()
        {
            InitializeComponent();
            Disciplina = new Disciplina(); // Inicializa um novo objeto Disciplina
            BindingContext = this; // Define o contexto de dados da p�gina como esta inst�ncia
            AoFechar = () => { }; // Inicializa o evento AoFechar
        }

        // Construtor para editar uma disciplina existente
        public CriarEditarDisciplinasPage(int id)
        {
            InitializeComponent();
            Disciplina = _genericService.GetOne(d => d.Id == id)!; // Obt�m a disciplina pelo ID

            var user = UsuariosService.GetUsuarioLogado();

            // Preenche os campos de entrada com os dados da disciplina
            NomeEntry.Text = Disciplina.Nome;
            SiglaEntry.Text = Disciplina.Sigla;
            ProfessorEntry.Text = Disciplina.Professor;
            BindingContext = this; // Define o contexto de dados da p�gina como esta inst�ncia
            AoFechar = () => { }; // Inicializa o evento AoFechar
        }

        // M�todo chamado quando o bot�o de salvar � clicado
        private async void OnSaveClicked(object sender, EventArgs e)
        {
            var user = UsuariosService.GetUsuarioLogado(); // Obt�m o usu�rio logado

            // Preenche os dados da disciplina com os valores dos campos de entrada
            Disciplina.Nome = NomeEntry.Text;
            Disciplina.Sigla = SiglaEntry.Text.ToUpper();
            Disciplina.Professor = ProfessorEntry.Text;
            Disciplina.AlunoId = user.Id;

            // Verifica se os campos obrigat�rios est�o preenchidos
            if (string.IsNullOrWhiteSpace(Disciplina.Nome) || string.IsNullOrWhiteSpace(Disciplina.Sigla) || string.IsNullOrWhiteSpace(Disciplina.Professor))
            {
                await DisplayAlert(Translator.Instance["error"], Translator.Instance["fillFieldsAlert"], "OK");
                return;
            }

			// Verifica se a sigla da disciplina j� existe cadastrada pelo usu�rio logado
			if (_genericService.GetMany(d => d.AlunoId == user.Id && d.Sigla == Disciplina.Sigla).Any(d => d.Id != Disciplina.Id))
			{
				await DisplayAlert(Translator.Instance["error"], Translator.Instance["Sigla"], "OK");
				return;
			}

            try
            {
                if (Disciplina.Id == 0)
                {
                    _genericService.InsertOne(Disciplina); // Adiciona uma nova disciplina
                }
                else
                {
                    _genericService.UpdateOne(Disciplina); // Atualiza a disciplina existente
                }
                AoFechar?.Invoke(); // Invoca o evento de fechamento
                await Navigation.PopModalAsync(); // Fecha a p�gina modal
            }
            catch (Exception ex)
            {
                await DisplayAlert(Translator.Instance["error"], Translator.Instance["failedToSaveDisc"] + $": {ex.Message}", "OK");
            }
        }

        // M�todo chamado quando o bot�o de fechar � clicado
        private async void CloseButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync(); // Fecha a p�gina modal
        }
    }
}
