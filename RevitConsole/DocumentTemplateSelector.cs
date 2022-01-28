using System.Windows;
using System.Windows.Controls;

namespace RevitConsole
{
    public class DocumentTemplateSelector : DataTemplateSelector
    {
        public DataTemplate DocumentViewTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if ( item is DocumentViewModel )
                return DocumentViewTemplate;

            return base.SelectTemplate(item, container);
        }
    }
}
