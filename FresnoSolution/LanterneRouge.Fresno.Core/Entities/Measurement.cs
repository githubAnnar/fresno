namespace LanterneRouge.Fresno.Core.Entities
{
    public class Measurement : BaseEntity<Measurement>, IComparable<Measurement>
    {
        public int HeartRate { get; set; }

        public float Lactate { get; set; }

        public float Load { get; set; }

        public int StepTestId { get; set; }

        public int Sequence { get; set; }

        public bool InCalculation { get; set; }

        public StepTest ParentStepTest { get; set; }

        public override bool IsValid => StepTestId > 0;

        public static Measurement Create(int sequence, int stepTestId, int heartRate, float lactate, float load)
        {
            var newMeasurement = new Measurement { Sequence = sequence, StepTestId = stepTestId, HeartRate = heartRate, Lactate = lactate, Load = load };
            newMeasurement.AcceptChanges();
            return newMeasurement;
        }

        public int CompareTo(Measurement other) => other == null
                ? 1
                : StepTestId > other.StepTestId ? 1 : StepTestId < other.StepTestId ? -1 : Sequence.CompareTo(other.Sequence);
    }
}
