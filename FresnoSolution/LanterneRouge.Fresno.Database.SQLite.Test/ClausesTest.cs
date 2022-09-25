using LanterneRouge.Fresno.Database.SQLite.Clauses;
using System;
using Xunit;

namespace LanterneRouge.Fresno.Database.SQLite.Test
{
    public class ClausesTest
    {
        [Fact]
        public void ConflicCauseTest()
        {
            var testObject = new ConflictClause
            {
                Fail = true
            };

            var actual = testObject.GenerateConflictClause();
            Assert.Equal("ON CONFLICT FAIL", actual);

            testObject.Fail = false;
            testObject.Abort = true;

            actual = testObject.GenerateConflictClause();
            Assert.Equal("ON CONFLICT ABORT", actual);

            testObject.Abort = false;
            testObject.Replace = true;

            actual = testObject.GenerateConflictClause();
            Assert.Equal("ON CONFLICT REPLACE", actual);

            testObject.Replace = false;
            testObject.Rollback = true;

            actual = testObject.GenerateConflictClause();
            Assert.Equal("ON CONFLICT ROLLBACK", actual);

            testObject.Rollback = false;
            testObject.Ignore = true;

            actual = testObject.GenerateConflictClause();
            Assert.Equal("ON CONFLICT IGNORE", actual);
        }

        [Fact]
        public void TooManyCausesTest()
        {
            var testObject = new ConflictClause
            {
                Fail = true,
                Abort = true
            };

            Assert.Throws<ArgumentException>(testObject.GenerateConflictClause);
        }

        [Fact]
        public void NoClausesTest()
        {
            var testObject = new ConflictClause();
            var actual = testObject.GenerateConflictClause();
            Assert.Equal(string.Empty, actual);
        }
    }
}
