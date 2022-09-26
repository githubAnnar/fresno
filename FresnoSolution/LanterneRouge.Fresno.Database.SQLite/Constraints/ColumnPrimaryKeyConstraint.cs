using LanterneRouge.Fresno.Database.SQLite.Clauses;
using LanterneRouge.Fresno.Database.SQLite.Constraints;
using System;

namespace LanterneRouge.Fresno.Database.SQLite
{
    /// <summary>
    /// PrimaryKey Constraint
    /// For Column
    /// </summary>
    public class ColumnPrimaryKeyConstraint : BaseConstraint
    {
        #region Fields

        private bool _isAscending;
        private bool _isDescending;

        #endregion

        public ColumnPrimaryKeyConstraint(ConflictClause conflictClause) : this(null, conflictClause)
        { }

        public ColumnPrimaryKeyConstraint(string name, ConflictClause conflictClause) : base(name)
        {
            ConflictClause = conflictClause;
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

        public ConflictClause ConflictClause { get; }

        public override string GenerateConstraint()
        {
            if (ConflictClause != null)
            {
                return $"{base.GenerateConstraint()}PRIMARY KEY{Direction}{ConflictClause.GenerateConflictClause()}{(IsAutoIncrement ? " AUTOINCREMENT" : string.Empty)}";
            }

            throw new ArgumentException($"Missing {nameof(ConflictClause)}");
        }
    }
}
