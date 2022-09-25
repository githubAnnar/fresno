namespace LanterneRouge.Fresno.Database.SQLite.ColumnConstraints
{
    public interface IColumnConstraint
    {
        string Name { get; set; }

        string GenerateConstraint();
    }
}
