using LanterneRouge.Fresno.Core.Configuration;
using LanterneRouge.Fresno.Core.Interface;
using Microsoft.EntityFrameworkCore;

namespace LanterneRouge.Fresno.Core.Entity
{
    [EntityTypeConfiguration(typeof(MeasurementConfig))]
    public class Measurement : IMeasurementEntity
    {
        public Measurement()
        { }

        public required Guid Id { get; set; }

        public required int HeartRate { get; set; }

        public required float Lactate { get; set; }

        public required float Load { get; set; }

        public required Guid StepTestId { get; set; }

        public required int Sequence { get; set; }

        public required bool InCalculation { get; set; }

        public StepTest? StepTest { get; set; }

        public static Measurement Create(int newSequence, Guid stepTestId, float newLoad) => new()
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
