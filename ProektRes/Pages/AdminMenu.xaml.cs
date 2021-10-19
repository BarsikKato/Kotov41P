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

namespace Kotov_41P_Project
{
    /// <summary>
    /// Логика взаимодействия для AdminMenu.xaml
    /// </summary>
    public partial class AdminMenu : Page
    {
        List<users> users;
        public AdminMenu()
        {
            InitializeComponent();
            UpdateList();
        }

        private void UpdateList()
        {
            users = BaseConnect.baseModel.users.ToList();
            lbUsersList.ItemsSource = users;
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            LoadPage.frameLoad.GoBack();
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32(((Button)e.OriginalSource).Tag);
            auth SelectedUser = BaseConnect.baseModel.auth.Find(id);
            BaseConnect.baseModel.auth.Remove(SelectedUser);
            BaseConnect.baseModel.SaveChanges();
            MessageBox.Show("Пользователь под номером " + id + " был успешно удален");
            UpdateList();
        }
        private void changeButton_Click(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32(((Button)e.OriginalSource).Tag);
            ChangeUserWindow cuw = new ChangeUserWindow(id);
            cuw.ShowDialog();
            UpdateList();
        }

        private void lbTraits_Loaded(object sender, RoutedEventArgs e)
        {
            ListBox lb = (ListBox)sender;
            int index = Convert.ToInt32(lb.Uid);
            lb.ItemsSource = BaseConnect.baseModel.users_to_traits.Where(x => x.id_user == index).ToList();
            lb.DisplayMemberPath = "traits.trait";
        }
    }
}
