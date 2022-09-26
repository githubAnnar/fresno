using LanterneRouge.Fresno.Database.SQLite.Clauses;
using LanterneRouge.Fresno.Database.SQLite.Statements;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LanterneRouge.Fresno.Database.SQLite.Constraints
{
    /// <summary>
    /// Unique Constraint
    /// For Table
    /// </summary>
    public class TableUniqueConstraint : BaseConstraint
    {
        public TableUniqueConstraint(IEnumerable<ColumnStatement> columnStatements, ConflictClause conflictClause) : this(null, columnStatements, conflictClause)
        { }

        public TableUniqueConstraint(string name, IEnumerable<ColumnStatement> columnStatements, ConflictClause conflictClause) : base(name)
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
                return $"{base.GenerateConstraint()}UNIQUE ({string.Join(", ", IndexedColumns.Select(c => c.Name))}){(ConflictClause != null ? $"{ConflictClause.GenerateConflictClause()}" : string.Empty)}";
            }

            throw new ArgumentException($"No {nameof(IndexedColumns)} given");
        }
    }
}
