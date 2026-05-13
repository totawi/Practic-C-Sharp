using System.Windows;
using EventApp.Models;

namespace EventApp.Views
{
    public partial class EventEditDialog : Window
    {
        public EventEditDialog(Event ev, bool isNew)
        {
            InitializeComponent();
            DataContext = ev;
            Title = isNew ? "Создать мероприятие" : "Редактировать мероприятие";
            BtnOk.Content = isNew ? "Создать" : "Сохранить";
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
