using System;

namespace LanterneRouge.Fresno.netcore.DataLayer2.DataAccess.Entities
{
    public interface IMeasurement : IBaseEntity, IComparable<IMeasurement>
    {
        int HeartRate { get; set; }

        float Lactate { get; set; }

        float Load { get; set; }

        int StepTestId { get; set; }

        int Sequence { get; set; }

        bool InCalculation { get; set; }

        IStepTest ParentStepTest { get; set; }
    }
}
