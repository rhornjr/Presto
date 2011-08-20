using System.Windows;
using PrestoCore.BusinessLogic.Attributes;
using PrestoViewModel.Dialogs;

namespace Presto.Dialogs
{
    /// <summary>
    /// Interaction logic for CustomVariableView.xaml
    /// </summary>
    [ViewModel( typeof(CustomVariableViewModel) )]
    public partial class CustomVariableView : Window
    {
        public CustomVariableView()
        {
            InitializeComponent();
        }
    }
}
