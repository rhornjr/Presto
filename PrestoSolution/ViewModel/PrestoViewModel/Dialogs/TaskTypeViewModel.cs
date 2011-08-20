using Presto.MvvmTools;
using PrestoCore.BusinessLogic.BusinessEntities;
using PrestoCore.BusinessLogic.BusinessComponents;
using System.Windows.Input;

namespace PrestoViewModel.Dialogs
{
    public class TaskTypeViewModel : ViewModelBase
    {
        private TaskType taskType;
        private RelayCommand okCommand;

        public TaskType TaskType
        {
            get
            {
                if( taskType == null )
                {
                    taskType = new TaskType();
                }
                return taskType;
            }
            set
            {
                taskType = value;
                OnPropertyChanged( "TaskType" );
            }
        }

        public ICommand OkCommand
        {
            get
            {
                if( okCommand == null )
                {
                    okCommand = new RelayCommand( param => CanAddTaskType(), param => SaveTaskType() );
                }
                return okCommand;
            }
        }        

        public TaskTypeViewModel( IWindowLoader windowLoader ) : base( windowLoader )
        {
            this.DisplayName = "Task Types";
        }

        private void SaveTaskType()
        {
            TaskTypeLogic.Save( this.TaskType );
            base.OnRequestClose();
        }

        private bool CanAddTaskType()
        {
            return true;
        }
    }
}
