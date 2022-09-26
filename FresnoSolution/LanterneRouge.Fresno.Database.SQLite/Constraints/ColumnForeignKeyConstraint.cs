using LanterneRouge.Fresno.Database.SQLite.Clauses;
using System;

namespace LanterneRouge.Fresno.Database.SQLite.Constraints
{
    /// <summary>
    /// ForeignKey Constraint
    /// For Column
    /// </summary>
    public class ColumnForeignKeyConstraint : BaseConstraint
    {
        public ColumnForeignKeyConstraint(ForeignKeyClause foreignKeyClause) : this(null, foreignKeyClause)
        { }

        public ColumnForeignKeyConstraint(string name, ForeignKeyClause foreignKeyClause) : base(name)
        {
            ForeignKeyClause = foreignKeyClause;
        }

        public ForeignKeyClause ForeignKeyClause { get; }

        public override string GenerateConstraint()
        {
            if (ForeignKeyClause != null)
            {
                return $"{base.GenerateConstraint()}{ForeignKeyClause.GenerateForeignKeyClause()}";
            }

            throw new ArgumentException($"Missing {nameof(ForeignKeyClause)}");
        }
    }
}
