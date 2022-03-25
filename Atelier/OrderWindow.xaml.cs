using System;
using System.Collections.Generic;
using System.Data;
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
using Excel = Microsoft.Office.Interop.Excel;

namespace Atelier
{
    /// <summary>
    /// Логика взаимодействия для OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        AtelierDBEntities entities = new AtelierDBEntities();
        
        public OrderWindow()
        {
            InitializeComponent();
            foreach (var client in entities.Clients)
                comboBoxClients.Items.Add(client);
            foreach (var product in entities.Products)
                comboBoxProducts.Items.Add(product);
            foreach (var material in entities.Materials)
                comboBoxMaterials.Items.Add(material);
            foreach (var worker in entities.Workers)
                comboBoxWorkers.Items.Add(worker);
            
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var order = from orders in entities.Orders select orders;
            OrderGrid.ItemsSource = order.ToList();
            int count_row = OrderGrid.Items.Count - 1;
            textBoxchislo.Text = count_row.ToString();
        }
        private void сlientMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ClientWindow clientWin = new ClientWindow();
            Close();
            clientWin.ShowDialog(); 
        }

        private void productMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ProductWindow productWin = new ProductWindow();
            Close();
            productWin.ShowDialog();
        }

        private void workersMenuItem_Click(object sender, RoutedEventArgs e)
        {
            WorkerWindow workerWin = new WorkerWindow();
            Close();
            workerWin.ShowDialog();
        }

        private void usersMenuItem_Click(object sender, RoutedEventArgs e)
        {
            UserWindow userWin = new UserWindow();
            Close();
            userWin.ShowDialog();
        }

        private void ExitItem_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            Close();
            main.ShowDialog();
        }


        private void buttonClear_Click(object sender, RoutedEventArgs e)
        {
            comboBoxClients.SelectedIndex = -1;
            comboBoxProducts.SelectedIndex = -1;
            comboBoxMaterials.SelectedIndex = -1;
            comboBoxWorkers.SelectedIndex = -1;
            dataPickerOrders.SelectedDate = null;
            textBoxDate.Clear();
            textBoxSizeNumber.Clear();
            textBoxChestGirth.Clear();
            textBoxWaistGirth.Clear();
            radioButtonProgress.IsChecked = false;
            radioButtonCompleted.IsChecked = false;
            OrderGrid.SelectedIndex = -1;
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            var selectOrder = OrderGrid.SelectedItem as Orders;

            if (comboBoxClients.SelectedIndex == -1 || comboBoxProducts.SelectedIndex == -1 || comboBoxMaterials.SelectedIndex == -1 || comboBoxWorkers.SelectedIndex == -1 || textBoxSizeNumber.Text == "" || dataPickerOrders.SelectedDate == null || textBoxDate.Text == "" || radioButtonProgress.IsChecked == false && radioButtonCompleted.IsChecked == false)
            {
                MessageBox.Show("Заполните поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (selectOrder == null)
                {
                    selectOrder = new Orders();
                    entities.Orders.Add(selectOrder);

                }

                selectOrder.Id_client = (comboBoxClients.SelectedItem as Clients).Id_client;
                selectOrder.Id_product = (comboBoxProducts.SelectedItem as Products).Id_product;
                selectOrder.Id_material = (comboBoxMaterials.SelectedItem as Materials).Id_material;
                selectOrder.Id_worker = (comboBoxWorkers.SelectedItem as Workers).Id_worker;
                selectOrder.Date_order = (DateTime)dataPickerOrders.SelectedDate;
                selectOrder.Price = decimal.Parse(textBoxDate.Text);
                if (radioButtonProgress.IsChecked == true)
                {
                    selectOrder.Order_status = "Выполняется";
                }
                else
                {
                    selectOrder.Order_status = "Завершен";
                }
                selectOrder.Size_Number = textBoxSizeNumber.Text;
                selectOrder.Chest_girth = decimal.Parse(textBoxChestGirth.Text);
                selectOrder.Waist_girth = decimal.Parse(textBoxWaistGirth.Text);

                entities.SaveChanges();
                OrderGrid.ItemsSource = entities.Orders.ToList();

                MessageBox.Show("Запись сохранена", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            textBoxchislo.Text = Number_of_rows().ToString();
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            var deleteOrder = OrderGrid.SelectedItem as Orders;
            if (deleteOrder != null)
            {
                var rez = MessageBox.Show("Удалить выбранную запись?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (rez == MessageBoxResult.No)
                {
                    return;
                }
                else
                {
                    Orders order = (from i in entities.Orders where i.Id_order == deleteOrder.Id_order select i).SingleOrDefault();
                    entities.Orders.Remove(order);
                    entities.SaveChanges();
                    OrderGrid.ItemsSource = entities.Orders.ToList();

                    comboBoxClients.SelectedIndex = -1;
                    comboBoxProducts.SelectedIndex = -1;
                    comboBoxMaterials.SelectedIndex = -1;
                    comboBoxWorkers.SelectedIndex = -1;
                    dataPickerOrders.SelectedDate = null;
                    textBoxDate.Text = "";
                    radioButtonProgress.IsChecked = false;
                    radioButtonCompleted.IsChecked = false;
                    MessageBox.Show("Запись удалена", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Запись не выбрана", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            textBoxchislo.Text = Number_of_rows().ToString();
        }

        private void OrderGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectOrder = OrderGrid.SelectedItem as Orders;

            if (selectOrder != null)
            {
                comboBoxClients.SelectedItem = (from client in entities.Clients where client.Id_client == selectOrder.Id_client select client).Single<Clients>();
                comboBoxProducts.SelectedItem = (from product in entities.Products where product.Id_product == selectOrder.Id_product select product).Single<Products>();
                comboBoxMaterials.SelectedItem = (from material in entities.Materials where material.Id_material == selectOrder.Id_material select material).Single<Materials>();
                comboBoxWorkers.SelectedItem = (from worker in entities.Workers where worker.Id_worker == selectOrder.Id_worker select worker).Single<Workers>();
                dataPickerOrders.SelectedDate = selectOrder.Date_order;
                textBoxDate.Text = selectOrder.Price.ToString();
                textBoxSizeNumber.Text = selectOrder.Size_Number;
                textBoxChestGirth.Text = Convert.ToString(selectOrder.Chest_girth);
                textBoxWaistGirth.Text = selectOrder.Waist_girth.ToString();
                if (selectOrder.Order_status == "Выполняется")
                {
                    radioButtonProgress.IsChecked = true;
                }
                else
                {
                    radioButtonCompleted.IsChecked = true;
                }
            }
        }

        private void textBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox txt = sender as TextBox;
            comboBoxFilters.SelectedIndex = -1;
            if (txt != null)
            {
                string serch = txt.Text;
                if (!string.IsNullOrEmpty(serch))
                {
                    var query =
                        from order in entities.Orders
                        where order.Clients.Surname.Contains(serch)
                        select order;
                    OrderGrid.ItemsSource = query.ToList();
                }
                else
                {
                    var query = from order in entities.Orders select order;
                    OrderGrid.ItemsSource = query.ToList();
                }
            }

            textBoxchislo.Text = Number_of_rows().ToString();
        }

        private void comboBoxFilters_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
           
            if (comboBoxFilters.SelectedItem == Сompleted_order)
            {
                var query =
                    from order in entities.Orders
                    where order.Order_status == "Выполняется"
                    select order;
                OrderGrid.ItemsSource = query.ToList();

                textBoxchislo.Text = Number_of_rows().ToString();
                
            }
            else if(comboBoxFilters.SelectedItem == Finish_order)
            {
                var query =
                    from order in entities.Orders
                    where order.Order_status == "Завершен"
                    select order;
                OrderGrid.ItemsSource = query.ToList();

                textBoxchislo.Text = Number_of_rows().ToString();
            }
            else if (comboBoxFilters.SelectedItem == Shirt)
            {
                var query =
                    from order in entities.Orders
                    where order.Products.Name_product == "Рубашка"
                    select order;
                OrderGrid.ItemsSource = query.ToList();

                textBoxchislo.Text = Number_of_rows().ToString();
            }
            else if (comboBoxFilters.SelectedItem == Vest)
            {
                var query =
                    from order in entities.Orders
                    where order.Products.Name_product == "Жилетка"
                    select order;
                OrderGrid.ItemsSource = query.ToList();

                textBoxchislo.Text = Number_of_rows().ToString(); ;
            }
            else if (comboBoxFilters.SelectedItem == Jacket)
            {
                var query =
                    from order in entities.Orders 
                    where order.Products.Name_product == "Пиджак"
                    select order;
                OrderGrid.ItemsSource = query.ToList();

                textBoxchislo.Text = Number_of_rows().ToString();
            }
            else if (comboBoxFilters.SelectedItem == Coat)
            {
                var query =
                    from order in entities.Orders
                    where order.Products.Name_product == "Пальто"
                    select order;
                OrderGrid.ItemsSource = query.ToList();

                textBoxchislo.Text = Number_of_rows().ToString();
            }
            else if (comboBoxFilters.SelectedItem == All_order)
            {
                var query =
                    from order in entities.Orders
                    select order;
                OrderGrid.ItemsSource = query.ToList();

                textBoxchislo.Text = Number_of_rows().ToString();
                
            }
        }

        private int Number_of_rows()
        {
            textBoxchislo.Clear();
            int count_row = OrderGrid.Items.Count - 1;
            return count_row;
        }
        private void ExportDataSetToExcel(DataSet ds)
        {
            Excel.Application excelApp = new Excel.Application();
            excelApp.Visible = true;
            Excel.Workbook excelWorkBook = excelApp.Workbooks.Add(1);

            foreach (DataTable table in ds.Tables)
            {
                Excel.Worksheet excelWorkSheet = excelWorkBook.Sheets.Add();
                excelWorkSheet.Name = table.TableName;

                for (int i = 1; i < table.Columns.Count + 1; i++)
                {
                    excelWorkSheet.Columns.ColumnWidth = 30;
                    excelWorkSheet.Cells[1, i] = table.Columns[i - 1].ColumnName;
                }

                for (int j = 0; j < table.Rows.Count; j++)
                {
                    for (int k = 0; k < table.Columns.Count; k++)
                    {
                        excelWorkSheet.Cells[j + 2, k + 1] = table.Rows[j].ItemArray[k].ToString();
                    }
                }
            }
        }

        private void AddExcel_Click(object sender, RoutedEventArgs e)
        {
            OrderWindow orderWindow = new OrderWindow();

            DataTable order = new DataTable("Orders");
            order.Columns.Add("Клиент");
            order.Columns.Add("Изделие");
            order.Columns.Add("Материал");
            order.Columns.Add("Дата заказа");
            order.Columns.Add("Размер");
            order.Columns.Add("Статуст заказа");
            foreach (var orders in entities.Orders)
                order.Rows.Add(orders.Clients, orders.Products, orders.Materials, orders.Date_order, orders.Size_Number, orders.Order_status);

            DataSet dataset = new DataSet("Orders");
            dataset.Tables.Add(order);

            orderWindow.ExportDataSetToExcel(dataset);
        }
    }
}
