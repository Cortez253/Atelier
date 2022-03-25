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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Atelier
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AtelierDBEntities entities = new AtelierDBEntities();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void buttonEnter_Click(object sender, RoutedEventArgs e)
        {
            bool rez = false;
            if (textBoxLogin.Text == "")
            {
                MessageBox.Show("Введите логин!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (passwordBox.Password == "")
            {
                MessageBox.Show("Введите пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                foreach (var user in entities.Users)
                {

                    if (user.Login == textBoxLogin.Text && user.Password == passwordBox.Password)
                    {
                        if (user.Access_rights == "admin")
                        {
                            OrderWindow OrdWin = new OrderWindow();
                            Close();
                            OrdWin.ShowDialog();
                            rez = true;
                        }
                        else
                        {
                            ClientOrderWindow OrdWin = new ClientOrderWindow();
                            Close();
                            OrdWin.ShowDialog();
                            rez = true;
                        }

                    }

                }
                if (rez == false)
                {
                    MessageBox.Show("Указан не верный логин или пароль", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void buttonReg_Click(object sender, RoutedEventArgs e)
        {
            RegWindow regWin = new RegWindow();
            Close();
            regWin.ShowDialog();
        }
    }
}
