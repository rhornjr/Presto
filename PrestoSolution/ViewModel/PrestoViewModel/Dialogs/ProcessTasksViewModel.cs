using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Presto.MvvmTools;
using PrestoCore.BusinessLogic.BusinessComponents;
using PrestoCore.BusinessLogic.BusinessEntities;

namespace PrestoViewModel.Dialogs
{
    public class ProcessTasksViewModel : ViewModelBase
    {
        #region [Private Member Variables]

        private ObservableCollection<TaskBase> selectedTasks;
        private RelayCommand startCommand;
        private RelayCommand doneCommand;        

        // For progress bar
        private int    progressMinimum = 0;  // Initial defaults so progress bar looks empty.
        private int    progressMaximum = 10;
        private int    progressValue   = 0;
        private string currentlyExecutingTask;
        private string status;
        private bool   taskExecutionStarted;
        private bool   taskExecutionComplete;

        #endregion

        #region [Public Properties]

        public bool TaskExecutionComplete
        {
            get { return taskExecutionComplete; }
            set
            {
                taskExecutionComplete = value;
                OnPropertyChanged( "TaskExecutionComplete" );
            }
        }

        public bool TaskExecutionStarted
        {
            get { return taskExecutionStarted; }
            set
            {
                taskExecutionStarted = value;
                OnPropertyChanged( "TaskExecutionStarted" );
            }
        }

        public ObservableCollection<TaskBase> SelectedTasks
        {
            get { return selectedTasks; }
            set { selectedTasks = value; }
        }

        public int ProgressValue
        {
            get
            {
                return this.progressValue;
            }

            set
            {
                this.progressValue = value;
                OnPropertyChanged( "ProgressValue" );
            }
        }

        public int ProgressMaximum
        {
            get
            {
                return this.progressMaximum;
            }

            set
            {
                this.progressMaximum = value;
                OnPropertyChanged( "ProgressMaximum" );
            }
        }

        public int ProgressMinimum
        {
            get
            {
                return this.progressMinimum;
            }

            set
            {
                this.progressMinimum = value;
                OnPropertyChanged( "ProgressMinimum" );
            }
        }

        public string CurrentlyExecutingTask
        {
            get
            {
                return this.currentlyExecutingTask;
            }
            set
            {
                this.currentlyExecutingTask = value;
                OnPropertyChanged( "CurrentlyExecutingTask" );
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

        public RelayCommand StartCommand
        {
            get
            {
                if( this.startCommand == null )
                {
                    this.startCommand = new RelayCommand( param => true, param => ExecuteTasks() );
                }
                return startCommand;
            }
        }

        public RelayCommand DoneCommand
        {
            get
            {
                if( this.doneCommand == null )
                {
                    this.doneCommand = new RelayCommand( param => true, param => CloseWindow() );
                }
                return doneCommand;
            }
        }

        #endregion

        #region [Constructors]

        public ProcessTasksViewModel( IWindowLoader windowLoader ) : base( windowLoader )
        {

        }

        #endregion

        #region [Private Methods]

        private void ExecuteTasks()
        {
            // As soon as we get out of this method, the UI will be responsive again. So make a call to execute all of
            // the tasks asynchronously and then return.

            this.TaskExecutionStarted = true;

            BackgroundWorker backgroundWorker = new BackgroundWorker();
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler( backgroundWorker_RunWorkerCompleted );

            backgroundWorker.DoWork += new DoWorkEventHandler( ExecuteTasksAsync );

            backgroundWorker.RunWorkerAsync();
        }

        private void ExecuteTasksAsync( object sender, DoWorkEventArgs e )
        {
            this.ProgressMinimum = 0;
            this.ProgressMaximum = this.SelectedTasks.Count;
            this.ProgressValue   = 0;

            bool atLeastOneTaskFailed = false;

            try
            {
                List<TaskBase> tasksInOrderBySequence = new List<TaskBase>( this.SelectedTasks );

                tasksInOrderBySequence.Sort();

                foreach( TaskBase task in tasksInOrderBySequence )
                {
                    this.CurrentlyExecutingTask = task.Description;
                    TaskBaseLogic taskLogic = GetTaskLogicClassOfSelectedTask( task );

                    taskLogic.Execute( task );

                    if( task.TaskSucceeded == false ) 
                    {
                        atLeastOneTaskFailed = true;                        
                        if( task.FailureCausesAllStop == 1 )
                        {
                            this.ProgressValue = this.ProgressMaximum; // Show as completely done, because we're exiting.
                            break;
                        }
                    }
                    
                    this.ProgressValue++;
                }
            }
            catch( Exception ex )
            {
                atLeastOneTaskFailed = true;
                Utility.ProcessException( string.Empty, ex, true );
            }
            finally
            {
                if( atLeastOneTaskFailed )
                {
                    this.Status = "Done. One or more tasks failed. See error log.";
                }
                else
                {
                    this.Status = "Done. All tasks completed successfully.";
                }
                this.TaskExecutionComplete = true;
            }
        }

        private void backgroundWorker_RunWorkerCompleted( object sender, RunWorkerCompletedEventArgs e )
        {
            //OnRequestClose();
        }

        private void CloseWindow()
        {
            OnRequestClose();
        }

        /// <summary>
        /// Gets an instance of a logic class based on its type.
        /// </summary>
        /// <param name="task"><see cref="TaskBase"/></param>
        /// <returns>Logic class intance, for example: TaskXmlModifyLogic.</returns>
        private TaskBaseLogic GetTaskLogicClassOfSelectedTask( TaskBase task )
        {
            TaskType taskType = TaskTypeLogic.GetById( task.TaskTypeId );

            // Note: Need ", PrestoCore" below because we're activating a type in another assembly.
            string className = "PrestoCore.BusinessLogic.BusinessComponents." + taskType.Description + "Logic, PrestoCore";

            return Activator.CreateInstance( Type.GetType( className ) ) as TaskBaseLogic;
        }

        #endregion
    }
}
