using System.Collections;
using System.Linq;

namespace LanterneRouge.Fresno.Database.SQLite.Common
{
    public static class BitArrayExtensions
    {
        public static int TrueCount(this BitArray array) => array.Cast<bool>().Count(l => l);
    }
}
