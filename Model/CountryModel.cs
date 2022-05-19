using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Runtime;
using Hangfire.Annotations;

namespace Курсовой_Будякова.Model
{

    public class CountryModel : INotifyPropertyChanged
    {
        
        public int _ID { get; set; }
        private string _countryFull { get; set; }
        public string _CountryFull {
            get { return _countryFull; }
            set
            {
                _countryFull = value;
                OnPropertyChanged("_CountryFull");
            }
        }
        private string _countryShort { get; set; }
        public string _CountryShort {
            get { return _countryShort; }
            set
            {
                _countryShort = value;
                OnPropertyChanged("_CountryShort");
            }
        }




        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName]
            string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        }

        public CountryModel()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID">ID Страны</param>
        /// <param name="countryFull">полное наименование страны</param>
        /// <param name="countryShort">краткое наименование страны</param>
        public CountryModel(int ID, string countryFull, string countryShort)
        {
            this._ID = ID;
            this._CountryFull = countryFull;
            this._CountryShort = countryShort;
        }
        public CountryModel ShallowCopy()
        {
            return (CountryModel)this.MemberwiseClone();
        }
    }
}
