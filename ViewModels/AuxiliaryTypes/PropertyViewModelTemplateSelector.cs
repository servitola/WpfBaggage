using System.Windows;
using System.Windows.Controls;
using WpfBaggage.ViewModels.Properties;

namespace WpfBaggage.ViewModels.AuxiliaryTypes
{
	public class PropertiesDataTemplateSelector : DataTemplateSelector
	{
		public override DataTemplate SelectTemplate(object item, DependencyObject container)
		{
			var element = container as FrameworkElement;

			if (element != null && item is PropertyViewModelBase)
				return ((PropertyViewModelBase)item).EditorTemplate;

			return null;
		}
	}
}
