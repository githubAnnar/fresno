using Xunit;

namespace LanterneRouge.Fresno.Database.SQLite.Options
{
    public class OptionTest
    {
        [Fact]
        public void TableOptionTest()
        {
            var testObject = new TableOption
            {
                IsStrict = true
            };

            var actual = testObject.GenerateOption();
            Assert.Equal("STRICT", actual);

            testObject = new TableOption
            {
                WithoutRowId = true
            };

            actual = testObject.GenerateOption();
            Assert.Equal("WITHOUT ROWID", actual);

            testObject = new TableOption
            {
                WithoutRowId = true,
                IsStrict = true
            };

            actual = testObject.GenerateOption();
            Assert.Equal("WITHOUT ROWID, STRICT", actual);

            testObject = new TableOption();

            actual = testObject.GenerateOption();
            Assert.Equal(string.Empty, actual);
        }
    }
}
