using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Themes = SAGE.Resources.Styles.Themes;

namespace SAGE.Extension
{
    /// <summary>
    /// Classe gerenciadora dos temas da aplicação.
    /// </summary>
    public static class ThemeManager
    {
        /// <summary>
        /// Caminho onde fica salvo o arquivo de lembrar usuário.
        /// </summary>
        public static readonly string TemaCaminho = Path.Combine(FileSystem.AppDataDirectory, "tema.txt");

        private static readonly IDictionary<string, ResourceDictionary> _themes = new Dictionary<string, ResourceDictionary>
        {
            [nameof(Themes.LightTheme)] = new Themes.LightTheme(),
            [nameof(Themes.DarkTheme)] = new Themes.DarkTheme(),
        };

        /// <summary>
        /// Tema selecionado atualmente.
        /// </summary>
        public static string SelectedTheme { get; set; } = nameof(Themes.LightTheme);

        /// <summary>
        /// Carrega o tema atual do arquivo.
        /// </summary>
        public static void LoadCurrentTheme()
        {
            if (File.Exists(TemaCaminho))
            {
                var tema = File.ReadAllText(TemaCaminho);
                SetTheme(tema);
            }
        }

        /// <summary>
        /// Define o tema selecionado.
        /// </summary>
        /// <param name="selectedTheme">Nome do tema a ser aplicado.</param>
        public static void SetTheme(string selectedTheme)
        {
            if (SelectedTheme == selectedTheme) return;

            var themeToBeApplied = _themes[selectedTheme];

            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(themeToBeApplied);

            SelectedTheme = selectedTheme;

            SaveCurrentTheme();
        }

        /// <summary>
        /// Salva o tema atual no arquivo.
        /// </summary>
        public static void SaveCurrentTheme()
        {
            File.WriteAllText(TemaCaminho, SelectedTheme);
        }
    }
}
