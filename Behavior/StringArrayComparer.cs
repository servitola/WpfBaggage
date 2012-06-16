using System;
using System.Collections.Generic;
using System.Linq;

namespace WpfBaggage.Behavior
{
    class StringArrayComparer : IEqualityComparer<string[]>
    {
        public bool Equals(string[] x, string[] y)
        {
            for (var i = 0; i < Math.Min(x.Count(),y.Count()); i++)
                if (x[i] != y[i]) return false;
            return true;
        }

        public int GetHashCode(string[] obj)
        {
            throw new NotImplementedException();
        }

        public static StringArrayComparer Instance = new StringArrayComparer();
    }
}
