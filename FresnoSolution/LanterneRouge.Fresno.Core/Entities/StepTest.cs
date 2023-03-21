namespace LanterneRouge.Fresno.Core.Entities
{
    public class StepTest : BaseEntity<StepTest>
    {
        public StepTest() : base()
        { }

        public int UserId { get; set; }

        public string TestType { get; set; } = string.Empty;

        public string EffortUnit { get; set; } = string.Empty;

        public long StepDuration { get; set; }

        public float LoadPreset { get; set; }

        public float Increase { get; set; }

        public float Temperature { get; set; }

        public float Weight { get; set; }

        public DateTime TestDate { get; set; }

        //public List<Measurement> Measurements { get; set; } = new List<Measurement>();

        public override bool IsValid => UserId > 0;

        //public User? ParentUser { get; set; }

        public static StepTest Create(int userId, string testType, string effortUnit, long stepDuration, float loadPreset, float increase, float temperature, float weight, DateTime testDate)
        {
            var newStepTest = new StepTest { UserId = userId, TestType = testType, EffortUnit = effortUnit, StepDuration = stepDuration, LoadPreset = loadPreset, Increase = increase, Temperature = temperature, Weight = weight, TestDate = testDate };
            newStepTest.AcceptChanges();
            return newStepTest;
        }
    }
}
