using System.ComponentModel;

namespace Core.EventModels
{
    public class PropertyObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        protected internal void raisePropertyChanged(params string[] properties)
        {
            if (PropertyChanged != null && properties != null)
                foreach (var p in properties)
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p));
        }
    }
}
