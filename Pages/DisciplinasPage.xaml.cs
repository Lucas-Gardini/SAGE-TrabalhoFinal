using SAGE.Modules.Generic;
using SAGE.Modules.Disciplinas;
using SAGE.Modules.Usuarios;

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
	/// M�todo de inicializa��o para carregar as disciplinas.
	/// </summary>
	private void Startup()
	{
		Disciplinas = _disciplinasService.GetMany();
		DisciplinasCollectionView.ItemsSource = Disciplinas;
	}

	private void FabButton_Clicked(object sender, EventArgs e)
	{
		var modal = new CriarEditarDisciplinasPage(); // Cria a p�gina modal para cria��o de um nova disciplina

		modal.AoFechar += Startup;

		Navigation.PushModalAsync(modal);
	}

	/// <summary>
	/// M�todo acionado quando o bot�o de exclus�o � clicado.
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
			DisplayAlert("Erro", "N�o foi poss�vel excluir a disciplina", "OK"); // Exibe mensagem de erro
		}

		Startup(); // Atualiza a lista de disciplinas
	}

	/// <summary>
	/// M�todo acionado quando o bot�o de edi��o � clicado.
	/// </summary>
	private void OnEditItem_Clicked(object sender, EventArgs e)
	{
		var menuItem = sender as Button;

		if (menuItem == null)
			return;

		var disciplinaId = menuItem.CommandParameter;

		var modal = new CriarEditarDisciplinasPage(Convert.ToInt32(disciplinaId)); // Cria a p�gina modal para edi��o

		modal.AoFechar += Startup; // Associa o evento de fechamento para atualizar a lista de usu�rios

		Navigation.PushModalAsync(modal); // Abre a p�gina modal
	}

	private void OnNotasItem_Clicked(object sender, EventArgs e)
	{
		var menuItem = sender as Button;

        if (menuItem == null)
            return;

        var disciplinaId = menuItem.CommandParameter;

        var modal = new NotasPage(Convert.ToInt32(disciplinaId)); // Cria a p�gina modal para edi��o

		modal.AoFechar += Startup;

        Navigation.PushModalAsync(modal); // Abre a p�gina modal
    }

    private void DisciplinasCollectionView_Scrolled(object sender, ItemsViewScrolledEventArgs e)
    {
        // Verifica se o usu�rio chegou ao final da visualiza��o
        if (e.VerticalDelta > 0 && e.VerticalOffset + DisciplinasCollectionView.Height >= DisciplinasCollectionView.ItemsSource.Cast<object>().Count())
        {
            // Obt�m o �ltimo item da fonte de itens
            var lastItem = DisciplinasCollectionView.ItemsSource.Cast<object>().LastOrDefault();

            // Rola at� o �ltimo item
            DisciplinasCollectionView.ScrollTo(lastItem, ScrollToPosition.End, animate: true);
        }
    }
}