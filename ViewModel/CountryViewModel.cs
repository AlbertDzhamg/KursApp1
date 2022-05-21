using Hangfire.Annotations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.IO;
using Npgsql;
using System.Windows;

namespace Курсовой_Будякова.ViewModel
{

    public class CountryViewModel : INotifyPropertyChanged
    {


       private Model.CountryModel selectedCountry;
        public Model.CountryModel SelectedCountry
        {
            get
            {
                return selectedCountry;
            }
            set
            {
                selectedCountry = value;
                OnPropertyChanged("SelectedCountry");
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName]
            string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<Model.CountryModel> CountryTable { get; set; } =
            new ObservableCollection<Model.CountryModel>();   
        public ObservableCollection<Model.CountryModel> CountryTableDpo { get; set; } =
            new ObservableCollection<Model.CountryModel>();


        public CountryViewModel()
        {

        }

     

        public int MaxId()
        {
            int max = 0;
            foreach (var r in this.CountryTable)
            {
                if (max < r._ID)
                {
                    max = r._ID;
                };
            }
            return max;
        }


        private Helper.RelayCommand addCountry;
        public Helper.RelayCommand AddCountry
        {
            get
            {
                return addCountry ??
                (addCountry = new Helper.RelayCommand(obj =>
                {
                    View.CountryAdd wnCountry = new View.CountryAdd
                    {
                        Title = "Новая страна",
                    };
                    int maxIdCountry = MaxId() + 1;
                    Model.CountryModel Country = new Model.CountryModel { _ID = maxIdCountry };
                    wnCountry.DataContext = Country;
                    if (wnCountry.ShowDialog() == true)
                    {
                        try
                        {
                            using var con = new NpgsqlConnection(App.sc);
                        con.Open();


                        var sql = "INSERT INTO Country (_ID, _countryFull, _countryShort)" +
                                  " VALUES(" + Country._ID + ", '" + Country._CountryFull + "', '" + Country._CountryShort + "')" +
                                  " ON CONFLICT(_ID) DO UPDATE" +
                                  " SET _countryFull = excluded._countryFull," +
                                  " _countryShort = excluded._countryShort; ";

                        using var cmd = new NpgsqlCommand(sql, con);

                            cmd.ExecuteNonQuery();
                            con.Close();
                        CountryTable.Add(Country);
                        }
                        catch (Exception e) { MessageBox.Show(e.Message); }
                    }
                   
                    SelectedCountry = Country;
                   
                }));
            }
        }

        private Helper.RelayCommand editCountry;
        public Helper.RelayCommand EditCountry
        {
            get
            {
                return editCountry ??
                (editCountry = new Helper.RelayCommand(obj =>
                {
                    View.CountryAdd wnCountry = new View.CountryAdd
                    { Title = "Редактирование страны", };
                    Model.CountryModel country = SelectedCountry;
                    Model.CountryModel tempCountry = new Model.CountryModel();
                    tempCountry = country.ShallowCopy();
                    wnCountry.DataContext = tempCountry;
                    if (wnCountry.ShowDialog() == true)
                    {
                        try
                        {
                            using var con = new NpgsqlConnection(App.sc);
                            con.Open();


                            var sql = "INSERT INTO Country (_ID, _countryFull, _countryShort)" +
                                      " VALUES(" + tempCountry._ID + ", '" + tempCountry._CountryFull + "', '" + tempCountry._CountryShort + "')" +
                                      " ON CONFLICT(_ID) DO UPDATE" +
                                      " SET _countryFull = excluded._countryFull," +
                                      " _countryShort = excluded._countryShort; ";

                            using var cmd = new NpgsqlCommand(sql, con);

                            cmd.ExecuteNonQuery();
                            con.Close();
                            country._CountryShort = tempCountry._CountryShort;
                            country._CountryFull = tempCountry._CountryFull;
                        }
                        catch(Exception e) { MessageBox.Show(e.Message); }
                    }
                   
                }, (obj) => SelectedCountry != null && CountryTable.Count > 0));
            }
        }

        private Helper.RelayCommand deleteCountry;
        public Helper.RelayCommand DeleteCountry
        {
            get
            {
                return deleteCountry ??
                (deleteCountry = new Helper.RelayCommand(obj =>
                {
                    Model.CountryModel country = SelectedCountry;
                    MessageBoxResult result = MessageBox.Show("Удалить данные по стране: " +
                        country._CountryFull, "Предупреждение", MessageBoxButton.OKCancel,
                        MessageBoxImage.Warning);
                    if (result == MessageBoxResult.OK)
                    {
                        try
                        {
                            using var con = new NpgsqlConnection(App.sc);
                            con.Open();

                            var sql = "DELETE FROM Country WHERE _ID = " + country._ID;

                            using var cmd = new NpgsqlCommand(sql, con);

                            cmd.ExecuteNonQuery();
                            con.Close();
                            CountryTable.Remove(country);
                        }
                        catch (Exception e) { MessageBox.Show(e.Message); }
                        
                    }
                   
                }, (obj) => SelectedCountry != null && CountryTable.Count > 0));
            }
        }


        public Helper.RelayCommand сountryGet { get; set; }
        public Helper.RelayCommand CountryGet
        {
            get
            {
                return сountryGet ??
                (сountryGet = new Helper.RelayCommand(obj =>
                {
                    LoadTable(LoadCountry());
                },
                (obj) => true));
            }
        }

        private void LoadTable(ObservableCollection<Model.CountryModel> countries)
        {
            CountryTable.Clear();

            foreach (Model.CountryModel country in countries)
            {
                CountryTable.Add(country);
            }
        }


        public ObservableCollection<Model.CountryModel> LoadCountry()
        {
            try
            {
                ObservableCollection<Model.CountryModel> model = new ObservableCollection<Model.CountryModel>();

               

                using var con = new NpgsqlConnection(App.sc);
                con.Open();

                var sql = "SELECT * From Country ORDER BY _ID";

                using var cmd = new NpgsqlCommand(sql, con);

                var rcmd = cmd.ExecuteReader();
                while (rcmd.Read())
                {
                    model.Add( new Model.CountryModel((int)rcmd["_ID"], rcmd["_countryFull"].ToString(), rcmd["_countryShort"].ToString()) );
                }
                con.Close();

                return model;
            }
            catch(Exception e) { MessageBox.Show(e.Message); return null; }
        }

       




    }
}
