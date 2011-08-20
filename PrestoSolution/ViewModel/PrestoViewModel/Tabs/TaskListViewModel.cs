using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Windows.Input;
using System.Xml.Serialization;
using Presto.MvvmTools;
using PrestoCore.BusinessLogic.BusinessComponents;
using PrestoCore.BusinessLogic.BusinessEntities;
using PrestoViewModel.Dialogs;

namespace PrestoViewModel.Tabs
{
    public class TaskListViewModel : ViewModelBase
    {
        #region [Private Variables]

        private ObservableCollection<TaskBase> tasks;        
        private ObservableCollection<TaskType> taskTypes;
        private ObservableCollection<TaskBase> selectedTasks = new ObservableCollection<TaskBase>();

        private TaskType     selectedTaskType;
        private RelayCommand addCommand;
        private RelayCommand modifyCommand;
        private RelayCommand deleteCommand;
        private RelayCommand executeTasksCommand;
        private RelayCommand exportTasksCommand;
        private RelayCommand importTasksCommand;
        private RelayCommand moveRowUpCommand;
        private RelayCommand moveRowDownCommand;
        private RelayCommand customVariablesCommand;
        private int?         taskGroupId;
        private string       status;
        
        #endregion

        #region [Public Properties]

        public int? TaskGroupId
        {
            get { return taskGroupId; }
            set { taskGroupId = value; }
        }

        public ObservableCollection<TaskBase> Tasks
        {
            get
            {
                if( tasks == null && taskGroupId != null )
                {
                    tasks = new ObservableCollection<TaskBase>( 
                                  TaskBaseLogic.GetTasksByGroupId( (int)taskGroupId ) );                    
                }

                return tasks;
            }

            set
            {
                tasks = value;
                OnPropertyChanged( "Tasks" );
            }
        }

        public TaskType SelectedTaskType
        {
            get
            {
                if( selectedTaskType == null )
                {
                    selectedTaskType = this.TaskTypes[ 0 ];  // Default is first item.
                }
                return selectedTaskType;
            }

            set
            {
                selectedTaskType = value;
                OnPropertyChanged( "SelectedTaskType" );
            }
        }

        public ObservableCollection<TaskType> TaskTypes
        {
            get
            {
                if( taskTypes == null )
                {
                    taskTypes = new ObservableCollection<TaskType>( TaskTypeLogic.GetAll() );
                }
                return taskTypes;
            }

            set
            {
                taskTypes = value;
                OnPropertyChanged( "TaskTypes" );
            }
        }

        public ObservableCollection<TaskBase> SelectedTasks
        {
            get
            {
                return this.selectedTasks;
            }

            set
            {
                selectedTasks = value;
                OnPropertyChanged( "SelectedTask" );
            }
        }

        public string Status
        {
            get
            {
                return this.status;
            }
            set
            {
                this.status = value;
                OnPropertyChanged( "Status" );
            }
        }
        
#endregion

        #region [Commands]

        public ICommand AddCommand
        {
            get
            {
                if ( addCommand == null )
                {
                    addCommand = new RelayCommand( param => CanAddTask(), param => AddTask() );
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
                    modifyCommand = new RelayCommand( param => CanModifyTask(), param => ModifyTask() );
                }

                return modifyCommand;
            }
        }

        public ICommand DeleteCommand
        {
            get
            {
                if ( deleteCommand == null )
                {
                    deleteCommand = new RelayCommand( param => CanDeleteTask(), param => DeleteTask() );
                }

                return deleteCommand;
            }
        }

        public ICommand MoveRowUpCommand
        {
            get
            {
                if( this.moveRowUpCommand == null )
                {
                    this.moveRowUpCommand = new RelayCommand( param => true, param => MoveRowUp() );
                }
                return this.moveRowUpCommand;
            }
        }

        public ICommand MoveRowDownCommand
        {
            get
            {
                if( this.moveRowDownCommand == null )
                {
                    this.moveRowDownCommand = new RelayCommand( param => true, param => MoveRowDown() );
                }
                return this.moveRowDownCommand;
            }
        }

