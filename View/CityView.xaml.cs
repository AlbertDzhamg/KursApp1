using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using System.Windows.Shapes;

namespace Курсовой_Будякова.View
{
    /// <summary>
    /// Логика взаимодействия для CityView.xaml
    /// </summary>
    public partial class CityView : Window
    {
        private ViewModel.CityViewModel vmCity;
        public CityView()
        {
            InitializeComponent();

            vmCity = new ViewModel.CityViewModel();
            DataContext = vmCity;
        }

        private void lvCity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vmCity.SelectedCityDpo = (Model.CityDBO)lvCity.SelectedItem;
        }

        private void btnGet_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
