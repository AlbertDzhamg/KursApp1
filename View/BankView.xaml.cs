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

namespace Kurs.View
{
    /// <summary>
    /// Логика взаимодействия для BankView.xaml
    /// </summary>
    public partial class BankView : Window
    {
        private ViewModel.BankViewModel vmBank;
        public BankView()
        {
            InitializeComponent();
            vmBank = new ViewModel.BankViewModel();
            DataContext = vmBank;
        }

       

        private void lvBank_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vmBank.SelectedBank = (Model.BankModel)lvBank.SelectedItem;
        }
    }
}
