namespace LanterneRouge.Fresno.Calculations.Base
{
    public struct Zone
    {
        public Zone(Zone lowerZone, string name, double load, double heartRate)
        {
            Name = name;
            UpperLoad = load;
            UpperHeartRate = heartRate;
            LowerHeartRate = lowerZone.UpperHeartRate;
            LowerLoad = lowerZone.UpperLoad;
        }

        public Zone(string name, double lowerLoad, double upperLoad, double lowerHeartRate, double upperHeartRate)
        {
            Name = name;
            UpperLoad = upperLoad;
            UpperHeartRate = upperHeartRate;
            LowerLoad = lowerLoad;
            LowerHeartRate = lowerHeartRate;
        }

        public string Name { get; }

        public double UpperLoad { get; }

        public double UpperHeartRate { get; }

        public double LowerLoad { get; }

        public double LowerHeartRate { get; }
    }
}

