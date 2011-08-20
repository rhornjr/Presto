using System.Collections.Generic;
using System.DirectoryServices;
using System.Windows.Forms;
using System.Windows.Input;
using Presto.MvvmTools;
using PrestoCore.BusinessLogic.BusinessComponents;
using PrestoCore.BusinessLogic.BusinessEntities;

namespace PrestoViewModel.Dialogs
{
    /// <summary>
    /// 
    /// </summary>
    public class TaskMsiViewModel : TaskViewModel
    {
        #region [Private Member Variables]

        private RelayCommand okCommand;
        private RelayCommand browsePathCommand;
        private RelayCommand browseFileCommand;
        private TaskMsi      taskMsi;
        private List<string> iisWebSites;        
        private bool         taskMsiCreated;

        #endregion

        #region [Public Properties]

        /// <summary>
        /// 
        /// </summary>
        public TaskMsi TaskMsi
        {
            get
            {
                if( this.taskMsi == null && this.TaskItemId == 0 )
                {
                    this.taskMsi = new TaskMsi();
                }

                if( this.taskMsiCreated == false && this.TaskItemId > 0 )
                {
                    this.taskMsi = TaskMsiLogic.GetObjectById( this.TaskItemId );
                    this.taskMsiCreated = true;
                }
                return this.taskMsi;
            }
            set
            {
                this.taskMsi = value;
                OnPropertyChanged( "TaskMsi" );
            }
        }

        public List<string> IisWebSites
        {
            get
            {
                if( this.iisWebSites == null )
                {
                    this.iisWebSites = GetLocalIisWebSites();
                }
                return this.iisWebSites;
            }
            set
            {
                this.iisWebSites = value;
                OnPropertyChanged( "IisWebSites" );
            }
        }

        #endregion        

        #region [Commands]

        /// <summary>
        /// 
        /// </summary>
        public ICommand OkCommand
        {
            get
            {
                if( okCommand == null )
                {
                    okCommand = new RelayCommand( param => CanAddMsiTask(), param => AddMsiTask() );
                }
                return okCommand;
            }
        }

        public ICommand BrowsePathCommand
        {
            get
            {
                if( this.browsePathCommand == null )
                {
                    this.browsePathCommand = new RelayCommand( param => true, param => BrowsePath() );
                }
                return this.browsePathCommand;
            }
        }

        public ICommand BrowseFileCommand
        {
            get
            {
                {
                    if( this.browseFileCommand == null )
                    {
                        this.browseFileCommand = new RelayCommand( param => true, param => BrowseFile() );
                    }
                }
                return this.browseFileCommand;
            }           
        }   

        #endregion

        #region [Constructors]

        /// <summary>
        /// 
        /// </summary>
        /// <param name="windowLoader"></param>
        public TaskMsiViewModel( IWindowLoader windowLoader ) : base( windowLoader )
        {
            this.DisplayName = "TaskMsi";
        }

        #endregion

        #region [Private Methods]

        private bool CanAddMsiTask()
        {
            return true;
        }        

        private void AddMsiTask()
        {
            this.TaskMsi.TaskGroupId = this.TaskGroupId;
            this.TaskMsi.TaskTypeId  = this.TaskTypeId;

            TaskMsiLogic.Save( this.TaskMsi );
            base.OnRequestClose();
        }        

        private void BrowsePath()
        {            
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

            folderBrowserDialog.SelectedPath = this.TaskMsi.Path;

            DialogResult dialogResult = folderBrowserDialog.ShowDialog();

            if( dialogResult == DialogResult.OK )
            {
                this.TaskMsi.Path = folderBrowserDialog.SelectedPath;
                OnPropertyChanged( "TaskMsi" );
            }
        }

        private void BrowseFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = this.TaskMsi.Path;

            DialogResult dialogResult = openFileDialog.ShowDialog();

            if( dialogResult == DialogResult.OK )
            {
                this.TaskMsi.FileName = openFileDialog.SafeFileName;
                OnPropertyChanged( "TaskMsi" );
            }
        }

        private List<string> GetLocalIisWebSites()
        {
            List<string> localWebSites = new List<string>();

            DirectoryEntry w3svc = new DirectoryEntry( "IIS://localhost/w3svc" );
             
            // Add default empty string first
            localWebSites.Add( string.Empty );

            foreach( DirectoryEntry directoryEntry in w3svc.Children )
            {
               if( directoryEntry.SchemaClassName == "IIsWebServer" )
               {
                   localWebSites.Add( "/LM/W3SVC/" + directoryEntry.Name + " (" + directoryEntry.Properties["ServerComment"].Value.ToString() + ")" );
               }         
            }

            return localWebSites;
        }

        #endregion
    }
}
