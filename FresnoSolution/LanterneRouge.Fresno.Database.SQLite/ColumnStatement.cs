using LanterneRouge.Fresno.Database.SQLite.Common;
using System.Collections.Generic;
using System.Text;

namespace LanterneRouge.Fresno.Database.SQLite
{
    public class ColumnStatement : BaseStatement
    {
        public ColumnStatement(string name) : base(name)
        { }

        public AffinityType Affinity { get; set; } = AffinityType.INTEGER;

        public List<ColumnConstraint> ColumnConstraints { get; set; } = null;

        public override string GenerateStatement()
        {
            var builder = new StringBuilder();

            builder.Append(Name);

            return builder.ToString();
        }
    }
}