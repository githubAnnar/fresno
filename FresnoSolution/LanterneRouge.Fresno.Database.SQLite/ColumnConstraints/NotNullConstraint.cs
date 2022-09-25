using LanterneRouge.Fresno.Database.SQLite.Clauses;
using System.Text;

namespace LanterneRouge.Fresno.Database.SQLite.ColumnConstraints
{
    public class NotNullConstraint : BaseColumnConstraint
    {
        public NotNullConstraint() : base(null)
        { }

        public NotNullConstraint(string name) : base(name)
        { }

        public ConflictClause ConflictCause { get; } = new ConflictClause();

        public override string GenerateConstraint()
        {
            var builder = new StringBuilder($"{base.GenerateConstraint()}NOT NULL");
            if (!string.IsNullOrEmpty(ConflictCause.GenerateConflictClause()))
            {
                builder.Append($" {ConflictCause.GenerateConflictClause()}");
            }

            return builder.ToString();
        }
    }
}
