namespace LanterneRouge.Fresno.Database.SQLite.ColumnConstraints
{
    public class CollateConstraint : BaseColumnConstraint
    {
        public CollateConstraint(string name, string collationName) : base(name)
        {
            CollationName = collationName;
        }

        public CollateConstraint(string collationName) : this(null, collationName)
        { }

        public string CollationName { get; }

        public override string GenerateConstraint() => $"{base.GenerateConstraint()}COLLATE ({CollationName})";
    }
}
