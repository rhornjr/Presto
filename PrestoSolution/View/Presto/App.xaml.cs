using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using Presto.MvvmTools;
using PrestoCore.BusinessLogic.Attributes;
using PrestoCore.BusinessLogic.BusinessComponents;
using PrestoCore.DataAccess;
using PrestoViewModel;

namespace Presto
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup( StartupEventArgs e )
        {
            this.DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler( appDispatcherUnhandledException );

            DataAccessBase.InitializeDataSet( AppDomain.CurrentDomain.BaseDirectory + @"\" +
                                                ConfigurationManager.AppSettings[ "PrestoDatasetFileName" ] );

            WindowLoader windowLoader = CreateWindowLoaderWithViewAndViewModelAssociations();

            MainWindowViewModel viewModel = new MainWindowViewModel( windowLoader );

            windowLoader.Show( viewModel );
        }

        private WindowLoader CreateWindowLoaderWithViewAndViewModelAssociations()
        {
            // Each view has an attribute by which it identifies the ViewModel associated with the View. It looks like this:
            //   [ViewModel( typeof(MainWindowViewModel) )]
            //   public partial class MainWindowView : Window

            // Loop through the views, looking for ViewModel attributes. Store the view/view model associations
            // in the window loader.

            WindowLoader windowLoader = new WindowLoader();
            
            Assembly assembly = Assembly.GetExecutingAssembly();

            List<Type> types = new List<Type>( assembly.GetTypes() );

            Attribute[] attributes;
            foreach( Type theType in types )
            {
                attributes = Attribute.GetCustomAttributes( theType );

                foreach( ViewModelAttribute attribute in attributes.Where( attr => attr is ViewModelAttribute ) )
                {
                    windowLoader.Register( attribute.ViewModelType, theType );
                }
            }

            return windowLoader;
        }

        private void appDispatcherUnhandledException( object sender, DispatcherUnhandledExceptionEventArgs e )
        {
            Utility.Log( e.Exception.Message );
            MessageBox.Show( "An error has occurred. Please see the error log for details.\n" + e.Exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation );
            e.Handled = true;
        }
    }
}
