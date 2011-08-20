using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Presto.MvvmTools;
using PrestoCore.BusinessLogic.BusinessEntities;
using System.Windows.Input;
using PrestoViewModel.Tabs;

namespace PrestoViewModel.Dialogs
{
    public class CustomVariableViewModel : ViewModelBase
    {
        private CustomVariable customVariable;
        private RelayCommand   okCommand;
        private RelayCommand   cancelCommand;

        public CustomVariable CustomVariable
        {
            get
            {
                if( customVariable == null )
                {
                    customVariable = new CustomVariable();
                }
                return customVariable;
            }
            set
            {
                customVariable = value;
                OnPropertyChanged( "CustomVariable" );
            }
        }

        public ICommand OkCommand
        {
            get
            {
                if( okCommand == null )
                {
                    okCommand = new RelayCommand( param => CanAddCustomVariable(), param => SaveCustomVariable() );
                }
                return okCommand;
            }
        }

        public ICommand CancelCommand
        {
            get
            {
                if( cancelCommand == null )
                {
                    cancelCommand = new RelayCommand( param => CanCancel(), param => Cancel() );
                }
                return cancelCommand;
            }
        }

        public CustomVariableViewModel( IWindowLoader windowLoader )
            : base( windowLoader )
        {            
        }

        private bool CanAddCustomVariable()
        {
            //if( string.IsNullOrEmpty( this.TaskGroup.TaskGroupName ) ) { return false; }

            return true;
        }

        private void SaveCustomVariable()
        {
            CustomVariableListViewModel customVariableListViewModel = base.Parent as CustomVariableListViewModel;

            customVariableListViewModel.SaveCustomVariable( this.CustomVariable );

            base.OnRequestClose();
        }

        private bool CanCancel()
        {
            return true;
        }

        private void Cancel()
        {
            base.OnRequestClose();
        }
    }
}
