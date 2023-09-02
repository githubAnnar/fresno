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

        public StepTest? ParentStepTest { get; set; }

        public static Measurement Create(int sequence, Guid stepTestId, float load) => new()
        {
            Id = Guid.Empty,
            HeartRate = 0,
            Lactate = 0f,
            Load = load,
            InCalculation = true,
            StepTestId = stepTestId,
            Sequence = sequence
        };
    }
}
