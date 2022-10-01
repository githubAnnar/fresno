using LanterneRouge.Fresno.Calculations.Base;
using LanterneRouge.Fresno.DataLayer.DataAccess.Entities;
using System.Collections.Generic;

namespace LanterneRouge.Fresno.Calculations
{
    public class DmaxCalculation : BaseCalculation
    {
        #region Fields

        private float _lactateThreshold = 0;
        private float _heartRateThreshold = 0;

        #endregion

        #region Constructors

        public DmaxCalculation(IEnumerable<Measurement> measurements) : base(measurements)
        { }

        #endregion

        #region Properties

        public override float LoadThreshold => base.LoadThreshold;

        public override float HeartRateThreshold => base.HeartRateThreshold;

        #endregion

        #region Methods

        private double CalculateDmax()
        {
            return 0d;
        }

        #endregion
    }
}
