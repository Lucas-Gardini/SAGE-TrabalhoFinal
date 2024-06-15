using SAGE.Resources.i18n;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAGE.Extension
{
    internal class Translator : INotifyPropertyChanged
    {
        public static readonly string LinguaCaminho = Path.Combine(FileSystem.AppDataDirectory, "lingua.txt");

        public string this[string key]
        {
            get => AppResources.ResourceManager.GetString(key, Culture);
        }

        public CultureInfo Culture { get; set; } = new CultureInfo("pt-BR");

        public static Translator Instance { get; set; } = new Translator();

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged()
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(null));
        }

        public void SaveCurrentCulture()
        {
            File.WriteAllText(LinguaCaminho, Culture.Name);
        }

        public void SaveCurrentCulture(string culture)
        {
            File.WriteAllText(LinguaCaminho, culture);
        }

        public string? GetSavedCulture()
        {
            if (File.Exists(LinguaCaminho))
            {
                return File.ReadAllText(LinguaCaminho);
            }

            return null;
        }
    }
}
