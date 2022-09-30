using LanterneRouge.Fresno.Database.SQLite.Clauses;
using LanterneRouge.Fresno.Database.SQLite.Constraints;
using LanterneRouge.Fresno.Database.SQLite.Statements;
using LanterneRouge.Fresno.Database.SQLite.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace LanterneRouge.Fresno.Database.SQLite.Test
{
    public class ConstraintTest
    {
        [Fact]
        public void CheckConstraintTest()
        {
            var testObject = new CheckConstraint("CheckTest", "this is an expression");
            var actual = testObject.GenerateConstraint();
            Assert.Equal("CONSTRAINT CheckTest CHECK (this is an expression)", actual);

            testObject = new CheckConstraint("this is an expression");
            actual = testObject.GenerateConstraint();
            Assert.Equal("CHECK (this is an expression)", actual);
        }

        [Fact]
        public void CollateConstraintTest()
        {
            var testObject = new CollateConstraint("CollateTest", "collate name");
            var actual = testObject.GenerateConstraint();
            Assert.Equal("CONSTRAINT CollateTest COLLATE (collate name)", actual);

            testObject = new CollateConstraint("collate name");
            actual = testObject.GenerateConstraint();
            Assert.Equal("COLLATE (collate name)", actual);
        }

        [Fact]
        public void DefaultConstraintTest()
        {
            var testObject = new DefaultConstraint("DefaultTest", "default expression");
            var actual = testObject.GenerateConstraint();
            Assert.Equal("CONSTRAINT DefaultTest DEFAULT (default expression)", actual);

            testObject = new DefaultConstraint(null, null)
            {
                Value = new LiteralValue() { Value = Types.LiteralValueEnum.CURRENT_DATE }
            };
            actual = testObject.GenerateConstraint();
            Assert.Equal("DEFAULT CURRENT_DATE", actual);

            testObject = new DefaultConstraint(null, null)
            {
                Value = new LiteralValue() { Value = LiteralValueEnum.CURRENT_DATE, StringLiteral = "Not allowed!" }
            };
            Assert.Throws<ArgumentException>(testObject.GenerateConstraint);
        }

        [Fact]
        public void ColumnForeignKeyConstraintTest()
        {
            var columnStatement = new ColumnStatement("FirstRow");
            var tableStatement = new TableStatement("TestTable")
            {
                Columns = new[] { columnStatement }.ToList(),
            };
            var fkConstraint = new ForeignKeyClause(tableStatement, new[] { "FirstRow" })
            {
                Set = new List<SetEnum> { SetEnum.UpdateSetNull }
            };
            var testObject = new ColumnForeignKeyConstraint(fkConstraint);
            var actual = testObject.GenerateConstraint();
            Assert.Equal("REFERENCES TestTable (FirstRow) ON UPDATE SET NULL", actual);

            testObject = new ColumnForeignKeyConstraint("TestFK", fkConstraint);
            actual = testObject.GenerateConstraint();
            Assert.Equal("CONSTRAINT TestFK REFERENCES TestTable (FirstRow) ON UPDATE SET NULL", actual);

            testObject = new ColumnForeignKeyConstraint("TestFK", null);
            Assert.Throws<ArgumentException>(testObject.GenerateConstraint);
        }

        [Fact]
        public void ColumnPrimaryKeyConstraintTest()
        {
            var testObject = new ColumnPrimaryKeyConstraint("TestPK", null);
            Assert.Throws<ArgumentException>(testObject.GenerateConstraint);

            testObject = new ColumnPrimaryKeyConstraint("TestPK", new ConflictClause
            {
                Replace = true
            });
            var actual = testObject.GenerateConstraint();
            Assert.Equal("CONSTRAINT TestPK PRIMARY KEY ON CONFLICT REPLACE", actual);

            testObject = new ColumnPrimaryKeyConstraint(new ConflictClause
            {
                Replace = true
            });
            actual = testObject.GenerateConstraint();
            Assert.Equal("PRIMARY KEY ON CONFLICT REPLACE", actual);
        }

        [Fact]
        public void ColumnUniqueConstraintTest()
        {
            var testObject = new ColumnUniqueConstraint("TestUnique", null);
            Assert.Throws<ArgumentException>(testObject.GenerateConstraint);

            testObject = new ColumnUniqueConstraint("TestUnique", new ConflictClause
            {
                Replace = true
            });
            var actual = testObject.GenerateConstraint();
            Assert.Equal("CONSTRAINT TestUnique UNIQUE ON CONFLICT REPLACE", actual);

            testObject = new ColumnUniqueConstraint(new ConflictClause
            {
                Replace = true
            });
            actual = testObject.GenerateConstraint();
            Assert.Equal("UNIQUE ON CONFLICT REPLACE", actual);
        }

        [Fact]
        public void GeneratedAlwaysConstraintTest()
        {
            var testObject = new GeneratedAlwaysConstraint("GeneratedAlwaysTest", "default expression");
            var actual = testObject.GenerateConstraint();
            Assert.Equal("CONSTRAINT GeneratedAlwaysTest GENERATED ALWAYS AS (default expression)", actual);

            testObject = new GeneratedAlwaysConstraint("GeneratedAlwaysTest", "default expression")
            {
                ShowGeneratedAlways = false
            };
            actual = testObject.GenerateConstraint();
            Assert.Equal("CONSTRAINT GeneratedAlwaysTest AS (default expression)", actual);

            testObject = new GeneratedAlwaysConstraint("GeneratedAlwaysTest", "default expression")
            {
                ShowGeneratedAlways = false,
                IsStored = true
            };
            actual = testObject.GenerateConstraint();
            Assert.Equal("CONSTRAINT GeneratedAlwaysTest AS (default expression) STORED", actual);

            testObject = new GeneratedAlwaysConstraint("GeneratedAlwaysTest", "default expression")
            {
                ShowGeneratedAlways = false,
                IsStored = true,
                IsVirtual = true
            };
            actual = testObject.GenerateConstraint();
            Assert.Equal("CONSTRAINT GeneratedAlwaysTest AS (default expression) VIRTUAL", actual);

            testObject = new GeneratedAlwaysConstraint("GeneratedAlwaysTest", null)
            {
                ShowGeneratedAlways = false,
                IsStored = true,
                IsVirtual = true
            };
            Assert.Throws<ArgumentException>(testObject.GenerateConstraint);
        }

        [Fact]
        public void NotNullConstraintTest()
        {

            var testObject = new NotNullConstraint("NotNullTest");
            testObject.ConflictCause.Ignore = true;
            var actual = testObject.GenerateConstraint();
            Assert.Equal("CONSTRAINT NotNullTest NOT NULL ON CONFLICT IGNORE", actual);

            testObject = new NotNullConstraint();
            testObject.ConflictCause.Ignore = true;
            actual = testObject.GenerateConstraint();
            Assert.Equal("NOT NULL ON CONFLICT IGNORE", actual);

            testObject = new NotNullConstraint();
            actual = testObject.GenerateConstraint();
            Assert.Equal("NOT NULL", actual);
        }

        [Fact]
        public void TableForeignKeyConstraintTest()
        {
            var columnStatement = new ColumnStatement("FirstRow");
            var tableStatement = new TableStatement("TestTable")
            {
                Columns = new[] { columnStatement }.ToList(),
            };
            var fkConstraint = new ForeignKeyClause(tableStatement, new[] { "FirstRow" })
            {
                Set = new List<SetEnum> { SetEnum.UpdateSetNull }
            };

            var testObject = new TableForeignKeyConstraint(new[] { "Column1", "Column2" }, fkConstraint);
            var actual = testObject.GenerateConstraint();
            Assert.Equal("FOREIGN KEY (Column1, Column2) REFERENCES TestTable (FirstRow) ON UPDATE SET NULL", actual);

            testObject = new TableForeignKeyConstraint("TestFK", new[] { "Column1", "Column2" }, fkConstraint);
            actual = testObject.GenerateConstraint();
            Assert.Equal("CONSTRAINT TestFK FOREIGN KEY (Column1, Column2) REFERENCES TestTable (FirstRow) ON UPDATE SET NULL", actual);

            testObject = new TableForeignKeyConstraint("TestFK", null, null);
            Assert.Throws<ArgumentException>(testObject.GenerateConstraint);
        }

        [Fact]
        public void TablePrimaryKeyConstraintTest()
        {
            var testObject = new TablePrimaryKeyConstraint("TestPK", null, null);
            Assert.Throws<ArgumentException>(testObject.GenerateConstraint);

            testObject = new TablePrimaryKeyConstraint("TestPK", new[] { new ColumnStatement("TestColumn") }, new ConflictClause());
            var actual = testObject.GenerateConstraint();
            Assert.Equal("CONSTRAINT TestPK PRIMARY KEY (TestColumn)", actual);

            testObject = new TablePrimaryKeyConstraint(new[] { new ColumnStatement("TestColumn") }, new ConflictClause { Abort = true });
            actual = testObject.GenerateConstraint();
            Assert.Equal("PRIMARY KEY (TestColumn) ON CONFLICT ABORT", actual);
        }

        [Fact]
        public void TableUniqueConstraintTest()
        {
            var testObject = new TableUniqueConstraint("TestUnique", null, null);
            Assert.Throws<ArgumentException>(testObject.GenerateConstraint);

            testObject = new TableUniqueConstraint("TestUnique", new[] { new ColumnStatement("TestColumn") }, new ConflictClause());
            var actual = testObject.GenerateConstraint();
            Assert.Equal("CONSTRAINT TestUnique UNIQUE (TestColumn)", actual);

            testObject = new TableUniqueConstraint(new[] { new ColumnStatement("TestColumn") }, new ConflictClause { Abort = true });
            actual = testObject.GenerateConstraint();
            Assert.Equal("UNIQUE (TestColumn) ON CONFLICT ABORT", actual);
        }
    }
}
