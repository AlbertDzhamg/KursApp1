
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.IO;
using Npgsql;
using System.Windows;
using Hangfire.Annotations;
using Newtonsoft.Json;

namespace Курсовой_Будякова.ViewModel
{
    public class RegionViewModel : INotifyPropertyChanged
    {
          
       
        private CountryViewModel vmCountry = new CountryViewModel();
        private Model.RegionDBO selectedRegionDpo;
        /// <summary>
        /// выделенные в списке данные по региону
        /// </summary>
        public Model.RegionDBO SelectedRegionDpo

        {
            get { return selectedRegionDpo; }
            set
            {
                selectedRegionDpo = value;
                OnPropertyChanged("SelectedRegionDpo");
                EditRegion.CanExecute(true);
                DeleteRegion.CanExecute(true);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName]
            string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<Model.RegionModel> RegionTable { get; set; } =
            new ObservableCollection<Model.RegionModel>();
        public ObservableCollection<Model.RegionDBO> RegionTableDpo { get; set; } = 
            new ObservableCollection<Model.RegionDBO>();



        public RegionViewModel()
        {
            RegionTable= LoadRole();
        }

        public ObservableCollection<Model.RegionDBO> GetListPersonDpo()
        {
            RegionTableDpo.Clear();

            foreach (var person in RegionTable)
            {
                Model.RegionDBO p = new Model.RegionDBO();
                p = p.CopyFromPerson(person);
                RegionTableDpo.Add(p);
            }
            return RegionTableDpo;
        }

        public int MaxId()
        {
            int max = 0;
            foreach (var r in this.RegionTable)
            {
                if (max < r._ID)
                {
                    max = r._ID;
                };
            }
            return max;
        }




        private Helper.RelayCommand addRegion;
        /// <summary>
        /// добавление региона
        /// </summary>
        public Helper.RelayCommand AddRegion
        {
            get
            {
                return addRegion ??
                (addRegion = new Helper.RelayCommand(obj =>
                {
                    View.RegionAdd wnRegion = new View.RegionAdd
                    {
                        Title = "Новый регион"
                    };
                    int maxIdRegion = MaxId() + 1;
                    Model.RegionDBO reg = new Model.RegionDBO
                    {

                        _ID = maxIdRegion
                    };
                    wnRegion.cbCountry.ItemsSource = new CountryViewModel().LoadCountry();
                    wnRegion.DataContext = reg;
                    if (wnRegion.ShowDialog() == true)
                    {
                        Model.CountryModel r = (Model.CountryModel)wnRegion.cbCountry.SelectedValue;
                        reg._CountryID = r._CountryShort;
                        RegionTableDpo.Add(reg);
                        Model.RegionModel p = new Model.RegionModel();
                        p = p.CopyFromPersonDPO(reg);
                        try
                        {
                            using var con = new NpgsqlConnection(App.sc);
                            con.Open();

                            var sql = "INSERT INTO Region (_ID, _countryID, _region)" +
                                      " VALUES(" + p._ID + ", '" + p._CountryID + "', '" + p._Region + "')" +
                                      " ON CONFLICT(_ID) DO UPDATE" +
                                      " SET _countryID = excluded._countryID," +
                                      " _region = excluded._region; ";

                            using var cmd = new NpgsqlCommand(sql, con);

                            cmd.ExecuteNonQuery();
                            con.Close();
                            RegionTable.Add(p);

                        }
                        catch (Exception e) { MessageBox.Show(e.Message); }                        
                    }
                   
                },
                (obj) => true));
            }
        }


        private Helper.RelayCommand editRegion;
        public Helper.RelayCommand EditRegion
        {
            get
            {
                return editRegion ??
                (editRegion = new Helper.RelayCommand(obj =>
                {
                    
                    View.RegionAdd wnRegion = new View.RegionAdd()
                    {
                        Title = "Редактирование данных региона",
                    };
                    Model.RegionDBO regionDpo = SelectedRegionDpo;
                    Model.RegionDBO tempPerson = new Model.RegionDBO();
                    tempPerson = regionDpo.ShallowCopy();
                    
                    wnRegion.DataContext = tempPerson;
                    ObservableCollection<Model.CountryModel> countryList = vmCountry.LoadCountry();
                    wnRegion.cbCountry.ItemsSource = countryList;
                    Helper.FindRegion finder = new Helper.FindRegion(tempPerson._ID);
                    Model.RegionModel region = RegionTable.ToList().Find(new Predicate<Model.RegionModel>(finder.RegionPredicate));
                    Model.CountryModel regionRole = countryList.ToList()
                        .Find(new Predicate<Model.CountryModel>(role => role._ID == region._CountryID));
                    wnRegion.cbCountry.SelectedItem = regionRole;

                    if (wnRegion.ShowDialog() == true)
                    {
                        var r = (Model.CountryModel)wnRegion.cbCountry.SelectedValue;
                        regionDpo._CountryID = r._CountryShort;
                        regionDpo._Region = tempPerson._Region;
                        regionDpo._ID = tempPerson._ID;
                        var rtemp = RegionTable;
                        var per = RegionTable.FirstOrDefault(p => p._ID == regionDpo._ID);
                        if (per != null)
                        {
                            try
                            {
                                using var con = new NpgsqlConnection(App.sc);
                                con.Open();

                                per = per.CopyFromPersonDPO(regionDpo);
                                var sql = "INSERT INTO Region (_ID, _countryID, _region)" +
                                          " VALUES(" + per._ID + ", '" + per._CountryID + "', '" + per._Region + "')" +
                                          " ON CONFLICT(_ID) DO UPDATE" +
                                          " SET _countryID = excluded._countryID," +
                                          " _region = excluded._region; ";

                                using var cmd = new NpgsqlCommand(sql, con);

                                cmd.ExecuteNonQuery();
                                con.Close();
                                
                                
                            }
                            catch (Exception e) { MessageBox.Show(e.Message); }
                           
                        }                    

                    }
                }, (obj) => SelectedRegionDpo != null && RegionTableDpo.Count > 0));
            }
        }



        private Helper.RelayCommand deleteRegion;
        public Helper.RelayCommand DeleteRegion
        {
            get
            {
                return deleteRegion ??
                (deleteRegion = new Helper.RelayCommand(obj =>
                {
                    Model.RegionDBO region = SelectedRegionDpo;
                    MessageBoxResult result = MessageBox.Show("Удалить" +
                        " данные по региону: \n" + region._Region ,
                        "Предупреждение", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.OK)
                    {

                        RegionTableDpo.Remove(region);
                        // поиск удаляемого класса в коллекции 
                        var per = RegionTable.FirstOrDefault(p => p._ID == region._ID);
                        if (per != null)
                        {
                            try
                            {
                                using var con = new NpgsqlConnection(App.sc);
                                con.Open();

                                var sql = "DELETE FROM Region WHERE _ID = " + per._ID;

                                using var cmd = new NpgsqlCommand(sql, con);

                                cmd.ExecuteNonQuery();
                                con.Close();
                                RegionTable.Remove(per);
                            }
                            catch (Exception e) { MessageBox.Show(e.Message); }                          
                                                       
                        }
                    }
                   
                }, (obj) => SelectedRegionDpo != null && RegionTableDpo.Count > 0));
            }
        }


        private Helper.RelayCommand getRegion;
        public Helper.RelayCommand GetRegion
        {
            get
            {
                return getRegion ??
                (getRegion = new Helper.RelayCommand(obj =>
                {
                    RegionTable = LoadRole();
                    RegionTableDpo = GetListPersonDpo();
                },
                (obj) => true));
            }
        }


        public ObservableCollection<Model.RegionModel> LoadRole()
        {
            try
            {
                ObservableCollection<Model.RegionModel> model = new ObservableCollection<Model.RegionModel>();



                using var con = new NpgsqlConnection(App.sc);
                con.Open();

                var sql = "SELECT * From Region ORDER BY _ID";

                using var cmd = new NpgsqlCommand(sql, con);

                var rcmd = cmd.ExecuteReader();
                while (rcmd.Read())
                {
                    model.Add(new Model.RegionModel((int)rcmd["_ID"], (int)rcmd["_countryID"], rcmd["_region"].ToString()));
                }
                con.Close();

                return model;
            }
            catch (Exception e) { MessageBox.Show(e.Message); return null; }
        }

        
    }
}
