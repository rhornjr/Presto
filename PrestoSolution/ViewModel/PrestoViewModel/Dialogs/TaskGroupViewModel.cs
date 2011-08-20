using System.Windows.Input;
using Presto.MvvmTools;
using PrestoCore.BusinessLogic.BusinessEntities;
using PrestoCore.BusinessLogic.BusinessComponents;
using PrestoViewModel.Tabs;

namespace PrestoViewModel.Dialogs
{
    public class TaskGroupViewModel : ViewModelBase
    {
        private TaskGroup    taskGroup;
        private RelayCommand okCommand;
        private RelayCommand cancelCommand;

        public TaskGroup TaskGroup
        {
            get
            {
                if( taskGroup == null )
                {
                    taskGroup = new TaskGroup();
                }
                return taskGroup;
            }
            set
            {
                taskGroup = value;
                OnPropertyChanged( "TaskGroup" );
            }
        }

        public ICommand OkCommand
        {
            get
            {
                if( okCommand == null )
                {
                    okCommand = new RelayCommand( param => CanAddTaskGroup(), param => SaveTaskGroup() );
                }
                return okCommand;
            }
        }

        public ICommand CancelCommand
        {
            get
            {
                if( cancelCommand == null )
                {
                    cancelCommand = new RelayCommand( param => CanCancel(), param => Cancel() );
                }
                return cancelCommand;
            }
        }

        public TaskGroupViewModel( IWindowLoader windowLoader ) : base( windowLoader )
        {            
        }

        private bool CanAddTaskGroup()
        {
            //if( string.IsNullOrEmpty( this.TaskGroup.TaskGroupName ) ) { return false; }

            return true;
        }

        private void SaveTaskGroup()
        {
            TaskGroupListViewModel taskGroupListViewModel = base.Parent as TaskGroupListViewModel;

            taskGroupListViewModel.SaveTaskGroup( this.TaskGroup );
            //TaskGroupLogic.Save( this.TaskGroup );

            base.OnRequestClose();
        }

        private bool CanCancel()
        {
            return true;
        }

        private void Cancel()
        {
            base.OnRequestClose();
        }
    }
}
