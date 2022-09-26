using LanterneRouge.Fresno.Database.SQLite.Clauses;
using LanterneRouge.Fresno.Database.SQLite.Statements;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LanterneRouge.Fresno.Database.SQLite.Constraints
{
    /// <summary>
    /// PrimaryKey Constraint
    /// For Table
    /// </summary>
    public class TablePrimaryKeyConstraint : BaseConstraint
    {
        public TablePrimaryKeyConstraint(IEnumerable<ColumnStatement> columnStatements, ConflictClause conflictClause) : this(null, columnStatements, conflictClause)
        { }

        public TablePrimaryKeyConstraint(string name, IEnumerable<ColumnStatement> columnStatements, ConflictClause conflictClause) : base(name)
        {
            IndexedColumns = columnStatements;
            ConflictClause = conflictClause;
        }

        public IEnumerable<ColumnStatement> IndexedColumns { get; }

        public ConflictClause ConflictClause { get; }

        public override string GenerateConstraint()
        {
            if (IndexedColumns != null && IndexedColumns.Any())
            {
                return $"{base.GenerateConstraint()}PRIMARY KEY ({string.Join(", ", IndexedColumns.Select(c => c.Name))}){(ConflictClause != null ? $"{ConflictClause.GenerateConflictClause()}" : string.Empty)}";
            }

            throw new ArgumentException($"No {nameof(IndexedColumns)} given");
        }
    }
}
