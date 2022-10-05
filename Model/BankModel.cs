using Hangfire.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace Kurs.Model
{
    public class BankModel : INotifyPropertyChanged
    {
        
        public int _Id { get; set; }
        private string _namefull { get; set; }
        public string _Namefull {
            get { return _namefull; }
            set{ _namefull = value; OnPropertyChanged("_Namefull"); }
        }
        private string _nameshort { get; set; }
        public string _Nameshort
        {
            get { return _nameshort; }
            set { _nameshort = value; OnPropertyChanged("_Nameshort"); }
        }

        private string _inn { get; set; }
        public string _Inn
        {
            get { return _inn; }
            set { _inn = value; OnPropertyChanged("_Inn"); }
        }
        private string _bik { get; set; }
        public string _Bik
        {
            get { return _bik; }
            set { _bik = value; OnPropertyChanged("_Bik"); }
        }
        private string _coraccount { get; set; }
        public string _Coraccount
        {
            get { return _coraccount; }
            set { _coraccount = value; OnPropertyChanged("_Coraccount"); }
        }

        private string _accuont { get; set; }
        public string _Account
        {
            get { return _accuont; }
            set { _accuont = value; OnPropertyChanged("_Account"); }
        }

        private string _city { get; set; }
        public string _City
        {
            get { return _city; }
            set { _city = value; OnPropertyChanged("_City"); }
        }




        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName]
            string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        }

        public BankModel(){ }

        public BankModel(int Id, string namefull, string nameshort, string inn, string bik, string coraccount, string account, string city)
        {
            this._Id = Id;
            this._Namefull = namefull;
            this._Nameshort = nameshort;
            this._Inn = inn;
            this._Bik = bik;
            this._Coraccount = coraccount;
            this._Account = account;
            this._City = city;
        }


        public BankModel ShallowCopy()
        {
            return (BankModel)this.MemberwiseClone();
        }



    }
}
