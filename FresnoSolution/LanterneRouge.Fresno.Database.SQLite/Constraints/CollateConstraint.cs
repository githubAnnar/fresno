namespace LanterneRouge.Fresno.Database.SQLite.Constraints
{
    /// <summary>
    /// Collate Constraint
    /// For Column
    /// </summary>
    public class CollateConstraint : BaseConstraint
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
