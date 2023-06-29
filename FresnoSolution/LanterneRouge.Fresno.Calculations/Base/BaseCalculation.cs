using LanterneRouge.Fresno.Core.Interface;
using MathNet.Numerics;
using MathNet.Numerics.LinearRegression;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LanterneRouge.Fresno.Calculations.Base
{
    public abstract class BaseCalculation : ICalculation
    {
        #region Constructor

        public BaseCalculation(IEnumerable<IMeasurementEntity> measurements)
        {
            Measurements = measurements.Where(m => m.InCalculation).ToList();
            Measurements.Sort();
        }

        #endregion

        #region Properties

        public List<IMeasurementEntity> Measurements { get; }

        public List<double> Loads => Measurements.Select(m => (double)m.Load).ToList();

        public List<double> Lactates => Measurements.Select(m => (double)m.Lactate).ToList();

        public List<double> HeartRates => Measurements.Select(m => (double)m.HeartRate).ToList();

        public virtual float LoadThreshold { get; }

        public virtual float HeartRateThreshold { get; }

        public Func<double, double> FittedLactateCurve => Fit.PolynomialFunc(Loads.ToArray(), Lactates.ToArray(), 3, DirectRegressionMethod.QR);

        public Func<double, double> L3Curve => Fit.PolynomialFunc(Loads.ToArray(), Lactates.ToArray(), 3, DirectRegressionMethod.QR);

        public double[] L3Factors => Fit.Polynomial(Loads.ToArray(), Lactates.ToArray(), 3);

        public Func<double, double> L2Curve => Fit.LineFunc(Loads.ToArray(), Lactates.ToArray());

        public (double a, double b) L2Factors => Loads.Count > 2 ? Fit.Line(Loads.ToArray(), Lactates.ToArray()) : (0d, 0d);

        public (double a, double b) L2FactorsMin => Fit.Line(new[] { Loads[0], Loads[^1] }, new[] { Lactates[0], Lactates[^1] });

        public Func<double, double> FittedHeartRateCurve => Fit.LineFunc(Loads.ToArray(), HeartRates.ToArray());

        #endregion

        #region Methods

        public double FindLoadFromLactate(double lactate)
        {
            if (Loads == null || Loads.Count <= 3)
            {
                return 0d;
            }

            double func(double x) => FittedLactateCurve(x) - lactate;
            var root = FindRoots.OfFunction(func, Loads.Min(), Loads.Max());

            return root;
        }

        #endregion
    }
}
