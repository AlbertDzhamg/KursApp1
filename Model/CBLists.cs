using Hangfire.Annotations;
using Kurs;
using Kurs.Model;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Kurs.Model
{
    public class BankModelList : INotifyPropertyChanged
    {
/// <summary>
/// ///
/// </summary>
        private int _id { get; set; }  
        public int _Id
        {
            get { return _id; }
            set
            { 
                _id = value;
                OnPropertyChanged("_Id");
            }
        }

        public BankModelList() { }

        public BankModelList(int Inn)
        {
            this._Id = Inn;

        }

        public List<int> LoadBankList()
        {
            try
            {
                List<int> model = new List<int>();

                using var con = new NpgsqlConnection(App.sc);
                con.Open();

                var sql = "SELECT _id From bank ORDER BY _id";

                using var cmd = new NpgsqlCommand(sql, con);

                var rcmd = cmd.ExecuteReader();
                while (rcmd.Read())
                {
                    model.Add((int)rcmd["_Id"]);
                }
                con.Close();

                return model;
            }
            catch (Exception e) { MessageBox.Show(e.Message); return null; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName]
            string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
    }
/// <summary>
/// //////////////
/// </summary>

    public class AggrementModelList : INotifyPropertyChanged
    {

        private int _id { get; set; }
        public int _Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("_Id");
            }
        }

        public AggrementModelList() { }

        public AggrementModelList(int Inn)
        {
            this._Id = Inn;

        }



        public List<int> LoadAggrementList()
        {
            try
            {
                List<int> model = new List<int>();

                using var con = new NpgsqlConnection(App.sc);
                con.Open();

                var sql = "SELECT _id From aggrement ORDER BY _id";

                using var cmd = new NpgsqlCommand(sql, con);

                var rcmd = cmd.ExecuteReader();
                while (rcmd.Read())
                {
                    model.Add((int)rcmd["_Id"]);
                }
                con.Close();

                return model;
            }
            catch (Exception e) { MessageBox.Show(e.Message); return null; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName]
            string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
    }
/// <summary>
/// ///////////////////
/// </summary>
    public class TypeModelList : INotifyPropertyChanged
    {

        private int _id { get; set; }
        public int _Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("_Id");
            }
        }

        public TypeModelList() { }

        public TypeModelList(int Inn)
        {
            this._Id = Inn;

        }



        public List<int> LoadTypeList()
        {
            try
            {
                List<int> model = new List<int>();

                using var con = new NpgsqlConnection(App.sc);
                con.Open();

                var sql = "SELECT _id From typeaccount ORDER BY _id";

                using var cmd = new NpgsqlCommand(sql, con);

                var rcmd = cmd.ExecuteReader();
                while (rcmd.Read())
                {
                    model.Add((int)rcmd["_Id"]);
                }
                con.Close();

                return model;
            }
            catch (Exception e) { MessageBox.Show(e.Message); return null; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName]
            string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
    }


}