        public ICommand ExecuteTasksCommand
        {
            get
            {
                if( this.executeTasksCommand == null )
                {
                    this.executeTasksCommand = new RelayCommand( param => true, param => ExecuteTasks() );
                }
                return this.executeTasksCommand;
            }
        }

        public ICommand ExportTasksCommand
        {
            get
            {
                if( this.exportTasksCommand == null )
                {
                    this.exportTasksCommand = new RelayCommand( param => true, param => ExportTasks() );
                }
                return this.exportTasksCommand;
            }
        }

        public ICommand ImportTasksCommand
        {
            get
            {
                if( this.importTasksCommand == null )
                {
                    this.importTasksCommand = new RelayCommand( param => true, param => ImportTasks() );
                }
                return this.importTasksCommand;
            }
        }

        public ICommand CustomVariablesCommand
        {
            get
            {
                if( this.customVariablesCommand == null )
                {
                    this.customVariablesCommand = new RelayCommand( param => true, param => ShowCustomVariables() );
                }
                return this.customVariablesCommand;
            }
        }        

        #endregion        

        #region [Constructors]

        public TaskListViewModel( IWindowLoader windowLoader ) : base( windowLoader )
        {
        }

        #endregion

        private bool CanAddTask()
        {
            return true;
        }

        private bool CanModifyTask()
        {
            return true;
        }

        private bool CanDeleteTask()
        {
            return true;
        }

        private void MoveRowUp()
        {
            MoveRow( -1 );
        }

        private void MoveRowDown()
        {
            MoveRow( +1 );
        }

        private void MoveRow( int moveAmount )
        {
            if( this.SelectedTasks == null || this.SelectedTasks.Count != 1 ) { return; }

            // Find the index of the selected task, and move it.
            int indexOfSelectedTask = this.Tasks.IndexOf( this.SelectedTasks[0] );

            if( indexOfSelectedTask + moveAmount < 0 || indexOfSelectedTask + moveAmount > this.Tasks.Count - 1 )
            {
                return;  // Can't move an item before the beginning (or after the end) of the list.
            }

            this.Tasks.Move( indexOfSelectedTask, indexOfSelectedTask + moveAmount );

            SaveNewOrder();

            RefreshTaskList();

            // After doing all of this, select the task again so it stays highlighted for the user.
            this.SelectedTasks.Clear();
            this.SelectedTasks.Add( this.Tasks[ indexOfSelectedTask + moveAmount ] );
        }

        private void SaveNewOrder()
        {
            // Loop through the tasks, now that they're in their new order, and save the sequence.
            int sequence = 0;
            foreach( TaskBase theTask in this.Tasks )
            {
                theTask.Sequence = sequence;
                TaskBaseLogic.Save( theTask );
                sequence++;
            }
        }

        private void AddTask()
        {
            TaskViewModel viewModel = Activator.CreateInstance( Type.GetType( "PrestoViewModel.Dialogs." + SelectedTaskType.Description + "ViewModel" ),
                                                                base.WindowLoader ) as TaskViewModel;
            ShowTask( 0, viewModel, (int)this.SelectedTaskType.TaskTypeId );

            SaveNewOrder();
            RefreshTaskList();
        }

        private void RefreshTaskList()
        {
            this.Tasks = new ObservableCollection<TaskBase>( TaskBaseLogic.GetTasksByGroupId( (int)taskGroupId ) );
        }

        private void ModifyTask()
        {
            if( this.SelectedTasks == null || this.SelectedTasks.Count != 1 ) { return; }
            TaskBase taskBase = TaskBaseLogic.GetById( (int)SelectedTasks[0].TaskItemId );  // Get the base object so we can get to its task type.
            TaskType taskType = TaskTypeLogic.GetById( taskBase.TaskTypeId );
            TaskViewModel viewModel = Activator.CreateInstance( Type.GetType( "PrestoViewModel.Dialogs." + taskType.Description + "ViewModel" ),
                                                                base.WindowLoader ) as TaskViewModel;
            ShowTask( (int)SelectedTasks[0].TaskItemId, viewModel, (int)taskType.TaskTypeId );
        }

