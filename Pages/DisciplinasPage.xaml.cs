using SAGE.Modules.Generic;
using SAGE.Modules.Disciplinas;
using SAGE.Modules.Usuarios;

namespace SAGE.Pages;

public partial class DisciplinasPage : ContentPage
{
	private readonly GenericService<Disciplina> _disciplinasService = new();
	List<Disciplina> Disciplinas { get; set; } = new();

	private readonly UsuariosService _usuariosService = new();
	private Usuario professor { get; set; }

	public DisciplinasPage()
	{
		InitializeComponent();
		Startup();
	}

	/// <summary>
	/// Método de inicialização para carregar as disciplinas.
	/// </summary>
	private void Startup()
	{
		Disciplinas = _disciplinasService.GetMany();
		DisciplinasCollectionView.ItemsSource = Disciplinas;
	}

	private void FabButton_Clicked(object sender, EventArgs e)
	{
		var modal = new CriarEditarDisciplinasPage(); // Cria a página modal para criação de um nova disciplina

		modal.AoFechar += Startup;

		Navigation.PushModalAsync(modal);
	}

	/// <summary>
	/// Método acionado quando o botão de exclusão é clicado.
	/// </summary>
	private void OnDeleteItem_Clicked(object sender, EventArgs e)
	{
		var menuItem = sender as Button;

		if (menuItem == null)
			return;

		var disciplinaId = menuItem.CommandParameter;

		try
		{
			_disciplinasService.DeleteOne(_disciplinasService.GetOne(d => d.Id == Convert.ToInt32(disciplinaId))!); // Exclui a disciplina
		}
		catch
		{
			DisplayAlert("Erro", "Não foi possível excluir a disciplina", "OK"); // Exibe mensagem de erro
		}

		Startup(); // Atualiza a lista de disciplinas
	}

	/// <summary>
	/// Método acionado quando o botão de edição é clicado.
	/// </summary>
	private void OnEditItem_Clicked(object sender, EventArgs e)
	{
		var menuItem = sender as Button;

		if (menuItem == null)
			return;

		var disciplinaId = menuItem.CommandParameter;

		var modal = new CriarEditarDisciplinasPage(Convert.ToInt32(disciplinaId)); // Cria a página modal para edição

		modal.AoFechar += Startup; // Associa o evento de fechamento para atualizar a lista de usuários

		Navigation.PushModalAsync(modal); // Abre a página modal
	}

	/// <summary>
	/// Método acionado quando a label com id do professor é carregada
	/// e a transforma no nome do professor.
	/// </summary>
	private void CarregaNomeProfessor(object sender, EventArgs e)
	{
		var label = sender as Label;

		if (label == null)
			return;

		var disciplinaId = label.BindingContext as Disciplina;

		if (disciplinaId == null)
			return;

		professor = _usuariosService.GetOne(u => u.Id == disciplinaId.ProfessorId); // Busca o professor da disciplina

		// Verifica se o professor foi encontrado
		if (professor != null) {
			label.Text = professor.Nome; // Exibe o nome do professor
		} else {
			label.Text = "Professor não encontrado";
		}
	}

    private void DisciplinasCollectionView_Scrolled(object sender, ItemsViewScrolledEventArgs e)
    {
        // Verifica se o usuário chegou ao final da visualização
        if (e.VerticalDelta > 0 && e.VerticalOffset + DisciplinasCollectionView.Height >= DisciplinasCollectionView.ItemsSource.Cast<object>().Count())
        {
            // Obtém o último item da fonte de itens
            var lastItem = DisciplinasCollectionView.ItemsSource.Cast<object>().LastOrDefault();

            // Rola até o último item
            DisciplinasCollectionView.ScrollTo(lastItem, ScrollToPosition.End, animate: true);
        }
    }
}