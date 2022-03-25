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
    /// Логика взаимодействия для ProductWindow.xaml
    /// </summary>
    public partial class ProductWindow : Window
    {
        AtelierDBEntities entities = new AtelierDBEntities();
        public ProductWindow()
        {
            InitializeComponent();
            foreach (var product in entities.Products)
                listBoxProduct.Items.Add(product);
            foreach (var material in entities.Materials)
                listBoxMaterial.Items.Add(material);
        }

        private void buttonSaveProduct_Click(object sender, RoutedEventArgs e)
        {
            var addProduct = listBoxProduct.SelectedItem as Products;
            if (textBoxNameProduct.Text == "")
            {
                MessageBox.Show("Заполните поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (addProduct == null)
                {
                    addProduct = new Products();
                    entities.Products.Add(addProduct);
                    listBoxProduct.Items.Add(addProduct);
                }
                addProduct.Name_product = textBoxNameProduct.Text;
                entities.SaveChanges();
                listBoxProduct.Items.Refresh();
                MessageBox.Show("Запись сохранена", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void buttonDeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            var deleteProduct = listBoxProduct.SelectedItem as Products;

            try
            {
                var ex = (from order in entities.Orders where order.Id_product == deleteProduct.Id_product select order);
                MessageBox.Show("Невозможно удалить выбранное изделие", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch
            {
                if (deleteProduct != null)
                {
                    var question = MessageBox.Show("Запись не выбрана", "Ошибка", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (question == MessageBoxResult.No)
                    {
                        return;
                    }
                    entities.Products.Remove(deleteProduct);
                    listBoxProduct.Items.Remove(deleteProduct);
                    entities.SaveChanges();
                    listBoxProduct.Items.Refresh();
                    textBoxNameProduct.Clear();
                    MessageBox.Show("Запись удалена", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Запись не выбрана", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void buttonClearProduct_Click(object sender, RoutedEventArgs e)
        {
            textBoxNameProduct.Clear();
            listBoxProduct.SelectedIndex = -1;
        }

        private void listBoxProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectProduct = listBoxProduct.SelectedItem as Products;

            if (selectProduct != null)
            {
                textBoxNameProduct.Text = selectProduct.Name_product;
            }
            else
            {
                textBoxNameProduct.Text = "";
            }
        }

        private void buttonSaveMaterial_Click(object sender, RoutedEventArgs e)
        {
            var addMaterial = listBoxMaterial.SelectedItem as Materials;
            if (textBoxNameMaterial.Text == "")
            {
                MessageBox.Show("Заполните поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (addMaterial == null)
                {
                    addMaterial = new Materials();
                    entities.Materials.Add(addMaterial);
                    listBoxMaterial.Items.Add(addMaterial);
                }
                addMaterial.Name_material = textBoxNameMaterial.Text;
                entities.SaveChanges();
                listBoxMaterial.Items.Refresh();
                MessageBox.Show("Запись сохранена", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void buttonDeleteMaterial_Click(object sender, RoutedEventArgs e)
        {
            var deleteMaterial = listBoxMaterial.SelectedItem as Materials;

            try
            {
                var ex = (from order in entities.Orders where order.Id_material == deleteMaterial.Id_material select order);
                MessageBox.Show("Невозможно удалить выбранный материал", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch
            {
                if (deleteMaterial != null)
                {
                    var question = MessageBox.Show("Запись не выбрана", "Ошибка", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (question == MessageBoxResult.No)
                    {
                        return;
                    }
                    entities.Materials.Remove(deleteMaterial);
                    listBoxMaterial.Items.Remove(deleteMaterial);
                    entities.SaveChanges();
                    listBoxMaterial.Items.Refresh();
                    textBoxNameMaterial.Clear();
                    MessageBox.Show("Запись удалена", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Запись не выбрана", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void buttonClearMaterial_Click(object sender, RoutedEventArgs e)
        {
            textBoxNameMaterial.Clear();
            listBoxMaterial.SelectedIndex = -1;
        }

        private void listBoxMaterial_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectMaterial = listBoxMaterial.SelectedItem as Materials;

            if (selectMaterial != null)
            {
                textBoxNameMaterial.Text = selectMaterial.Name_material;
            }
            else
            {
                textBoxNameMaterial.Text = "";
            }
        }

        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            OrderWindow orderWin = new OrderWindow();
            Close();
            orderWin.ShowDialog();
        }
    }
}
