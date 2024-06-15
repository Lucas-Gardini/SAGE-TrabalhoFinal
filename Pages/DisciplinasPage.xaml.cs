using SAGE.Modules.Generic;
using SAGE.Modules.Disciplinas;
using SAGE.Modules.Usuarios;
using SAGE.Extension;

namespace SAGE.Pages;

public partial class DisciplinasPage : ContentPage
{
	private readonly GenericService<Disciplina> _disciplinasService = new();
	List<Disciplina> Disciplinas { get; set; } = new();

	private readonly UsuariosService _usuariosService = new();

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
			DisplayAlert(Translator.Instance["error"], Translator.Instance["notDelDisc"], "OK"); // Exibe mensagem de erro
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

	private void OnNotasItem_Clicked(object sender, EventArgs e)
	{
		var menuItem = sender as Button;

		if (menuItem == null)
			return;

		var disciplinaId = menuItem.CommandParameter;

		var modal = new NotasPage(Convert.ToInt32(disciplinaId)); // Cria a página modal para edição

		modal.AoFechar += Startup;

		Navigation.PushModalAsync(modal); // Abre a página modal
	}

	private async void SearchBtn_Clicked(object sender, EventArgs e)
	{
		var searchTerm = await DisplayPromptAsync(Translator.Instance["searchTitle"], Translator.Instance["searchDsc"], "OK", "Cancel", keyboard: Keyboard.Text);

		if (!string.IsNullOrWhiteSpace(searchTerm))
		{
			SearchDisciplinas(searchTerm);
		}
		else
		{
			Startup(); // Se o termo de busca estiver vazio, mostra todas as disciplinas novamente
		}
	}

	/// <summary>
	/// Método para buscar disciplinas com base em um termo de busca.
	/// </summary>
	/// <param name="searchTerm">Termo de busca fornecido pelo usuário.</param>
	private void SearchDisciplinas(string searchTerm)
	{
		var filteredDisciplinas = Disciplinas.Where(d => d.Sigla.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) || d.Nome.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();
		DisciplinasCollectionView.ItemsSource = filteredDisciplinas;
		RefreshBtn.IsVisible = true;
	}

	private void RefreshBtn_Clicked(object sender, EventArgs e)
	{
		Startup();
		RefreshBtn.IsVisible = false;
	}
}