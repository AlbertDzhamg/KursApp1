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
using System.Globalization;
using System.Threading;

namespace Kurs.ViewModel
{

    public class AggrViewModel : INotifyPropertyChanged
    {
        private Model.AggrModel selectedAggr;
        public Model.AggrModel SelectedAggr
        {
            get
            {
                return selectedAggr;
            }
            set
            {
                selectedAggr = value;
                OnPropertyChanged("SelectedAggr");
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName]
            string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<Model.AggrModel> AggrTable { get; set; } =
            new ObservableCollection<Model.AggrModel>();   
        public ObservableCollection<Model.AggrModel> AggrTableDpo { get; set; } =
            new ObservableCollection<Model.AggrModel>();


        public AggrViewModel()
        {
            AggrTable = LoadAggr();
        }

     

        public int MaxId()
        {
            int max = 0;
            foreach (var r in this.AggrTable)
            {
                if (max < r._Id)
                {
                    max = r._Id;
                };
            }
            return max;
        }


        private Helper.RelayCommand addAggr;
        public Helper.RelayCommand AddAggr
        {
            get
            {
                return addAggr ??
                (addAggr = new Helper.RelayCommand(obj =>
                {
                    View.AggrAdd wnAggr = new View.AggrAdd
                    {
                        Title = "Новый договор",
                    };

                    int maxIdAggr = MaxId() + 1;
                    Model.AggrModel Aggr = new Model.AggrModel { _Id = maxIdAggr };
                    wnAggr.DataContext = Aggr;
                    Aggr._DataOpen = System.DateTime.Now;
                    Aggr._DataClose = System.DateTime.Now;
                    if (wnAggr.ShowDialog() == true)
                    {
                        try
                        {
                            using var con = new NpgsqlConnection(App.sc);
                        con.Open();


                            var sql = "INSERT INTO public.aggrement (\r\n_number, \"_dataOpen\", \"_dataClose\") VALUES ('"+ Aggr._Number +"'::integer, '" + (DateTime)Aggr._DataOpen + "'::date, '" + (DateTime)Aggr._DataClose + "'::date)\r\n returning _id;";

                            using var cmd = new NpgsqlCommand(sql, con);

                            cmd.ExecuteNonQuery();
                            con.Close();
                        AggrTable.Add(Aggr);
                        }
                        catch (Exception e) { MessageBox.Show(e.Message); }
                    }
                   
                    SelectedAggr = Aggr;
                   
                }));
            }
        }

        private Helper.RelayCommand editAggr;
        public Helper.RelayCommand EditAggr
        {
            get
            {
                return editAggr ??
                (editAggr = new Helper.RelayCommand(obj =>
                {
                    View.AggrAdd wnAggr = new View.AggrAdd
                    { Title = "Редактирование договора", };
                    Model.AggrModel aggr = SelectedAggr;
                    Model.AggrModel tempAggr = new Model.AggrModel();
                    tempAggr = aggr.ShallowCopy();
                    wnAggr.DataContext = tempAggr;
                    if (wnAggr.ShowDialog() == true)
                    {
                        try
                        {
                            using var con = new NpgsqlConnection(App.sc);
                            con.Open();

                            var sql = "UPDATE public.aggrement " +
                            "SET" +
                            "\"_dataClose\" = '" + (DateTime)tempAggr._DataClose + "'::date," +
                            "\"_dataOpen\" = '" + (DateTime)tempAggr._DataOpen + "'::date," +
                            "_number = '" + tempAggr._Number + "'::integer " +
                            " WHERE\r\n_id =" + tempAggr._Id + ";";


                            using var cmd = new NpgsqlCommand(sql, con);

                            cmd.ExecuteNonQuery();
                            con.Close();
                            aggr._DataOpen = tempAggr._DataOpen;
                            aggr._Number = tempAggr._Number;
                            aggr._DataClose = tempAggr._DataClose;
                        }
                        catch(Exception e) { MessageBox.Show(e.Message); }
                    }
                   
                }, (obj) => SelectedAggr != null && AggrTable.Count > 0));
            }
        }

        private Helper.RelayCommand deleteAggr;
        public Helper.RelayCommand DeleteAggr
        {
            get
            {
                return deleteAggr ??
                (deleteAggr = new Helper.RelayCommand(obj =>
                {
                    Model.AggrModel aggr = SelectedAggr;
                    MessageBoxResult result = MessageBox.Show("Удалить данные по договору: " +
                        aggr._Number, "Предупреждение", MessageBoxButton.OKCancel,
                        MessageBoxImage.Warning);
                    if (result == MessageBoxResult.OK)
                    {
                        try
                        {
                            using var con = new NpgsqlConnection(App.sc);
                            con.Open();

                            var sql = "DELETE FROM aggrement WHERE _id = " + aggr._Id;

                            using var cmd = new NpgsqlCommand(sql, con);

                            cmd.ExecuteNonQuery();
                            con.Close();
                            AggrTable.Remove(aggr);
                        }
                        catch (Exception e) { MessageBox.Show(e.Message); }
                        
                    }
                   
                }, (obj) => SelectedAggr != null && AggrTable.Count > 0));
            }
        }


        public Helper.RelayCommand aggrGet { get; set; }
        public Helper.RelayCommand AggrGet
        {
            get
            {
                return aggrGet ??
                (aggrGet = new Helper.RelayCommand(obj =>
                {
                    LoadTable(LoadAggr());
                },
                (obj) => true));
            }
        }

        private void LoadTable(ObservableCollection<Model.AggrModel> aggrement)
        {
            AggrTable.Clear();

            foreach (Model.AggrModel aggrenemt in aggrement)
            {
                AggrTable.Add(aggrenemt);
            }
        }


        public ObservableCollection<Model.AggrModel> LoadAggr()
        {
            try
            {
                ObservableCollection<Model.AggrModel> model = new ObservableCollection<Model.AggrModel>();

               

                using var con = new NpgsqlConnection(App.sc);
                con.Open();

                var sql = "SELECT * From aggrement ORDER BY _id";

                using var cmd = new NpgsqlCommand(sql, con);

                var rcmd = cmd.ExecuteReader();
                while (rcmd.Read())
                {
                    model.Add( new Model.AggrModel((int)rcmd["_id"], (int)rcmd["_number"], (DateTime)rcmd["_dataOpen"], (DateTime)rcmd["_dataClose"]));
                }
                con.Close();

                return model;
            }
            catch(Exception e) { MessageBox.Show(e.Message); return null; }
        }     

    }
}
