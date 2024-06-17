using SAGE.Extension; // Importa o namespace para TranslateExtension
using SAGE.Modules.Disciplinas; // Importa o namespace para Disciplina
using SAGE.Modules.Generic; // Importa o namespace para GenericService
using SAGE.Modules.Usuarios; // Importa o namespace para UsuariosService

namespace SAGE.Pages
{
    public partial class NotasPage : ContentPage
    {
        private readonly GenericService<Disciplina> _disciplinaService = new(); // Instância do serviço genérico para Disciplina
        private readonly GenericService<Notas> _notasService = new(); // Instância do serviço genérico para Notas

        private Disciplina Disciplina { get; set; } // Propriedade para armazenar a disciplina atual
        private double Media { get; set; } // Propriedade para armazenar a média das notas

        public List<Notas> Notas { get; set; } = new(); // Lista para armazenar as notas
        public event Action AoFechar; // Evento para fechar a página

        public NotasPage(int id)
        {
            InitializeComponent(); // Inicializa os componentes da página
            Disciplina = _disciplinaService.GetOne(d => d.Id == id)!; // Obtém a disciplina pelo ID
            AoFechar = () => { }; // Inicializa o evento de fechar
            Startup(); // Inicializa a página
        }

        // Método para iniciar a página
        private void Startup()
        {
            var usuario = UsuariosService.GetUsuarioLogado(); // Obtém o usuário logado

            // Obtém as notas da disciplina para o aluno específico
            Notas = _notasService.GetMany(n => n.DisciplinaId == Disciplina.Id && n.AlunoId == usuario.Id);

            // Calcula a média a partir das notas
            foreach (var nota in Notas)
            {
                Media = Notas.Average(n => n.Nota);
            }

            // Se não houver notas, a média é 0
            if (Notas.Count == 0) Media = 0;

            // Define a situação do aluno com base na média
            if (Media >= 6)
                situacaoAlunoLabel.Text = Translator.Instance["approved"]; // Aprovado
            else
                situacaoAlunoLabel.Text = Translator.Instance["disapproved"]; // Reprovado

            // Exibe a média formatada
            MediaLabel.Text = Translator.Instance["averageGrade"] + $": {Media.ToString("0.00")}";

            NotasCollectionView.ItemsSource = Notas; // Define a coleção de notas na CollectionView
        }

        // Método chamado quando o botão Adicionar Nota é clicado
        private async void AddNotaButton_Clicked(object sender, EventArgs e)
        {
            var usuario = UsuariosService.GetUsuarioLogado(); // Obtém o usuário logado

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

                Startup(); // Atualiza a página
            }
            catch (Exception)
            {
                await DisplayAlert(Translator.Instance["error"], Translator.Instance["noAddGrade"], "OK");
            }
        }

        // Método chamado quando um item da lista de notas é tocado
        private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            var tappedEventArgs = (TappedEventArgs)e;
            if (tappedEventArgs.Parameter is int notaId)
            {
                // Pergunta ao usuário se deseja excluir a nota
                var confirm = await DisplayAlert(Translator.Instance["delete"], Translator.Instance["wantDeleteGrade"], Translator.Instance["yes"], Translator.Instance["no"]);

                if (!confirm)
                    return;

                try
                {
                    _notasService.DeleteMany(n => n.Id == notaId); // Exclui a nota
                    Startup(); // Atualiza a página
                }
                catch
                {
                    await DisplayAlert(Translator.Instance["error"], Translator.Instance["noDeleteGrade"], "OK");
                }
            }
        }

        // Método chamado quando um item da lista de notas é carregado
        private void Label_Loaded(object sender, EventArgs e)
        {
            // Define o texto da label com base se é prova ou trabalho
            var label = (Label)sender;
            var nota = (Notas)label.BindingContext;
            label.Text = nota.Prova ? Translator.Instance["test"] : Translator.Instance["homeWork"];
        }

        // Método chamado quando o botão Fechar é clicado
        private async void CloseButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync(); // Fecha a página modal
        }
    }
}
