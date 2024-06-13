using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SchemeEditor.Infrastructure
{
    // The base class that implements INotifyPropertyChanged
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
