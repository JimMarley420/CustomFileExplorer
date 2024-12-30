using System.Windows;

namespace CustomFileExplorer.Views
{
    public partial class ReplaceDialog : Window
    {
        public string SearchText { get; private set; }
        public string ReplaceText { get; private set; }

        public ReplaceDialog()
        {
            InitializeComponent();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            SearchText = SearchTextBox.Text;
            ReplaceText = ReplaceTextBox.Text;
            DialogResult = true; // Valide la fenêtre
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false; // Annule la fenêtre
            Close();
        }
    }
}