        private void DeleteTask()
        {
            if( this.SelectedTasks == null || this.SelectedTasks.Count != 1 ) { return; }
            TaskBaseLogic.DeleteTaskById( (int)SelectedTasks[0].TaskItemId );
            RefreshTaskList();  // Refresh the list so the now-deleted item is excluded.
            SaveNewOrder();     // Save the new order.
            RefreshTaskList();  // Refresh now that the new order has been saved.
        }

        private void ExportTasks()
        {
            this.Status = string.Empty;

            string tasksFile = ConfigurationManager.AppSettings[ "PrestoExportedTasksPathAndFileName" ];

            using( FileStream fileStream = new FileStream( tasksFile, FileMode.Create ) )
            {
                XmlSerializer xmlSerializer = new XmlSerializer( typeof( ObservableCollection<TaskBase> ) );
                xmlSerializer.Serialize( fileStream, this.selectedTasks );
            }

            this.Status = "Tasks exported: " + tasksFile;
        }

        private void ImportTasks()
        {
            ObservableCollection<TaskBase> taskBases = new ObservableCollection<TaskBase>();

            using( FileStream fileStream = new FileStream( ConfigurationManager.AppSettings[ "PrestoExportedTasksPathAndFileName" ], FileMode.Open ) )
            {
                XmlSerializer xmlSerializer = new XmlSerializer( typeof( ObservableCollection<TaskBase> ) );
                taskBases = xmlSerializer.Deserialize( fileStream ) as ObservableCollection<TaskBase>;
            }

            foreach( TaskBase taskBase in taskBases )
            {
                taskBase.TaskGroupId = (int)this.taskGroupId;  // This task may have come from anywhere, so use our group ID, not the one it had natively.
                taskBase.TaskItemId = null;  // If null, will be saved as new.
                SaveAsConcreteTask( taskBase );
            }

            RefreshTaskList();
            SaveNewOrder();
            RefreshTaskList();
        }

        private void SaveAsConcreteTask( TaskBase taskBase )
        {            
            TaskType taskType = TaskTypeLogic.GetById( taskBase.TaskTypeId );

            string assemblyQualifiedClassName = "PrestoCore.BusinessLogic.BusinessComponents." + taskType.Description + "Logic, PrestoCore";

            TaskBaseLogic taskBaseLogic = Activator.CreateInstance( Type.GetType( assemblyQualifiedClassName, true, true ) ) as TaskBaseLogic;

            taskBaseLogic.SaveTaskBaseAsConcreteTask( taskBase );
        }

        private void ShowTask( int taskItemId, TaskViewModel viewModel, int taskTypeId )
        {
            viewModel.Parent      = this;
            viewModel.TaskItemId  = taskItemId;
            viewModel.TaskGroupId = (int)this.TaskGroupId;
            viewModel.TaskTypeId  = taskTypeId;

            base.WindowLoader.ShowDialog( viewModel );
            
            RefreshTaskList();
        }

        private void ExecuteTasks()
        {
            ProcessTasksViewModel viewModel = new ProcessTasksViewModel( base.WindowLoader );
            viewModel.SelectedTasks = this.SelectedTasks;
            base.WindowLoader.ShowDialog( viewModel );
        }

        private void ShowCustomVariables()
        {
            CustomVariableListViewModel viewModel = new CustomVariableListViewModel( base.WindowLoader );
            viewModel.TaskGroupId = (int)this.TaskGroupId;
            viewModel.Parent = this;
            viewModel.DisplayName = "Variables";
            ( this.MainWindowViewModel as MainWindowViewModel ).CreateTab( viewModel );
        }
    }
}
