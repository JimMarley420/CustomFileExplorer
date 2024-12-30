using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CustomFileExplorer.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;

namespace CustomFileExplorer.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private FileItem selectedItem;

        [ObservableProperty]
        private ObservableCollection<FileItem> items;

        [ObservableProperty]
        private string currentPath;

        [ObservableProperty]
        private string searchQuery;

        [ObservableProperty]
        private int fileCount;

        public IRelayCommand<FileItem> NavigateToCommand { get; }
        public IRelayCommand GoBackCommand { get; }
        public IRelayCommand RefreshCommand { get; }
        public IRelayCommand SearchCommand { get; }

        public MainViewModel()
        {
            Items = new ObservableCollection<FileItem>();
            NavigateToCommand = new RelayCommand<FileItem>(NavigateTo);
            GoBackCommand = new RelayCommand(GoBack);
            RefreshCommand = new RelayCommand(Refresh);
            SearchCommand = new RelayCommand(Search);

            // Charger le dossier par défaut
            LoadDirectory(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
        }

        private void NavigateTo(FileItem item)
        {
            if (item == null) return;

            if (item.IsDirectory)
            {
                // Charger le contenu du dossier
                LoadDirectory(item.FullPath);
            }
            else
            {
                // Ouvrir un fichier avec l'application par défaut
                Process.Start(new ProcessStartInfo
                {
                    FileName = item.FullPath,
                    UseShellExecute = true
                });
            }
        }

        private void GoBack()
        {
            if (!string.IsNullOrEmpty(CurrentPath))
            {
                var parentDir = Directory.GetParent(CurrentPath);
                if (parentDir != null)
                {
                    LoadDirectory(parentDir.FullName);
                }
            }
        }

        private void Refresh()
        {
            if (!string.IsNullOrEmpty(CurrentPath))
            {
                LoadDirectory(CurrentPath);
            }
        }

        private void Search()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery)) return;

            var filteredItems = Items.Where(item =>
                item.Name.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase)).ToList();

            Items.Clear();
            foreach (var item in filteredItems)
            {
                Items.Add(item);
            }
        }

        private void LoadDirectory(string path)
        {
            if (!Directory.Exists(path)) return;

            CurrentPath = path;
            Items.Clear();

            // Ajouter les dossiers
            foreach (var directory in Directory.GetDirectories(path))
            {
                Items.Add(new FileItem
                {
                    Name = Path.GetFileName(directory),
                    FullPath = directory,
                    IsDirectory = true
                });
            }

            // Ajouter les fichiers
            foreach (var file in Directory.GetFiles(path))
            {
                Items.Add(new FileItem
                {
                    Name = Path.GetFileName(file),
                    FullPath = file,
                    IsDirectory = false
                });
            }

            FileCount = Items.Count;
        }
    }
}
