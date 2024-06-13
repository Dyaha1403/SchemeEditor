using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using SchemeEditor.Controls;
using SchemeEditor.Infrastructure;

namespace SchemeEditor.ViewModels
{
    public class ToolBoxViewModel : ObservableObject
    {
        #region Fields
        private ObservableCollection<BaseControl>? _controls;
        private BaseControl? _selectedControls;
        #endregion

        #region Properties
        public ObservableCollection<BaseControl>? Controls
        {
            get => _controls;
            set
            {
                _controls = value;
                OnPropertyChanged();
            }
        }

        public BaseControl? SelectedControl
        {
            get => _selectedControls;
            set
            {
                _selectedControls = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Commands
        public ICommand DragStartCommand { get; }
        private void OnDragStartCommandExecute(object parameters)
        {
            if(Mouse.LeftButton == MouseButtonState.Pressed && SelectedControl != null)
            {
                DragDrop.DoDragDrop(SelectedControl, new DataObject(typeof(BaseControl), SelectedControl), DragDropEffects.Copy);
                SelectedControl = null;
            }
        }
        #endregion

        #region Constructors
        public ToolBoxViewModel()
        {
            Controls = new ObservableCollection<BaseControl>()
            {
                new Fuse(true),
                new FuseImproved(true),
                new CircuitBreaker(true),
                new CircuitBreakerImproved(true),
                new DrawOutCircuitBreaker(true),
                new DrawOutCircuitBreakerImproved(true),
                new Disconnect(true),
                new OverloadHeater(true),
                new LightningArrestor(true),
                new Contactor(true),
                new Transformer(true),
                new Generator(true),
                new Motor(true),
            };

            DragStartCommand = new LambdaCommand(OnDragStartCommandExecute);
        }
        #endregion
    }
}
