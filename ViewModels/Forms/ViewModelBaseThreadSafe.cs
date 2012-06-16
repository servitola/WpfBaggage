using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Threading;

//Jeremy Alles, France - I LOVE THIS MAN
namespace WpfBaggage.ViewModels.Forms
{
	public class ViewModelBaseThreadSafe : INotifyPropertyChanged
	{
		/// <summary>
		/// Raised when a property on this object has a new value.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// A static set of argument instances, one per property name.
        /// </summary>
        /*private static readonly Dictionary<string, PropertyChangedEventArgs> ArgumentInstances = new Dictionary<string, PropertyChangedEventArgs>();*/

		protected ViewModelBaseThreadSafe()
		{
			Dispatcher = Dispatcher.CurrentDispatcher;
		}

		#region Dispatcher

		/// <summary>
		/// Gets the dispatcher used by this view model to execute actions on the thread it is associated with.
		/// </summary>
		/// <value>
		/// The <see cref="System.Windows.Threading.Dispatcher"/> used by this view model to 
		/// execute actions on the thread it is associated with. 
		/// The default value is the <see cref="System.Windows.Threading.Dispatcher.CurrentDispatcher"/>.
		/// </value>
		protected Dispatcher Dispatcher { get; private set; }

		#endregion

		/// <summary>
		/// Warns the developer if this object does not have a public property with
		/// the specified name. This method does not exist in a Release build.
		/// </summary>
		[Conditional("DEBUG")]
		[DebuggerStepThrough]
		public void VerifyPropertyName(string propertyName)
		{
			// verify that the property name matches a real,  
			// public, instance property on this object.
			if (TypeDescriptor.GetProperties(this)[propertyName] == null)
			    Debug.Assert(false,"Invalid property name: " + propertyName);
		}

		/// <summary>
		/// Raises this object's PropertyChanged event.
		/// </summary>
		/// <param name="propertyName">The name of the property that has a new value.</param>
		protected virtual void OnPropertyChanged(string propertyName)
		{
			VerifyPropertyName(propertyName);

			Execute(  () =>	{
								if (PropertyChanged != null)
									PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
							});

			/*/*if (PropertyChanged != null)
			    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));#1#
			
            var handler = PropertyChanged;
            if (handler != null)
            {
                PropertyChangedEventArgs args;
                if (!ArgumentInstances.TryGetValue(propertyName, out args))
                {
                    args = new PropertyChangedEventArgs(propertyName);
                    ArgumentInstances[propertyName] = args;
                }

                // Fire the change event. The smart dispatcher will directly
                // invoke the handler if this change happened on the UI thread,
                // otherwise it is sent to the proper dispatcher.
                SmartDispatcher.BeginInvoke(() => handler(this, args));
            }*/

			PropertyChangedCompleted(propertyName);
		}

		protected virtual void PropertyChangedCompleted(string propertyName)
		{
		}

		/// <summary>
		/// Executes the specified <paramref name="action"/> synchronously on the thread 
		/// the <see cref="ViewModelBaseThreadSafe"/> is associated with.
		/// </summary>
		/// <param name="action">The <see cref="Action"/> to execute.</param>
		protected void Execute(Action action)
		{
			if (Dispatcher.CheckAccess())
				action.Invoke();
			else
				Dispatcher.Invoke(DispatcherPriority.DataBind, action);
		}
	}
}
