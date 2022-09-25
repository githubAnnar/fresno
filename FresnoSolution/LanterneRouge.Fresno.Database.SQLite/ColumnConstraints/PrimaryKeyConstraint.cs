using LanterneRouge.Fresno.Database.SQLite.Clauses;
using LanterneRouge.Fresno.Database.SQLite.ColumnConstraints;

namespace LanterneRouge.Fresno.Database.SQLite
{
    public class PrimaryKeyConstraint : BaseColumnConstraint
    {
        #region Fields

        private bool _isAscending;
        private bool _isDescending;

        #endregion

        public PrimaryKeyConstraint() : base(null)
        {
            IsAscending = false;
            IsDescending = false;
        }

        public PrimaryKeyConstraint(string name) : base(name)
        {
            IsAscending = false;
            IsDescending = false;
        }

        public bool IsAscending
        {
            get => _isAscending;
            set
            {
                _isAscending = value;
                if (_isAscending)
                {
                    _isDescending = false;
                }
            }
        }

        public bool IsDescending
        {
            get => _isDescending;
            set
            {
                _isDescending = value;
                if (_isDescending)
                {
                    _isAscending = false;
                }
            }
        }

        private string Direction => !IsAscending && !IsDescending ? string.Empty : IsAscending ? "ASC " : IsDescending ? "DESC " : string.Empty;

        public bool IsAutoIncrement { get; set; }

        public ConflictClause ConflictCause { get; set; }

        public override string GenerateConstraint() => $"{base.GenerateConstraint()}PRIMARY KEY {Direction}{ConflictCause.GenerateConflictClause()}{(IsAutoIncrement ? " AUTOINCREMENT" : string.Empty)}";
    }
}
