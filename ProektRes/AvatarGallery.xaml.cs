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
using System.Windows.Shapes;

namespace Kotov_ProektRes
{
    /// <summary>
    /// Логика взаимодействия для AvatarGallery.xaml
    /// </summary>
    public partial class AvatarGallery : Window
    {
        int id;
        public AvatarGallery(int id)
        {
            InitializeComponent();
            this.id = id;
            Title = "Галерея пользователя: " + BaseConnect.baseModel.users.FirstOrDefault(x => x.id == id).name;
            LoadUserImages();
        }

        private void LoadUserImages()
        {
            avatarList.ItemsSource = BaseConnect.baseModel.usersimage.Where(x => x.id_user == id).ToList();
            if (avatarList.Items.Count == 0)
                MessageBox.Show("Галерея пустая.");
        }

        private void Image_Loaded(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Image IMG = sender as System.Windows.Controls.Image;
            var pep = Convert.ToInt32(IMG.Uid);
            usersimage UI = BaseConnect.baseModel.usersimage.FirstOrDefault(x => x.id == pep);
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
            IMG.Source = BI;
        }

        private void SetImageButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Image IMG = sender as System.Windows.Controls.Image;
            foreach (usersimage ui in BaseConnect.baseModel.usersimage.Where(x => x.id_user == id))
            {
                ui.avatar = false;
            }
            var pep = Convert.ToInt32(((Button)e.OriginalSource).Tag);
            usersimage UI = BaseConnect.baseModel.usersimage.FirstOrDefault(x => x.id == pep);
            UI.avatar = true;
            BaseConnect.baseModel.SaveChanges();
            MessageBox.Show("Ваш аватар был успешно изменен.");
            Close();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
