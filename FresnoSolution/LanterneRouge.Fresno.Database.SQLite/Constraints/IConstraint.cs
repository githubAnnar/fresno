namespace LanterneRouge.Fresno.Database.SQLite.Constraints
{
    public interface IConstraint
    {
        string Name { get; set; }

        string GenerateConstraint();
    }
}
