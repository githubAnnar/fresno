using LanterneRouge.Fresno.Database.SQLite.Common;

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

        internal virtual ValidateResultList CheckValidity()
        {
            return new ValidateResultList();
        }
    }
}
