using LanterneRouge.Fresno.DataLayer.DataAccess.Entities;
using LanterneRouge.Fresno.DataLayer.DataAccess.Repositories;
using LanterneRouge.Fresno.DataLayer.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SQLite;
using System.IO;

namespace LanternRouge.Fresno.DataLayer.Test
{
    [TestClass]
    public class RepositoryTests
    {
        [TestMethod]
        public void AllRepositoriesTest()
        {
            // Create temp generator
            var generator = new Generator(Path.GetTempFileName());

            // Create db
            Assert.IsTrue(generator.CreateDatabase());

            // Create tables
            Assert.IsTrue(generator.CreateTables());

            // Connect database
            var connectionString = new SQLiteConnectionStringBuilder
            {
                DataSource = generator.Filename,
                ForeignKeys = true,
                Version = 3
            };

            using (var sqlite_connection = new SQLiteConnection(connectionString.ToString()))
            {
                sqlite_connection.Open();
                using (var transaction = sqlite_connection.BeginTransaction())
                {
                    var user = new User { FirstName = "Annar", LastName = "Gaustad", Email = "annar.gaustad@gmail.com" };
                    var userRepo = new UserRepository(transaction);
                    userRepo.Add(user);

                    var stepTest = new StepTest { Sequence = 1, UserId = user.Id };
                    var stepTestRepo = new StepTestRepository(transaction);
                    stepTestRepo.Add(stepTest);

                    var measurement = new Measurement { HeartRate = 130, Lactate = 1.0f, Load = 130f, StepTestId = stepTest.Id };
                    var measurementRepo = new MeasurementRepository(transaction);
                    measurementRepo.Add(measurement);

                    var user2 = userRepo.Find(user.Id);

                    Assert.IsTrue(user.Id == user2.Id);
                    transaction.Commit();
                }
            }
        }
    }
}
