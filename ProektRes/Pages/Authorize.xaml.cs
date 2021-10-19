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

namespace ProektRes
{
    /// <summary>
    /// Логика взаимодействия для Authorize.xaml
    /// </summary>
    public partial class Authorize : Page
    {
        public Authorize()
        {
            InitializeComponent();
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                auth CurrentUser = BaseConnect.baseModel.auth.FirstOrDefault(x => x.login == loginText.Text && x.password == passwordText.Password);
                if (CurrentUser != null)
                {
                    switch (CurrentUser.role)
                    {
                        case 1:
                            MessageBox.Show("Вы вошли как администратор");
                            LoadPage.frameLoad.Navigate(new AdminMenu());
                            break;
                        case 2:
                        default:
                            MessageBox.Show("Вы вошли как обычный пользователь");
                            LoadPage.frameLoad.Navigate(new MainMenu(CurrentUser));
                            break;

                    }

                }
                else
                {
                    MessageBox.Show("Введен неверный логин или пароль.");
                }
            }
            catch
            {
                MessageBox.Show("Произошла неизвестная ошибка.");
            }
        }

        private void regButton_Click(object sender, RoutedEventArgs e)
        {
            LoadPage.frameLoad.Navigate(new Register());
        }
    }
}
