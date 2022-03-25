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
    /// Логика взаимодействия для WorkerWindow.xaml
    /// </summary>
    public partial class WorkerWindow : Window
    {
        AtelierDBEntities entities = new AtelierDBEntities();
        public WorkerWindow()
        {
            InitializeComponent();
            foreach (var worker in entities.Workers)
                listBoxWorker.Items.Add(worker);
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            var addWorker = listBoxWorker.SelectedItem as Workers;
            if (textBoxName.Text == "" || textBoxSurname.Text == "" || textBoxPatronymic.Text == "")
            {
                MessageBox.Show("Заполните поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (addWorker == null)
                {
                    addWorker = new Workers();
                    entities.Workers.Add(addWorker);
                    listBoxWorker.Items.Add(addWorker);
                }
                addWorker.Surname = textBoxSurname.Text;
                addWorker.Name = textBoxName.Text;
                addWorker.Patronymic = textBoxPatronymic.Text;
                entities.SaveChanges();
                listBoxWorker.Items.Refresh();
                MessageBox.Show("Запись сохранена", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            var deleteWorker = listBoxWorker.SelectedItem as Workers;

            try
            {
                var ex = (from order in entities.Orders where order.Id_worker == deleteWorker.Id_worker select order).First();
                MessageBox.Show("Невозможно удалить выбранного сотрудника", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch
            {
                if (deleteWorker != null)
                {
                    var question = MessageBox.Show("Запись не выбрана", "Ошибка", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (question == MessageBoxResult.No)
                    {
                        return;
                    }
                    entities.Workers.Remove(deleteWorker);
                    listBoxWorker.Items.Remove(deleteWorker);
                    entities.SaveChanges();
                    listBoxWorker.Items.Refresh();
                    textBoxName.Clear();
                    textBoxPatronymic.Clear();
                    textBoxSurname.Clear();
                    MessageBox.Show("Запись удалена", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Запись не выбрана", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void buttonClear_Click(object sender, RoutedEventArgs e)
        {
            textBoxName.Clear();
            textBoxSurname.Clear();
            textBoxPatronymic.Clear();
            listBoxWorker.SelectedIndex = -1;
        }

        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            OrderWindow orderWin = new OrderWindow();
            Close();
            orderWin.ShowDialog();
        }

        private void listBoxClients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectWorker = listBoxWorker.SelectedItem as Workers;

            if (selectWorker != null)
            {
                textBoxSurname.Text = selectWorker.Surname;
                textBoxName.Text = selectWorker.Name;
                textBoxPatronymic.Text = selectWorker.Patronymic;
            }
            else
            {
                textBoxSurname.Text = "";
                textBoxName.Text = "";
                textBoxPatronymic.Text = "";
            }
        }


        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            listBoxWorker.Items.Clear();
            listBoxWorker.Items.Refresh();
            if (string.IsNullOrWhiteSpace(textBoxSerch.Text))
            {
                foreach (var item in entities.Workers)
                {
                    listBoxWorker.Items.Add(item);
                    listBoxWorker.Items.Refresh();
                }
            }
            else
            {
                foreach (var a in entities.Workers)
                {
                    if (a.Surname.StartsWith(textBoxSerch.Text))
                    {
                        listBoxWorker.Items.Add(a);
                    }
                }
            }
        }
    }
}
