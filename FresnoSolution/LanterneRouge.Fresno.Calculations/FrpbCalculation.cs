using LanterneRouge.Fresno.Calculations.Base;
using LanterneRouge.Fresno.DataLayer.DataAccess.Entities;
using System.Collections.Generic;
using System.Linq;

namespace LanterneRouge.Fresno.Calculations
{
    public class FrpbCalculation : BaseCalculation
    {
        #region Fields

        private float _lactateThreshold = 0;
        private float _heartRateThreshold = 0;

        #endregion

        #region Constructors

        public FrpbCalculation(IEnumerable<Measurement> measurements, double marker) : base(measurements)
        {
            Marker = marker;
        }

        #endregion

        #region Properties

        public double Marker { get; }

        public override float LoadThreshold
        {
            get
            {
                if (Measurements != null)
                {
                    var baseMeasurements = Measurements.ToList();
                    if (_lactateThreshold == 0 && baseMeasurements.Count > 2)
                    {
                        var baseLine = baseMeasurements[0];
                        _lactateThreshold = (float)FindLoadFromLactate(baseMeasurements[0].Lactate + Marker);
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
    }
}
