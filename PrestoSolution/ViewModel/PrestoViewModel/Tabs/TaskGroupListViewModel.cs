using System.Collections.ObjectModel;
using System.Windows.Input;
using Presto.MvvmTools;
using PrestoCore.BusinessLogic.BusinessComponents;
using PrestoCore.BusinessLogic.BusinessEntities;
using PrestoViewModel.Dialogs;

namespace PrestoViewModel.Tabs
{
    public class TaskGroupListViewModel : ViewModelBase
    {
        # region [ Private Properties ]

        private RelayCommand addCommand;
        private RelayCommand modifyCommand;
        private RelayCommand deleteCommand;
        private RelayCommand showTasksCommand;

        private ObservableCollection<TaskGroup> taskGroups;
        private TaskGroup selectedTaskGroup;

        # endregion [ Private Properties ]

        #region [ Public Properties ]

        public ObservableCollection<TaskGroup> TaskGroups
        {
            get
            {
                if( this.taskGroups == null )
                {
                    this.taskGroups = new ObservableCollection<TaskGroup>( TaskGroupLogic.GetAll() );
                }
                return taskGroups;
            }

            set
            {
                taskGroups = value;
                OnPropertyChanged( "TaskGroups" );
            }
        }

        public TaskGroup SelectedTaskGroup
        {
            get
            {
                return selectedTaskGroup;
            }

            set
            {
                selectedTaskGroup = value;
                OnPropertyChanged( "SelectedTaskGroup" );
            }
        }

        #endregion

        #region [ Constructor ]

        public TaskGroupListViewModel( IWindowLoader windowLoader ) : base( windowLoader )
        {
            base.DisplayName = "Task Groups";            
        }

        #endregion

        public ICommand AddCommand
        {
            get
            {
                if ( addCommand == null )
                {
                    addCommand = new RelayCommand( param => CanAddTaskGroup(), param => AddTaskGroup() );
                }

                return addCommand;
            }
        }

        public ICommand ModifyCommand
        {
            get
            {
                if ( modifyCommand == null )
                {
                    modifyCommand = new RelayCommand( param => CanModifyTaskGroup(), param => ModifyTaskGroup() );
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
                    deleteCommand = new RelayCommand( param => CanDeleteTaskGroup(), param => DeleteTaskGroup() );
                }
                return deleteCommand;
            }
        }

        public ICommand ShowTasksCommand
        {
            get
            {
                if( showTasksCommand == null )
                {
                    showTasksCommand = new RelayCommand( param => CanShowTasks(), param => ShowTasks() );
                }
                return showTasksCommand;
            }            
        }

        private bool CanShowTasks()
        {
            return true;
        }

        private void ShowTasks()
        {
            TaskListViewModel taskListViewModel = new TaskListViewModel( base.WindowLoader );
            taskListViewModel.TaskGroupId = SelectedTaskGroup.TaskGroupId;
            taskListViewModel.Parent = this;
            taskListViewModel.DisplayName = GetTaskListDisplayName();
            //base.WindowLoader.Show( taskListViewModel );
            //( this.MainWindowViewModel as MainWindowViewModel ).CreateTab( typeof( TaskListViewModel ) );
            ( this.MainWindowViewModel as MainWindowViewModel ).CreateTab( taskListViewModel );
        }

        public void SaveTaskGroup( TaskGroup taskGroup )
        {
            TaskGroupLogic.Save( taskGroup );

            if( taskGroup.TaskGroupId == null || taskGroup.TaskGroupId == 0 )
            {
                TaskGroups.Add( taskGroup );  // New TaskGroup, so add it to the collection.
            }
            
            // Refresh the list
            this.TaskGroups = new ObservableCollection<TaskGroup>( TaskGroupLogic.GetAll() );

            SelectedTaskGroup = taskGroup;
        }

        private string GetTaskListDisplayName()
        {
            // HACK: This is the wrong place to make display decisions.
            // Send, at most, a certain number of characters (so it fits on the tab) of the Task Group Name as the display name.

            const int MaxLengthOfTaskGroupName = 10;

            string taskGroupName = SelectedTaskGroup.TaskGroupName;
            string suffix = string.Empty;

            if( taskGroupName.Length > MaxLengthOfTaskGroupName )
            {
                taskGroupName = taskGroupName.Substring( 0, MaxLengthOfTaskGroupName - 1 );
                suffix = "...";  // Since we had to chop some of the name, show the ...
            }

            return taskGroupName + suffix;
        }

        private bool CanAddTaskGroup()
        {           
            return true;
        }

        private void GetNewTaskGroup( TaskGroup taskGroup )
        {
            TaskGroupViewModel viewModel = new TaskGroupViewModel( base.WindowLoader );
            viewModel.DisplayName = "Add Task Group";
            viewModel.TaskGroup = taskGroup;
            viewModel.Parent = this;
            base.WindowLoader.ShowDialog( viewModel );
        }

        private bool CanModifyTaskGroup()
        {
            return true;
        }

        private void AddTaskGroup()
        {
            GetNewTaskGroup( null );
        }

        private void ModifyTaskGroup()
        {
            GetNewTaskGroup( SelectedTaskGroup );
        }

        private bool CanDeleteTaskGroup()
        {
            return true;
        }

        private void DeleteTaskGroup()
        {
            TaskGroupLogic.Delete( SelectedTaskGroup );
            this.TaskGroups.Remove( SelectedTaskGroup );
        }
    }
}
