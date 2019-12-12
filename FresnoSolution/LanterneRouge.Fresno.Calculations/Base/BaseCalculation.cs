using LanterneRouge.Fresno.DataLayer.DataAccess.Entities;
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

        public BaseCalculation(IEnumerable<Measurement> measurements)
        {
            Measurements = measurements.Where(m => m.InCalculation).ToList();
            Measurements.Sort();
        }

        #endregion

        #region Properties

        public List<Measurement> Measurements { get; }

        public List<double> Loads => Measurements.Select(m => (double)m.Load).ToList();

        public List<double> Lactates => Measurements.Select(m => (double)m.Lactate).ToList();

        public List<double> HeartRates => Measurements.Select(m => (double)m.HeartRate).ToList();

        public virtual float LoadThreshold { get; }

        public virtual float HeartRateThreshold { get; }

        public Func<double, double> FittedLactateCurve => Fit.PolynomialFunc(Loads.ToArray(), Lactates.ToArray(), 3, DirectRegressionMethod.QR);

        public Func<double, double> FittedHeartRateCurve => Fit.LineFunc(Loads.ToArray(), HeartRates.ToArray());

        #endregion

        #region Methods

        public double FindLoadFromLactate(double lactate)
        {
            if (Loads == null || Loads.Count() <= 3)
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
