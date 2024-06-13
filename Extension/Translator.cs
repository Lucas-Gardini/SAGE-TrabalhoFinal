﻿using SAGE.Resources.i18n;
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
        public string this[string key]
        {
            get => AppResources.ResourceManager.GetString(key, Culture);
        }

        public CultureInfo Culture { get; set; } = new CultureInfo("");

        public static Translator Instance { get; set; } = new Translator();

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged()
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(null));
        }
    }
}
