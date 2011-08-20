using System;
using System.ComponentModel;
using System.Windows.Input;

namespace Presto.MvvmTools
{
    /// <summary>
    /// 
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged
    {        
        # region [ Constructor ]

        /// <summary>
        /// 
        /// </summary>
        /// <param name="windowLoader"></param>
        public ViewModelBase(IWindowLoader windowLoader)
        {
            this.WindowLoader = windowLoader;
        }

        # endregion [ Constructor ]

        # region [ Close Command ]

        RelayCommand _closeCommand;

        /// <summary>
        /// Returns the command that, when invoked, attempts
        /// to remove this workspace from the user interface.
        /// </summary>
        public ICommand CloseCommand
        {
            get
            {
                if (_closeCommand == null)
                    _closeCommand = new RelayCommand(param => this.OnRequestClose());

                return _closeCommand;
            }
        }

        # endregion  [ Close Command ]

        # region [ RequestClose Event ]

        /// <summary>
        /// Raised when this workspace should be removed from the UI.
        /// </summary>
        public event EventHandler RequestClose;

        /// <summary>
        /// 
        /// </summary>
        public void OnRequestClose()
        {
            EventHandler handler = this.RequestClose;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        # endregion  [ RequestClose Event ]

        # region [ PropertyChanged Event ]

        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        # endregion [ Property Changed ]

        #region [ Public Properties ]

        /// <summary>
        /// A reference to the view model instance that created this instance.
        /// </summary>
        public ViewModelBase Parent { get; set; }

        /// <summary>
        /// An IWindowLoader provides methods that take a view model and, based
        /// on its type, will resolve the appropriate view, set the view's 
        /// DataContext to the view model and then display in the view in a
        /// new window.
        /// </summary>
        public IWindowLoader WindowLoader { get; set; }

        /// <summary>
        /// A user-friendly name for this object that can be used for anything
        /// like window titles, tab captions, etc.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ViewModelBase MainWindowViewModel { get; set; }

        #endregion // DisplayName
    }
}
