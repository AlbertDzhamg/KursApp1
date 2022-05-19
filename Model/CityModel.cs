using Hangfire.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Курсовой_Будякова.Model
{
    public class CityModel : INotifyPropertyChanged
    {
        
        public int _ID { get; set; }
        public int _regionID { get; set; }
        public int _RegionID
        {
            get { return _regionID; }
            set
            {
                _regionID = value;
                OnPropertyChanged("_RegionID");
            }
        }
        private string _city { get; set; }
        public string _City {
            get { return _city; }
            set
            {
                _city = value;
                OnPropertyChanged("_City");
            }
        }

        public CityModel()
        {
        }
        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName]
            string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID">суррогатный ключ</param>
        /// <param name="regionID">внешний ключ для связи с моделью Region</param>
        /// <param name="city">город</param>
        public CityModel(int ID, int regionID, string city)
        {
            _ID = ID;
            _RegionID = regionID;
            _City = city;
        }

        public override bool Equals(object obj)
        {
            return obj is CityModel model;
        }
        public CityModel CopyFromPersonDPO(CityDBO Reg)
        {
            ViewModel.RegionViewModel vmCountry = new ViewModel.RegionViewModel();
            int CountryId = 0;
            foreach (var r in vmCountry.RegionTable)
            {
                if (r._Region == Reg._RegionID)
                {
                    CountryId = r._ID;
                    break;
                }
            }
            if (CountryId != 0)
            {
                this._ID = Reg._ID;
                this._RegionID = CountryId;
                this._City = Reg._City;

            }
            return this;
        }
    }
}
