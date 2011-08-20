using System.Windows;
using PrestoCore.BusinessLogic.Attributes;
using PrestoViewModel.Dialogs;

namespace Presto.Dialogs
{
    /// <summary>
    /// Interaction logic for TaskMsiView.xaml
    /// </summary>
    [ViewModel( typeof(TaskMsiViewModel) )]
    public partial class TaskMsiView : Window
    {
        public TaskMsiView()
        {
            InitializeComponent();
        }
    }
}
