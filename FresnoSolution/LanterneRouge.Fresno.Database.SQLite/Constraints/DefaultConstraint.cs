using LanterneRouge.Fresno.Database.SQLite.Types;
using System;

namespace LanterneRouge.Fresno.Database.SQLite.Constraints
{
    public class DefaultConstraint : BaseConstraint
    {
        public DefaultConstraint(string name, string expression) : base(name)
        {
            Expression = expression;
        }

        public DefaultConstraint(string expression) : this(null, expression)
        { }

        public string Expression { get; }

        public LiteralValue Value { get; set; }

        public int? SignedNumber { get; set; } = null;

        private bool IsValid
        {
            get
            {
                if (Value == null && string.IsNullOrEmpty(Expression) && !SignedNumber.HasValue)
                {
                    return false;
                }

                if (Value == null && string.IsNullOrEmpty(Expression) && SignedNumber.HasValue)
                {
                    return true;
                }

                if (Value == null && !string.IsNullOrEmpty(Expression) && !SignedNumber.HasValue)
                {
                    return true;
                }

                if (Value != null && string.IsNullOrEmpty(Expression) && !SignedNumber.HasValue)
                {
                    return true;
                }

                return false;
            }
        }

        public override string GenerateConstraint()
        {
            if (IsValid)
            {
                if (!string.IsNullOrEmpty(Expression))
                {
                    return $"{base.GenerateConstraint()}DEFAULT ({Expression})";
                }

                if (Value != null)
                {
                    return $"{base.GenerateConstraint()}DEFAULT {Value.GetLiteralValue()}";
                }

                return $"{base.GenerateConstraint()}DEFAULT {SignedNumber}";
            }

            throw new ArgumentException($"Too many {nameof(DefaultConstraint)} values is set. Only one allowed");
        }
    }
}
