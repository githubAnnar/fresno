using LanterneRouge.Fresno.Database.SQLite.Constraints;
using LanterneRouge.Fresno.Database.SQLite.Options;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LanterneRouge.Fresno.Database.SQLite.Statements
{
    public class TableStatement : BaseStatement
    {
        public TableStatement(string name) : base(name)
        {
            Name = name;
        }

        #region Properties

        public string SchemaName { get; set; }

        public List<ColumnStatement> Columns { get; set; }

        public List<IConstraint> TableConstraints { get; set; }

        public TableOption TableOptions { get; set; } = null;

        public bool IsTemporary { get; set; } = false;

        public bool ExistsCheck { get; set; } = false;

        public override string GenerateStatement()
        {
            var builder = new StringBuilder();

            builder.Append("CREATE");

            if (IsTemporary)
            {
                builder.Append(" TEMP");
            }

            builder.Append(" TABLE");

            if (ExistsCheck)
            {
                builder.Append(" IF NOT EXISTS");
            }

            if (!string.IsNullOrEmpty(SchemaName))
            {
                builder.Append($" {SchemaName}.");
            }

            builder.Append($" {Name}");

            builder.Append(" (");

            // column statements
            builder.Append(string.Join(", ", Columns.Select(c => c.GenerateStatement())));

            // table constraints
            builder.Append(string.Join(", ", TableConstraints.Select(t => t.GenerateConstraint())));

            builder.Append(") ");

            // table options
            if (TableOptions.HasContent)
            {
                builder.Append(TableOptions.GenerateOption());
            }

            return builder.ToString();
        }

        #endregion
    }
}
