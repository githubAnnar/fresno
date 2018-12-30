using LanterneRouge.Fresno.Calculations.Base;
using LanterneRouge.Fresno.DataLayer.DataAccess.Entities;
using System.Collections.Generic;

namespace LanterneRouge.Fresno.Calculations
{
    public class FlbcCalculation : BaseCalculation
    {
        #region Fields

        private float _lactateThreshold = 0;
        private float _heartRateThreshold = 0;

        #endregion

        #region Constructors

        public FlbcCalculation(IEnumerable<Measurement> measurements, double marker) : base(measurements)
        {
            Marker = marker;
        }

        #endregion

        #region Properties

        public double Marker { get; }

        public override float LactateThreshold
        {
            get
            {
                if (_lactateThreshold == 0)
                {
                    _lactateThreshold = (float)FindLoadFromLactate(Marker);
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
                    _heartRateThreshold = (float)FittedHeartRateCurve(LactateThreshold);
                }

                return _heartRateThreshold;
            }
        }

        #endregion

        //public float GetLt()
        //{
        //    var delta = new List<Tuple<double, double>>();
        //    for (var k = Loads.Min() + 1d; k < Loads.Max() - 1d; k = k + 0.93)
        //    {
        //        var p = Fit.LinearCombinationFunc(
        //            Loads.ToArray(),
        //            Lactates.ToArray(),
        //            x => 1d,
        //            x => x >= k ? x : k,
        //            x => x < k ? 0 : (x - k));

        //        var pVal = Loads.Select(l => p(l)).ToList();
        //        //var results = (from load in Loads from lactate in Lactates from calc in pVal select new Result { x = load, y = lactate, Y = calc }).ToList();
        //        //var results = (Loads.Zip(Lactates, (first, second) => new { x = first, y = second }).Zip(pVal, (first, second) => new { x = first.x, y = first.y, Y = second })).ToList();

        //        //var seg1ObsYAve = results.Where(r => r.x < k).Average(r => r.Y);
        //        //var seg2ObsYAve = results.Where(r => r.x > k).Average(r => r.Y);

        //        //var teller1 = results.Where(r => r.x < k).Sum(r => Math.Pow(r.y - r.Y, 2d));
        //        //var teller2 = results.Where(r => r.x >= k).Sum(r => Math.Pow(r.y - r.Y, 2d));
        //        //var nevner1 = results.Where(r => r.x < k).Sum(r => Math.Pow(r.y - seg1ObsYAve, 2d));
        //        //var nevner2 = results.Where(r => r.x >= k).Sum(r => Math.Pow(r.y - seg2ObsYAve, 2d));

        //        //var r1 = 1 - (teller1 / nevner1);
        //        //var r2 = 1 - (teller2 / nevner2);
        //        var cod = GoodnessOfFit.CoefficientOfDetermination(pVal, Lactates);


        //        delta.Add(new Tuple<double, double>(k, cod));
        //    }



        //    return (float)0;
        //}        
    }
}
