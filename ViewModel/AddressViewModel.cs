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

namespace Курсовой_Будякова.ViewModel
{
    class AddressViewModel : INotifyPropertyChanged
    {        
        private CountryViewModel vmCountry = new CountryViewModel();          
        private CityViewModel vmCity = new CityViewModel();
        private Model.AddressDBO selectedAddressDpo;
        /// <summary>
        /// выделенные в списке данные по адресу
        /// </summary>
        public Model.AddressDBO SelectedAddressDpo

        {
            get { return selectedAddressDpo; }
            set
            {
                selectedAddressDpo = value;
                OnPropertyChanged("SelectedAddressDpo");
                
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName]
            string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ObservableCollection<Model.AddressModel> AddressTable { get; set; } 
        public ObservableCollection<Model.AddressDBO> AddressTableDpo { get; set; } 
        public AddressViewModel()
        {
            AddressTableDpo = new ObservableCollection<Model.AddressDBO>();
            AddressTable = LoadAddress();
            AddressTableDpo = GetListAddressDpo();
        }
        public ObservableCollection<Model.AddressDBO> GetListAddressDpo()
        {
            foreach (var person in AddressTable)
            {
                Model.AddressDBO p = new Model.AddressDBO();
                p = p.CopyFromAdrress(person);
                AddressTableDpo.Add(p);
            }
            return AddressTableDpo;
        }
        public int MaxId()
        {
            int max = 0;
            foreach (var r in this.AddressTable)
            {
                if (max < r._ID)
                {
                    max = r._ID;
                };
            }
            return max;
        }


        private Helper.RelayCommand addAddress;
        /// <summary>
        /// добавление адреса
        /// </summary>
        public Helper.RelayCommand AddAddress
        {
            get
            {
                return addAddress ??
                (addAddress = new Helper.RelayCommand(obj =>
                {
                    View.AddressAdd wnAddress = new View.AddressAdd
                    {
                        Title = "Новый адрес"
                    };
                    int maxIdAddress = MaxId() + 1;
                    Model.AddressDBO reg = new Model.AddressDBO
                    {

                        _ID = maxIdAddress
                    };
                    wnAddress.cbCity.ItemsSource = new CityViewModel().CityTable;
                    wnAddress.DataContext = reg;
                    if (wnAddress.ShowDialog() == true)
                    {

                        try
                        {
                            Model.CityModel r = (Model.CityModel)wnAddress.cbCity.SelectedValue;
                            reg._cityID = r._City;
                            AddressTableDpo.Add(reg);
                            Model.AddressModel p = new Model.AddressModel();
                            p = p.CopyFromPersonDPO(reg);
                            using var con = new NpgsqlConnection(App.sc);
                            con.Open();
                            var sql = "INSERT INTO Address (_ID, _cityID, _person,_street,_bulding,_office)" +
                                      " VALUES(" + p._ID + ", '" + p._CityID + "', '" + p._Person + "', '" + p._Street + "', '" + p._Bulding + "', '" + p._Office + "')" +
                                      " ON CONFLICT(_ID) DO UPDATE" +
                                      " SET _cityID = excluded._cityID," +
                                      " _person = excluded._person, " +
                                      " _street = excluded._street, " +
                                      " _bulding = excluded._bulding, " +
                                      " _office = excluded._office; ";

                            using var cmd = new NpgsqlCommand(sql, con);
                            cmd.ExecuteNonQuery();
                            con.Close();
                            AddressTable.Add(p);
                        }
                        catch (Exception e) { MessageBox.Show(e.Message); }
                       
                        
                    }
                   
                },
                (obj) => true));
            }
        }


        private Helper.RelayCommand editAddress;
        public Helper.RelayCommand EditAddress
        {
            get
            {
                return editAddress ??
                (editAddress = new Helper.RelayCommand(obj =>
                {

                    View.AddressAdd wnAddress = new View.AddressAdd()
                    {
                        Title = "Редактирование данных адреса",
                    };
                    Model.AddressDBO AddressDpo = SelectedAddressDpo;
                    Model.AddressDBO tempAddress = new Model.AddressDBO();
                    tempAddress = AddressDpo.ShallowCopy();

                    wnAddress.DataContext = tempAddress;

                    wnAddress.cbCity.ItemsSource = vmCity.CityTable;
                    Helper.FindAddress finder = new Helper.FindAddress(tempAddress._ID);
                    Model.AddressModel address = AddressTable.ToList().Find(new Predicate<Model.AddressModel>(finder.AdderssPredicate));
                    Model.CityModel personRole = vmCity.CityTable.ToList()
                        .Find(new Predicate<Model.CityModel>(citi => citi._ID == address._CityID));
                    wnAddress.cbCity.SelectedItem = personRole;

                    if (wnAddress.ShowDialog() == true)
                    {
                        try
                        {
                            var r = (Model.CityModel)wnAddress.cbCity.SelectedValue;
                            AddressDpo._CityID = r._City;
                            AddressDpo._Street = tempAddress._Street;
                            AddressDpo._Person = tempAddress._Person;
                            AddressDpo._Office = tempAddress._Office;
                            AddressDpo._Bulding = tempAddress._Bulding;
                            AddressDpo._ID = tempAddress._ID;
                            var rtemp = AddressTable;
                            var per = AddressTable.FirstOrDefault(p => p._ID == AddressDpo._ID);
                            if (per != null)
                            {
                                try
                                {
                                    using var con = new NpgsqlConnection(App.sc);
                                con.Open();

                                per = per.CopyFromPersonDPO(AddressDpo);
                                var sql = "INSERT INTO Address (_ID, _cityID, _person,_street,_bulding,_office)" +
                                          " VALUES(" + per._ID + ", '" + per._CityID + "', '" + per._Person + "', '" + per._Street + "', '" + per._Bulding + "', '" + per._Office + "')" +
                                          " ON CONFLICT(_ID) DO UPDATE" +
                                          " SET _cityID = excluded._cityID," +
                                          " _person = excluded._person, " +
                                          " _street = excluded._street, " +
                                          " _bulding = excluded._bulding, " +
                                          " _office = excluded._office; ";

                                using var cmd = new NpgsqlCommand(sql, con);
                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                }
                                catch (Exception e) { MessageBox.Show(e.Message); }
                            }

                        }
                        catch (Exception e) { MessageBox.Show(e.Message); }


                    }
                    
                }, (obj) => SelectedAddressDpo != null && AddressTableDpo.Count > 0));
            }
        }



        private Helper.RelayCommand deleteAddress;
        public Helper.RelayCommand DeleteAddress
        {
            get
            {
                return deleteAddress ??
                (deleteAddress = new Helper.RelayCommand(obj =>
                {
                    Model.AddressDBO address = SelectedAddressDpo;
                    MessageBoxResult result = MessageBox.Show("Удалить" +
                        " данные по адресу: \n" + address._Person,
                        "Предупреждение", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.OK)
                    {
                        AddressTableDpo.Remove(address);
                        
                        var adr = AddressTable.FirstOrDefault(p => p._ID == address._ID);
                        if (adr != null)
                        {
                            try
                            {
                                using var con = new NpgsqlConnection(App.sc);
                            con.Open();

                            var sql = "DELETE FROM Address WHERE _ID = " + adr._ID;

                            using var cmd = new NpgsqlCommand(sql, con);

                            cmd.ExecuteNonQuery();
                            con.Close();
                           
                            AddressTable.Remove(adr);
                                // сохраненее данных в файле json
                            }
                            catch (Exception e) { MessageBox.Show(e.Message); }
                        }


                    }
                    
                }, (obj) => SelectedAddressDpo != null && AddressTableDpo.Count > 0));
            }
        }


        public ObservableCollection<Model.AddressModel> LoadAddress()
        {
            try
            {
                ObservableCollection<Model.AddressModel> model = new ObservableCollection<Model.AddressModel>();


                using var con = new NpgsqlConnection(App.sc);
                con.Open();

                var sql = "SELECT * From Address ORDER BY _ID";

                using var cmd = new NpgsqlCommand(sql, con);

                var rcmd = cmd.ExecuteReader();
                while (rcmd.Read())
                {
                    model.Add(new Model.AddressModel((int)rcmd["_ID"],rcmd["_person"].ToString(), (int)rcmd["_cityID"], rcmd["_street"].ToString(), rcmd["_bulding"].ToString(),rcmd["_office"].ToString()));
                }
                con.Close();

                return model;
            }
            catch (Exception e) { MessageBox.Show(e.Message); return null; }
        }

        
    }
}
