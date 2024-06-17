using SAGE.Extension; // Importa o namespace para TranslateExtension
using SAGE.Modules.Disciplinas; // Importa o namespace para Disciplina
using SAGE.Modules.Generic; // Importa o namespace para GenericService
using SAGE.Modules.Usuarios; // Importa o namespace para UsuariosService

namespace SAGE.Pages
{
    public partial class NotasPage : ContentPage
    {
        private readonly GenericService<Disciplina> _disciplinaService = new(); // Inst�ncia do servi�o gen�rico para Disciplina
        private readonly GenericService<Notas> _notasService = new(); // Inst�ncia do servi�o gen�rico para Notas

        private Disciplina Disciplina { get; set; } // Propriedade para armazenar a disciplina atual
        private double Media { get; set; } // Propriedade para armazenar a m�dia das notas

        public List<Notas> Notas { get; set; } = new(); // Lista para armazenar as notas
        public event Action AoFechar; // Evento para fechar a p�gina

        public NotasPage(int id)
        {
            InitializeComponent(); // Inicializa os componentes da p�gina
            Disciplina = _disciplinaService.GetOne(d => d.Id == id)!; // Obt�m a disciplina pelo ID
            AoFechar = () => { }; // Inicializa o evento de fechar
            Startup(); // Inicializa a p�gina
        }

        // M�todo para iniciar a p�gina
        private void Startup()
        {
            var usuario = UsuariosService.GetUsuarioLogado(); // Obt�m o usu�rio logado

            // Obt�m as notas da disciplina para o aluno espec�fico
            Notas = _notasService.GetMany(n => n.DisciplinaId == Disciplina.Id && n.AlunoId == usuario.Id);

            // Calcula a m�dia a partir das notas
            foreach (var nota in Notas)
            {
                Media = Notas.Average(n => n.Nota);
            }

            // Se n�o houver notas, a m�dia � 0
            if (Notas.Count == 0) Media = 0;

            // Define a situa��o do aluno com base na m�dia
            if (Media >= 6)
                situacaoAlunoLabel.Text = Translator.Instance["approved"]; // Aprovado
            else
                situacaoAlunoLabel.Text = Translator.Instance["disapproved"]; // Reprovado

            // Exibe a m�dia formatada
            MediaLabel.Text = Translator.Instance["averageGrade"] + $": {Media.ToString("0.00")}";

            NotasCollectionView.ItemsSource = Notas; // Define a cole��o de notas na CollectionView
        }

        // M�todo chamado quando o bot�o Adicionar Nota � clicado
        private async void AddNotaButton_Clicked(object sender, EventArgs e)
        {
            var usuario = UsuariosService.GetUsuarioLogado(); // Obt�m o usu�rio logado

            // Abre um alerta para inserir a nota do aluno
            var nota = await DisplayPromptAsync(Translator.Instance["Grade"], Translator.Instance["entryGrade"], "OK", Translator.Instance["cancel"], keyboard: Keyboard.Numeric);
            var isProva = await DisplayAlert(Translator.Instance["test"], Translator.Instance["isTest"], Translator.Instance["yes"], Translator.Instance["no"]);

            if (nota == null)
                return;

            try
            {
                // Insere uma nova nota
                _notasService.InsertOne(new Notas
                {
                    DisciplinaId = Disciplina.Id,
                    Nota = Convert.ToDouble(nota),
                    Prova = isProva,
                    AlunoId = usuario.Id,
                });

                Startup(); // Atualiza a p�gina
            }
            catch (Exception)
            {
                await DisplayAlert(Translator.Instance["error"], Translator.Instance["noAddGrade"], "OK");
            }
        }

        // M�todo chamado quando um item da lista de notas � tocado
        private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            var tappedEventArgs = (TappedEventArgs)e;
            if (tappedEventArgs.Parameter is int notaId)
            {
                // Pergunta ao usu�rio se deseja excluir a nota
                var confirm = await DisplayAlert(Translator.Instance["delete"], Translator.Instance["wantDeleteGrade"], Translator.Instance["yes"], Translator.Instance["no"]);

                if (!confirm)
                    return;

                try
                {
                    _notasService.DeleteMany(n => n.Id == notaId); // Exclui a nota
                    Startup(); // Atualiza a p�gina
                }
                catch
                {
                    await DisplayAlert(Translator.Instance["error"], Translator.Instance["noDeleteGrade"], "OK");
                }
            }
        }

        // M�todo chamado quando um item da lista de notas � carregado
        private void Label_Loaded(object sender, EventArgs e)
        {
            // Define o texto da label com base se � prova ou trabalho
            var label = (Label)sender;
            var nota = (Notas)label.BindingContext;
            label.Text = nota.Prova ? Translator.Instance["test"] : Translator.Instance["homeWork"];
        }

        // M�todo chamado quando o bot�o Fechar � clicado
        private async void CloseButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync(); // Fecha a p�gina modal
        }
    }
}
