using Presto.MvvmTools;
using PrestoCore.BusinessLogic.BusinessEntities;
using System.Windows.Input;
using PrestoViewModel.Dialogs;
using System.Collections.ObjectModel;
using PrestoCore.BusinessLogic.BusinessComponents;

namespace PrestoViewModel.Tabs
{
    public class TaskTypeListViewModel : ViewModelBase
    {
        private TaskType taskType;
        private RelayCommand addCommand;
        private RelayCommand modifyCommand;
        private RelayCommand deleteCommand;
        private TaskType selectedTaskType;
        private ObservableCollection<TaskType> taskTypes;

        public ObservableCollection<TaskType> TaskTypes
        {
            get
            {
                if( this.taskTypes == null )
                {
                    this.taskTypes = new ObservableCollection<TaskType>( TaskTypeLogic.GetAll() );
                }
                return taskTypes;
            }
            set
            {
                taskTypes = value;
                OnPropertyChanged( "TaskTypes" );
            }
        }

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

        public TaskType SelectedTaskType
        {
            get
            {
                return selectedTaskType;
            }
            set
            {
                selectedTaskType = value;
                OnPropertyChanged( "SelectedTaskType" );
            }
        }

        public ICommand AddCommand
        {
            get
            {
                if( addCommand == null )
                {
                    addCommand = new RelayCommand( param => CanAddTaskType(), param => AddTaskType() );
                }
                return addCommand;
            }
        }

        public ICommand ModifyCommand
        {
            get
            {
                if( modifyCommand == null )
                {
                    modifyCommand = new RelayCommand( param => CanModifyTaskType(), param => ModifyTaskType() );
                }
                return modifyCommand;
            }
        }

        public ICommand DeleteCommand
        {
            get
            {
                if( deleteCommand == null )
                {
                    deleteCommand = new RelayCommand( param => CanDeleteTaskType(), param => DeleteTaskType( SelectedTaskType ) );
                }
                return deleteCommand;
            }
        }

        public TaskTypeListViewModel( IWindowLoader windowLoader ) : base( windowLoader )
        {
            this.DisplayName = "Task Types";
        }

        private void ShowTaskType( TaskType taskType )
        {
            TaskTypeViewModel viewModel = new TaskTypeViewModel( base.WindowLoader );
            viewModel.Parent = this;
            viewModel.TaskType = taskType;
            base.WindowLoader.ShowDialog( viewModel );
            this.TaskTypes = new ObservableCollection<TaskType>( TaskTypeLogic.GetAll() );  // Refresh
        }

        private void DeleteTaskType( TaskType taskType )
        {
            if( SelectedTaskType == null ) { return; }
            TaskTypeLogic.Delete( taskType );
            this.TaskTypes = new ObservableCollection<TaskType>( TaskTypeLogic.GetAll() );  // Refresh
        }

        private void AddTaskType()
        {
            ShowTaskType( null );
        }

        private void ModifyTaskType()
        {
            if( SelectedTaskType == null ) { return; }
            ShowTaskType( SelectedTaskType );
        }

        private bool CanAddTaskType()
        {
            return true;
        }

        private bool CanModifyTaskType()
        {
            return true;
        }

        private bool CanDeleteTaskType()
        {
            return true;
        }
    }
}
