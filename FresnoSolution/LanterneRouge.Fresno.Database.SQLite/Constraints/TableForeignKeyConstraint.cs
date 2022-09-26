using LanterneRouge.Fresno.Database.SQLite.Clauses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LanterneRouge.Fresno.Database.SQLite.Constraints
{
    public class TableForeignKeyConstraint : BaseConstraint
    {
        public TableForeignKeyConstraint(IEnumerable<string> columnNames, ForeignKeyClause foreignKeyClause) : this(null, columnNames, foreignKeyClause)
        { }

        public TableForeignKeyConstraint(string name, IEnumerable<string> columnNames, ForeignKeyClause foreignKeyClause) : base(name)
        {
            ColumnNames = columnNames;
            ForeignKeyClause = foreignKeyClause;
        }

        public IEnumerable<string> ColumnNames { get; }

        public ForeignKeyClause ForeignKeyClause { get; }

        public override string GenerateConstraint()
        {
            if (ColumnNames != null && ColumnNames.Any() && ForeignKeyClause != null)
            {
                return $"{base.GenerateConstraint()}FOREIGN KEY ({string.Join(", ", ColumnNames)}) {ForeignKeyClause.GenerateForeignKeyClause()}";
            }

            throw new ArgumentException($"Missing {nameof(ColumnNames)} or {nameof(ForeignKeyClause)}");
        }
    }
}
