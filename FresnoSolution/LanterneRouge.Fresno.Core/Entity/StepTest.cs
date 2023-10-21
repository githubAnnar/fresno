using LanterneRouge.Fresno.Core.Configuration;
using LanterneRouge.Fresno.Core.Interface;
using Microsoft.EntityFrameworkCore;

namespace LanterneRouge.Fresno.Core.Entity
{
    [EntityTypeConfiguration(typeof(StepTestConfig))]
    public class StepTest : IStepTestEntity
    {
        private StepTest()
        { }

        public required Guid Id { get; set; }

        public required Guid UserId { get; set; }

        public required string TestType { get; set; }

        public required string EffortUnit { get; set; }

        public required long StepDuration { get; set; }

        public required float LoadPreset { get; set; }

        public required float Increase { get; set; }

        public float Temperature { get; set; }

        public float Weight { get; set; }

        public required DateTime TestDate { get; set; }

        public User? User { get; set; }

        public ICollection<Measurement>? Measurements { get; set; }

        public static StepTest Create(Guid userId) => new()
        {
            Id = Guid.Empty,
            EffortUnit = "W",
            Increase = 0f,
            Temperature = 0f,
            Weight = 0f,
            LoadPreset = 0f,
            StepDuration = TimeSpan.FromMinutes(4d).Ticks,
            TestDate = DateTime.Now,
            TestType = "Bike",
            UserId = userId
        };
    }
}
