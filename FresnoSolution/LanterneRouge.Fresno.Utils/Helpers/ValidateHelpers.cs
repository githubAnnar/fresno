namespace LanterneRouge.Fresno.Utils.Helpers
{
    public static class ValidateHelpers
    {
        public static bool IsStringMissing(string value) => value == null || string.IsNullOrEmpty(value.Trim());

        public static bool IsDateMissing(DateTime? value) => !value.HasValue;

        public static bool IsTimeSpanLTZero(TimeSpan value) => value <= TimeSpan.Zero;

        public static bool IsFloatLTZero(float value) => value <= 0.0;

        public static bool IsIntLTZero(int value) => value <= 0;
    }
}
