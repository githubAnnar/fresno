using LanterneRouge.Fresno.Core.Interface;
using System;
using System.Collections.Generic;

namespace LanterneRouge.Fresno.Calculations.Base
{
    public interface ICalculation
    {
        List<IMeasurementEntity> Measurements { get; }

        List<double> Loads { get; }

        List<double> Lactates { get; }

        List<double> HeartRates { get; }

        float LoadThreshold { get; }

        float HeartRateThreshold { get; }

        Func<double, double> FittedLactateCurve { get; }

        Func<double, double> FittedHeartRateCurve { get; }

        double FindLoadFromLactate(double lactate);
    }
}
