using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace WpfBaggage.Commands
{
	public class LoadFormDependentRelayCommandWrapper
	{
		public LoadFormDependentRelayCommandWrapper(Type formType, Func<bool> canExecute = null, params INotifyPropertyChanged[] changeableObjects)
		{
			var constructor = formType.GetConstructors().First();
			Command = new DependentRelayCommand(parameter =>
			          	{
							if (_form == null)
							{
								_form = (Window) constructor.Invoke(new object[] { });
								_form.Show();
							}
							else
								_form.Focus();

							_form.Closed += (o, e) => { _form = null; };
			          	},canExecute,changeableObjects);
		}

		public  DependentRelayCommand Command;
		private Window _form;
	}
}
