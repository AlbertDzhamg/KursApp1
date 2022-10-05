using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kurs.Helper
{
    class FindAggr
    {
        int id;
        public FindAggr(int id)
        {
            this.id = id;
        }
        public bool AggrPredicate(Model.AggrModel aggr)
        {
            return aggr._Id == id;
        }

    }
    class FindBank
    {
        int id;
        public FindBank(int id)
        {
            this.id = id;
        }
        public bool AdderssPredicate(Model.BankModel bank)
        {
            return bank._Id == id;
        }

    }
    class FindType
    {
        int id;
        public FindType(int id)
        {
            this.id = id;
        }
        public bool RegionPredicate(Model.TypeModel type)
        {
            return type._Id == id;
        }

    }
    class FindAccount
    {
        int id;
        public FindAccount(int id)
        {
            this.id = id;
        }
        public bool AccountPredicate(Model.AccountModel acc)
        {
            return acc._Id == id;
        }

    }
}
