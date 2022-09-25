using LanterneRouge.Fresno.Database.SQLite.Common;
using System;
using System.Collections;
using System.Text;

namespace LanterneRouge.Fresno.Database.SQLite.Clauses
{

    public class ConflictClause
    {
        public bool Rollback { get; set; }

        public bool Abort { get; set; }

        public bool Fail { get; set; }

        public bool Ignore { get; set; }

        public bool Replace { get; set; }

        public string GenerateConflictClause()
        {
            var values = new BitArray(new[] { Rollback, Abort, Fail, Ignore, Replace });

            if (values.TrueCount() == 0)
            {
                return string.Empty;
            }

            if (values.TrueCount() != 1)
            {
                throw new ArgumentException($"Too many Clauses set!");
            }

            var builder = new StringBuilder("ON CONFLICT");
            if (Rollback)
            {
                builder.Append(" ROLLBACK");
            }

            if (Abort)
            {
                builder.Append(" ABORT");
            }

            if (Fail)
            {
                builder.Append(" FAIL");
            }

            if (Ignore)
            {
                builder.Append(" IGNORE");
            }

            if (Replace)
            {
                builder.Append(" REPLACE");
            }

            return builder.ToString();
        }
    }
}
