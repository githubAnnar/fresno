using LanterneRouge.Fresno.Core.Interface;

namespace LanterneRouge.Fresno.Services.Models
{
    public class MeasurementModel : IMeasurementEntity
    {
        public Guid Id { get; set; }

        public int HeartRate { get ; set ; }

        public float Lactate { get; set; }

        public float Load { get; set; }

        public Guid StepTestId { get; set; }

        public int Sequence { get; set; }

        public bool InCalculation { get; set; }

        public StepTestModel? StepTest { get; set; }

        public static MeasurementModel Create(int newSequence, Guid stepTestId, float newLoad) => new()
        {
            Id = Guid.NewGuid(),
            HeartRate = 0,
            Load = newLoad,
            InCalculation = true,
            Lactate = 0,
            Sequence = newSequence,
            StepTestId = stepTestId
        };
    }
}
