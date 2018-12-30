using System.Collections.Generic;

namespace LanterneRouge.Fresno.Calculations.Base
{
    public interface IZoneRange
    {
        IEnumerable<Zone> Zones { get; }
    }
}
