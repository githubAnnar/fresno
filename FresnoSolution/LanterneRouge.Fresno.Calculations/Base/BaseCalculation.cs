﻿using LanterneRouge.Fresno.DataLayer.DataAccess.Entities;
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
            Measurements = measurements;
        }

        #endregion

        #region Properties

        public IEnumerable<Measurement> Measurements { get; }

        public IEnumerable<double> Loads => Measurements.Select(m => (double)m.Load);

        public IEnumerable<double> Lactates => Measurements.Select(m => (double)m.Lactate);

        public IEnumerable<double> HeartRates => Measurements.Select(m => (double)m.HeartRate);

        public virtual float LactateThreshold { get; }

        public virtual float HeartRateThreshold { get; }

        public Func<double, double> FittedLactateCurve => Fit.PolynomialFunc(Loads.ToArray(), Lactates.ToArray(), 3, DirectRegressionMethod.QR);

        public Func<double, double> FittedHeartRateCurve => Fit.PolynomialFunc(Loads.ToArray(), HeartRates.ToArray(), 3, DirectRegressionMethod.QR);

        #endregion

        #region Methods

        public double FindLoadFromLactate(double lactate)
        {
            double func(double x) => FittedLactateCurve(x) - lactate;
            var root = FindRoots.OfFunction(func, Loads.Min(), Loads.Max());

            return root;
        }

        #endregion
    }
}