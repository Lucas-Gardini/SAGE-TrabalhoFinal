using SAGE.Modules.Disciplinas;
using SAGE.Modules.Usuarios;
using SAGE.Modules.Generic;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Platform;

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
		PickerProfessores(); // Carrega os professores no Picker
		BindingContext = this;
		AoFechar = () => { };
	}

	public CriarEditarDisciplinasPage(int id)
	{
		InitializeComponent();
		Disciplina = _genericService.GetOne(d => d.Id == id)!; // Obtém a disciplina pelo ID
		PickerProfessores(); // Carrega os professores no Picker
		NomeEntry.Text = Disciplina.Nome;
		SiglaEntry.Text = Disciplina.Sigla;
		BindingContext = this;
		AoFechar = () => { };
	}

	public void PickerProfessores() 
	{
		Usuarios = _usuariosServiceGeneric.GetMany();

		// Limpa os itens existentes no Picker
		ProfessoresPicker.ItemsSource = null;

		// Adiciona os nomes dos usuários ao Picker
		foreach (var usuario in Usuarios)
		{
			ProfessoresPicker.Items.Add(usuario.Nome);

			if (Disciplina.ProfessorId == usuario.Id)
			{
				ProfessoresPicker.SelectedIndex = Usuarios.IndexOf(usuario);
			}
		}
	}

	private async void OnSaveClicked(object sender, EventArgs e)
	{
		Disciplina.Nome = NomeEntry.Text;
		Disciplina.Sigla = SiglaEntry.Text.ToUpper();

		if (string.IsNullOrWhiteSpace(Disciplina.Nome) || string.IsNullOrWhiteSpace(Disciplina.Sigla) || ProfessoresPicker.SelectedIndex == -1)
		{
			await DisplayAlert("Erro", "Por favor, preencha todos os campos obrigatórios.", "OK");
			return;
		}

		// Obtém o ID do professor selecionado
		string professorNome = ProfessoresPicker.SelectedItem.ToString();
		Disciplina.ProfessorId = _usuariosServiceGeneric.GetOne(u => u.Nome == professorNome)?.Id ?? 0;

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
			await DisplayAlert("Sucesso", "Disciplina salva com sucesso!", "OK");
			AoFechar?.Invoke(); // Invoca o evento de fechamento
			await Navigation.PopModalAsync(); // Fecha a página modal
		}
		catch (Exception ex)
		{
			await DisplayAlert("Erro", $"Falha ao salvar disciplina: {ex.Message}", "OK");
		}
	}

	private async void CloseButton_Clicked(object sender, EventArgs e)
	{
		await Navigation.PopModalAsync(); // Fecha a página modal
	}
}