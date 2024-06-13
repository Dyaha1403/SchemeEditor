using SchemeEditor.Entities;
using SchemeEditor.Infrastructure;
using SchemeEditor.Services;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace SchemeEditor.ViewModels
{
    public class ChangeShemeViewModel : ObservableObject
    {
        #region Fields
        private ObservableCollection<SchemeDTO> _schemes;
        private string _nameScheme;
        private SchemeDTO? _selectedScheme;
        private int _selectedIndex;
        #endregion

        #region Properties
        public ObservableCollection<SchemeDTO> Schemes
        {
            get => _schemes;
            set
            {
                _schemes = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DisplaySchemes));
            }
        }

        public ObservableCollection<string> DisplaySchemes
        {
            get
            {
                var displaySchemes = new ObservableCollection<string> { "New..." };
                foreach (var scheme in Schemes)
                {
                    displaySchemes.Add(scheme.Name);
                }
                return displaySchemes;
            }
        }

        public string NameScheme
        {
            get => _nameScheme;
            set
            {
                _nameScheme = value;
                OnPropertyChanged();
            }
        }

        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                _selectedIndex = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Commands
        public ICommand SelectionChangedCommand { get; }
        private void SelectionChangedCommandExecute(object parameter)
        {
            if (SelectedIndex == 0)
            {
                NameScheme = "Untitled";
                _selectedScheme = null;
            }
            else
            {
                var scheme = Schemes[SelectedIndex - 1];
                if (scheme != null)
                {
                    NameScheme = scheme.Name;
                    _selectedScheme = scheme;
                }
            }
        }

        public ICommand CloseWindowCommand { get; }
        private void CloseWindowCommandExecute(object parameter)
        {
            if (parameter is Window window)
            {
                window.Close();
            }
        }

        public ICommand OpenCreateCommand { get; }
        private void OpenCreateCommandExecute(object parameter)
        {
            if(_selectedScheme == null)
            {
                SchemeService schemeService = new SchemeService(new ApplicationContext());
                _selectedScheme = schemeService.CreateScheme(NameScheme);
            }
            else
            {
                SchemeService schemeService = new SchemeService(new ApplicationContext());
                _selectedScheme = schemeService.ChangeNameScheme(_selectedScheme.Id, NameScheme);
            }

            var mainWindow = new MainWindow(_selectedScheme);
            mainWindow.Show();

            if (parameter is Window window)
            {
                window.Close();
            }
        }

        public ICommand DeleteCommand { get; }
        private void DeleteCommandExecute(object parameter)
        {
            if(_selectedScheme != null)
            {
                SchemeService schemeService = new SchemeService(new ApplicationContext());
                schemeService.DeleteScheme(_selectedScheme.Id);

                schemeService = new SchemeService(new ApplicationContext());

                SelectedIndex = 0;
                Schemes = new ObservableCollection<SchemeDTO>(schemeService.GetAll());
            }
        }
        #endregion

        #region Constructors
        public ChangeShemeViewModel()
        {
            SchemeService schemeService = new SchemeService(new ApplicationContext());
            Schemes = new ObservableCollection<SchemeDTO>(schemeService.GetAll());
            SelectedIndex = 0;

            NameScheme = "Untitled";

            SelectionChangedCommand = new LambdaCommand(SelectionChangedCommandExecute);
            CloseWindowCommand = new LambdaCommand(CloseWindowCommandExecute);
            OpenCreateCommand = new LambdaCommand(OpenCreateCommandExecute);
            DeleteCommand = new LambdaCommand(DeleteCommandExecute);
        }
        #endregion
    }
}
