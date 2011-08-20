using System.Windows.Controls;
using PrestoCore.BusinessLogic.Attributes;
using PrestoViewModel.Tabs;

namespace Presto.Tabs
{
    /// <summary>
    /// Interaction logic for CustomVariableListView.xaml
    /// </summary>
    [ViewModel( typeof(CustomVariableListViewModel) )]
    public partial class CustomVariableListView : UserControl
    {
        public CustomVariableListView()
        {
            InitializeComponent();
        }
    }
}
