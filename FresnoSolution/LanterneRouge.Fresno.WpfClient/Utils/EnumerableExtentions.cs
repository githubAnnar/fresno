﻿using System.Collections.Generic;
using System.Linq;

namespace LanterneRouge.Fresno.WpfClient.Utils
{
    public static class EnumerableExtentions
    {
        public static int GetSequenceHashCode<TItem>(this IEnumerable<TItem> list)
        {
            if (list == null) return 0;
            const int seedValue = 0x2D2816FE;
            const int primeNumber = 397;
            return list.Aggregate(seedValue, (current, item) => (current * primeNumber) + (Equals(item, default(TItem)) ? 0 : item.GetHashCode()));
        }
    }
}
