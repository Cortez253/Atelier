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
    /// Логика взаимодействия для ClientWindow.xaml
    /// </summary>
    public partial class ClientWindow : Window
    {
        AtelierDBEntities entities = new AtelierDBEntities();
        public ClientWindow()
        {
            InitializeComponent();
            foreach (var client in entities.Clients)
                listBoxClients.Items.Add(client);
        }

        private void buttonClear_Click(object sender, RoutedEventArgs e)
        {
            textBoxName.Clear();
            textBoxSurname.Clear();
            textBoxPatronymic.Clear();
            textBoxTelephone.Clear();
            listBoxClients.SelectedIndex = -1;
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            var addClient = listBoxClients.SelectedItem as Clients;
            if (textBoxName.Text == "" || textBoxSurname.Text == "" || textBoxPatronymic.Text == "" || textBoxTelephone.Text == "")
            {
                MessageBox.Show("Заполните поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (addClient == null)
                {
                    addClient = new Clients();
                    entities.Clients.Add(addClient);
                    listBoxClients.Items.Add(addClient);
                }
                addClient.Surname = textBoxSurname.Text;
                addClient.Name = textBoxName.Text;
                addClient.Patronymic = textBoxPatronymic.Text;
                addClient.Telephone = textBoxTelephone.Text;
                entities.SaveChanges();
                listBoxClients.Items.Refresh();
                MessageBox.Show("Запись сохранена", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            var deleteCLient = listBoxClients.SelectedItem as Clients;

            try
            {
                var ex = (from order in entities.Orders where order.Id_client == deleteCLient.Id_client select order).First();
                MessageBox.Show("Невозможно удалить выбранного клиента", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch
            {
                if (deleteCLient != null)
                {
                    var question = MessageBox.Show("Запись не выбрана", "Ошибка", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (question == MessageBoxResult.No)
                    {
                        return;
                    }
                    entities.Clients.Remove(deleteCLient);
                    listBoxClients.Items.Remove(deleteCLient);
                    entities.SaveChanges();
                    listBoxClients.Items.Refresh();
                    textBoxName.Clear();
                    textBoxPatronymic.Clear();
                    textBoxSurname.Clear();
                    textBoxTelephone.Clear();
                    MessageBox.Show("Запись удалена", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Запись не выбрана", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            
        }

        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            OrderWindow orderWin = new OrderWindow();
            Close();
            orderWin.ShowDialog();
        }

        private void listBoxClients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectClient = listBoxClients.SelectedItem as Clients;

            if (selectClient != null)
            {
                textBoxSurname.Text = selectClient.Surname;
                textBoxName.Text = selectClient.Name;
                textBoxPatronymic.Text = selectClient.Patronymic;
                textBoxTelephone.Text = selectClient.Telephone;
            }
            else
            {
                textBoxSurname.Text = "";
                textBoxName.Text = "";
                textBoxPatronymic.Text = "";
                textBoxTelephone.Text = "";
            }
        }


        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            listBoxClients.Items.Clear();
            listBoxClients.Items.Refresh();
            if (string.IsNullOrWhiteSpace(textBoxSerch.Text))
            {
                foreach (var item in entities.Clients)
                {
                    listBoxClients.Items.Add(item);
                    listBoxClients.Items.Refresh();
                }
            }
            else
            {
                foreach (var a in entities.Clients)
                {
                    if (a.Surname.StartsWith(textBoxSerch.Text))
                    {
                        listBoxClients.Items.Add(a);
                    }
                }
            }
        }
    }
}
