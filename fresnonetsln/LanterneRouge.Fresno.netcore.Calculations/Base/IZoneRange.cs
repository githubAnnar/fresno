using System.Collections.Generic;

namespace LanterneRouge.Fresno.netcore.Calculations.Base
{
    public interface IZoneRange
    {
        IEnumerable<Zone> Zones { get; }
    }
}