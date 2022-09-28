using LanterneRouge.Fresno.Database.SQLite.Constraints;
using LanterneRouge.Fresno.Database.SQLite.Types;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LanterneRouge.Fresno.Database.SQLite.Statements
{
    public class ColumnStatement : BaseStatement
    {
        public ColumnStatement(string name) : base(name)
        { }

        public AffinityType? Affinity { get; set; }

        public List<IConstraint> ColumnConstraints { get; set; } = null;

        public override string GenerateStatement()
        {
            var builder = new StringBuilder();

            builder.Append(Name);

            if (Affinity.HasValue)
            {
                builder.Append($" {Affinity.Value}");
            }

            if (ColumnConstraints != null && ColumnConstraints.Any())
            {
                builder.Append($" {string.Join(", ", ColumnConstraints.Select(c => c.GenerateConstraint()))}");
            }

            return builder.ToString();
        }
    }
}