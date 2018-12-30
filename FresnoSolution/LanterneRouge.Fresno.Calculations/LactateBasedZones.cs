using LanterneRouge.Fresno.Calculations.Base;
using System;
using System.Collections.Generic;

namespace LanterneRouge.Fresno.Calculations
{
    public class LactateBasedZones : IZoneRange
    {
        private List<Zone> _zones;

        public LactateBasedZones(ICalculation calculation, double[] limits)
        {
            Calculation = calculation;
            Array.Sort(limits);
            Limits = limits;
        }

        public IEnumerable<Zone> Zones
        {
            get
            {
                if (_zones == null)
                {
                    _zones = new List<Zone>();
                    for (int i = 1; i <= Limits.Length; i++)
                    {
                        var lLoad = Calculation.FindLoadFromLactate(Limits[i - 1]);
                        var lHr = Calculation.FittedHeartRateCurve(lLoad);
                        var uLoad = i == Limits.Length ? double.PositiveInfinity : Calculation.FindLoadFromLactate(Limits[i]);
                        var uHr = i == Limits.Length ? double.PositiveInfinity : Calculation.FittedHeartRateCurve(uLoad);
                        _zones.Add(new Zone($"Zone {i}", lLoad, uLoad, lHr, uHr));
                    }
                }

                return _zones;
            }
        }

        private ICalculation Calculation { get; set; }

        private double[] Limits { get; set; }
    }
}
