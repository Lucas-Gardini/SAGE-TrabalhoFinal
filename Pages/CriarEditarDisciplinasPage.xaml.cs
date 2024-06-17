using SAGE.Modules.Disciplinas; // Importa o namespace para Disciplina
using SAGE.Modules.Usuarios; // Importa o namespace para Usuario
using SAGE.Modules.Generic; // Importa o namespace para GenericService
using SAGE.Extension; // Importa o namespace para TranslateExtension

namespace SAGE.Pages
{
    public partial class CriarEditarDisciplinasPage : ContentPage
    {
        // Serviço genérico para operações relacionadas a Disciplina
        private readonly GenericService<Disciplina> _genericService = new();
        // Objeto Disciplina para manipulação na página
        private Disciplina Disciplina { get; set; }

        // Serviço genérico para operações relacionadas a Usuario
        private readonly GenericService<Usuario> _usuariosServiceGeneric = new();
        // Lista de Usuarios para manipulação na página
        List<Usuario> Usuarios { get; set; } = new();

        // Evento de fechar a página
        public event Action AoFechar;

        // Construtor padrão da página para criar uma nova disciplina
        public CriarEditarDisciplinasPage()
        {
            InitializeComponent();
            Disciplina = new Disciplina(); // Inicializa um novo objeto Disciplina
            BindingContext = this; // Define o contexto de dados da página como esta instância
            AoFechar = () => { }; // Inicializa o evento AoFechar
        }

        // Construtor para editar uma disciplina existente
        public CriarEditarDisciplinasPage(int id)
        {
            InitializeComponent();
            Disciplina = _genericService.GetOne(d => d.Id == id)!; // Obtém a disciplina pelo ID

            var user = UsuariosService.GetUsuarioLogado();

            // Preenche os campos de entrada com os dados da disciplina
            NomeEntry.Text = Disciplina.Nome;
            SiglaEntry.Text = Disciplina.Sigla;
            ProfessorEntry.Text = Disciplina.Professor;
            BindingContext = this; // Define o contexto de dados da página como esta instância
            AoFechar = () => { }; // Inicializa o evento AoFechar
        }

        // Método chamado quando o botão de salvar é clicado
        private async void OnSaveClicked(object sender, EventArgs e)
        {
            var user = UsuariosService.GetUsuarioLogado(); // Obtém o usuário logado

            // Preenche os dados da disciplina com os valores dos campos de entrada
            Disciplina.Nome = NomeEntry.Text;
            Disciplina.Sigla = SiglaEntry.Text.ToUpper();
            Disciplina.Professor = ProfessorEntry.Text;
            Disciplina.AlunoId = user.Id;

            // Verifica se os campos obrigatórios estão preenchidos
            if (string.IsNullOrWhiteSpace(Disciplina.Nome) || string.IsNullOrWhiteSpace(Disciplina.Sigla) || string.IsNullOrWhiteSpace(Disciplina.Professor))
            {
                await DisplayAlert(Translator.Instance["error"], Translator.Instance["fillFieldsAlert"], "OK");
                return;
            }

			// Verifica se a sigla da disciplina já existe cadastrada pelo usuário logado
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
                await Navigation.PopModalAsync(); // Fecha a página modal
            }
            catch (Exception ex)
            {
                await DisplayAlert(Translator.Instance["error"], Translator.Instance["failedToSaveDisc"] + $": {ex.Message}", "OK");
            }
        }

        // Método chamado quando o botão de fechar é clicado
        private async void CloseButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync(); // Fecha a página modal
        }
    }
}
