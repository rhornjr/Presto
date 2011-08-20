using System.IO;
using System.Windows.Input;
//using Microsoft.Win32;
using Presto.MvvmTools;
using PrestoCore.BusinessLogic.BusinessComponents;
using PrestoCore.BusinessLogic.BusinessEntities;
using System.Windows.Forms;

namespace PrestoViewModel.Dialogs
{
    public class TaskCopyFileViewModel : TaskViewModel
    {
        private RelayCommand okCommand;
        private RelayCommand browseSourcePathCommand;
        private RelayCommand browseSourceFileCommand;
        private RelayCommand browseDestinationPathCommand;
        private TaskCopyFile taskCopyFile;
        private bool         taskCopyFileCreated;

        public TaskCopyFile TaskCopyFile
        {
            get
            {
                if( taskCopyFile == null && this.TaskItemId == 0 )
                {
                    taskCopyFile = new TaskCopyFile();                    
                }

                if( taskCopyFileCreated == false && this.TaskItemId > 0 )
                {
                    taskCopyFile = TaskCopyFileLogic.GetObjectById( this.TaskItemId );
                    taskCopyFileCreated = true;
                }

                return taskCopyFile;
            }
            set
            {
                taskCopyFile = value;
                OnPropertyChanged( "TaskCopyFile" );
            }
        }

        public TaskCopyFileViewModel( IWindowLoader windowLoader ) : base( windowLoader )
        {
            this.DisplayName = "TaskCopyFile";
        }

        public ICommand OkCommand
        {
            get
            {
                if( okCommand == null )
                {
                    okCommand = new RelayCommand( param => CanAddCopyFileTask(), param => AddCopyFileTask() );
                }
                return okCommand;
            }
        }

        public ICommand BrowseSourcePathCommand
        {
            get
            {
                if( this.browseSourcePathCommand == null )
                {
                    this.browseSourcePathCommand = new RelayCommand( param => true, param => BrowseSourcePath() );
                }
                return this.browseSourcePathCommand;
            }
        }

        public ICommand BrowseSourceFileCommand
        {
            get
            {
                {
                    if( this.browseSourceFileCommand == null )
                    {
                        this.browseSourceFileCommand = new RelayCommand( param => true, param => BrowseSourceFile() );
                    }
                }
                return this.browseSourceFileCommand;
            }           
        }        

        public ICommand BrowseDestinationPathCommand
        {
            get
            {
                if( this.browseDestinationPathCommand == null )
                {
                    this.browseDestinationPathCommand = new RelayCommand( param => true, param => BrowseDestinationPath() );
                }
                return this.browseDestinationPathCommand;
            }
        }

        private void BrowseSourcePath()
        {            
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

            folderBrowserDialog.SelectedPath = this.taskCopyFile.SourcePath;

            DialogResult dialogResult = folderBrowserDialog.ShowDialog();

            if( dialogResult == DialogResult.OK )
            {
                this.TaskCopyFile.SourcePath = folderBrowserDialog.SelectedPath;
                OnPropertyChanged( "TaskCopyFile" );
            }
        }

        private void BrowseSourceFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = this.taskCopyFile.SourcePath;

            DialogResult dialogResult = openFileDialog.ShowDialog();

            if( dialogResult == DialogResult.OK )
            {
                this.TaskCopyFile.SourceFileName = openFileDialog.SafeFileName;
                OnPropertyChanged( "TaskCopyFile" );
            }
        }

        private void BrowseDestinationPath()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

            folderBrowserDialog.SelectedPath = this.taskCopyFile.DestinationPath;

            DialogResult dialogResult = folderBrowserDialog.ShowDialog();

            if( dialogResult == DialogResult.OK )
            {
                this.TaskCopyFile.DestinationPath = folderBrowserDialog.SelectedPath;
                OnPropertyChanged( "TaskCopyFile" );
            }
        }

        private bool CanAddCopyFileTask()
        {
            return true;
        }

        private void AddCopyFileTask()
        {
            this.TaskCopyFile.TaskGroupId = this.TaskGroupId;
            this.TaskCopyFile.TaskTypeId  = this.TaskTypeId;

            TaskCopyFileLogic.Save( TaskCopyFile );
            base.OnRequestClose();
        }
    }
}
