using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace WpfBaggage.BindingCreator
{
    public class MultiBindingCreator : BindingCreator
    {
        public MultiBindingCreator(string path, FrameworkElement element, List<string> canvasPaths, string elementName,
                                    DependencyProperty property, IMultiValueConverter converter)
            : base(path, element, elementName,property)
        {
            _canvasPaths = canvasPaths;
            _converter = converter;
        }

        private readonly List<string> _canvasPaths;
        private readonly IMultiValueConverter _converter;

        public override void SetBinding(object parameter)
        {
            //генерим биндинги
            var bindings = _canvasPaths.Select(t => new Binding(Path) { Path = new PropertyPath(t), ElementName = ElementName, }).ToList();

            var multibinding = new MultiBinding
            {
                Converter = _converter,
                ConverterParameter = parameter,
            };

            foreach (var binding in bindings)
                multibinding.Bindings.Add(binding);

            Element.SetBinding(Property, multibinding);
        }
    }
}
