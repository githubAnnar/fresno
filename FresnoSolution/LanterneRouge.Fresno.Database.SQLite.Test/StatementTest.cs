using LanterneRouge.Fresno.Database.SQLite.Clauses;
using LanterneRouge.Fresno.Database.SQLite.Constraints;
using LanterneRouge.Fresno.Database.SQLite.Options;
using LanterneRouge.Fresno.Database.SQLite.Statements;
using LanterneRouge.Fresno.Database.SQLite.Types;
using System;
using System.Collections.Generic;
using Xunit;

namespace LanterneRouge.Fresno.Database.SQLite.Test
{
    public class StatementTest
    {
        [Fact]
        public void ColumnStatementTest()
        {
            var testObject = new ColumnStatement("TestColumn");
            var actual = testObject.GenerateStatement();
            Assert.Equal("TestColumn", actual);

            testObject = new ColumnStatement("TestColumn")
            {
                Affinity = AffinityType.INTEGER
            };
            actual = testObject.GenerateStatement();
            Assert.Equal("TestColumn INTEGER", actual);

            testObject = new ColumnStatement("TestColumn")
            {
                Affinity = AffinityType.INTEGER
            };
            var conflictCause = new ConflictClause { Fail = true };
            var columnConstraint1 = new ColumnPrimaryKeyConstraint(conflictCause);
            testObject.ColumnConstraints = new List<IConstraint>
            {
                columnConstraint1
            };
            actual = testObject.GenerateStatement();
            Assert.Equal("TestColumn INTEGER PRIMARY KEY ON CONFLICT FAIL", actual);
        }

        [Fact]
        public void TableStatementTest()
        {
            var testObject = new TableStatement("TestTable");
            Assert.Throws<ArgumentNullException>(testObject.GenerateStatement);

            testObject = new TableStatement("TestTable")
            {
                SchemaName = "feil"
            };
            Assert.Throws<ArgumentException>(testObject.GenerateStatement);

            testObject = new TableStatement("TestTable");
            var column = new ColumnStatement("TestColumn")
            {
                Affinity = AffinityType.INTEGER
            };
            var conflictCause = new ConflictClause { Fail = true };
            var columnConstraint1 = new ColumnPrimaryKeyConstraint(conflictCause);
            column.ColumnConstraints = new List<IConstraint>
            {
                columnConstraint1
            };
            testObject.Columns = new List<ColumnStatement>
            {
                column
            };

            var actual = testObject.GenerateStatement();
            Assert.Equal("CREATE TABLE TestTable (TestColumn INTEGER PRIMARY KEY ON CONFLICT FAIL)", actual);

            testObject = new TableStatement("TestTable")
            {
                Columns = new List<ColumnStatement> { column },
                SchemaName = "main",
                TableOptions = new TableOption { WithoutRowId = true },
                ExistsCheck = true
            };

            actual = testObject.GenerateStatement();
            Assert.Equal("CREATE TABLE IF NOT EXISTS main.TestTable (TestColumn INTEGER PRIMARY KEY ON CONFLICT FAIL) WITHOUT ROWID", actual);
        }
    }
}
