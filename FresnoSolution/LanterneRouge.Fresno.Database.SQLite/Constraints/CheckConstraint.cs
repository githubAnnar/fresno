namespace LanterneRouge.Fresno.Database.SQLite.Constraints
{
    /// <summary>
    /// Check Constraint
    /// For both Column and Table
    /// </summary>
    public class CheckConstraint : BaseConstraint
    {
        public CheckConstraint(string name, string expression) : base(name)
        {
            Expression = expression;
        }

        public CheckConstraint(string expression) : this(null, expression)
        { }

        public string Expression { get; }

        public override string GenerateConstraint() => $"{base.GenerateConstraint()}CHECK ({Expression})";
    }
}
