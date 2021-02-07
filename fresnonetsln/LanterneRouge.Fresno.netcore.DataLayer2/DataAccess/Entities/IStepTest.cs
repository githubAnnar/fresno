using System;
using System.Collections.Generic;

namespace LanterneRouge.Fresno.netcore.DataLayer2.DataAccess.Entities
{
    public interface IStepTest:IBaseEntity
    {
        int UserId { get; set; }

        string TestType { get; set; }

        string EffortUnit { get; set; }

        long StepDuration { get; set; }

        float LoadPreset { get; set; }

        float Increase { get; set; }

        float Temperature { get; set; }

        float Weight { get; set; }

        DateTime TestDate { get; set; }

        IList<IMeasurement> Measurements { get; set; }

        IUser ParentUser { get; set; }
    }
}