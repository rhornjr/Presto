using System.Windows.Input;
using Presto.MvvmTools;
using PrestoCore.BusinessLogic.BusinessComponents;
using PrestoCore.BusinessLogic.BusinessEntities;

namespace PrestoViewModel.Dialogs
{
    public class TaskDosCommandViewModel : TaskViewModel
    {
        private RelayCommand   okCommand;
        private TaskDosCommand taskDosCommand;
        private bool           taskDosCommandCreated;

        public TaskDosCommand TaskDosCommand
        {
            get
            {
                if( taskDosCommand == null && this.TaskItemId == 0 )
                {
                    taskDosCommand = new TaskDosCommand();                    
                }

                if( taskDosCommandCreated == false && this.TaskItemId > 0 )
                {
                    taskDosCommand = TaskDosCommandLogic.GetObjectById( this.TaskItemId );
                    taskDosCommandCreated = true;
                }

                return taskDosCommand;
            }
            set
            {
                taskDosCommand = value;
                OnPropertyChanged( "TaskDosCommand" );
            }
        }

        public ICommand OkCommand
        {
            get
            {
                if( okCommand == null )
                {
                    okCommand = new RelayCommand( param => CanAddDosCommand(), param => AddDosCommand() );
                }
                return okCommand;
            }
        }

        public TaskDosCommandViewModel( IWindowLoader windowLoader ) : base( windowLoader )
        {
            this.DisplayName = "TaskDosCommand";
        }

        private bool CanAddDosCommand()
        {
            return true;
        }

        private void AddDosCommand()
        {
            this.TaskDosCommand.TaskGroupId = this.TaskGroupId;
            this.TaskDosCommand.TaskTypeId  = this.TaskTypeId;

            TaskDosCommandLogic.Save( TaskDosCommand );
            base.OnRequestClose();
        }
    }
}
