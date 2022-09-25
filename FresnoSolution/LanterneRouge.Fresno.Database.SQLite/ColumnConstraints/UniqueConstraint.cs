using LanterneRouge.Fresno.Database.SQLite.Clauses;

namespace LanterneRouge.Fresno.Database.SQLite.ColumnConstraints
{
    public class UniqueConstraint : BaseColumnConstraint
    {
        public UniqueConstraint() : base(null)
        { }

        public UniqueConstraint(string name) : base(name)
        { }

        public ConflictClause ConflictCause { get; set; }

        public override string GenerateConstraint() => $"{base.GenerateConstraint()}UNIQUE {ConflictCause.GenerateConflictClause()}";
    }
}
