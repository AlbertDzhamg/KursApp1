using Hangfire.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Курсовой_Будякова.Model
{
    public class RegionDBO : INotifyPropertyChanged
    {
        public int _ID { get; set; }
        
        public string _countryID { get; set; }
        public string _CountryID
        {
            get { return _countryID; }
            set
            {
                _countryID = value;
                OnPropertyChanged("_CountryID");
            }
        }
        private string _region { get; set; }
        public string _Region {
            get { return _region; }
            set
            {
                _region = value;
                OnPropertyChanged("_Region");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName]
            string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
        public RegionDBO() { }

        public RegionDBO(int iD, string countryID, string region)
        {
            _ID = iD;
            _CountryID = countryID;
            _Region = region;
        }
        public RegionDBO ShallowCopy()
        {
            return (RegionDBO)this.MemberwiseClone();
        }

        public RegionDBO CopyFromPerson(RegionModel regionModel)
        {
            RegionDBO RegionDPO = new RegionDBO();
            ViewModel.CountryViewModel vmRegion = new ViewModel.CountryViewModel();
            string country = string.Empty;
            foreach (var r in vmRegion.LoadCountry())
            {
                if (r._ID == regionModel._CountryID)
                {
                    country = r._CountryShort;
                    break;
                }
            }

            RegionDPO._ID = regionModel._ID;
            RegionDPO._CountryID = country;
            RegionDPO._Region = regionModel._Region;
            if (country != string.Empty)
            { }
            return RegionDPO;
        }

    }

    public class CityDBO : INotifyPropertyChanged
    {
        public int _ID { get; set; }
        public string _regionID { get; set; }
        public string _RegionID {
            get { return _regionID; }
            set
            {
                _regionID = value;
                OnPropertyChanged("_RegionID");
            }
        }
        public string _City {
            get { return _city; }
            set
            {
                _city = value;
                OnPropertyChanged("_City");
            }
        }
        private string _city { get; set; }
        

public CityDBO() { }

        public CityDBO(int iD, string regionID, string city)
        {
            _ID = iD;
            _RegionID = regionID;
            _City = city;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName]
            string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

        public CityDBO ShallowCopy()
        {
            return (CityDBO)this.MemberwiseClone();
        }

        public CityDBO CopyFromPerson(CityModel cytyModel)
        {
            CityDBO CityDPO = new CityDBO();
            ViewModel.RegionViewModel vmRegion = new ViewModel.RegionViewModel();
            string country = string.Empty;
            foreach (var r in vmRegion.RegionTable)
            {
                if (r._ID == cytyModel._RegionID)
                {
                    country = r._Region;
                    break;
                }
            }

            CityDPO._ID = cytyModel._ID;
            CityDPO._RegionID = country;
            CityDPO._City = cytyModel._City;
            if (country != string.Empty)
            { }
            return CityDPO;
        }
    }

    public class AddressDBO : INotifyPropertyChanged
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

        public string _cityID { get; set; }
        public string _CityID
        {
            get { return _cityID; }
            set
            {
                _cityID = value;
                OnPropertyChanged("_CityID");
            }
        }

        public AddressDBO() { }

        public AddressDBO(int iD, 
            string person, 
            string cityID, 
            string street, 
            string bulding, 
            string office
            )
        {
            _ID = iD;
            _Person = person;
            _CityID = cityID;
            _Street = street;
            _Bulding = bulding;
            _Office = office;
        }

                

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName]
            string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

        public AddressDBO ShallowCopy()
        {
            return (AddressDBO)this.MemberwiseClone();
        }

        public AddressDBO CopyFromAdrress(AddressModel cytyModel)
        {
            AddressDBO AddressDPO = new AddressDBO();
            ViewModel.CityViewModel vmCity = new ViewModel.CityViewModel();
            string city = string.Empty;
            foreach (var r in vmCity.CityTable)
            {
                if (r._ID == cytyModel._CityID)
                {
                    city = r._City;
                    break;
                }
            }

            AddressDPO._ID = cytyModel._ID;
            AddressDPO._Person = cytyModel._Person;
            AddressDPO._CityID = city;
            AddressDPO._Street = cytyModel._Street;
            AddressDPO._Bulding = cytyModel._Bulding;
            AddressDPO._Office = cytyModel._Office;

            return AddressDPO;
        }
    }
}
