using System;
using System.Windows.Input;

[assembly: CLSCompliant( true )]
namespace Presto.MvvmTools
{
    /// <summary>
    /// 
    /// </summary>
    public class RelayCommand : ICommand
    {
        #region [ Member Fields ]

        readonly Action<object> execute;
        readonly Predicate<object> canExecute;

        #endregion [ Member Fields ]

        #region [ Constructors ]

        /// <summary>
        /// 
        /// </summary>
        /// <param name="execute"></param>
        public RelayCommand(Action<object> execute) : this(null, execute) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="canExecute"></param>
        /// <param name="execute"></param>
        public RelayCommand(Predicate<object> canExecute, Action<object> execute)
        {
            if (execute == null) throw new ArgumentNullException("execute");

            this.execute = execute;
            this.canExecute = canExecute;
        }

        #endregion [ Constructors ]

        #region [ ICommand Members ]

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return this.canExecute == null ? true : this.canExecute(parameter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            this.execute(parameter);
        }

        #endregion [ ICommand Members ]
    }
}
