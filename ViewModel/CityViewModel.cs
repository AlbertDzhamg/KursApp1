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

namespace Курсовой_Будякова.ViewModel
{
    public class CityViewModel: INotifyPropertyChanged
    {             

        private CountryViewModel vmCountry = new CountryViewModel();                  
        private RegionViewModel vmRegion = new RegionViewModel();
        private Model.CityDBO selectedCityDpo;
        /// <summary>
        /// выделенные в списке данные по городу
        /// </summary>
        public Model.CityDBO SelectedCityDpo

        {
            get { return selectedCityDpo; }
            set
            {
                selectedCityDpo = value;
                OnPropertyChanged("SelectedCityDpo");
                
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName]
            string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        }

        public ObservableCollection<Model.CityModel> CityTable { get; set; } =
            new ObservableCollection<Model.CityModel>();
        public ObservableCollection<Model.CityDBO> CityTableDpo { get; set; } =
            new ObservableCollection<Model.CityDBO>();

        public CityViewModel()
        {
            CityTable=Loadcity();
            CityTableDpo = GetListPersonDpo();
        }
        public ObservableCollection<Model.CityDBO> GetListPersonDpo()
        {
            foreach (var city in CityTable)
            {
                Model.CityDBO p = new Model.CityDBO();
                p = p.CopyFromPerson(city);
                CityTableDpo.Add(p);
            }
            return CityTableDpo;
        }
        public int MaxId()
        {
            int max = 0;
            foreach (var r in this.CityTable)
            {
                if (max < r._ID)
                {
                    max = r._ID;
                };
            }
            return max;
        }




        private Helper.RelayCommand addCity;
        public Helper.RelayCommand AddCity
        {
            get
            {
                return addCity ??
                (addCity = new Helper.RelayCommand(obj =>
                {
                    View.CityAdd wnCity = new View.CityAdd
                    {
                        Title = "Новый город"
                    };
                    int maxIdCity = MaxId() + 1;
                    Model.CityDBO reg = new Model.CityDBO
                    {

                        _ID = maxIdCity
                    };
                    wnCity.DataContext = reg;
                     wnCity.cbRegion.ItemsSource = vmRegion.RegionTable;
                    if (wnCity.ShowDialog() == true)
                    {
                        Model.RegionModel r = (Model.RegionModel)wnCity.cbRegion.SelectedValue;
                        reg._RegionID = r._Region;
                        CityTableDpo.Add(reg);
                        Model.CityModel p = new Model.CityModel();
                        p = p.CopyFromPersonDPO(reg);
                        try
                        {
                            using var con = new NpgsqlConnection(App.sc);
                            con.Open();

                            var sql = "INSERT INTO City (_ID, _regionID, _City)" +
                                          " VALUES(" + p._ID + ", '" + p._RegionID + "', '" + p._City + "')" +
                                          " ON CONFLICT(_ID) DO UPDATE" +
                                          " SET _regionID = excluded._regionID," +
                                          " _City = excluded._City; ";

                            using var cmd = new NpgsqlCommand(sql, con);

                            cmd.ExecuteNonQuery();
                            con.Close();
                            CityTable.Add(p);

                        }
                        catch (Exception e) { MessageBox.Show(e.Message); }
                        
                    }
                   
                },
                (obj) => true));
            }
        }
              

