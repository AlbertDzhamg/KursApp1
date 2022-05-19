using Hangfire.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Курсовой_Будякова.Model
{
    public class AddressModel : INotifyPropertyChanged
    {
        
        public int _ID { get; set; }
        private string _person { get; set; }
        public string _Person {
            get { return _person; }
            set
            {
                _person = value;
                OnPropertyChanged("_Person");
            }
        }
        private int _cityID { get; set; }
        public int _CityID {
            get { return _cityID; }
            set
            {
                _cityID = value;
                OnPropertyChanged("_CityID");
            }
        }
        private string _street { get; set; }
        public string _Street {
            get { return _street; }
            set
            {
                _street = value;
                OnPropertyChanged("_Street");
            }
        }
        private string _bulding { get; set; }
        public string _Bulding {
            get { return _bulding; }
            set
            {
                _bulding = value;
                OnPropertyChanged("_Bulding");
            }
        }
        private string _office { get; set; }
        public string _Office {
            get { return _office; }
            set
            {
                _office = value;
                OnPropertyChanged("_Office");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName]
            string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        }

        public AddressModel()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID">суррогатный ключ</param>
        /// <param name="person">Клиент</param>
        /// <param name="cityID">внешний ключ для связи с моделью City</param>
        /// <param name="street">наименование улицы</param>
        /// <param name="bulding">номер строения, дома</param>
        /// <param name="office">номер офиса</param>
        public AddressModel(int ID, string person, int cityID, string street, string bulding, string office)
        {
            _ID = ID;
            _Person = person;
            _CityID = cityID;
            _Street = street;
            _Bulding = bulding;
            _Office = office;
        }
        public AddressModel CopyFromPersonDPO(AddressDBO cytyModel)
        {
            AddressModel AddressDPO = new AddressModel();
            ViewModel.CityViewModel vmCity = new ViewModel.CityViewModel();
            int city =0;
            foreach (var r in vmCity.CityTable)
            {
                if (r._City == cytyModel._CityID)
                {
                    city = r._ID;
                    break;
                }
            }

            

            this._ID      = cytyModel._ID;
            this._Person  = cytyModel._Person;
            this._CityID  = city;
            this._Street  = cytyModel._Street;
            this._Bulding = cytyModel._Bulding;
            this._Office  = cytyModel._Office;

            return this;
        }

    }
}
