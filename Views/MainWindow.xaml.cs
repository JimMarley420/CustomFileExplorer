using System.Windows;
using System.Windows.Input;
using CustomFileExplorer.ViewModels;

namespace CustomFileExplorer.Views
{
    public partial class MainWindow : Window
    {
        private MainViewModel ViewModel => DataContext as MainViewModel;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ViewModel?.SelectedItem != null)
            {
                if (ViewModel.NavigateToCommand.CanExecute(ViewModel.SelectedItem))
                {
                    ViewModel.NavigateToCommand.Execute(ViewModel.SelectedItem);
                }
            }
        }
    }
}
