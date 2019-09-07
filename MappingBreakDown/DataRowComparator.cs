using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HierarchicalGrid
{
    class DataRowComparator : IEqualityComparer<DataRow>
    {
        public bool Equals(DataRow x, DataRow y)
        {
            if (x.ItemArray.Length != y.ItemArray.Length)
                return false;

            if (x.ItemArray.Length == 1)
                return x.Field<string>("Group").Equals(y.Field<string>("Group"));
            else
                return x.Field<string>("Group").Equals(y.Field<string>("Group")) &&
                   x.Field<int>("Index").Equals(y.Field<int>("Index")) &&
                   x.Field<int>("SecondaryIndex").Equals(y.Field<int>("SecondaryIndex"));
        }

        public int GetHashCode(DataRow obj)
        {
            return obj["Group"].GetHashCode();
        }
    }
}
