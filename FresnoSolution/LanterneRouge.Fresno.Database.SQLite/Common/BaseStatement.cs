namespace LanterneRouge.Fresno.Database.SQLite.Common
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
