using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Presto.MvvmTools;
using System.Windows.Input;

namespace PrestoViewModel
{
    /// <summary>
    /// Base view model class for the tasks.
    /// </summary>
    public class TaskViewModel : ViewModelBase
    {
        private RelayCommand  cancelCommand;

        /// <summary>
        /// 
        /// </summary>
        public int TaskItemId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TaskGroupId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int TaskTypeId { get; set; }

        public TaskViewModel( IWindowLoader windowLoader ) : base( windowLoader )
        {}

        /// <summary>
        /// 
        /// </summary>
        public ICommand CancelCommand
        {
            get
            {
                if( this.cancelCommand == null )
                {
                    this.cancelCommand = new RelayCommand( param => true, param => CloseDialog() );
                }
                return this.cancelCommand;
            }            
        }

        private void CloseDialog()
        {
            base.OnRequestClose();
        }
    }
}
