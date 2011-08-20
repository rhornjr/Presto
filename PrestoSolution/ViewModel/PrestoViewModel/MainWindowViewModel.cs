using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using Presto.MvvmTools;
using PrestoViewModel.Tabs;
using PrestoCore.BusinessLogic.BusinessComponents;
using PrestoCore.BusinessLogic.BusinessEntities;

namespace PrestoViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        # region [ Private Members ]
        
        private ReadOnlyCollection<CommandViewModel> commands;

        # endregion [ Private Members ]

        # region [ Constructors ]

        public MainWindowViewModel( IWindowLoader windowLoader ) : base( windowLoader ) 
        {            
            this.Tabs = new ObservableCollection<ViewModelBase>();

            // TaskTypes need to exist in the DB for Presto to run.
            InsertTaskTypes();
        }

        # endregion [ Constructors ]

        # region [ Data Properties ]

        public ObservableCollection<ViewModelBase> Tabs { get; set; }
        
        # endregion [ Data Properties ]

        # region [ Command Properties ]

        public ReadOnlyCollection<CommandViewModel> Commands
        {
            get
            {
                if (commands == null)
                {
                    List<CommandViewModel> newCommands = this.CreateCommands();
                    commands = new ReadOnlyCollection<CommandViewModel>(newCommands);
                }
                return commands;
            }
        }

        # endregion [ Command Properties ]

        # region [ Command Methods ]

        private void CreateTaskGroupListTab()
        {
            CreateTab( typeof( TaskGroupListViewModel ) );
        }

        private void CreateTaskTypeListTab()
        {
            CreateTab( typeof( TaskTypeListViewModel ) );
        }

        # endregion [ Command Methods ]

        # region [ Tab Management ]

        public void CreateTab(Type viewModelType)
        {
            ViewModelBase viewModel = GetTab(viewModelType);
            
            if (viewModel != null)
            {
                this.SetActiveTab(viewModel);
                return;
            }

            object target = Activator.CreateInstance(viewModelType, base.WindowLoader);
            viewModel = target as ViewModelBase;
            viewModel.MainWindowViewModel = this;

            viewModel.RequestClose += delegate(object sender, EventArgs e) { this.CloseTab(viewModel); };
            this.Tabs.Add(viewModel);
            this.SetActiveTab(viewModel);
            
        }

        public void CreateTab( ViewModelBase viewModel )
        {
            viewModel.MainWindowViewModel = this;
            viewModel.RequestClose += delegate(object sender, EventArgs e) { this.CloseTab(viewModel); };
            this.Tabs.Add( viewModel );
            this.SetActiveTab( viewModel );
        }

        ViewModelBase GetTab(Type viewModelType)
        {
            foreach (ViewModelBase vm in Tabs)
            {
                if (viewModelType == vm.GetType()) return vm;
            }

            return null;
        }

        void SetActiveTab(ViewModelBase workspace)
        {
            if (!this.Tabs.Contains(workspace)) return;

            ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.Tabs);
            if (collectionView != null)
                collectionView.MoveCurrentTo(workspace);
        }

        void CloseTab(ViewModelBase tabViewModel)
        {
            //tabViewModel.Dispose();
            this.Tabs.Remove(tabViewModel);
        }

        # endregion [ Tab Management ]

        # region [ Private Methods ]

        private List<CommandViewModel> CreateCommands()
        {
            return new List<CommandViewModel>
            {
                new CommandViewModel(
                    "Task Groups",
                    new RelayCommand( param => this.CreateTaskGroupListTab() ) ),

                // 23-Jun-2010 Bob Horn: Don't do this anymore. We now reflectively load the types (see InsertTaskTypes() below).
                //new CommandViewModel(
                //    "Task Types",
                //    new RelayCommand(param => this.CreateTaskTypeListTab() ) )
            };
        }

        # endregion [ Private Methods ]

        #region [ Preloaded Data ]

        private void InsertTaskTypes()
        {
            TaskTypeLogic.CreateAllTaskTypes();
        }

        #endregion
    }
}
