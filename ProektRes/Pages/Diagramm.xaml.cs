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

namespace Kotov_ProektRes.Pages
{
    /// <summary>
    /// Логика взаимодействия для Diagramm.xaml
    /// </summary>
    public partial class Diagramm : Page
    {
        public Diagramm()
        {
            InitializeComponent();
        }

        private void gridDiagram_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double maxX = gridDiagram.ActualWidth;//получаем текущую ширину и высоту
            double maxY = gridDiagram.ActualHeight;
            gridDiagram.Children.Clear();//очишаем графическое поле
            gridDiagram.Children.Add(line(maxX / 20, maxY / 20, maxX / 20, maxY - maxY / 20));//помещаем созданный объект на grid
            gridDiagram.Children.Add(line(maxX / 20, maxY - maxY / 20, maxX - maxX / 20, maxY - maxY / 20));
            double count = 20, stepX = (maxX - maxX / 10) / count, stepY = (maxY - maxY / 10) / count;
            var users = BaseConnect.baseModel.users.OrderBy(x => x.dr).Take((int)count);
            var eldest = users.First();
            int i = 0;
            foreach (var user in users)
            {
                var year = user.dr.Year - eldest.dr.Year + 1;
                Line L = line(maxX / 20 + stepX * i, maxY - maxY / 20, maxX / 20 + stepX * i, maxY - maxY / 20 - stepY * year);
                L.Stroke = Brushes.Aqua;
                L.StrokeThickness = maxX / 100;
                gridDiagram.Children.Add(L);
                TextBlock TB = new TextBlock();
                TB.Margin = new Thickness(maxX / 20 + stepX * i, maxY - maxY / 20 - stepY * year, 0, 0);
                TB.Text = user.dr.Year.ToString() + "\n" + user.name;
                gridDiagram.Children.Add(TB);
                i++;
                //gridDiagram.Children.Add(polygon(maxX / 20 * i, (maxY - maxY / 20) * i, maxX / 20 * i + maxX / 40, maxY / 20));
            }
        }

        private Line line(double x1, double y1, double x2, double y2)
        {
            Line L = new Line();
            L.X1 = x1;
            L.Y1 = y1;
            L.X2 = x2;
            L.Y2 = y2;
            L.Stroke = Brushes.Black;
            return L;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            LoadPage.frameLoad.Navigate(new AdminMenu());
        }
    }
}
