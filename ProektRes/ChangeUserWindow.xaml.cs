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

namespace ProektRes
{
    /// <summary>
    /// Логика взаимодействия для ChangeUserWindow.xaml
    /// </summary>
    public partial class ChangeUserWindow : Window
    {
        int id;
        public ChangeUserWindow(int id)
        {
            InitializeComponent();
            this.id = id;
            GetData();

        }

        private void GetData()
        {
            auth SelectedUser = BaseConnect.baseModel.auth.Find(id);
            boxName.Text = SelectedUser.users.name;
            boxBirthday.Text = SelectedUser.users.dr.ToString("yyyy MMMM dd");

            boxSexes.ItemsSource = BaseConnect.baseModel.genders.ToList();
            boxSexes.SelectedValuePath = "id";
            boxSexes.DisplayMemberPath = "gender";
            boxSexes.SelectedIndex = SelectedUser.users.gender - 1;

            lbTraits.ItemsSource = BaseConnect.baseModel.traits.ToList();
            lbTraits.SelectedValuePath = "id";

            List<users_to_traits> LUTT = BaseConnect.baseModel.users_to_traits.Where(x => x.id_user == SelectedUser.id).ToList();
            foreach (users_to_traits ut in LUTT)
            {
                int i = 0;
                while (i <= LUTT.Count)
                {
                    if (ut.id_trait == i + 1)
                    {
                        lbTraits.SelectedItems.Add(lbTraits.Items[i]);
                        break;
                    }
                    i++;
                }
            }
        }

        private void buttonWrite_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                auth SelectedUser = BaseConnect.baseModel.auth.Find(id);
                SelectedUser.users.name = boxName.Text;
                SelectedUser.users.dr = (DateTime)boxBirthday.SelectedDate;
                SelectedUser.users.gender = (int)boxSexes.SelectedValue;

                var md = BaseConnect.baseModel.users_to_traits.Where(x => x.id_user == SelectedUser.id).ToList();

                foreach (users_to_traits t in md)
                {
                    BaseConnect.baseModel.users_to_traits.Remove(t);
                }

                foreach (traits t in lbTraits.SelectedItems)
                {
                    users_to_traits UTT = new users_to_traits();
                    UTT.id_user = id;
                    UTT.id_trait = t.id;
                    BaseConnect.baseModel.users_to_traits.Add(UTT);
                }
                BaseConnect.baseModel.SaveChanges();
                MessageBox.Show("Изменения пользователя " + id + " были успешно записаны.");
                Close();
            }
            catch
            {
                MessageBox.Show("Не удалось записать изменения.");
            }
        }

        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
