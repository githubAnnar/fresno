namespace LanterneRouge.Fresno.Database.SQLite.Constraints
{
    public abstract class BaseConstraint : IConstraint
    {
        public BaseConstraint(string name)
        {
            Name = name;
        }

        public virtual string Name { get; set; } = string.Empty;

        public virtual string GenerateConstraint() => !string.IsNullOrEmpty(Name) ? $"CONSTRAINT {Name} " : string.Empty;
    }
}
