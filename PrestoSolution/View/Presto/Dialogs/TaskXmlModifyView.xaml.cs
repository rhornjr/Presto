using System.Windows;
using PrestoCore.BusinessLogic.Attributes;
using PrestoViewModel.Dialogs;

namespace Presto.Dialogs
{
    /// <summary>
    /// Interaction logic for TaskXmlModifyView.xaml
    /// </summary>
    [ViewModel( typeof(TaskXmlModifyViewModel) )]
    public partial class TaskXmlModifyView : Window
    {
        public TaskXmlModifyView()
        {
            InitializeComponent();
        }
    }
}
