using LanterneRouge.Fresno.Core.Entity;
using LanterneRouge.Fresno.Core.Interface;

namespace LanterneRouge.Fresno.Services.Models
{
    public class StepTestModel : IStepTestEntity
    {
        public Guid Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Guid UserId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string TestType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string EffortUnit { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public long StepDuration { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public float LoadPreset { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public float Increase { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public float Temperature { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public float Weight { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public DateTime TestDate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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
