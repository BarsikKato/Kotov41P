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
    /// Логика взаимодействия для MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Page
    {
        auth CurrentUser;
        public MainMenu()
        {
            InitializeComponent();
        }

        public MainMenu(auth CurrentUser)
        {
            InitializeComponent();
            this.CurrentUser = CurrentUser;
            UpdateData();
        }

        private void UpdateData()
        {
            try
            {
                nameLabel.Text = CurrentUser.users.name;
                birthdayLabel.Text = CurrentUser.users.dr.ToString("yyyy MMMM dd");
                sexLabel.Text = CurrentUser.users.genders.gender;
                List<users_to_traits> LUTT = BaseConnect.baseModel.users_to_traits.Where(x => x.id_user == CurrentUser.id).ToList();
                addInfoLabel.Text = "";
                foreach (users_to_traits UT in LUTT)
                {
                    addInfoLabel.Text += UT.traits.trait + "; ";
                }
            }
            catch
            {
                MessageBox.Show("Не удалось загрузить данные.");
            }
        }

        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            LoadPage.frameLoad.GoBack();
        }

        private void buttonChange_Click(object sender, RoutedEventArgs e)
        {
            int id = CurrentUser.id;
            ChangeUserWindow cuw = new ChangeUserWindow(id);
            cuw.ShowDialog();
            UpdateData();
        }
    }
}
