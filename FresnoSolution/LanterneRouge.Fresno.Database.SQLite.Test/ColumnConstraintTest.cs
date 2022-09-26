using LanterneRouge.Fresno.Database.SQLite.Constraints;
using LanterneRouge.Fresno.Database.SQLite.Types;
using System;
using Xunit;

namespace LanterneRouge.Fresno.Database.SQLite.Test
{
    public class ColumnConstraintTest
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
        public void ForeignKeyConstraintTest()
        {

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
    }
}
