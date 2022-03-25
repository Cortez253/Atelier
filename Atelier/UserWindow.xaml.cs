using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Atelier
{
    /// <summary>
    /// Логика взаимодействия для UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        AtelierDBEntities entities = new AtelierDBEntities();
        public UserWindow()
        {
            InitializeComponent();
            foreach (var user in entities.Users)
                listBoxUsers.Items.Add(user);
                
        }

        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            listBoxUsers.Items.Clear();
            listBoxUsers.Items.Refresh();
            if (string.IsNullOrWhiteSpace(textBoxSerch.Text))
            {
                foreach (var item in entities.Users)
                {
                    listBoxUsers.Items.Add(item);
                    listBoxUsers.Items.Refresh();
                }
            }
            else
            {
                foreach (var a in entities.Users)
                {
                    if (a.Login.StartsWith(textBoxSerch.Text))
                    {
                        listBoxUsers.Items.Add(a);
                    }
                }
            }
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            var addUsers = listBoxUsers.SelectedItem as Users;
            if (textBoxLogin.Text == "" || textBoxPassword.Text == "" || textBoxAccess.Text == "")
            {
                MessageBox.Show("Заполните поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (addUsers == null)
                {
                    addUsers = new Users();
                    entities.Users.Add(addUsers);
                    listBoxUsers.Items.Add(addUsers);
                }
                addUsers.Login = textBoxLogin.Text;
                addUsers.Password = textBoxPassword.Text;
                addUsers.Access_rights = textBoxAccess.Text;
                entities.SaveChanges();
                listBoxUsers.Items.Refresh();
                MessageBox.Show("Запись сохранена", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            var deleteUsers = listBoxUsers.SelectedItem as Users;

            if (deleteUsers != null)
            {
                var question = MessageBox.Show("Запись не выбрана", "Ошибка", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (question == MessageBoxResult.No)
                {
                    return;
                }
                entities.Users.Remove(deleteUsers);
                listBoxUsers.Items.Remove(deleteUsers);
                entities.SaveChanges();
                listBoxUsers.Items.Refresh();
                textBoxLogin.Clear();
                textBoxPassword.Clear();
                textBoxAccess.Clear();
                MessageBox.Show("Запись удалена", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Запись не выбрана", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void buttonClear_Click(object sender, RoutedEventArgs e)
        {
            textBoxLogin.Clear();
            textBoxPassword.Clear();
            textBoxAccess.Clear();
            listBoxUsers.SelectedIndex = -1;
        }

        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            OrderWindow orderWin = new OrderWindow();
            Close();
            orderWin.ShowDialog();
        }

        private void listBoxClients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectUsers = listBoxUsers.SelectedItem as Users;

            if (selectUsers != null)
            {
                textBoxLogin.Text = selectUsers.Login;
                textBoxPassword.Text = selectUsers.Password;
                textBoxAccess.Text = selectUsers.Access_rights;
            }
            else
            {
                textBoxLogin.Text = "";
                textBoxPassword.Text = "";
                textBoxAccess.Text = "";
            }
        }
    }
}
