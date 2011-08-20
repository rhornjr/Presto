using System;
using System.Windows;

namespace Presto.MvvmTools
{
    /// <summary>
    /// 
    /// </summary>
    public interface IWindowLoader
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModel"></param>
        void Show(ViewModelBase viewModel);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModel"></param>
        void ShowDialog(ViewModelBase viewModel);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModelType"></param>
        /// <param name="viewType"></param>
        void Register(Type viewModelType, Type viewType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModelType"></param>
        void Unregister(Type viewModelType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageBoxText"></param>
        /// <param name="caption"></param>
        /// <param name="button"></param>
        /// <param name="icon"></param>
        /// <returns></returns>
        MessageBoxResult ShowMessageBox(string messageBoxText, string caption, MessageBoxButton button, MessageBoxImage icon);
    }
}
