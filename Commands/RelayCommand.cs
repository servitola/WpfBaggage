using System;
using System.Windows.Input;

namespace ServiWpfTools.Commands
{
    public class RelayCommand : ICommand
    {

        #region Fields

        protected readonly Action<object> _execute;
        readonly Func<bool> _canExecute;

        #endregion Fields

        #region Constructors

        public RelayCommand(Action<object> execute, Func<bool> canExecute = null)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }
        #endregion Constructors

        #region ICommand Members

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        #endregion ICommand Members

        #region Constructors

        protected void OnCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, EventArgs.Empty);
        }

        #endregion // ICommand Members

    }
}
