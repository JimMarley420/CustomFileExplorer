using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CustomFileExplorer.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;

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
        public IRelayCommand ReplaceCommand { get; }
        public IRelayCommand<FileItem> DeleteCommand { get; }
        public IRelayCommand<FileItem> RenameCommand { get; }

        public MainViewModel()
        {
            Items = new ObservableCollection<FileItem>();
            NavigateToCommand = new RelayCommand<FileItem>(NavigateTo);
            GoBackCommand = new RelayCommand(GoBack);
            RefreshCommand = new RelayCommand(Refresh);
            SearchCommand = new RelayCommand(Search);
            ReplaceCommand = new RelayCommand(Replace);
            DeleteCommand = new RelayCommand<FileItem>(Delete);
            RenameCommand = new RelayCommand<FileItem>(Rename);

            // Charger le dossier par défaut
            LoadDirectory(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
        }

        private void NavigateTo(FileItem item)
        {
            if (item == null) return;

            if (item.IsDirectory)
            {
                LoadDirectory(item.FullPath);
            }
            else
            {
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

        private void Replace()
        {
            var dialog = new Views.ReplaceDialog(); // Création de la boîte de dialogue
            if (dialog.ShowDialog() == true) // Si l'utilisateur valide
            {
                var search = dialog.SearchText;
                var replace = dialog.ReplaceText;

                foreach (var file in Items.Where(i => !i.IsDirectory))
                {
                    var content = File.ReadAllText(file.FullPath);
                    content = content.Replace(search, replace);
                    File.WriteAllText(file.FullPath, content);
                }

                MessageBox.Show("Remplacement effectué avec succès !");
            }
        }


        private void Delete(FileItem item)
        {
            if (item == null || !File.Exists(item.FullPath)) return;

            File.Delete(item.FullPath);
            Items.Remove(item);
        }

        private void Rename(FileItem item)
        {
            if (item == null || !File.Exists(item.FullPath)) return;

            var newName = Microsoft.VisualBasic.Interaction.InputBox("Entrez le nouveau nom :", "Renommer", item.Name);
            if (!string.IsNullOrWhiteSpace(newName))
            {
                var newPath = Path.Combine(Path.GetDirectoryName(item.FullPath)!, newName);
                File.Move(item.FullPath, newPath);
                item.Name = newName;
                item.FullPath = newPath;
            }
        }

        private void LoadDirectory(string path)
        {
            if (!Directory.Exists(path)) return;

            CurrentPath = path;
            Items.Clear();

            foreach (var directory in Directory.GetDirectories(path))
            {
                Items.Add(new FileItem
                {
                    Name = Path.GetFileName(directory),
                    FullPath = directory,
                    IsDirectory = true
                });
            }

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
