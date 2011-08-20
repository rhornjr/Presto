using System.Windows;
using PrestoCore.BusinessLogic.Attributes;
using PrestoViewModel.Dialogs;

namespace Presto.Dialogs
{
    /// <summary>
    /// Interaction logic for TaskDosCommandView.xaml
    /// </summary>
    [ViewModel( typeof(TaskDosCommandViewModel) )]
    public partial class TaskDosCommandView : Window
    {
        public TaskDosCommandView()
        {
            InitializeComponent();
        }
    }
}
