using LanterneRouge.Fresno.Core.Entities;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace LanterneRouge.Fresno.Calculations
{
    public class LactateCalculationTest
    {
        private readonly List<Measurement> TestMeasurementData;

        public LactateCalculationTest()
        {
            TestMeasurementData = new List<Measurement>
            {
                new Measurement {Sequence = 1,Load = 120f, HeartRate = 104, Lactate = 1.4f, StepTestId = 1, InCalculation = true},
                new Measurement {Sequence = 2,Load = 140f, HeartRate = 110, Lactate = 1.7f, StepTestId = 1, InCalculation = true},
                new Measurement {Sequence = 3,Load = 160f, HeartRate = 113, Lactate = 1.6f, StepTestId = 1, InCalculation = true},
                new Measurement {Sequence = 4,Load = 180f, HeartRate = 122, Lactate = 1.8f, StepTestId = 1, InCalculation = true},
                new Measurement {Sequence = 5,Load = 200f, HeartRate = 131, Lactate = 2.1f, StepTestId = 1, InCalculation = true},
                new Measurement {Sequence = 6,Load = 220f, HeartRate = 140, Lactate = 2.7f, StepTestId = 1, InCalculation = true},
                new Measurement {Sequence = 7,Load = 240f, HeartRate = 148, Lactate = 3.3f, StepTestId = 1, InCalculation = true},
                new Measurement {Sequence = 8,Load = 260f, HeartRate = 157, Lactate = 5.0f, StepTestId = 1, InCalculation = true},
                new Measurement {Sequence = 9,Load = 280f, HeartRate = 162, Lactate = 8.7f, StepTestId = 1, InCalculation = true }
            };
        }

        [Fact]
        public void CalculationTest()
        {
            var actual = new FblcCalculation(TestMeasurementData, 4d);
            var ltL = actual.LoadThreshold;
            var ltH = actual.HeartRateThreshold;

            var actualLBZones = new LactateBasedZones(actual, new[] { 0.8, 1.5, 2.5, 4.0, 6.0, 10.0 });
            Assert.True(actualLBZones.Zones.ToList().Count == 6);

            var actualPBZones = new PercentOfLTBasedZones(actual, new[] { 0.4, 0.55, 0.75, 0.90, 1.05, 1.2 });
            Assert.True(actualPBZones.Zones.ToList().Count == 6);
        }

        [Fact]
        public void LTCalculationTest()
        {
            var actual = new LTCalculation(TestMeasurementData);
            var t1 = actual.LoadThreshold;
            var t2 = actual.HeartRateThreshold;
        }

        [Fact]
        public void DMaxCalculationTest()
        {
            var actual = new DmaxCalculation(TestMeasurementData, true);
            var t1 = actual.LoadThreshold;
            var t2 = actual.HeartRateThreshold;
        }
    }
}
