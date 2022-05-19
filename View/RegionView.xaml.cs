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
    /// Логика взаимодействия для RegionView.xaml
    /// </summary>
    public partial class RegionView : Window
    {
        private ViewModel.RegionViewModel vmRegion;
        public RegionView()
        {
            InitializeComponent();
            vmRegion = new ViewModel.RegionViewModel();
            DataContext = vmRegion;
            lvRegion.ItemsSource = vmRegion.RegionTableDpo;
        }

        private void lvRegion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vmRegion.SelectedRegionDpo = (Model.RegionDBO)lvRegion.SelectedItem;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

