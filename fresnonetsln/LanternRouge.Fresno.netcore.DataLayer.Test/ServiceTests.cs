using LanterneRouge.Fresno.netcore.DataLayer2.DataAccess.Entities;
//using LanterneRouge.Fresno.DataLayer.DataAccess.Services;
using LanterneRouge.Fresno.netcore.DataLayer2.Database;
//using LanterneRouge.Fresno.netcore.WpfClient.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace LanternRouge.Fresno.DataLayer.Test
{
    public class ServiceTests
    {
        [Fact]
        public void ServiceTest()
        {
            //// Create temp generator
            //var generator = new Generator(null);

            //// Create db
            //Assert.IsTrue(generator.CreateDatabase());

            //// Create tables
            //generator.CreateTables();

            //if (ServiceLocator.Instance.GetService(typeof(IDataService)) is IDataService service)
            //{
            //    var user = new User { FirstName = "Annar", LastName = "Gaustad", Email = "annar.gaustad@gmail.com", BirthDate = new DateTime(1968, 3, 10), Height = 183, PostCity = "Lillesand", PostCode = "4790", Sex = "M", Street = "Noan Christian Gauslaasgate 1A" };
            //    service.AddUser(user);

            //    var stepTest = new StepTest { EffortUnit = "W", Increase = 20f, LoadPreset = 120f, StepDuration = TimeSpan.FromMinutes(4), TestType = "Bike", UserId = user.Id, Temperature = 20f, Weight = 90.1f, TestDate = DateTime.Parse("11.05.2017 19:00") };
            //    service.AddStepTest(stepTest);

            //    var measurement = new List<Measurement> {
            //            new Measurement {Sequence = 1,Load = 120f, HeartRate = 104, Lactate = 1.4f, StepTestId = stepTest.Id },
            //            new Measurement {Sequence = 2,Load = 140f, HeartRate = 110, Lactate = 1.7f, StepTestId = stepTest.Id },
            //            new Measurement {Sequence = 3,Load = 160f, HeartRate = 113, Lactate = 1.6f, StepTestId = stepTest.Id },
            //            new Measurement {Sequence = 4,Load = 180f, HeartRate = 122, Lactate = 1.8f, StepTestId = stepTest.Id },
            //            new Measurement {Sequence = 5,Load = 200f, HeartRate = 131, Lactate = 2.1f, StepTestId = stepTest.Id },
            //            new Measurement {Sequence = 6,Load = 220f, HeartRate = 140, Lactate = 2.7f, StepTestId = stepTest.Id },
            //            new Measurement {Sequence = 7,Load = 240f, HeartRate = 148, Lactate = 3.3f, StepTestId = stepTest.Id },
            //            new Measurement {Sequence = 8,Load = 260f, HeartRate = 157, Lactate = 5.0f, StepTestId = stepTest.Id },
            //            new Measurement {Sequence = 9,Load = 280f, HeartRate = 162, Lactate = 8.7f, StepTestId = stepTest.Id }
            //        };

            //    foreach (var item in measurement)
            //    {
            //        service.AddMeasurement(item);
            //    }

            //    service.Commit();

            //    var allUsers = service.GetAllUsers().ToList();
            //    Assert.IsTrue(allUsers.Count > 0);
            //    allUsers[0].FirstName = "Annar Test";
            //    service.UpdateUser(allUsers[0]);

            //    var allStepTests = service.GetAllStepTests().ToList();
            //    Assert.IsTrue(allStepTests.Count > 0);
            //    allStepTests[0].TestType = "Run";
            //    service.UpdateStepTest(allStepTests[0]);

            //    var allMeasurements = service.GetAllMeasurements().ToList();
            //    Assert.IsTrue(allMeasurements.Count > 0);
            //    allMeasurements[0].Load = 140f;
            //    service.UpdateMeasurement(allMeasurements[0]);

            //    service.Commit();

            //    var allMeasurments = service.GetAllMeasurements().ToList();
            //    allMeasurments.ForEach(m => service.RemoveMeasurement(m));

            //    allStepTests = service.GetAllStepTests().ToList();
            //    allStepTests.ForEach(m => service.RemoveStepTest(m));

            //    allUsers = service.GetAllUsers().ToList();
            //    allUsers.ForEach(m => service.RemoveUser(m));

            //    service.Commit();

            //    allMeasurments = service.GetAllMeasurements().ToList();
            //    Assert.IsTrue(allMeasurments.Count == 0);

            //    allStepTests = service.GetAllStepTests().ToList();
            //    Assert.IsTrue(allStepTests.Count == 0);

            //    allUsers = service.GetAllUsers(true).ToList();
            //    Assert.IsTrue(allUsers.Count == 0);
            //}
        }
    }
}