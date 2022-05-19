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
using System.Windows.Shapes;

namespace Курсовой_Будякова.View
{
    /// <summary>
    /// Логика взаимодействия для CountryView.xaml
    /// </summary>
    public partial class CountryView : Window
    {
        private ViewModel.CountryViewModel vmCountry;
        public CountryView()
        {
            InitializeComponent();


            vmCountry = new ViewModel.CountryViewModel();
            DataContext = vmCountry;

        }

        private void lvCountry_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vmCountry.SelectedCountry = (Model.CountryModel)lvCountry.SelectedItem;
        }
    }
}
