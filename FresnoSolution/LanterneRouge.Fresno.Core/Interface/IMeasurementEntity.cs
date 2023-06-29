namespace LanterneRouge.Fresno.Core.Interface
{
    public interface IMeasurementEntity
    {
        int Id { get; set; }

        int HeartRate { get; set; }

        float Lactate { get; set; }

        float Load { get; set; }

        int StepTestId { get; set; }

        int Sequence { get; set; }

        bool InCalculation { get; set; }
    }
}
