using LanterneRouge.Fresno.DataLayer.DataAccess.Entities;
using LanterneRouge.Fresno.DataLayer.DataAccess.Repositories;
using LanterneRouge.Fresno.DataLayer.Database;
using log4net;
using log4net.Config;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;

namespace LanternRouge.Fresno.DataLayer.Test
{
    [TestClass]
    public class RepositoryTests
    {
        [AssemblyInitialize]
        public static void Configure(TestContext tc)
        {
            var repository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));
        }

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
                    var user = new User { FirstName = "Annar", LastName = "Gaustad", Email = "annar.gaustad@gmail.com", BirthDate = new DateTime(1968, 3, 10), Height = 183, PostCity = "Lillesand", PostCode = "4790", Sex = "M", Street = "Noan Christian Gauslaasgate 1A" };
                    var userRepo = new UserRepository(transaction);
                    userRepo.Add(user);

                    var stepTest = new StepTest { EffortUnit = "W", Increase = 20f, LoadPreset = 120f, StepDuration = TimeSpan.FromMinutes(4), TestType = "Bike", UserId = user.Id, Temperature = 20f, Weight = 90.1f, TestDate = DateTime.Parse("11.05.2017 19:00") };
                    var stepTestRepo = new StepTestRepository(transaction);
                    stepTestRepo.Add(stepTest);

                    var measurement = new List<Measurement> {
                        new Measurement {Sequence = 1,Load = 120f, HeartRate = 104, Lactate = 1.4f, StepTestId = stepTest.Id },
                        new Measurement {Sequence = 2,Load = 140f, HeartRate = 110, Lactate = 1.7f, StepTestId = stepTest.Id },
                        new Measurement {Sequence = 3,Load = 160f, HeartRate = 113, Lactate = 1.6f, StepTestId = stepTest.Id },
                        new Measurement {Sequence = 4,Load = 180f, HeartRate = 122, Lactate = 1.8f, StepTestId = stepTest.Id },
                        new Measurement {Sequence = 5,Load = 200f, HeartRate = 131, Lactate = 2.1f, StepTestId = stepTest.Id },
                        new Measurement {Sequence = 6,Load = 220f, HeartRate = 140, Lactate = 2.7f, StepTestId = stepTest.Id },
                        new Measurement {Sequence = 7,Load = 240f, HeartRate = 148, Lactate = 3.3f, StepTestId = stepTest.Id },
                        new Measurement {Sequence = 8,Load = 260f, HeartRate = 157, Lactate = 5.0f, StepTestId = stepTest.Id },
                        new Measurement {Sequence = 9,Load = 280f, HeartRate = 162, Lactate = 8.7f, StepTestId = stepTest.Id }
                    };

                    var measurementRepo = new MeasurementRepository(transaction);
                    foreach (var item in measurement)
                    {
                        measurementRepo.Add(item);
                    }

                    transaction.Commit();
                }

                using (var transaction = sqlite_connection.BeginTransaction())
                {
                    var userRepo = new UserRepository(transaction);
                    var allUsers = userRepo.All().ToList();
                    Assert.IsTrue(allUsers.Count > 0);
                    allUsers[0].FirstName = "Annar Test";
                    userRepo.Update(allUsers[0]);

                    var stepTestRepo = new StepTestRepository(transaction);
                    var allStepTests = stepTestRepo.All().ToList();
                    Assert.IsTrue(allStepTests.Count > 0);
                    allStepTests[0].TestType = "Run";
                    stepTestRepo.Update(allStepTests[0]);

                    var measurementRepo = new MeasurementRepository(transaction);
                    var allMeasurements = measurementRepo.All().ToList();
                    Assert.IsTrue(allMeasurements.Count > 0);
                    allMeasurements[0].Load = 140f;
                    measurementRepo.Update(allMeasurements[0]);

                    transaction.Commit();
                }

                using (var transaction = sqlite_connection.BeginTransaction())
                {
                    var measurementRepo = new MeasurementRepository(transaction);
                    var allMeasurments = measurementRepo.All().ToList();
                    allMeasurments.ForEach(m => measurementRepo.Remove(m));

                    var stepTestRepo = new StepTestRepository(transaction);
                    var allStepTests = stepTestRepo.All().ToList();
                    allStepTests.ForEach(m => stepTestRepo.Remove(m));

                    var usertRepo = new UserRepository(transaction);
                    var allUsers = usertRepo.All().ToList();
                    allUsers.ForEach(m => usertRepo.Remove(m));

                    transaction.Commit();
                }

                using (var transaction = sqlite_connection.BeginTransaction())
                {
                    var measurementRepo = new MeasurementRepository(transaction);
                    var allMeasurments = measurementRepo.All().ToList();
                    Assert.IsTrue(allMeasurments.Count == 0);

                    var stepTestRepo = new StepTestRepository(transaction);
                    var allStepTests = stepTestRepo.All().ToList();
                    Assert.IsTrue(allStepTests.Count == 0);

                    var usertRepo = new UserRepository(transaction);
                    var allUsers = usertRepo.All().ToList();
                    Assert.IsTrue(allUsers.Count == 0);
                }

                sqlite_connection.Close();
                sqlite_connection.Dispose();
            }

            generator.RemoveDatabase();
        }
    }
}
