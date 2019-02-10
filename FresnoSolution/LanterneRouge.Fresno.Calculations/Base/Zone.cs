namespace LanterneRouge.Fresno.Calculations.Base
{
    public struct Zone
    {
        public Zone(Zone lowerZone, string name, double load, double heartRate, int index)
        {
            Name = name;
            UpperLoad = load;
            UpperHeartRate = heartRate;
            LowerHeartRate = lowerZone.UpperHeartRate;
            LowerLoad = lowerZone.UpperLoad;
            Index = index;
        }

        public Zone(string name, double lowerLoad, double upperLoad, double lowerHeartRate, double upperHeartRate, int index)
        {
            Name = name;
            UpperLoad = upperLoad;
            UpperHeartRate = upperHeartRate;
            LowerLoad = lowerLoad;
            LowerHeartRate = lowerHeartRate;
            Index = index;
        }

        public int Index { get; }

        public string Name { get; }

        public double UpperLoad { get; }

        public double UpperHeartRate { get; }

        public double LowerLoad { get; }

        public double LowerHeartRate { get; }

        public override string ToString() => $"{Name} HR: {LowerHeartRate.ToString("#.0")}-{UpperHeartRate.ToString("#.0")} LD: {LowerLoad.ToString("#.0")}-{UpperLoad.ToString("#.0")}";
    }
}

