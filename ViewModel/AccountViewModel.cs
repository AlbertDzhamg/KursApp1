using Hangfire.Annotations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.IO;
using Npgsql;
using Kurs.Model;

namespace Kurs.ViewModel
{
    public class AccountViewModel : INotifyPropertyChanged
    {
        private BankModelList vmBank = new BankModelList();
        private AggrementModelList vmAggrement = new AggrementModelList();
        private TypeModelList vmType = new TypeModelList();


        private Model.AccountModel selectedAccount;
        public Model.AccountModel SelectedAccount
        {
            get { return selectedAccount; }
            set
            {
                selectedAccount = value;
                OnPropertyChanged("SelectedAccount");
                EditAccount.CanExecute(true);
                DeleteAccount.CanExecute(true);
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName]
            string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public ObservableCollection<Model.AccountModel> AccountTable { get; set; } =
            new ObservableCollection<Model.AccountModel>();



        public AccountViewModel()
        {
            AccountTable = LoadAccount();
        }


        public int MaxId()
        {
            int max = 0;
            foreach (var r in this.AccountTable)
            {
                if (max < r._Id)
                {
                    max = r._Id;
                };
            }
            return max;
        }
        private Helper.RelayCommand addAccount;
        public Helper.RelayCommand AddAccount
        {
            get
            {
                return addAccount ??
                (addAccount = new Helper.RelayCommand(obj =>
                {
                    View.AccountAdd wnAccount = new View.AccountAdd
                    {
                        Title = "Новый счет",
                    };
                    int maxIdAccount = MaxId() + 1;
                    Model.AccountModel Account = new Model.AccountModel { _Id = maxIdAccount };
                    wnAccount.DataContext = Account;
                    wnAccount.cbBank.ItemsSource = vmBank.LoadBankList();
                    wnAccount.cbAggrement.ItemsSource = vmAggrement.LoadAggrementList();
                    wnAccount.cbType.ItemsSource = vmType.LoadTypeList();

                    if (wnAccount.ShowDialog() == true)
                    {
                        Account._BankID = (int)wnAccount.cbBank.SelectedValue;
                        Account._AggrementID = (int)wnAccount.cbAggrement.SelectedValue;
                        Account._TypeID = (int)wnAccount.cbType.SelectedValue;



                        try
                        {
                            using var con = new NpgsqlConnection(App.sc);
                            con.Open();

                            var sql = "INSERT INTO public.account (_bankid, _aggrementid, _typeid, _account) VALUES ('" + Account._BankID + "'::integer, '" + Account._AggrementID + "'::integer, '" + Account._TypeID + "'::integer, '" + Account._Account + "'::character varying)\r\n returning _id;";

                            using var cmd = new NpgsqlCommand(sql, con);

                            cmd.ExecuteNonQuery();
                            con.Close();
                            AccountTable.Add(Account);
                        }
                        catch (Exception e) { MessageBox.Show(e.Message); }
                    }

                    SelectedAccount = Account;

                }));
            }


        }


        private Helper.RelayCommand editAccount;
        public Helper.RelayCommand EditAccount
        {
            get
            {
                return editAccount ??
                (editAccount = new Helper.RelayCommand(obj =>
                {
                    View.AccountAdd wnAccount = new View.AccountAdd
                    {
                        Title = "Редактирование счета",
                    };
                    Model.AccountModel Account = SelectedAccount;
                    Model.AccountModel tempAcc = new Model.AccountModel();
                    tempAcc = Account.ShallowCopy();


                    wnAccount.DataContext = tempAcc;
                    wnAccount.cbBank.ItemsSource = vmBank.LoadBankList();
                    wnAccount.cbAggrement.ItemsSource = vmAggrement.LoadAggrementList();
                    wnAccount.cbType.ItemsSource = vmType.LoadTypeList();

                    if (wnAccount.ShowDialog() == true)
                    {
                        tempAcc._BankID = (int)wnAccount.cbBank.SelectedValue;
                        tempAcc._AggrementID = (int)wnAccount.cbAggrement.SelectedValue;
                        tempAcc._TypeID = (int)wnAccount.cbType.SelectedValue;



                        try
                        {
                            using var con = new NpgsqlConnection(App.sc);
                            con.Open();

                            var sql = "UPDATE public.account SET\r\n_bankid = '" + tempAcc._BankID + "'::integer, _aggrementid = '" + tempAcc._AggrementID + "'::integer, _account = '" + tempAcc._Account + "'::character varying, _typeid = '" + tempAcc._TypeID + "'::integer WHERE\r\n_id = " + tempAcc._Id + ";";

                            using var cmd = new NpgsqlCommand(sql, con);

                            cmd.ExecuteNonQuery();
                            con.Close();
                            Account._BankID = tempAcc._BankID;
                            Account._AggrementID = tempAcc._AggrementID;
                            Account._TypeID = tempAcc._TypeID;
                            Account._Account = tempAcc._Account;
                        }
                        catch (Exception e) { MessageBox.Show(e.Message); }
                    }

                }, (obj) => SelectedAccount != null && AccountTable.Count > 0));
            }

        }


        private Helper.RelayCommand deleteAccount;
        public Helper.RelayCommand DeleteAccount
        {

            get
            {
                return deleteAccount ??
                (deleteAccount = new Helper.RelayCommand(obj =>
                {
                    Model.AccountModel acc = SelectedAccount;
                    MessageBoxResult result = MessageBox.Show("Удалить данные по счету: " +
                        acc._Account, "Предупреждение", MessageBoxButton.OKCancel,
                        MessageBoxImage.Warning);
                    if (result == MessageBoxResult.OK)
                    {
                        try
                        {
                            using var con = new NpgsqlConnection(App.sc);
                            con.Open();

                            var sql = "DELETE FROM account WHERE _id = " + acc._Id;

                            using var cmd = new NpgsqlCommand(sql, con);

                            cmd.ExecuteNonQuery();
                            con.Close();
                            AccountTable.Remove(acc);
                        }
                        catch (Exception e) { MessageBox.Show(e.Message); }

                    }

                }, (obj) => SelectedAccount != null && AccountTable.Count > 0));
            }
        }



        private Helper.RelayCommand getAccount;
        public Helper.RelayCommand GetAccount
        {
            get
            {
                return getAccount ??
                (getAccount = new Helper.RelayCommand(obj =>
                {
                    LoadTable(LoadAccount());
                },
                (obj) => true));
            }
        }




        private void LoadTable(ObservableCollection<Model.AccountModel> accounts)
        {
            AccountTable.Clear();

            foreach (Model.AccountModel acc in accounts)
            {
                AccountTable.Add(acc);
            }
        }


        public ObservableCollection<Model.AccountModel> LoadAccount()
        {
            try
            {
                ObservableCollection<Model.AccountModel> model = new ObservableCollection<Model.AccountModel>();



                using var con = new NpgsqlConnection(App.sc);
                con.Open();

                var sql = "SELECT * From account ORDER BY _id";

                using var cmd = new NpgsqlCommand(sql, con);

                var rcmd = cmd.ExecuteReader();
                while (rcmd.Read())
                {
                    model.Add(new Model.AccountModel((int)rcmd["_id"], (int)rcmd["_bankid"], (int)rcmd["_aggrementid"], (int)rcmd["_typeid"], rcmd["_account"].ToString()));
                }
                con.Close();

                return model;
            }
            catch (Exception e) { MessageBox.Show(e.Message); return null; }
        }

    }
}

