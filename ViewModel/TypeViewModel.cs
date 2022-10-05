
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

namespace Kurs.ViewModel
{
    public class TypeViewModel : INotifyPropertyChanged
    {


        private Model.TypeModel selectedType;
        public Model.TypeModel SelectedType
        {
            get
            {
                return selectedType;
            }
            set
            {
                selectedType = value;
                OnPropertyChanged("SelectedType");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName]
            string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<Model.TypeModel> TypeTable { get; set; } =
            new ObservableCollection<Model.TypeModel>();

        public ObservableCollection<Model.TypeModel> TypeTableDpo { get; set; } =
            new ObservableCollection<Model.TypeModel>();


        public TypeViewModel()
        {
            TypeTable = LoadType();
        }
/*
        public ObservableCollection<Model.TypeModel> GetListPersonDpo()
        {
            TypeTableDpo.Clear();

            foreach (var person in TypeTable)
            {
                Model.TypeModel p = new Model.TypeModel();
                p = p.CopyFromPerson(person);
                TypeTableDpo.Add(p);
            }
            return TypeTableDpo;
        }
*/
        public int MaxId()
        {
            int max = 0;
            foreach (var r in this.TypeTable)
            {
                if (max < r._Id)
                {
                    max = r._Id;
                };
            }
            return max;
        }




        private Helper.RelayCommand addType;
        public Helper.RelayCommand AddType
        {
            get
            {
                return addType ??
                (addType = new Helper.RelayCommand(obj =>
                {
                    View.TypeAdd wnType = new View.TypeAdd
                    {
                        Title = "Новый тип счета",
                    };
                    int maxIdType = MaxId() + 1;
                    Model.TypeModel TypeB = new Model.TypeModel { _Id = maxIdType };
                    wnType.DataContext = TypeB;
                    if (wnType.ShowDialog() == true)
                    {
                        try
                        {
                            using var con = new NpgsqlConnection(App.sc);
                            con.Open();

                            /*
                        var sql = "INSERT INTO aggrement (_Id, _number, _dataOpen, _dataClose)" +
                                  " VALUES(" + Aggr._Id + ", '" + Aggr._Number + "', '" + (DateTime)Aggr._DataOpen + "','"+ (DateTime)Aggr._DataClose + "')" +
                                  " ON CONFLICT(_Id) DO UPDATE" +
                                  " SET _number = excluded._number," +
                                  " _dataOpen = excluded._dataOpen, " +
                                  " _dataClose = exclided._dataClose";

*/
                            var sql = "INSERT INTO public.typeaccount (\"_type\") VALUES ('" + TypeB._type.ToString() + "'::character varying) returning _id;";

                            using var cmd = new NpgsqlCommand(sql, con);

                            cmd.ExecuteNonQuery();
                            con.Close();
                            TypeTable.Add(TypeB);
                        }
                        catch (Exception e) { MessageBox.Show(e.Message); }
                    }

                    SelectedType = TypeB;

                }));
            }
        }



        private Helper.RelayCommand editType;
        public Helper.RelayCommand EditType
        {
            get
            {
                return editType ??
                (editType = new Helper.RelayCommand(obj =>
                {
                    View.TypeAdd wnType = new View.TypeAdd
                    { Title = "Редактирование типа счета", };
                    Model.TypeModel typeB = SelectedType;
                    Model.TypeModel tempType = new Model.TypeModel();
                    tempType = typeB.ShallowCopy();
                    wnType.DataContext = tempType;
                    if (wnType.ShowDialog() == true)
                    {
                        try
                        {
                            using var con = new NpgsqlConnection(App.sc);
                            con.Open();

                            var sql = "UPDATE public.typeaccount " +
                            "SET" +
                            "\"_type\" = '" + tempType._Type.ToString() + "'::character varying" +
                            " WHERE _id =" + tempType._Id + ";"; ;


                            using var cmd = new NpgsqlCommand(sql, con);

                            cmd.ExecuteNonQuery();
                            con.Close();
                            typeB._Type = tempType._Type;
                        }
                        catch (Exception e) { MessageBox.Show(e.Message); }
                    }

                }, (obj) => SelectedType != null && TypeTable.Count > 0));
            }
        }



        private Helper.RelayCommand deleteType;
        public Helper.RelayCommand DeleteType
        {
            get
            {
                return deleteType ??
                (deleteType = new Helper.RelayCommand(obj =>
                {
                    Model.TypeModel typeB = SelectedType;
                    MessageBoxResult result = MessageBox.Show("Удалить данные по типу счета: " +
                        typeB._Type, "Предупреждение", MessageBoxButton.OKCancel,
                        MessageBoxImage.Warning);
                    if (result == MessageBoxResult.OK)
                    {
                        try
                        {
                            using var con = new NpgsqlConnection(App.sc);
                            con.Open();

                            var sql = "DELETE FROM typeaccount WHERE _id = " + typeB._Id;

                            using var cmd = new NpgsqlCommand(sql, con);

                            cmd.ExecuteNonQuery();
                            con.Close();
                            TypeTable.Remove(typeB);
                        }
                        catch (Exception e) { MessageBox.Show(e.Message); }

                    }

                }, (obj) => SelectedType != null && TypeTable.Count > 0));
            }
        }


        private Helper.RelayCommand getType;
        public Helper.RelayCommand GetType
        {
            get
            {
                return getType ??
                (getType = new Helper.RelayCommand(obj =>
                {
                    LoadTable(LoadType());
                },
                (obj) => true));
            }
        }


        private void LoadTable(ObservableCollection<Model.TypeModel> typeaccs)
        {
            TypeTable.Clear();

            foreach (Model.TypeModel typeacc in typeaccs)
            {
                TypeTable.Add(typeacc);
            }
        }

        public ObservableCollection<Model.TypeModel> LoadType()
        {
            try
            {
                ObservableCollection<Model.TypeModel> model = new ObservableCollection<Model.TypeModel>();



                using var con = new NpgsqlConnection(App.sc);
                con.Open();

                var sql = "SELECT * From typeaccount ORDER BY _id";

                using var cmd = new NpgsqlCommand(sql, con);

                var rcmd = cmd.ExecuteReader();
                while (rcmd.Read())
                {
                    model.Add(new Model.TypeModel((int)rcmd["_Id"], rcmd["_type"].ToString()));
                }
                con.Close();

                return model;
            }
            catch (Exception e) { MessageBox.Show(e.Message); return null; }
        }

        
    }
}
