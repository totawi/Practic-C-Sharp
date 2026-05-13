using System.Windows;
using EventApp.Models;

namespace EventApp.Views
{
    public partial class ParticipantEditDialog : Window
    {
        public ParticipantEditDialog(Participant participant, bool isNew)
        {
            InitializeComponent();
            DataContext = participant;
            Title = isNew ? "Регистрация участника" : "Редактировать участника";
            BtnOk.Content = isNew ? "Зарегистрировать" : "Сохранить";
        }

        private void BtnOk_Click(object sender, RoutedEventArgs e) => DialogResult = true;
        private void BtnCancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}
