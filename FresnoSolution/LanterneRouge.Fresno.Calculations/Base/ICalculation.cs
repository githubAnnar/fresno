using LanterneRouge.Fresno.DataLayer.DataAccess.Entities;
using System;
using System.Collections.Generic;

namespace LanterneRouge.Fresno.Calculations.Base
{
    public interface ICalculation
    {
        IEnumerable<Measurement> Measurements { get; }

        IEnumerable<double> Loads { get; }

        IEnumerable<double> Lactates { get; }

        IEnumerable<double> HeartRates { get; }

        float LactateThreshold { get; }

        float HeartRateThreshold { get; }

        Func<double, double> FittedLactateCurve { get; }

        Func<double, double> FittedHeartRateCurve { get; }

        double FindLoadFromLactate(double lactate);
    }
}
