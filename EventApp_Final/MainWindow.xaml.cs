using System.Windows;
using EventApp.ViewModels;

namespace EventApp
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _vm;

        public MainWindow()
        {
            InitializeComponent();
            _vm = new MainViewModel();
            DataContext = _vm;
        }

        private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            await _vm.SaveDataAsync();
        }

        // Подавить стандартный GridView header click в ListView
        private void ListView_ColumnHeaderClick(object sender, System.Windows.RoutedEventArgs e)
            => e.Handled = true;
    }
}
