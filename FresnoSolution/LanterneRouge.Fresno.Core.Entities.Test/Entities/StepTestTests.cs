using Bogus;

namespace LanterneRouge.Fresno.Core.Entity
{
    public class StepTestTests
    {
        private readonly StepTest _stepTest;

        public StepTestTests()
        {
            _stepTest = new Faker<StepTest>()
                .RuleFor(s => s.Id, f => f.Random.Int())
                .RuleFor(s => s.UserId, f => f.Random.Int(0))
                .RuleFor(s => s.EffortUnit, f => f.PickRandom(new[] { "W", "m-s" }))
                .RuleFor(s => s.Increase, f => f.Random.Float(0f, 40f))
                .RuleFor(s => s.LoadPreset, f => f.Random.Float(40f, 200f))
                .RuleFor(s => s.StepDuration, f => f.Random.Long(120L, 300L))
                .RuleFor(s => s.Temperature, f => f.Random.Float(0f, 30f))
                .RuleFor(s => s.TestDate, f => f.Date.Recent())
                .RuleFor(s => s.TestType, f => f.PickRandom(new[] { "Bike", "Run" }))
                .RuleFor(s => s.Weight, f => f.Random.Float(40f, 120f))
                .FinishWith((f, s) =>
                {
                    Console.WriteLine($"StepTest {s.LoadPreset} {s.Increase} Created");
                });
        }

        [Fact]
        public void CreateStepTestTest()
        {
            var testStepTest = new StepTest
            {
                Id = _stepTest.Id,
                UserId = _stepTest.UserId,
                TestType = _stepTest.TestType,
                EffortUnit = _stepTest.EffortUnit,
                StepDuration = _stepTest.StepDuration,
                LoadPreset = _stepTest.LoadPreset,
                Increase = _stepTest.Increase,
                Temperature = _stepTest.Temperature,
                Weight = _stepTest.Weight,
                TestDate = _stepTest.TestDate
            };

            Assert.Equal(_stepTest.UserId, testStepTest.UserId);
            Assert.Equal(_stepTest.EffortUnit, testStepTest.EffortUnit);
            Assert.Equal(_stepTest.Increase, testStepTest.Increase);
            Assert.Equal(_stepTest.LoadPreset, testStepTest.LoadPreset);
            Assert.Equal(_stepTest.StepDuration, testStepTest.StepDuration);
            Assert.Equal(_stepTest.Temperature, testStepTest.Temperature);
            Assert.Equal(_stepTest.TestDate, testStepTest.TestDate);
            Assert.Equal(_stepTest.TestType, testStepTest.TestType);
            Assert.Equal(_stepTest.Weight, testStepTest.Weight);
        }
    }
}
