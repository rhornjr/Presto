using System;
using System.Windows;

namespace Presto.MvvmTools
{
    /// <summary>
    /// 
    /// </summary>
    public class WindowLoader : IWindowLoader
    {
        ViewMapper viewMapper;

        /// <summary>
        /// 
        /// </summary>
        public WindowLoader()
        {
            viewMapper = new ViewMapper();
        }

        #region [ IWindowLoader Members ]

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModelType"></param>
        /// <param name="viewType"></param>
        public void Register(Type viewModelType, Type viewType)
        {
            viewMapper.Register(viewModelType, viewType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModelType"></param>
        public void Unregister(Type viewModelType)
        {
            viewMapper.Unegister(viewModelType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModel"></param>
        public void Show(ViewModelBase viewModel)
        {
            GetWindow(viewModel).Show();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModel"></param>
        public void ShowDialog(ViewModelBase viewModel)
        {
            Window window = GetWindow(viewModel);

            viewModel.RequestClose += delegate(object sender, EventArgs e) { window.Close(); };
            
            window.ShowDialog();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageBoxText"></param>
        /// <param name="caption"></param>
        /// <param name="button"></param>
        /// <param name="icon"></param>
        /// <returns></returns>
        public MessageBoxResult ShowMessageBox( string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon )
        {
            return MessageBox.Show( messageBoxText, caption, button, icon );
        }

        #endregion [ IWindowLoader Members ]

        private Window GetWindow(ViewModelBase viewModel)
        {
            object target = Activator.CreateInstance( viewMapper.ResolveView( viewModel ) );

            Window window = target as Window;

            window.DataContext = viewModel;

            return window;
        }
    }
}
