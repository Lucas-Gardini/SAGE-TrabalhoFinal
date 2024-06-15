using SAGE.Modules.Disciplinas;
using SAGE.Modules.Generic;

namespace SAGE.Pages;

public partial class NotasPage : ContentPage
{
    private readonly GenericService<Disciplina> _disciplinaService = new();
    private readonly GenericService<Notas> _notasService = new();

    private Disciplina Disciplina { get; set; }
    private double Media { get; set; }

    public List<Notas> Notas { get; set; } = new();
    public event Action AoFechar;

    public NotasPage(int id)
    {
        InitializeComponent();
        Disciplina = _disciplinaService.GetOne(d => d.Id == id)!; // Obtém a disciplina pelo ID
        AoFechar = () => { };

        Startup();
    }

    private void Startup()
    {
        Notas = _notasService.GetMany(n => n.DisciplinaId == Disciplina.Id); // Obtém as notas da disciplina

        // Calcula a média a partir das notas
        foreach (var nota in Notas)
        {
            Media = Notas.Average(n => n.Nota); 
        }

        if (Notas.Count == 0) Media = 0; // Se não houver notas, a média é 0

        if (Media >= 6)
            situacaoAlunoLabel.Text = "Aprovado"; // Se a média for maior ou igual a 6, o aluno está aprovado
        else
            situacaoAlunoLabel.Text = "Reprovado"; // Se a média for menor que 6, o aluno está reprovado

        MediaLabel.Text = $"Média: {Media.ToString("0.00")}"; // Exibe a média

        NotasCollectionView.ItemsSource = Notas; // Define a coleção de notas
    }

    private async void AddNotaButton_Clicked(object sender, EventArgs e)
    {
        // Abre um alerta de input e solicita a nota do aluno
        var nota = await DisplayPromptAsync("Nota", "Digite a nota do aluno", "OK", "Cancelar", keyboard: Keyboard.Numeric);
        var isProva = await DisplayAlert("Prova", "A nota é de uma prova?", "Sim", "Não");

        if (nota == null)
            return;

        try
        {
            _notasService.InsertOne(new Notas
            {
                DisciplinaId = Disciplina.Id,
                Nota = Convert.ToDouble(nota),
                Prova = isProva
            });

            Startup();
        } catch (Exception)
        {
            await DisplayAlert("Erro", "Não foi possível adicionar a nota", "OK");
        }
    }
    private async void CloseButton_Clicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync(); // Fecha a página modal
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        var tappedEventArgs = (TappedEventArgs)e;
        if (tappedEventArgs.Parameter is int notaId)
        {
            // Pergunta ao usuário se ele deseja excluir a nota
            var confirm = await DisplayAlert("Excluir", "Deseja excluir a nota?", "Sim", "Não");

            if (!confirm)
                return;

            try
            {
                _notasService.DeleteMany(n => n.Id == notaId); // Exclui a nota
                Startup();
            } catch
            {
                await DisplayAlert("Erro", "Não foi possível excluir a nota", "OK");
            }
        }
    }

    private void Label_Loaded(object sender, EventArgs e)
    {
        // Se for true, retorna "Prova", senão, retorna "Trabalho"
        var label = (Label)sender;
        var nota = (Notas)label.BindingContext;

        label.Text = nota.Prova ? "Prova" : "Trabalho";

    }
}