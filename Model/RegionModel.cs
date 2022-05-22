using Hangfire.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Курсовой_Будякова.Model
{
    public class RegionModel : INotifyPropertyChanged
    {
        public int _ID { get; set; }
        public int _countryID { get; set; }
        public int _CountryID
        {
            get { return _countryID; }
            set
            {
                _countryID = value;
                OnPropertyChanged("_CountryID");
            }
        }
        private string _region { get; set; }
        public string _Region
        {
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
        public RegionModel()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iD">суррогатный ключ</param>
        /// <param name="countryID">внешний ключ для связи с моделью Country</param>
        /// <param name="region">регион</param>
        public RegionModel(int ID, int countryID, string region)
        {
            _ID = ID;
            _CountryID = countryID;
            _Region = region;
        }

        public RegionModel CopyFromPersonDPO(RegionDBO Reg)
        {
            ViewModel.CountryViewModel vmCountry = new ViewModel.CountryViewModel();
            int CountryId = 0;
            foreach (var r in vmCountry.LoadCountry())
            {
                if (r._CountryShort == Reg._CountryID)
                {
                    CountryId = r._ID;
                    break;
                }
            }
            if (CountryId != 0)
            {
                this._ID = Reg._ID;
                this._CountryID = CountryId;
                this._Region = Reg._Region;
                
            }
            return this;
        }
        

    }
}
