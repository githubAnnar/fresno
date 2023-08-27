using LanterneRouge.Fresno.Core.Interface;

namespace LanterneRouge.Fresno.Services.Models
{
    public class MeasurementModel : IMeasurementEntity
    {
        public int Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int HeartRate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public float Lactate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public float Load { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int StepTestId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int Sequence { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool InCalculation { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public StepTestModel StepTest { get; set; }
    }
}
