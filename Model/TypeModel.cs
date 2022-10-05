using Hangfire.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Kurs.Model
{
    public class TypeModel : INotifyPropertyChanged
    {
        public int _Id { get; set; }
        public string _type { get; set; }
        public string _Type
        {
            get { return _type; }
            set
            {
                _type = value;
                OnPropertyChanged("_Type");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName]
            string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        }
        public TypeModel()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iD">суррогатный ключ</param>
        /// <param name="countryID">внешний ключ для связи с моделью Country</param>
        /// <param name="region">регион</param>
        public TypeModel(int ID, string countryID)
        {
            _Id = ID;
            _Type = countryID;
        }

        public TypeModel ShallowCopy()
        {
            return (TypeModel)this.MemberwiseClone();
        }

    }
}