        private Helper.RelayCommand _editCity;
        public Helper.RelayCommand EditCity
        {
            get
            {
                return _editCity ??
                       (_editCity = new Helper.RelayCommand(obj =>
                       {
                           View.CityAdd wnCity = new View.CityAdd()
                           {
                               Title = "Редактирование данных города",
                           };
                           Model.CityDBO cityDPO = SelectedCityDpo;
                           var tempcity = cityDPO.ShallowCopy();
                           wnCity.DataContext = tempcity;
                           wnCity.cbRegion.ItemsSource = vmRegion.RegionTable;
                           Helper.FindCity finder = new Helper.FindCity(tempcity._ID);
                           Model.CityModel city = CityTable.ToList().Find(new Predicate<Model.CityModel>(finder.CityPredicate));
                           Model.RegionModel personRole = vmRegion.RegionTable.ToList()
                               .Find(new Predicate<Model.RegionModel>(reg => reg._ID == city._RegionID));
                           wnCity.cbRegion.SelectedItem = personRole;
                           if (wnCity.ShowDialog() == true)
                           {
                               // сохранение данных в оперативной памяти
                               // перенос данных из временного класса в класс отображения данных 
                               var r = (Model.RegionModel)wnCity.cbRegion.SelectedValue;
                               if (r != null)
                               {
                                   cityDPO._RegionID = r._Region;
                                   cityDPO._City = tempcity._City;
                                   // перенос данных из класса отображения данных

                                   var per =CityTable.FirstOrDefault(p => p._ID == cityDPO._ID);
                                   if (per != null)
                                   {
                                       try
                                       {
                                           using var con = new NpgsqlConnection(App.sc);
                                       con.Open();
                                       per = per.CopyFromPersonDPO(cityDPO);
                                       var sql = "INSERT INTO City (_ID, _regionID, _City)" +
                                         " VALUES(" + per._ID + ", '" + per._RegionID + "', '" + per._City + "')" +
                                         " ON CONFLICT(_ID) DO UPDATE" +
                                         " SET _regionID = excluded._regionID," +
                                         " _City = excluded._City; ";

                                       using var cmd = new NpgsqlCommand(sql, con);

                                       cmd.ExecuteNonQuery();
                                       con.Close();
                                       }
                                       catch (Exception e) { MessageBox.Show(e.Message); }
                                   }
                                  
                               }
                               else
                               {
                                   //Message = "Необходимо выбрать город.";
                               }
                           }
                       }, (obj) => SelectedCityDpo != null && CityTableDpo.Count > 0));
            }
        }

        private Helper.RelayCommand deleteCity;
        public Helper.RelayCommand DeleteCity
        {
            get
            {
                return deleteCity ??
                (deleteCity = new Helper.RelayCommand(obj =>
                {
                    Model.CityDBO city = SelectedCityDpo;
                    MessageBoxResult result = MessageBox.Show("Удалить" +
                        " данные по городу: \n" + city._City,
                        "Предупреждение", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.OK)
                    {
                        CityTableDpo.Remove(city);
                        // поиск удаляемого класса в коллекции
                        var per = CityTable.FirstOrDefault(p => p._ID == city._ID);
                        if (per != null)
                        {
                            try
                            {
                                using var con = new NpgsqlConnection(App.sc);
                            con.Open();

                            var sql = "DELETE FROM City WHERE _ID = " + per._ID;

                            using var cmd = new NpgsqlCommand(sql, con);

                            cmd.ExecuteNonQuery();
                            con.Close();
                            CityTable.Remove(per);
                            }
                            catch (Exception e) { MessageBox.Show(e.Message); }
                        }
                    }
                   
                }, (obj) => SelectedCityDpo != null && CityTableDpo.Count > 0));
            }
        }


        private Helper.RelayCommand getCity;

        public Helper.RelayCommand GetCity
        {
            get
            {
                return getCity ??
                (getCity = new Helper.RelayCommand(obj =>
                {
                    CityTable = Loadcity();
                    CityTableDpo = GetListPersonDpo();
                },
                (obj) => true));
            }
        }


        public ObservableCollection<Model.CityModel> Loadcity()
        {
            try
            {
                ObservableCollection<Model.CityModel> model = new ObservableCollection<Model.CityModel>();



                using var con = new NpgsqlConnection(App.sc);
                con.Open();

                var sql = "SELECT * From City ORDER BY _ID";

                using var cmd = new NpgsqlCommand(sql, con);

                var rcmd = cmd.ExecuteReader();
                while (rcmd.Read())
                {
                    model.Add(new Model.CityModel((int)rcmd["_ID"], (int)rcmd["_regionID"], rcmd["_City"].ToString()));
                }
                con.Close();

                return model;
            }
            catch (Exception e) { MessageBox.Show(e.Message); return null; }
        }

        

    }
}
