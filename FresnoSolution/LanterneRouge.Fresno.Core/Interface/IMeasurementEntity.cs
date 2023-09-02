namespace LanterneRouge.Fresno.Core.Interface
{
    public interface IMeasurementEntity
    {
        Guid Id { get; set; }

        int HeartRate { get; set; }

        float Lactate { get; set; }

        float Load { get; set; }

        Guid StepTestId { get; set; }

        int Sequence { get; set; }

        bool InCalculation { get; set; }
    }
}
