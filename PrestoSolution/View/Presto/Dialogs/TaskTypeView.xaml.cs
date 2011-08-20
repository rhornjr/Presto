using System.Windows;
using PrestoCore.BusinessLogic.Attributes;
using PrestoViewModel.Dialogs;

namespace Presto.Dialogs
{
    /// <summary>
    /// Interaction logic for TaskTypeView.xaml
    /// </summary>
    [ViewModel( typeof(TaskTypeViewModel) )]
    public partial class TaskTypeView : Window
    {
        public TaskTypeView()
        {
            InitializeComponent();
        }
    }
}
