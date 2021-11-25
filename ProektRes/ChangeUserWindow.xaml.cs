using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Drawing;
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
                //SelectedUser.users.

                var md = BaseConnect.baseModel.users_to_traits.Where(x => x.id_user == SelectedUser.id).ToList();

                foreach (users_to_traits t in md)
                {
                    int i = 0;
                    foreach (traits it in lbTraits.SelectedItems)
                    {
                        if(t.id_trait == it.id)
                        {
                            i++;
                            break;
                        }
                    }
                    if (i == 0)
                        BaseConnect.baseModel.users_to_traits.Remove(t);
                }

                foreach (traits t in lbTraits.SelectedItems)
                {
                    users_to_traits UTT = new users_to_traits();
                    UTT.id_user = id;
                    UTT.id_trait = t.id;
                    if(BaseConnect.baseModel.users_to_traits.Where(x => x.id_user == UTT.id_user && x.id_trait == UTT.id_trait).ToList().Count == 0)
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

        private void UserImage_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Image IMG = sender as System.Windows.Controls.Image;
            users U = BaseConnect.baseModel.users.FirstOrDefault(x => x.id == id);
            usersimage UI = BaseConnect.baseModel.usersimage.FirstOrDefault(x => x.id_user == id && x.avatar == true);
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

        private void BtmAddImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".jpg";
            openFileDialog.Filter = "Изображения |*.jpg;*.png";
            var result = openFileDialog.ShowDialog();
            if (result == true)
            {
                System.Drawing.Image UserImage = System.Drawing.Image.FromFile(openFileDialog.FileName);
                ImageConverter IC = new ImageConverter();
                byte[] ByteArr = (byte[])IC.ConvertTo(UserImage, typeof(byte[]));
                usersimage UI = new usersimage() { id_user = id, image = ByteArr, avatar = true };
                try 
                {
                    BaseConnect.baseModel.usersimage.Add(UI);
                    foreach (usersimage ui in BaseConnect.baseModel.usersimage.Where(x => x.id_user == id))
                    {
                        ui.avatar = false;
                    }
                    BaseConnect.baseModel.SaveChanges();
                    BitmapImage BI = new BitmapImage();
                    BI.BeginInit();
                    BI.StreamSource = new MemoryStream(UI.image);
                    BI.EndInit();
                    userImage.Source = BI;
                }
                catch
                {
                    MessageBox.Show("Не удалось загрузить картинку в базу.");
                }
            }
        }

        private void BtnLoadImage_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
