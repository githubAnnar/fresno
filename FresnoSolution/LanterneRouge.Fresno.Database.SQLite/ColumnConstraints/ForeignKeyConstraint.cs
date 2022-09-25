using LanterneRouge.Fresno.Database.SQLite.Clauses;

namespace LanterneRouge.Fresno.Database.SQLite.ColumnConstraints
{
    public class ForeignKeyConstraint : BaseColumnConstraint
    {
        public ForeignKeyConstraint() : base(null)
        { }

        public ForeignKeyConstraint(string name) : base(name)
        { }

        public ForeignKeyClause ForeignKeyClause { get; set; }

        public override string GenerateConstraint() => $"{base.GenerateConstraint()} {ForeignKeyClause.GenerateForeignKeyClause()}";
    }
}
