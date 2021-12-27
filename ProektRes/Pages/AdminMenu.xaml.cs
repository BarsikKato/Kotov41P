using System;
using System.Collections.Generic;
using System.IO;
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

namespace Kotov_ProektRes
{
    /// <summary>
    /// Логика взаимодействия для AdminMenu.xaml
    /// </summary>
    public partial class AdminMenu : Page
    {
        PageChange pc = new PageChange();
        List<users> users;
        List<users> lu1;
        public AdminMenu()
        {
            InitializeComponent();
            UpdateList();
            lbGenderFilter.ItemsSource = BaseConnect.baseModel.genders.ToList();
            lbGenderFilter.SelectedValuePath = "id";
            lbGenderFilter.DisplayMemberPath = "gender";
            lu1 = users;
            DataContext = pc;
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

        private void createButton_Click(object sender, RoutedEventArgs e)
        {
            CreateUserWindow cuw = new CreateUserWindow();
            cuw.ShowDialog();
            UpdateList();
        }

        private void txtPageCount_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                pc.CountOnPage = Convert.ToInt32(txtPageCount.Text);
            }
            catch
            {
                pc.CountOnPage = lu1.Count;
            }
            pc.Countlist = users.Count;
            lbUsersList.ItemsSource = lu1.Skip(0).Take(pc.CountOnPage).ToList();
        }

        private void Filter(object sender, RoutedEventArgs e)
        {          
            try
            {
                int OT = Convert.ToInt32(txtOT.Text) - 1;
                int DO = Convert.ToInt32(txtDO.Text);
                lu1 = users.Skip(OT).Take(DO - OT).ToList();
            }
            catch
            {
                //Ничего
            }
            if (lbGenderFilter.SelectedValue != null)
                lu1 = lu1.Where(x => x.gender == (int)lbGenderFilter.SelectedValue).ToList();
            if (txtNameFilter.Text != "")
                lu1 = lu1.Where(x => x.name.Contains(txtNameFilter.Text)).ToList();

            lbUsersList.ItemsSource = lu1;
            pc.Countlist = lu1.Count;
        }

        private void GoPage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;
            switch (tb.Uid)
            {
                case "prev":
                    pc.CurrentPage--;
                    break;
                case "next":
                    pc.CurrentPage++;
                    break;
                default:
                    pc.CurrentPage = Convert.ToInt32(tb.Text);
                    break;
            }
            lbUsersList.ItemsSource = lu1.Skip(pc.CurrentPage * pc.CountOnPage - pc.CountOnPage).Take(pc.CountOnPage).ToList();
            txtCurrentPage.Text = "Текущая страница: " + (pc.CurrentPage).ToString();
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            lbUsersList.ItemsSource = users;
            lu1 = users;
            lbGenderFilter.SelectedIndex = -1;
            txtNameFilter.Text = "";
            txtOT.Text = "";
            txtDO.Text = "";
        }
        private void UserImage_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Image IMG = sender as System.Windows.Controls.Image;
            int ind = Convert.ToInt32(IMG.Uid);
            users U = BaseConnect.baseModel.users.FirstOrDefault(x => x.id == ind);
            usersimage UI = BaseConnect.baseModel.usersimage.FirstOrDefault(x => x.id_user == ind && x.avatar == true);
            BitmapImage BI = new BitmapImage();
            if (UI != null)
            {
                if (UI.path != null)
                {
                    BI = new BitmapImage(new Uri(UI.path, UriKind.Relative));
                }
                else
                {
                    BI.BeginInit();
                    BI.StreamSource = new MemoryStream(UI.image);
                    BI.EndInit();
                }
            }
            else
            {
                switch (U.gender)
                {
                    case 1:
                        BI = new BitmapImage(new Uri(@"/Images/Male.jpg", UriKind.Relative));
                        break;
                    case 2:
                        BI = new BitmapImage(new Uri(@"/Images/Female.jpg", UriKind.Relative));
                        break;
                    default:
                        BI = new BitmapImage(new Uri(@"/Images/Other.jpg", UriKind.Relative));
                        break;
                }
            }
            IMG.Source = BI;
        }

        private void Sort_Click(object sender, RoutedEventArgs e)
        {
            RadioButton RB = (RadioButton)sender;
            switch (RB.Uid)
            {
                case "name":
                    lu1 = lu1.OrderBy(x => x.name).ToList();
                    break;
                case "DR":
                    lu1 = lu1.OrderBy(x => x.dr).ToList();
                    break;
            }
            if (RBReverse.IsChecked == true) lu1.Reverse();
            lbUsersList.ItemsSource = lu1;
            txtPageCount_TextChanged(null, null);
        }

        private void btnStats_Click(object sender, RoutedEventArgs e)
        {
            LoadPage.frameLoad.Navigate(new Pages.Diagramm());
        }
    }
}
