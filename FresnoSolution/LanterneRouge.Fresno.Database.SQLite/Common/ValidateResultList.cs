using System.Collections.Generic;

namespace LanterneRouge.Fresno.Database.SQLite.Common
{

    internal class ValidateResultList : List<CheckResult>
    {
        public bool IsValid => Count == 0;
    }
}
