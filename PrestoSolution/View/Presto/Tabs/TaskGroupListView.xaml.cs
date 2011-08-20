using System.Windows.Controls;
using PrestoCore.BusinessLogic.Attributes;
using PrestoViewModel.Tabs;

namespace Presto.Tabs
{
    /// <summary>
    /// Interaction logic for TaskGroupView.xaml
    /// </summary>
    [ViewModel( typeof(TaskGroupListViewModel) )]
    public partial class TaskGroupListView : UserControl
    {
        public TaskGroupListView()
        {
            InitializeComponent();
        }
    }
}
