using System.Windows.Controls;
using PrestoCore.BusinessLogic.Attributes;
using PrestoViewModel.Tabs;

namespace Presto.Tabs
{
    /// <summary>
    /// Interaction logic for TaskTypeListView.xaml
    /// </summary>
    [ViewModel( typeof(TaskTypeListViewModel) )]
    public partial class TaskTypeListView : UserControl
    {
        public TaskTypeListView()
        {
            InitializeComponent();
        }
    }
}
