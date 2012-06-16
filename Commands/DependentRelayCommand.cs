using System;
using System.ComponentModel;
using System.Linq;
using ServiWpfTools.Commands;

namespace WpfBaggage.Commands
{
	public class DependentRelayCommand : RelayCommand
	{
		public DependentRelayCommand(Action<object> execute, Func<bool> canExecute, params INotifyPropertyChanged[] changeableObjects)
			: base(execute, canExecute)
		{
			if (changeableObjects == null)
				return;

			foreach (var changeableObject in changeableObjects.Where(item => item != null))
				changeableObject.PropertyChanged += ChangeableObjectPropertyChanged;
		}

		public void AddChangeableObject(INotifyPropertyChanged changeableObject)
		{
			changeableObject.PropertyChanged += ChangeableObjectPropertyChanged;
		}

		public void RemoveChangeableObject(INotifyPropertyChanged changeableObject)
		{
			changeableObject.PropertyChanged -= ChangeableObjectPropertyChanged;
		}

		private void ChangeableObjectPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			OnCanExecuteChanged();
		}
	}
}
