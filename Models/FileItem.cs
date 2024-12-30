using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace CustomFileExplorer.Models
{
    public class FileItem : INotifyPropertyChanged
    {
        private bool _isSelected;

        public string Name { get; set; }
        public string FullPath { get; set; }
        public bool IsDirectory { get; set; }

        public bool IsSelected
        {
            get => _isSelected;
            set { _isSelected = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
