namespace LanterneRouge.Fresno.Database.SQLite.Common
{
    internal struct CheckResult
    {
        public CheckResult(string propertyName, string message)
        {
            PropertyName = propertyName;
            Message = message;
        }

        public string PropertyName { get; }

        public string Message { get; }
    }
}
