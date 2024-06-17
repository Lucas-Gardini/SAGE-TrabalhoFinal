using SAGE.Resources.i18n;  // Importa o namespace para acessar os recursos de internacionalização.
using System;
using System.Collections.Generic;
using System.ComponentModel; // Importa para a interface INotifyPropertyChanged.
using System.Globalization; // Importa para CultureInfo.
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAGE.Extension
{
    internal class Translator : INotifyPropertyChanged  // Declaração da classe Translator que implementa INotifyPropertyChanged.
    {
        // Caminho onde o arquivo de idioma é armazenado.
        public static readonly string LinguaCaminho = Path.Combine(FileSystem.AppDataDirectory, "lingua.txt");

        // Indexador para acessar os recursos de idioma.
        public string this[string key]
        {
            get => AppResources.ResourceManager.GetString(key, Culture);  // Obtém a string traduzida usando a chave e a cultura.
        }

        // Propriedade para armazenar a cultura atual.
        public CultureInfo Culture { get; set; } = new CultureInfo("pt-BR");  // Define a cultura padrão como português brasileiro.

        // Propriedade estática para acessar a instância única do Translator.
        public static Translator Instance { get; set; } = new Translator();  // Cria uma instância única do Translator.

        // Evento PropertyChanged para notificar sobre alterações nas propriedades.
        public event PropertyChangedEventHandler PropertyChanged;

        // Método para invocar o evento PropertyChanged.
        public void OnPropertyChanged()
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(null));  // Invoca o evento PropertyChanged.
        }

        // Método para salvar a cultura atual no arquivo.
        public void SaveCurrentCulture()
        {
            File.WriteAllText(LinguaCaminho, Culture.Name);  // Salva o nome da cultura no arquivo de idioma.
        }

        // Método sobrecarregado para salvar uma cultura específica no arquivo.
        public void SaveCurrentCulture(string culture)
        {
            File.WriteAllText(LinguaCaminho, culture);  // Salva a cultura especificada no arquivo de idioma.
        }

        // Método para obter a cultura salva anteriormente.
        public string? GetSavedCulture()
        {
            if (File.Exists(LinguaCaminho))
            {
                return File.ReadAllText(LinguaCaminho);  // Retorna a cultura salva no arquivo de idioma.
            }

            return null;  // Retorna null se o arquivo de idioma não existe.
        }
    }
}
