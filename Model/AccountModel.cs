using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Runtime;
using Hangfire.Annotations;

namespace Kurs.Model
{
    public class AccountModel : INotifyPropertyChanged
    {
        public int _Id { get; set; }
        private int _bankID { get; set; }
        public int _BankID
        {
            get { return _bankID; }
            set
            {
                _bankID = value;
                OnPropertyChanged("_BankID");
            }
        }
        private int _aggrementID { get; set; }
        public int _AggrementID
        {
            get { return _aggrementID; }
            set
            {
                _aggrementID = value;
                OnPropertyChanged("_AggrementID");
            }
        }
        private int _typeID { get; set; }
        public int _TypeID
        {
            get { return _typeID; }
            set
            {
                _typeID = value;
                OnPropertyChanged("_TypeID");
            }
        }
        private string _account { get; set; }
        public string _Account
        {
            get { return _account; }
            set
            {
                _account = value;
                OnPropertyChanged("_Account");
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName]
            string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        }

        public AccountModel() { }
        public AccountModel(int id, int bankid, int aggrementid, int typeid, string account)
        {
            this._Id = id;
            this._BankID = bankid;
            this._AggrementID = aggrementid;
            this._TypeID = typeid;
            this._Account = account;
        }
        public AccountModel ShallowCopy()
        {
            return (AccountModel)this.MemberwiseClone();
        }

        public override bool Equals(object obj)
        {
            return obj is AccountModel model;
        }



    }
}

