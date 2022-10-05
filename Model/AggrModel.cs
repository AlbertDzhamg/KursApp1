using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Runtime;
using Hangfire.Annotations;

namespace Kurs.Model
{

    public class AggrModel : INotifyPropertyChanged
    {
        
        public int _Id { get; set; }
        private int _number { get; set; }
        public int _Number {
            get { return _number; }
            set
            {
                _number = value;
                OnPropertyChanged("_Number");
            }
        }
        private DateTime _dataOpen { get; set; }
        public DateTime _DataOpen {
            get { return _dataOpen; }
            set
            {
                _dataOpen = value;
                OnPropertyChanged("_DataOpen");
            }
        }

        private DateTime _dataClose { get; set; }
        public DateTime _DataClose
        {
            get { return _dataClose; }
            set
            {
                _dataClose = value;
                OnPropertyChanged("_DataClose");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName]
            string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        }

        public AggrModel()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID">ID Страны</param>
        /// <param name="number">полное наименование страны</param>
        /// <param name="dataopen">краткое наименование страны</param>
        public AggrModel(int ID, int number, DateTime dataopen, DateTime dataclose)
        {
            this._Id = ID;
            this._Number = number;
            this._DataOpen = dataopen;
            this._DataClose = dataclose;

        }
        public AggrModel ShallowCopy()
        {
            return (AggrModel)this.MemberwiseClone();
        }
    }
}
