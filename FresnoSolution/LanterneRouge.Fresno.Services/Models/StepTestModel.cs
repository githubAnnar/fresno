using LanterneRouge.Fresno.Core.Entity;
using LanterneRouge.Fresno.Core.Interface;

namespace LanterneRouge.Fresno.Services.Models
{
    public class StepTestModel : IStepTestEntity
    {
        public int Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int UserId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string TestType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string EffortUnit { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public long StepDuration { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public float LoadPreset { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public float Increase { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public float Temperature { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public float Weight { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime TestDate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public User User { get; set; }

        public ICollection<MeasurementModel> Measurements { get; set; }
    }
}
