using LanterneRouge.Fresno.netcore.DataLayer.Database;
using System.IO;
using Xunit;

namespace LanternRouge.Fresno.netcore.DataLayer.Test
{
    public class DatabaseGeneratorTests
    {
        [Fact]
        public void CreateDatabaseTest()
        {
            // Create object
            var actual = new Generator(Path.GetTempFileName());
            Assert.True(!string.IsNullOrEmpty(actual.Filename));

            // Create database
            Assert.True(actual.CreateDatabase());

            // Check file exists
            Assert.True(File.Exists(actual.Filename));

            // Delete file
            File.Delete(actual.Filename);
            Assert.False(File.Exists(actual.Filename));
        }

        [Fact]
        public void CreateTablesTest()
        {
            // Create object
            var actual = new Generator(Path.GetTempFileName());
            Assert.True(!string.IsNullOrEmpty(actual.Filename));

            // Create database
            Assert.True(actual.CreateDatabase());

            // Check File exits
            Assert.True(File.Exists(actual.Filename));

            // Create tables
            Assert.True(actual.CreateTables());

            // delete file
            File.Delete(actual.Filename);
            Assert.False(File.Exists(actual.Filename));
        }
    }
}