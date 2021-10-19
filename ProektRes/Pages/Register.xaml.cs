﻿using System;
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
    /// Логика взаимодействия для Register.xaml
    /// </summary>
    public partial class Register : Page
    {
        public Register()
        {
            InitializeComponent();
            boxSexes.ItemsSource = BaseConnect.baseModel.genders.ToList();
            boxSexes.SelectedValuePath = "id";
            boxSexes.DisplayMemberPath = "gender";

            lbTraits.ItemsSource = BaseConnect.baseModel.traits.ToList();
            lbTraits.SelectedValuePath = "id";
        }

        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            LoadPage.frameLoad.GoBack();
        }

        private void buttonWrite_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                auth logPass = new auth() { login = boxLogin.Text, password = boxPassword.Text, role = 2 };
                BaseConnect.baseModel.auth.Add(logPass);
                BaseConnect.baseModel.SaveChanges();

                users User = new users() { name = boxName.Text, id = logPass.id, gender = (int)boxSexes.SelectedValue, dr = (DateTime)boxBirthday.SelectedDate };
                BaseConnect.baseModel.users.Add(User);

                foreach (traits t in lbTraits.SelectedItems)
                {
                    users_to_traits UTT = new users_to_traits();
                    UTT.id_user = User.id;
                    UTT.id_trait = t.id;
                    BaseConnect.baseModel.users_to_traits.Add(UTT);
                }
                BaseConnect.baseModel.SaveChanges();
                MessageBox.Show("Данные успешно записаны");
                LoadPage.frameLoad.GoBack();
            }
            catch
            {
                MessageBox.Show("Не удалось записать данные");
            }
        }
    }
}