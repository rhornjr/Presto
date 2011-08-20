using System.Windows;
using PrestoCore.BusinessLogic.Attributes;
using PrestoViewModel.Dialogs;

namespace Presto.Dialogs
{
    /// <summary>
    /// Interaction logic for ProcessTasksView.xaml
    /// </summary>
    [ViewModel( typeof(ProcessTasksViewModel) )]
    public partial class ProcessTasksView : Window
    {
        public ProcessTasksView()
        {
            InitializeComponent();
        }
    }
}
