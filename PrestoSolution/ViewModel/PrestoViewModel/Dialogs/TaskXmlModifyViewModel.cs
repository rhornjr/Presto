using Presto.MvvmTools;
using PrestoCore.BusinessLogic.BusinessEntities;
using PrestoCore.BusinessLogic.BusinessComponents;
using System.Windows.Input;
using System.Windows.Forms;
using System.IO;

namespace PrestoViewModel.Dialogs
{
    public class TaskXmlModifyViewModel : TaskViewModel
    {
        #region [Private Member Variables]

        private RelayCommand  okCommand;
        private RelayCommand  browseFileCommand;
        private TaskXmlModify taskXmlModify;
        private bool          taskXmlModifyCreated;

        #endregion

        #region [Public Properties]

        /// <summary>
        /// 
        /// </summary>
        public TaskXmlModify TaskXmlModify
        {
            get
            {
                if( this.taskXmlModify == null && this.TaskItemId == 0 )
                {
                    this.taskXmlModify = new TaskXmlModify();
                }

                if( this.taskXmlModifyCreated == false && this.TaskItemId > 0 )
                {
                    this.taskXmlModify = TaskXmlModifyLogic.GetObjectById( this.TaskItemId );
                    this.taskXmlModifyCreated = true;
                }
                return this.taskXmlModify;
            }
            set
            {
                this.taskXmlModify = value;
                OnPropertyChanged( "TaskXmlModify" );
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
                    okCommand = new RelayCommand( param => CanAddXmlModifyTask(), param => AddXmlModifyTask() );
                }
                return okCommand;
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
        public TaskXmlModifyViewModel( IWindowLoader windowLoader ) : base( windowLoader )
        {
            this.DisplayName = "TaskXmlModify";
        }

        #endregion

        #region [Private Methods]

        private bool CanAddXmlModifyTask()
        {
            return true;
        }

        private void AddXmlModifyTask()
        {
            this.TaskXmlModify.TaskGroupId = this.TaskGroupId;
            this.TaskXmlModify.TaskTypeId  = this.TaskTypeId;

            TaskXmlModifyLogic.Save( this.TaskXmlModify );
            base.OnRequestClose();
        }

        private void BrowseFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = Path.GetFullPath( this.TaskXmlModify.XmlPathAndFileName );

            DialogResult dialogResult = openFileDialog.ShowDialog();

            if( dialogResult == DialogResult.OK )
            {
                this.TaskXmlModify.XmlPathAndFileName = openFileDialog.FileName;
                OnPropertyChanged( "TaskXmlModify" );
            }
        }

        #endregion
    }
}
