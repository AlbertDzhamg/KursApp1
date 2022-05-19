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
    /// Логика взаимодействия для AddressView.xaml
    /// </summary>
    public partial class AddressView : Window
    {
        private ViewModel.AddressViewModel vmAddress;
        public AddressView()
        {
            InitializeComponent();
            vmAddress = new ViewModel.AddressViewModel();
            DataContext = vmAddress;
        }

       

        private void lvAddress_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vmAddress.SelectedAddressDpo = (Model.AddressDBO)lvAddress.SelectedItem;
        }
    }
}
