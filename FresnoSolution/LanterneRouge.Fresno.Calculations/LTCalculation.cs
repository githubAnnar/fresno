using LanterneRouge.Fresno.Calculations.Base;
using LanterneRouge.Fresno.Core.Entity;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LanterneRouge.Fresno.Calculations
{
    public class LTCalculation : BaseCalculation
    {
        #region Fields

        private float _lactateThreshold = 0;
        private float _heartRateThreshold = 0;

        #endregion

        #region Constructors

        public LTCalculation(List<Measurement> measurements) : base(measurements)
        { }

        #endregion

        #region Properties

        public override float LoadThreshold
        {
            get
            {
                if (Measurements != null)
                {
                    if (_lactateThreshold == 0)
                    {
                        _lactateThreshold = (float)CalculateLT();
                    }
                }

                return _lactateThreshold;
            }
        }

        public override float HeartRateThreshold
        {
            get
            {
                if (_heartRateThreshold == 0)
                {
                    _heartRateThreshold = (float)FittedHeartRateCurve(LoadThreshold);
                }

                return _heartRateThreshold;
            }
        }

        #endregion

        private double CalculateLT()
        {
            if (Measurements.Count < 4)
            {
                return 0d;
            }

            var count = Measurements.Count;

            var x = Loads.ToArray();
            var y = Lactates.ToArray();

            var results = new List<Result>();

            for (int i = 2; i < count - 2; i++)
            {
                var xsub1 = new double[i];
                var ysub1 = new double[i];
                var xsub2 = new double[count - i];
                var ysub2 = new double[count - i];

                Array.Copy(x, 0, xsub1, 0, i);
                Array.Copy(x, i, xsub2, 0, count - i);
                Array.Copy(y, 0, ysub1, 0, i);
                Array.Copy(y, i, ysub2, 0, count - i);

                var line1 = Fit.Line(xsub1, ysub1);
                var line2 = Fit.Line(xsub2, ysub2);

                var A = Matrix<double>.Build.DenseOfArray(new double[,] { { line1.B, -1 }, { line2.B, -1 } });
                var b = Vector<double>.Build.Dense(new double[] { line1.A * -1, line2.A * -1 });
                var xSolve = A.Solve(b);
                var gof = GoodnessOfFit.CoefficientOfDetermination(x.Select(x1 => x1 < xSolve[0] ? line1.A + (line1.B * x1) : line2.A + (line2.B * x1)), y);
                results.Add(new Result { LoadPoint = xSolve[0], LactatePoint = xSolve[1], CoeffOfFit = gof });
            }

            results.Sort(new ResultComparer());

            return results.Count == 0 ? 0d : results[0].LoadPoint;
        }

        private class Result
        {
            public double LoadPoint { get; set; }

            public double LactatePoint { get; set; }

            public double CoeffOfFit { get; set; }
        }

        private class ResultComparer : IComparer<Result>
        {
            public int Compare(Result x, Result y) => x == null ? y == null ? 0 : 1 : y == null ? -1 : x.CoeffOfFit.CompareTo(y.CoeffOfFit) * -1;
        }
    }
}
