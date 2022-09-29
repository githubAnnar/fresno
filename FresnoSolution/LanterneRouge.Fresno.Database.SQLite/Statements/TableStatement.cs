using LanterneRouge.Fresno.Database.SQLite.Common;
using LanterneRouge.Fresno.Database.SQLite.Constraints;
using LanterneRouge.Fresno.Database.SQLite.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LanterneRouge.Fresno.Database.SQLite.Statements
{
    /// <summary>
    /// Table statement
    /// </summary>
    /// <remarks>CREATE TABLE .. AS SELECT Statement is not implemented</remarks>
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

        internal override ValidateResultList CheckValidity()
        {
            var result = new ValidateResultList();

            if (!string.IsNullOrEmpty(SchemaName) && !(SchemaName.Equals("main", StringComparison.InvariantCultureIgnoreCase) || SchemaName.Equals("temp", StringComparison.InvariantCultureIgnoreCase)))
            {
                result.Add(new CheckResult(nameof(SchemaName), "Must be eiter 'main' or 'temp'"));
            }

            return result;
        }

        public override string GenerateStatement()
        {
            var validity = CheckValidity();
            if (!validity.IsValid)
            {
                throw new ArgumentException($"Not valid: {string.Join(", ", validity.Select(v => $"{v.PropertyName}: {v.Message}"))}");
            }

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
                builder.Append($" {SchemaName}.{Name}");
            }

            else
            {
                builder.Append($" {Name}");
            }

            builder.Append(" (");

            // column statements
            builder.Append(string.Join(", ", Columns.Select(c => c.GenerateStatement())));

            if (TableConstraints != null && TableConstraints.Count > 0)
            {
                builder.Append(", ");
                // table constraints
                builder.Append(string.Join(", ", TableConstraints.Select(t => t.GenerateConstraint())));
            }

            builder.Append(")");

            // table options
            if (TableOptions != null && TableOptions.HasContent)
            {
                builder.Append($" {TableOptions.GenerateOption()}");
            }

            return builder.ToString();
        }

        #endregion
    }
}
