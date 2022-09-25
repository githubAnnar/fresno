namespace LanterneRouge.Fresno.Database.SQLite.Statements
{
    public abstract class BaseStatement
    {
        public BaseStatement(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public abstract string GenerateStatement();
    }
}
