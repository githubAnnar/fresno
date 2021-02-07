using LanterneRouge.Fresno.netcore.DataLayer2.DataAccess.Entities;
using LanterneRouge.Fresno.netcore.DataLayer2.DataAccess.Infrastructure;
using LanterneRouge.Fresno.netcore.DataLayer2.DataAccess.UnitOfWork;
using LanterneRouge.Fresno.netcore.DataLayer2.Database;
using LanternRouge.Fresno.netcore._DataLayer.Test.EntityImplementations;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using Xunit;

namespace LanternRouge.Fresno.DataLayer.Test
{
    public class UnitOfWorkTests
    {
        [Fact]
        public void AllUnitOfWorksTest()
        {
            // Create temp generator
            var generator = new Generator(Path.GetTempFileName());

            // Create db
            Assert.True(generator.CreateDatabase());

            // Create tables
            Assert.True(generator.CreateTables());

            var connectionFactory = new ConnectionFactory(new SQLiteConnectionStringBuilder { DataSource = generator.Filename, ForeignKeys = true, Version = 3 }.ToString());

            var actual = new UnitOfWork(connectionFactory);

            var user = new TestUserImpl { FirstName = "Annar", LastName = "Gaustad", Email = "annar.gaustad@gmail.com", BirthDate = new DateTime(1968, 3, 10), Height = 183, PostCity = "Lillesand", PostCode = "4790", Sex = "M", Street = "Noan Christian Gauslaasgate 1A" };
            actual.UpsertUser(user);

            var stepTest = new TestStepTestImpl { EffortUnit = "W", Increase = 20f, LoadPreset = 120f, StepDuration = TimeSpan.FromMinutes(4).Ticks, TestType = "Bike", UserId = user.Id, Temperature = 20f, Weight = 90.1f, TestDate = DateTime.Parse("11.05.2017 19:00") };
            actual.UpsertStepTest(stepTest);

            var measurement = new List<IMeasurement> {
                        new TestMeasurementImpl {Sequence = 1,Load = 120f, HeartRate = 104, Lactate = 1.4f, StepTestId = stepTest.Id },
                        new TestMeasurementImpl {Sequence = 2,Load = 140f, HeartRate = 110, Lactate = 1.7f, StepTestId = stepTest.Id },
                        new TestMeasurementImpl {Sequence = 3,Load = 160f, HeartRate = 113, Lactate = 1.6f, StepTestId = stepTest.Id },
                        new TestMeasurementImpl {Sequence = 4,Load = 180f, HeartRate = 122, Lactate = 1.8f, StepTestId = stepTest.Id },
                        new TestMeasurementImpl {Sequence = 5,Load = 200f, HeartRate = 131, Lactate = 2.1f, StepTestId = stepTest.Id },
                        new TestMeasurementImpl {Sequence = 6,Load = 220f, HeartRate = 140, Lactate = 2.7f, StepTestId = stepTest.Id },
                        new TestMeasurementImpl {Sequence = 7,Load = 240f, HeartRate = 148, Lactate = 3.3f, StepTestId = stepTest.Id },
                        new TestMeasurementImpl {Sequence = 8,Load = 260f, HeartRate = 157, Lactate = 5.0f, StepTestId = stepTest.Id },
                        new TestMeasurementImpl {Sequence = 9,Load = 280f, HeartRate = 162, Lactate = 8.7f, StepTestId = stepTest.Id }
                    };

            foreach (var item in measurement)
            {
                actual.UpsertMeasurement(item);
            }

            actual.Commit();

            var allUsers = actual.GetAllUsers<TestUserImpl>().ToList();
            Assert.True(allUsers.Count > 0);
            allUsers[0].FirstName = "Annar Test";
            actual.UpsertUser(allUsers[0]);

            var allStepTests = actual.GetAllStepTests<TestUserImpl>().ToList();
            Assert.True(allStepTests.Count > 0);
            allStepTests[0].TestType = "Run";
            actual.UpsertStepTest(allStepTests[0]);

            var allMeasurements = actual.GetAllMeasurements<TestUserImpl>();
            Assert.True(allMeasurements.Count > 0);
            allMeasurements[0].Load = 140f;
            actual.UpsertMeasurement(allMeasurements[0]);

            actual.Commit();

            var allMeasurments = actual.GetAllMeasurements<TestUserImpl>().ToList();
            allMeasurments.ForEach(m => actual.RemoveMeasurement(m));

            allStepTests = actual.GetAllStepTests<TestUserImpl>().ToList();
            allStepTests.ForEach(m => actual.RemoveStepTest(m));

            allUsers = actual.GetAllUsers<TestUserImpl>().ToList();
            allUsers.ForEach(m => actual.RemoveUser(m));

            actual.Commit();

            allMeasurments = actual.GetAllMeasurements<TestUserImpl>().ToList();
            Assert.True(allMeasurments.Count == 0);

            allStepTests = actual.GetAllStepTests<TestUserImpl>().ToList();
            Assert.True(allStepTests.Count == 0);

            allUsers = actual.GetAllUsers<TestUserImpl>().ToList();
            Assert.True(allUsers.Count == 0);

            actual.Dispose();
            actual = null;

            generator.RemoveDatabase();
        }
    }
}