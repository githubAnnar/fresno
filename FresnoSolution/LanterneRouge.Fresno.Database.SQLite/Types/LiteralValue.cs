using System;

namespace LanterneRouge.Fresno.Database.SQLite.Types
{
    public enum LiteralValueEnum
    {
        NULL,
        TRUE,
        FALSE,
        CURRENT_TIME,
        CURRENT_DATE,
        CURRENT_TIMESTAMP
    }

    public class LiteralValue
    {
        public int? NumericLiteral { get; set; } = null;

        public string StringLiteral { get; set; } = null;

        public LiteralValueEnum? Value { get; set; }

        private bool IsValid => (!NumericLiteral.HasValue || string.IsNullOrEmpty(StringLiteral) || !Value.HasValue) && (!NumericLiteral.HasValue || string.IsNullOrEmpty(StringLiteral)) && (!NumericLiteral.HasValue || !Value.HasValue) && (string.IsNullOrEmpty(StringLiteral) || !Value.HasValue);

        public string GetLiteralValue()
        {
            if (IsValid)
            {
                if (NumericLiteral.HasValue)
                {
                    return NumericLiteral.Value.ToString();
                }

                if (!string.IsNullOrEmpty(StringLiteral))
                {
                    return StringLiteral;
                }

                return Value.Value.ToString();
            }

            throw new ArgumentException($"Too many {nameof(LiteralValue)} values is set. Only one allowed");
        }
    }
}
