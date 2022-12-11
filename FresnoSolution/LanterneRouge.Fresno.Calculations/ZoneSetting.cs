using System;
using System.Collections.Generic;

namespace LanterneRouge.Fresno.Calculations
{
    [Serializable]
    public class ZoneSetting
    {
        public string Name { get; set; }

        public IEnumerable<double> Limits { get; set; }
    }

    [Serializable]
    public class ZoneSettings : List<ZoneSetting> { }
}
