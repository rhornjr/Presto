using System.Windows;
using PrestoCore.BusinessLogic.Attributes;
using PrestoViewModel.Dialogs;

namespace Presto.Dialogs
{
    /// <summary>
    /// Interaction logic for TaskCopyFileView.xaml
    /// </summary>
    [ViewModel( typeof(TaskCopyFileViewModel) )]
    public partial class TaskCopyFileView : Window
    {
        public TaskCopyFileView()
        {
            InitializeComponent();
        }
    }
}
