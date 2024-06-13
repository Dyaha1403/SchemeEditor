using SchemeEditor.Entities;
using SchemeEditor.ViewModels;
using System.Windows;

namespace SchemeEditor
{
    public partial class MainWindow : Window
    {
        private MainViewModel _mainVM;
        public MainWindow(SchemeDTO schemeDTO)
        {
            _mainVM = new MainViewModel(schemeDTO);
            InitializeComponent();
            _mainVM.RequestClose += (s, e) => this.Close();
            DataContext = _mainVM;
        }
    }
}