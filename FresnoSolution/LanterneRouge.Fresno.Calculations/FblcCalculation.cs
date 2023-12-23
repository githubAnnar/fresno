using LanterneRouge.Fresno.Calculations.Base;
using LanterneRouge.Fresno.Services.Models;
using System.Collections.Generic;

namespace LanterneRouge.Fresno.Calculations
{
    public class FblcCalculation : BaseCalculation
    {
        #region Fields

        private float _lactateThreshold = 0;
        private float _heartRateThreshold = 0;

        #endregion

        #region Constructors

        public FblcCalculation(List<MeasurementModel> measurements, double marker) : base(measurements)
        {
            Marker = marker;
        }

        #endregion

        #region Properties

        public double Marker { get; }

        public override float LoadThreshold => _lactateThreshold == 0 ? (_lactateThreshold = (float)FindLoadFromLactate(Marker)) : _lactateThreshold;

        public override float HeartRateThreshold => _heartRateThreshold == 0 ? (_heartRateThreshold = (float)FittedHeartRateCurve(LoadThreshold)) : _heartRateThreshold;

        #endregion
    }
}
