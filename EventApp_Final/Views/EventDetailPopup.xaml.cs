using System.Windows;
using EventApp.Models;

namespace EventApp.Views
{
    public partial class EventDetailPopup : Window
    {
        public EventDetailPopup(Event ev)
        {
            InitializeComponent();
            DataContext = ev;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
            => Close();
    }
}
