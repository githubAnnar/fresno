using LanterneRouge.Fresno.netcore.DataLayer2.DataAccess.Entities;
using System;

namespace LanternRouge.Fresno.netcore._DataLayer.Test.EntityImplementations
{
    public class TestMeasurementImpl : BaseEntity, IMeasurement
    {
        public int HeartRate { get; set; }
        public float Lactate { get; set; }
        public float Load { get; set; }
        public int StepTestId { get; set; }
        public int Sequence { get; set; }
        public bool InCalculation { get; set; }
        public IStepTest ParentStepTest { get; set; }

        public int CompareTo(IMeasurement other)
        {
            throw new NotImplementedException();
        }
    }
}
