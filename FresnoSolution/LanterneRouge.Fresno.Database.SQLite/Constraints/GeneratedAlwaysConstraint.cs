using System;

namespace LanterneRouge.Fresno.Database.SQLite.Constraints
{
    /// <summary>
    /// GeneratedAlways Constraint
    /// For Column
    /// </summary>
    public class GeneratedAlwaysConstraint : BaseConstraint
    {
        #region Fields

        private bool _isStored;
        private bool _isVirtual;

        #endregion

        public GeneratedAlwaysConstraint(string name, string expression) : base(name)
        {
            Expression = expression;
        }

        public GeneratedAlwaysConstraint(string expression) : this(null, expression)
        { }

        public string Expression { get; }

        public bool ShowGeneratedAlways { get; set; } = true;

        public bool IsStored
        {
            get => _isStored;
            set
            {
                _isStored = value;
                if (_isStored)
                {
                    _isVirtual = false;
                }
            }
        }

        public bool IsVirtual
        {
            get => _isVirtual;
            set
            {
                _isVirtual = value;
                if (_isVirtual)
                {
                    _isStored = false;
                }
            }
        }

        private string StoreType => !IsStored && !IsVirtual ? string.Empty : IsStored ? " STORED" : IsVirtual ? " VIRTUAL" : string.Empty;

        public override string GenerateConstraint()
        {
            return string.IsNullOrEmpty(Expression)
                ? throw new ArgumentException($"No expresion for {nameof(GeneratedAlwaysConstraint)}")
                : $"{base.GenerateConstraint()}{(ShowGeneratedAlways ? "GENERATED ALWAYS " : string.Empty)}AS ({Expression}){StoreType}";
        }
    }
}
