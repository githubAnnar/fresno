﻿using LanterneRouge.Fresno.Core.Configuration;
using LanterneRouge.Fresno.Core.Interface;
using Microsoft.EntityFrameworkCore;

namespace LanterneRouge.Fresno.Core.Entity
{
    [EntityTypeConfiguration(typeof(MeasurementConfig))]
    public class Measurement : IMeasurementEntity
    {
        public required int Id { get; set; }

        public required int HeartRate { get; set; }

        public required float Lactate { get; set; }

        public required float Load { get; set; }

        public required int StepTestId { get; set; }

        public required int Sequence { get; set; }

        public required bool InCalculation { get; set; }

        public StepTest? StepTest { get; set; }
    }
}
