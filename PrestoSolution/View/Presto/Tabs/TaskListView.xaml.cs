using System.Windows.Controls;
using PrestoCore.BusinessLogic.Attributes;
using PrestoViewModel.Tabs;

namespace Presto.Tabs
{
    /// <summary>
    /// Interaction logic for TaskListView.xaml
    /// </summary>
    [ViewModel( typeof(TaskListViewModel) )]
    public partial class TaskListView : UserControl
    {
        public TaskListView()
        {
            InitializeComponent();
        }
    }
}
