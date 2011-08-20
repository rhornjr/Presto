using System.Windows;
using PrestoCore.BusinessLogic.Attributes;
using PrestoViewModel;

namespace Presto
{
    /// <summary>
    /// Interaction logic for MainWindowView.xaml
    /// </summary>
    [ViewModel( typeof(MainWindowViewModel) )]
    public partial class MainWindowView : Window
    {
        public MainWindowView()
        {
            InitializeComponent();
        }
    }
}
