using LanterneRouge.Fresno.Database.SQLite.Clauses;
using System;

namespace LanterneRouge.Fresno.Database.SQLite.Constraints
{
    /// <summary>
    /// Unique Constraint
    /// For Column
    /// </summary>
    public class ColumnUniqueConstraint : BaseConstraint
    {
        public ColumnUniqueConstraint(ConflictClause conflictClause) : this(null, conflictClause)
        { }

        public ColumnUniqueConstraint(string name, ConflictClause conflictClause) : base(name)
        {
            ConflictCause = conflictClause;
        }

        public ConflictClause ConflictCause { get; }

        public override string GenerateConstraint()
        {
            if (ConflictCause != null)
            {
                return $"{base.GenerateConstraint()}UNIQUE{ConflictCause.GenerateConflictClause()}";
            }

            throw new ArgumentException($"Missing {nameof(ConflictClause)}");
        }
    }
}
