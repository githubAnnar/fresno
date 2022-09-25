namespace LanterneRouge.Fresno.Database.SQLite.ColumnConstraints
{
    public abstract class BaseColumnConstraint : IColumnConstraint
    {
        public BaseColumnConstraint(string name)
        {
            Name = name;
        }

        public virtual string Name { get; set; } = string.Empty;

        public virtual string GenerateConstraint() => !string.IsNullOrEmpty(Name) ? $"CONSTRAINT {Name} " : string.Empty;
    }
}
