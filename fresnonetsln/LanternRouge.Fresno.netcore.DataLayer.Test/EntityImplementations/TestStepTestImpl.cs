using LanterneRouge.Fresno.netcore.DataLayer2.DataAccess.Entities;
using System;
using System.Collections.Generic;

namespace LanternRouge.Fresno.netcore._DataLayer.Test.EntityImplementations
{
    public class TestStepTestImpl : BaseEntity, IStepTest
    {
        public int UserId { get; set; }
        public string TestType { get; set; }
        public string EffortUnit { get; set; }
        public long StepDuration { get; set; }
        public float LoadPreset { get; set; }
        public float Increase { get; set; }
        public float Temperature { get; set; }
        public float Weight { get; set; }
        public DateTime TestDate { get; set; }
        public IList<IMeasurement> Measurements { get; set; }
        public IUser ParentUser { get; set; }
    }
}
