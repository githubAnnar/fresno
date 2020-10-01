using System;

namespace LanterneRouge.Fresno.WpfClient.Utils
{
    internal static class ValidateHelpers
    {
        internal static bool IsStringMissing(string value) => value == null || string.IsNullOrEmpty(value.Trim());

        internal static bool IsDateMissing(DateTime? value) => !value.HasValue;

        internal static bool IsTimeSpanLTZero(TimeSpan value) => value <= TimeSpan.Zero;

        internal static bool IsFloatLTZero(float value) => value <= 0.0;

        internal static bool IsIntLTZero(int value) => value <= 0;
    }
}
