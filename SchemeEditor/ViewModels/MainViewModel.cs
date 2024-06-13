using SchemeEditor.Entities;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SchemeEditor.Infrastructure;
using SchemeEditor.Views;

namespace SchemeEditor.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        public ToolBoxViewModel ToolBoxVM { get; private set; }
        public CanvasViewModel CanvasVM { get; private set; }
        private ContextMenu _contextMenu;
        public ContextMenu ContextMenu
        {
            get => _contextMenu;
            set
            {
                _contextMenu = value;
                OnPropertyChanged();
            }
        }
        public event EventHandler RequestClose;

        #region Commands
        public ICommand FileButtonCommand { get; }
        private void FileButtonCommandExecute(object parameter)
        {
            _contextMenu.IsOpen = true;
        }
        #endregion
        public MainViewModel(SchemeDTO schemeDTO)
        {
            CreateContextMenu();

            FileButtonCommand = new LambdaCommand(FileButtonCommandExecute);

            ToolBoxVM = new ToolBoxViewModel();
            CanvasVM = new CanvasViewModel(schemeDTO.Id);
        }
        private void CreateContextMenu()
        {
            _contextMenu = new ContextMenu();
            MenuItem changeSchemeMenuItem = new MenuItem();
            changeSchemeMenuItem.Header = "Change scheme";
            changeSchemeMenuItem.Click += ChangeScheme;

            _contextMenu.Items.Add(changeSchemeMenuItem);
        }

        private void ChangeScheme(object sender, RoutedEventArgs e)
        {
            ChangeSchemeWindow changeSchemeWindow = new ChangeSchemeWindow();
            changeSchemeWindow.Show();

            RequestClose?.Invoke(this, EventArgs.Empty);
        }
    }
}
