using Hangfire.Annotations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.IO;
using Npgsql;
using Newtonsoft.Json;
using Kurs.Model;

namespace Kurs.ViewModel
{
    public class BankViewModel : INotifyPropertyChanged
    {
        private Model.BankModel selectedBank;
        public Model.BankModel SelectedBank
        {
            get
            {
                return selectedBank;
            }
            set
            {
                selectedBank = value;
                OnPropertyChanged("SelectedBank");
                EditBank.CanExecute(true);
                DeleteBank.CanExecute(true);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName]
            string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<Model.BankModel> BankTable { get; set; } =
            new ObservableCollection<Model.BankModel>();
        public ObservableCollection<Model.BankModel> BankTableDpo { get; set; } =
            new ObservableCollection<Model.BankModel>();


        public BankViewModel()
        {
            BankTable = LoadBank();
        }



        public int MaxId()
        {
            int max = 0;
            foreach (var r in this.BankTable)
            {
                if (max < r._Id)
                {
                    max = r._Id;
                };
            }
            return max;
        }


        private Helper.RelayCommand addBank;
        public Helper.RelayCommand AddBank
        {
            get
            {
                return addBank ??
                (addBank = new Helper.RelayCommand(obj =>
                {
                    View.BankAdd wnBank = new View.BankAdd
                    {
                        Title = "Новый банк",
                    };
                    int maxIdBank = MaxId() + 1;
                    Model.BankModel bank = new Model.BankModel { _Id = maxIdBank };
                    wnBank.DataContext = bank;
                    if (wnBank.ShowDialog() == true)
                    {
                        try
                        {
                            using var con = new NpgsqlConnection(App.sc);
                            con.Open();


                            var sql = "INSERT INTO public.bank (_namefull, _nameshort, _inn, _bik, _coraccount, _account, _city) VALUES ('"+ bank._Namefull +"'::character varying, '"+ bank._Nameshort +"'::character varying, '"+ bank._Inn +"'::character varying, '"+ bank._Bik +"'::character varying, '"+ bank._Coraccount +"'::character varying, '"+ bank._Account +"'::character varying, '"+ bank._City +"'::character varying) returning _id;";


                            using var cmd = new NpgsqlCommand(sql, con);

                            cmd.ExecuteNonQuery();
                            con.Close();
                            BankTable.Add(bank);
                        }
                        catch (Exception e) { MessageBox.Show(e.Message); }
                    }

                    SelectedBank = bank;

                }));
            }
        }

        private Helper.RelayCommand editBank;
        public Helper.RelayCommand EditBank
        {
            get
            {
                return editBank ??
                (editBank = new Helper.RelayCommand(obj =>
                {
                    View.BankAdd wnBank = new View.BankAdd
                    { Title = "Редактирование банка", };
                    Model.BankModel bank = SelectedBank;
                    Model.BankModel tempBank = new Model.BankModel();
                    tempBank = bank.ShallowCopy();
                    wnBank.DataContext = tempBank;
                    if (wnBank.ShowDialog() == true)
                    {
                        try
                        {
                            using var con = new NpgsqlConnection(App.sc);
                            con.Open();
                            var sql = "UPDATE public.bank SET _namefull = '" + tempBank._Namefull + "'::character varying, _nameshort = '" + tempBank._Nameshort + "'::character varying, _inn = '" + tempBank._Inn + "'::character varying, _bik = '" + tempBank._Bik + "'::character varying, _coraccount = '" + tempBank._Coraccount + "'::character varying, _account = '" + tempBank._Account + "'::character varying, _city = '" + tempBank._City + "'::character varying WHERE _id = " + tempBank._Id + ";";
                            using var cmd = new NpgsqlCommand(sql, con);

                            cmd.ExecuteNonQuery();
                            con.Close();
                            bank._Id = tempBank._Id;
                            bank._Namefull = tempBank._Namefull;
                            bank._Nameshort = tempBank._Nameshort;
                            bank._Inn = tempBank._Inn;
                            bank._Bik = tempBank._Bik;
                            bank._Coraccount = tempBank._Coraccount;
                            bank._Account = tempBank._Account;
                            bank._City = tempBank._City;
                        }
                        catch (Exception e) { MessageBox.Show(e.Message); }
                    }

                }, (obj) => SelectedBank != null && BankTable.Count > 0));
            }
        }

        private Helper.RelayCommand deleteBank;
        public Helper.RelayCommand DeleteBank
        {
            get
            {
                return deleteBank ??
                (deleteBank = new Helper.RelayCommand(obj =>
                {
                    Model.BankModel bank = SelectedBank;
                    MessageBoxResult result = MessageBox.Show("Удалить данные по банку: " +
                        bank._Namefull, "Предупреждение", MessageBoxButton.OKCancel,
                        MessageBoxImage.Warning);
                    if (result == MessageBoxResult.OK)
                    {
                        try
                        {
                            using var con = new NpgsqlConnection(App.sc);
                            con.Open();

                            var sql = "DELETE FROM bank WHERE _id = " + bank._Id;

                            using var cmd = new NpgsqlCommand(sql, con);

                            cmd.ExecuteNonQuery();
                            con.Close();
                            BankTable.Remove(bank);
                        }
                        catch (Exception e) { MessageBox.Show(e.Message); }

                    }

                }, (obj) => SelectedBank != null && BankTable.Count > 0));
            }
        }


        public Helper.RelayCommand bankGet { get; set; }
        public Helper.RelayCommand BankGet
        {
            get
            {
                return bankGet ??
                (bankGet = new Helper.RelayCommand(obj =>
                {
                    LoadTable(LoadBank());
                },
                (obj) => true));
            }
        }

        private void LoadTable(ObservableCollection<Model.BankModel> banks)
        {
            BankTable.Clear();

            foreach (Model.BankModel bank in banks)
            {
                BankTable.Add(bank);
            }
        }


        public ObservableCollection<Model.BankModel> LoadBank()
        {
            try
            {
                ObservableCollection<Model.BankModel> model = new ObservableCollection<Model.BankModel>();



                using var con = new NpgsqlConnection(App.sc);
                con.Open();

                var sql = "SELECT * From bank ORDER BY _id";

                using var cmd = new NpgsqlCommand(sql, con);

                var rcmd = cmd.ExecuteReader();
                while (rcmd.Read())
                {
                    model.Add(new Model.BankModel((int)rcmd["_id"], rcmd["_namefull"].ToString(), rcmd["_nameshort"].ToString(),rcmd["_inn"].ToString(), rcmd["_bik"].ToString(), rcmd["_coraccount"].ToString(), rcmd["_account"].ToString(), rcmd["_city"].ToString()));
                }
                con.Close();

                return model;
            }
            catch (Exception e) { MessageBox.Show(e.Message); return null; }
        }




    }
}
