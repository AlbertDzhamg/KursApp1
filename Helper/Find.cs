using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Курсовой_Будякова.Helper
{
    class FindCountry
    {
        int id;
        public FindCountry(int id)
        {
            this.id = id;
        }
        public bool CountryPredicate(Model.CountryModel country)
        {
            return country._ID == id;
        }

    }
    class FindAddress
    {
        int id;
        public FindAddress(int id)
        {
            this.id = id;
        }
        public bool AdderssPredicate(Model.AddressModel country)
        {
            return country._ID == id;
        }

    }
    class FindRegion
    {
        int id;
        public FindRegion(int id)
        {
            this.id = id;
        }
        public bool RegionPredicate(Model.RegionModel region)
        {
            return region._ID == id;
        }

    }
    class FindCity
    {
        int id;
        public FindCity(int id)
        {
            this.id = id;
        }
        public bool CityPredicate(Model.CityModel city)
        {
            return city._ID == id;
        }

    }
}
