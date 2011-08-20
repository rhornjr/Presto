using System.Windows;
using PrestoCore.BusinessLogic.Attributes;
using PrestoViewModel.Dialogs;

namespace Presto.Dialogs
{
    /// <summary>
    /// Interaction logic for TaskGroupView.xaml
    /// </summary>
    [ViewModel( typeof(TaskGroupViewModel) )]
    public partial class TaskGroupView : Window
    {
        public TaskGroupView()
        {
            InitializeComponent();
        }
    }
}
