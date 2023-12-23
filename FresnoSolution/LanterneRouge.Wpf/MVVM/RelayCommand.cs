using System;
using System.Diagnostics;
using System.Windows.Input;

namespace LanterneRouge.Wpf.MVVM
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RelayCommand"/> class.
    /// </summary>
    /// <param name="execute">The execute.</param>
    /// <param name="canExecute">The can execute.</param>
    public class RelayCommand(Action<object> execute, Predicate<object> canExecute) : ICommand
    {
        #region Fields

        private readonly Action<object> _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        private readonly Predicate<object> _canExecute = canExecute;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="execute">The execute.</param>
        public RelayCommand(Action<object> execute)
            : this(execute, null)
        { }

        #endregion

        #region ICommand Members

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        [DebuggerStepThrough]
        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        public void Execute(object parameter) => _execute(parameter);

        #endregion // ICommand Members
    }
}
