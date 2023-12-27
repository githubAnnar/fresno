using LanterneRouge.Fresno.Core.Entity;
using LanterneRouge.Fresno.Core.Interface;

namespace LanterneRouge.Fresno.Services.Models
{
    public class StepTestModel : IStepTestEntity
    {
        public required Guid Id { get; set; }

        public required Guid UserId { get; set; }

        public required string TestType { get; set; }

        public required string EffortUnit { get; set; }

        public long StepDuration { get; set; }

        public float LoadPreset { get; set; }

        public float Increase { get; set; }

        public float? Temperature { get; set; }

        public float Weight { get; set; }

        public DateTime TestDate { get; set; }

        public User? User { get; set; }

        public ICollection<MeasurementModel>? Measurements { get; set; }

        public static StepTestModel Create(Guid userId) => new()
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
