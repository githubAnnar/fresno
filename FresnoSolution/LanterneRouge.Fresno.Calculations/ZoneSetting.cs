using System;
using System.Collections.Generic;
using System.Linq;

namespace LanterneRouge.Fresno.Calculations
{
    [Serializable]
    public class ZoneSetting
    {
        public string Name { get; set; }

        public IEnumerable<double> Limits { get; set; }
    }

    [Serializable]
    public class ZoneSettings : List<ZoneSetting>
    {
        public ZoneSetting GetZoneSetting(string name) => this.FirstOrDefault(s => s.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));

        public static ZoneSettings Default
        {
            get
            {
                var defaultSettings = new ZoneSettings
                {
                    new ZoneSetting {
                        Name = nameof(DmaxCalculation),
                        Limits = new[] { 0.4, 0.55, 0.75, 0.90, 1.05, 1.2 }
                    },
                    new ZoneSetting
                    {
                        Name = nameof(FblcCalculation),
                        Limits= new[] { 0.8, 1.5, 2.5, 4.0, 6.0, 10.0 }
                    },
                    new ZoneSetting
                    {
                        Name = nameof(FrpbCalculation),
                        Limits= new[] { 0.4, 0.55, 0.75, 0.90, 1.05, 1.2 }
                    },
                    new ZoneSetting
                    {
                        Name=nameof(LTCalculation),
                        Limits= new[] { 0.4, 0.55, 0.75, 0.90, 1.05, 1.2 }
                    },
                    new ZoneSetting
                    {
                        Name=nameof(LTLogCalculation),
                        Limits= new[] { 0.4, 0.55, 0.75, 0.90, 1.05, 1.2 }
                    }
                };

                return defaultSettings;
            }
        }
    }

}
