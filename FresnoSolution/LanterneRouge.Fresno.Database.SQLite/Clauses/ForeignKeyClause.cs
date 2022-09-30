using LanterneRouge.Fresno.Database.SQLite.Statements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LanterneRouge.Fresno.Database.SQLite.Clauses
{
    public enum SetEnum
    {
        DeleteSetNull,
        UpdateSetNull,
        DeleteSetDefault,
        UpdateSetSetDefault,
        DeleteCascade,
        UpdateCascade,
        DeleteRestrict,
        UpdateRestrict,
        DeleteNoAction,
        UpdateNoAction,
    }

    public class ForeignKeyClause
    {
        private readonly Dictionary<SetEnum, string> _setDictionary = new Dictionary<SetEnum, string> {
            { SetEnum.DeleteSetNull, "ON DELETE SET NULL" },
            { SetEnum.UpdateSetNull, "ON UPDATE SET NULL" },
            { SetEnum.DeleteSetDefault, "ON DELETE SET DEFAULT" },
            { SetEnum.UpdateSetSetDefault, "ON UPDATE SET DEFAULT" },
            { SetEnum.DeleteCascade, "ON DELETE CASCADE" },
            { SetEnum.UpdateCascade, "ON UPDATE CASCADE" },
            { SetEnum.DeleteRestrict, "ON DELETE RESTRICT" },
            { SetEnum.UpdateRestrict, "ON UPDATE RESTRICT" },
            { SetEnum.DeleteNoAction, "ON DELETE NO ACTION" },
            { SetEnum.UpdateNoAction, "ON UPDATE NO ACTION" }
        };

        public ForeignKeyClause(TableStatement foreignTable, IEnumerable<string> columnNames)
        {
            ForeignTable = foreignTable;
            Columns = columnNames;
        }

        public TableStatement ForeignTable { get; }

        public IEnumerable<string> Columns { get; }

        public List<SetEnum> Set { get; set; }

        private string GetSetString(SetEnum set) => _setDictionary[set];

        public string GenerateForeignKeyClause()
        {
            foreach (var item in Columns)
            {
                if (!ForeignTable.Columns.Select(s => s.Name).Contains(item))
                {
                    throw new ArgumentException($"Table '{ForeignTable.Name}' does not contain column '{item}'");
                }
            }

            var builder = new StringBuilder("REFERENCES ");
            builder.Append(ForeignTable.Name);
            if (Columns != null && Columns.Any())
            {
                builder.Append($" ({string.Join(", ", Columns)})");
            }

            if (Set != null && Set.Any())
            {
                builder.Append($" {string.Join(" ", Set.Select(s => GetSetString(s)))}");
            }

            return builder.ToString();
        }
    }
}
