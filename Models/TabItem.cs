using System.Collections.ObjectModel;

namespace CustomFileExplorer.Models
{
    public class TabItem
    {
        public string Title { get; set; }
        public bool IsFileTab { get; set; }
        public bool IsFolderTab { get; set; }
        public string Content { get; set; }
        public ObservableCollection<FileItem> Items { get; set; } = new();
    }
}
