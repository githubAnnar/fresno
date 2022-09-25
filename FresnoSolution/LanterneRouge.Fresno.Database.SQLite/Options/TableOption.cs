using System.Collections.Generic;

namespace LanterneRouge.Fresno.Database.SQLite.Options
{
    public class TableOption : BaseOption
    {
        public bool WithoutRowId { get; set; } = false;

        public bool IsStrict { get; set; } = false;

        public override string GenerateOption()
        {
            var options = new List<string>();
            if (WithoutRowId)
            {
                options.Add("WITHOUT ROWID");
            }

            if (IsStrict)
            {
                options.Add("STRICT");
            }

            return string.Join(", ", options);
        }

        public bool HasContent => WithoutRowId || IsStrict;
    }
}