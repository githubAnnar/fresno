namespace LanterneRouge.Fresno.Database.SQLite.TableConstraints
{
    public class BaseTableConstraint
    {
        public BaseTableConstraint(string name)
        {
            Name = name;
        }

        public virtual string Name { get; set; } = string.Empty;

        public virtual string GenerateConstraint() => !string.IsNullOrEmpty(Name) ? $"CONSTRAINT {Name} " : string.Empty;
    }
}
