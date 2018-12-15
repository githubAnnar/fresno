using LanterneRouge.Fresno.DataLayer.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace LanternRouge.Fresno.DataLayer.Test
{
    [TestClass]
    public class DatabaseGeneratorTests
    {
        [TestMethod]
        public void CreateDatabaseTest()
        {
            // Create object
            var actual = new Generator(null);
            Assert.IsTrue(!string.IsNullOrEmpty(actual.Filename));

            // Create database
            Assert.IsTrue(actual.CreateDatabase());

            // Check file exists
            Assert.IsTrue(File.Exists(actual.Filename));

            // Delete file
            File.Delete(actual.Filename);
            Assert.IsFalse(File.Exists(actual.Filename));
        }
    }
}
