using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAGE.Extension
{
    // Define uma extensão de marcação personalizada para fornecer suporte a tradução nas views do XAML.
    internal class TranslateExtansion : IMarkupExtension
    {
        // Propriedade que representa a chave do recurso de tradução.
        public string Key { get; set; }

        // Método que fornece o valor para a propriedade de destino.
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            // Cria uma nova instância de Binding (ligação de dados).
            var binding = new Binding
            {
                // Define o modo de binding como OneWay, ou seja, apenas do modelo para a view.
                Mode = BindingMode.OneWay,

                // Define o caminho do binding usando a chave fornecida.
                Path = $"[{Key}]",

                // Define a fonte do binding como a instância do Translator.
                Source = Translator.Instance,
            };

            // Retorna o objeto de binding configurado.
            return binding;
        }
    }
}
