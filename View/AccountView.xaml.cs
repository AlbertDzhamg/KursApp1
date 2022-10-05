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
using System.Windows.Shapes;

namespace Kurs.View
{
    /// <summary>
    /// Логика взаимодействия для AccountView.xaml
    /// </summary>
    public partial class AccountView : Window
    {
        private ViewModel.AccountViewModel vmAccount;
        public AccountView()
        {
            InitializeComponent();

            vmAccount = new ViewModel.AccountViewModel();
            DataContext = vmAccount;
        }
        private void lvAccount_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vmAccount.SelectedAccount = (Model.AccountModel)lvAccount.SelectedItem;
        }


    }
}
