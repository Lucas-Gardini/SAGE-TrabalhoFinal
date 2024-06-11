using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Themes = SAGE.Resources.Styles.Themes;

namespace SAGE.Extension
{
    public static class ThemeManager
    {
        private static readonly IDictionary<string, ResourceDictionary> _themes = new Dictionary<string, ResourceDictionary>
        {
            [nameof(Themes.LightTheme)] = new Themes.LightTheme(),
            [nameof(Themes.DarkTheme)] = new Themes.DarkTheme(),
        };

        public static string SelectedTheme { get; set; } = nameof(Themes.LightTheme);

        public static void SetTheme(string selectedTheme)
        {
            if(SelectedTheme == selectedTheme) return;

            var themeToBeApplied = _themes[selectedTheme];

            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(themeToBeApplied);

            SelectedTheme = selectedTheme;
        }
    }
}
