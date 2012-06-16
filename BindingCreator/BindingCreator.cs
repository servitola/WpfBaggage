using System.Windows;
using System.Windows.Data;

namespace WpfBaggage.BindingCreator
{
    public class BindingCreator
    {
        public BindingCreator(string path, FrameworkElement element, string canvasPath, string elementName,
                                DependencyProperty property, IValueConverter converter)
        {
            Path = path;
            Element = element;
            _canvasPath = canvasPath;
            Property = property;
            _converter = converter;
            ElementName = elementName;
        }

        public readonly DependencyProperty Property;
        protected readonly FrameworkElement Element;
        protected readonly string Path;
        protected readonly string ElementName;
        private readonly string _canvasPath;
        private readonly IValueConverter _converter;

        protected BindingCreator(string path, FrameworkElement element, string elementName, DependencyProperty canvasPath)
            : this(path, element, null, elementName, canvasPath, null) { }

        public virtual void SetBinding(object parameter)
        {
            var binding = new Binding(Path)
            {
                ElementName = ElementName,
                Path = new PropertyPath(_canvasPath),
                Converter = _converter,
                ConverterParameter = parameter
            };

            Element.SetBinding(Property, binding);
        }
    }
}
