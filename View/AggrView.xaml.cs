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

namespace Kurs.View
{
    /// <summary>
    /// Логика взаимодействия для AggrView.xaml
    /// </summary>
    public partial class AggrView : Window
    {
        private ViewModel.AggrViewModel vmAggr;
        public AggrView()
        {
            InitializeComponent();


            vmAggr = new ViewModel.AggrViewModel();
            DataContext = vmAggr;

        }

        private void lvAggr_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vmAggr.SelectedAggr = (Model.AggrModel)lvAggr.SelectedItem;
        }
    }
}
