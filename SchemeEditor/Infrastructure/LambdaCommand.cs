using System.Windows.Input;

namespace SchemeEditor.Infrastructure
{
    // Class for creating commands
    public class LambdaCommand : ICommand
    {
        private readonly Action<object>? _execute;
        private readonly Func<object, bool>? _canExecute;

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public LambdaCommand(Action<object> Execute, Func<object, bool>? CanExecute = null)
        {
            _execute = Execute ?? throw new ArgumentNullException(nameof(Execute));
            _canExecute = CanExecute;
        }

        public bool CanExecute(object? parameter) => _canExecute?.Invoke(parameter ?? new object()) ?? true;

        public void Execute(object? parameter) => _execute?.Invoke(parameter ?? new object());
    }

}
