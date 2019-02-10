using System;

namespace LanterneRouge.Fresno.WpfClient.Utils
{
    internal static class ValidateHelpers
    {
        internal static bool IsStringMissing(string value)
        {
            return value == null || string.IsNullOrEmpty(value.Trim());
        }

        internal static bool IsDateMissing(DateTime? value)
        {
            return !value.HasValue;
        }

        internal static bool IsTimeSpanLTZero(TimeSpan value)
        {
            return value <= TimeSpan.Zero;
        }

        internal static bool IsFloatLTZero(float value)
        {
            return value <= 0.0;
        }

        internal static bool IsIntLTZero(int value)
        {
            return value <= 0;
        }
    }
}
