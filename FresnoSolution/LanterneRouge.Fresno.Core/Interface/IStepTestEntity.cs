namespace LanterneRouge.Fresno.Core.Interface
{
    public interface IStepTestEntity
    {
        Guid Id { get; set; }

        Guid UserId { get; set; }

        string TestType { get; set; }

        string EffortUnit { get; set; }

        long StepDuration { get; set; }

        float LoadPreset { get; set; }

        float Increase { get; set; }

        float? Temperature { get; set; }

        float Weight { get; set; }

        DateTime TestDate { get; set; }
    }
}
