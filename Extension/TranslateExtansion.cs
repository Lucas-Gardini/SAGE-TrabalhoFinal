using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAGE.Extension
{
    internal class TranslateExtansion : IMarkupExtension
    {
        public string Key { get ; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            var binding = new Binding
            {
                Mode = BindingMode.OneWay,
                Path = $"[{Key}]",
                Source = Translator.Instance,
            };
            return binding;
        }
    }
}
