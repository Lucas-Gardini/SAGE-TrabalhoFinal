using SAGE.Modules.Disciplinas;
using SAGE.Modules.Usuarios;
using SAGE.Modules.Generic;
using SAGE.Extension;

namespace SAGE.Pages;

public partial class CriarEditarDisciplinasPage : ContentPage
{
	private readonly GenericService<Disciplina> _genericService = new();
	private Disciplina Disciplina { get; set; }

	private readonly GenericService<Usuario> _usuariosServiceGeneric = new();
	List<Usuario> Usuarios { get; set; } = new();

	public event Action AoFechar;

	public CriarEditarDisciplinasPage()
	{
		InitializeComponent();
		Disciplina = new Disciplina();
		BindingContext = this;
		AoFechar = () => { };
	}

	public CriarEditarDisciplinasPage(int id)
	{
		InitializeComponent();
		Disciplina = _genericService.GetOne(d => d.Id == id)!; // Obtém a disciplina pelo ID
		NomeEntry.Text = Disciplina.Nome;
		SiglaEntry.Text = Disciplina.Sigla;
		BindingContext = this;
		AoFechar = () => { };
	}
	private async void OnSaveClicked(object sender, EventArgs e)
	{
		Disciplina.Nome = NomeEntry.Text;
		Disciplina.Sigla = SiglaEntry.Text.ToUpper();
		Disciplina.Professor = ProfessorEntry.Text;

        if (string.IsNullOrWhiteSpace(Disciplina.Nome) || string.IsNullOrWhiteSpace(Disciplina.Sigla) || string.IsNullOrWhiteSpace(Disciplina.Professor))
        {
			await DisplayAlert(Translator.Instance["error"], Translator.Instance["fillFieldsAlert"], "OK");
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
			await DisplayAlert(Translator.Instance["error"], Translator.Instance["failedToSaveDisc"] +  $": {ex.Message}", "OK");
		}
	}

	private async void CloseButton_Clicked(object sender, EventArgs e)
	{
		await Navigation.PopModalAsync(); // Fecha a página modal
	}
}