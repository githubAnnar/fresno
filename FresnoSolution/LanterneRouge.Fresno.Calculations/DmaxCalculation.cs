using LanterneRouge.Fresno.Calculations.Base;
using LanterneRouge.Fresno.DataLayer.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.IO;

namespace LanterneRouge.Fresno.Calculations
{
    public class DmaxCalculation : BaseCalculation
    {
        #region Fields

        private float _loadThreshold = 0;
        private float _heartRateThreshold = 0;
        private float _lactateThreshold = 0;

        #endregion

        #region Constructors

        public DmaxCalculation(IEnumerable<Measurement> measurements) : base(measurements)
        { }

        #endregion

        #region Properties

        public override float LoadThreshold
        {
            get
            {
                if (Measurements != null)
                {
                    if (_loadThreshold == 0)
                    {
                        _loadThreshold = (float)CalculateDmax();
                    }
                }

                return _loadThreshold;
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

        public float LactateThreshold
        {
            get
            {
                if (_lactateThreshold == 0)
                {
                    _lactateThreshold = (float)FittedLactateCurve(LoadThreshold);
                }

                return _lactateThreshold;
            }
        }

        #endregion

        #region Methods

        private double CalculateDmax()
        {
            // Calculate the L2 Factors
            var (_, b) = L2FactorsMin;

            // Calculate the L3 Factors
            var L3f = L3Factors;


            var temp = Math.Sqrt(Math.Pow(L3f[2], 2d) - 3 * L3f[3] * (L3f[1] - b));
            var w1 = (-1 * L3f[2] + temp) / (3 * L3f[3]);
            var w2 = (-1 * L3f[1] - temp) / (3 * L3f[1]);

            return w1;
        }

        #endregion
    }
}
