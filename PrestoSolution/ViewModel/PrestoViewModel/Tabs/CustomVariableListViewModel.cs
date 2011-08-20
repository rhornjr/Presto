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
    public class CustomVariableListViewModel : ViewModelBase
    {
        # region [ Private Properties ]

        private ObservableCollection<CustomVariable> customVariables;
        private ObservableCollection<CustomVariable> selectedCustomVariables = new ObservableCollection<CustomVariable>();

        private RelayCommand   addCommand;
        private RelayCommand   modifyCommand;
        private RelayCommand   deleteCommand;
        private RelayCommand   exportTasksCommand;
        private RelayCommand   importTasksCommand;
        private int            taskGroupId;
        private string         status;

        # endregion [ Private Properties ]

        #region [Public Properties]

        public int TaskGroupId
        {
            get { return taskGroupId; }
            set { taskGroupId = value; }
        }

        public ObservableCollection<CustomVariable> CustomVariables
        {
            get
            {
                if( customVariables == null )
                {
                    customVariables = new ObservableCollection<CustomVariable>(
                                  CustomVariableLogic.GetCustomVariablesByGroupId( (int)taskGroupId ) );
                }

                return customVariables;
            }

            set
            {
                customVariables = value;
                OnPropertyChanged( "CustomVariables" );
            }
        }

        public ObservableCollection<CustomVariable> SelectedCustomVariables
        {
            get
            {
                return this.selectedCustomVariables;
            }

            set
            {
                this.selectedCustomVariables = value;
                OnPropertyChanged( "SelectedCustomVariables" );
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
                if( addCommand == null )
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
                if( modifyCommand == null )
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
                if( deleteCommand == null )
                {
                    deleteCommand = new RelayCommand( param => CanDeleteTask(), param => DeleteTask() );
                }

                return deleteCommand;
            }
        }        

        public ICommand ExportTasksCommand
        {
            get
            {
                if( this.exportTasksCommand == null )
                {
                    this.exportTasksCommand = new RelayCommand( param => true, param => ExportVariables() );
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
                    this.importTasksCommand = new RelayCommand( param => true, param => ImportVariables() );
                }
                return this.importTasksCommand;
            }
        }        

        #endregion        

        #region [ Constructor ]

        public CustomVariableListViewModel( IWindowLoader windowLoader )
            : base( windowLoader )
        {
            base.DisplayName = "Custom Variables";            
        }

        #endregion

        #region [ Public Methods ]

        public void SaveCustomVariable( CustomVariable customVariable )
        {
            customVariable.TaskGroupId = this.TaskGroupId;

            CustomVariableLogic.Save( customVariable );

            // Refresh the list
            this.CustomVariables = new ObservableCollection<CustomVariable>( CustomVariableLogic.GetCustomVariablesByGroupId( this.TaskGroupId ) );
            
            ObservableCollection<CustomVariable> customVariables = new ObservableCollection<CustomVariable>();
            customVariables.Add( customVariable );
            SelectedCustomVariables = customVariables;
        }

        #endregion

        #region [ Private Methods ]

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

        private void AddTask()
        {
            CustomVariableViewModel viewModel = new CustomVariableViewModel( base.WindowLoader );

            viewModel.Parent = this;

            base.WindowLoader.ShowDialog( viewModel );
        }

        private void ModifyTask()
        {
            if( this.SelectedCustomVariables == null || this.SelectedCustomVariables.Count != 1 ) { return; }

            CustomVariableViewModel viewModel = new CustomVariableViewModel( base.WindowLoader );

            viewModel.Parent = this;
            viewModel.CustomVariable = this.SelectedCustomVariables[ 0 ];

            base.WindowLoader.ShowDialog( viewModel );
        }

        private void DeleteTask()
        {
            if( this.SelectedCustomVariables == null || this.SelectedCustomVariables.Count != 1 ) { return; }

            CustomVariableLogic.Delete( this.SelectedCustomVariables[ 0 ] );

            RefreshCustomVariableList();
        }

        private void ExportVariables()
        {
            this.Status = string.Empty;

            string variablesFile = ConfigurationManager.AppSettings[ "PrestoExportedVariablesPathAndFileName" ];

            using( FileStream fileStream = new FileStream( variablesFile, FileMode.Create ) )
            {
                XmlSerializer xmlSerializer = new XmlSerializer( typeof( ObservableCollection<CustomVariable> ) );
                xmlSerializer.Serialize( fileStream, this.SelectedCustomVariables );
            }

            this.Status = "Custom variables exported: " + variablesFile;
        }

        private void ImportVariables()
        {
            ObservableCollection<CustomVariable> variables = new ObservableCollection<CustomVariable>();

            using( FileStream fileStream = new FileStream( ConfigurationManager.AppSettings[ "PrestoExportedVariablesPathAndFileName" ], FileMode.Open ) )
            {
                XmlSerializer xmlSerializer = new XmlSerializer( typeof( ObservableCollection<CustomVariable> ) );
                variables = xmlSerializer.Deserialize( fileStream ) as ObservableCollection<CustomVariable>;
            }

            foreach( CustomVariable variable in variables )
            {
                variable.TaskGroupId = (int)this.taskGroupId;  // This variable may have come from anywhere, so use our group ID, not the one it had natively.
                variable.CustomVariableId = null;  // Null, so it gets saved as a new custom variable.
                CustomVariableLogic.Save( variable );
            }

            RefreshCustomVariableList();
        }

        private void RefreshCustomVariableList()
        {
            this.CustomVariables = new ObservableCollection<CustomVariable>( CustomVariableLogic.GetCustomVariablesByGroupId( this.TaskGroupId ) );
        }

        #endregion
    }
}