using Bogus;

namespace LanterneRouge.Fresno.Core.Entity
{
    public class MeasurementTests
    {
        private readonly Measurement _measurement;

        public MeasurementTests()
        {
            _measurement = new Faker<Measurement>()
                .RuleFor(m => m.HeartRate, f => f.Random.Int(100, 210))
                .RuleFor(m => m.Lactate, f => f.Random.Float(0.5f, 12f))
                .RuleFor(m => m.Load, f => f.Random.Float(40f, 400f))
                .RuleFor(m => m.StepTestId, f => f.Random.Int(0))
                .RuleFor(m => m.Sequence, f => f.Random.Int(1))
                .RuleFor(m => m.InCalculation, f => f.Random.Bool())
                .FinishWith((f, m) =>
                {
                    Console.WriteLine($"Measurement {m.Sequence} {m.HeartRate} {m.Lactate} Created");
                });
        }

        [Fact]
        public void CreateMeasurementTest()
        {
            //var testMeasurement = Measurement.Create(_measurement.Sequence, _measurement.StepTestId, _measurement.HeartRate, _measurement.Lactate, _measurement.Load);
            //testMeasurement.InCalculation = _measurement.InCalculation;

            //Assert.Equal(_measurement.HeartRate, testMeasurement.HeartRate);
            //Assert.Equal(_measurement.InCalculation, testMeasurement.InCalculation);
            //Assert.Equal(_measurement.Lactate, testMeasurement.Lactate);
            //Assert.Equal(_measurement.Load, testMeasurement.Load);
            //Assert.Equal(_measurement.StepTestId, testMeasurement.StepTestId);
            //Assert.Equal(_measurement.Sequence, testMeasurement.Sequence);
        }
    }
}
