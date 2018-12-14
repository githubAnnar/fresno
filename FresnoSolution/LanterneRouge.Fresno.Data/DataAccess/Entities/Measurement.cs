namespace LanterneRouge.Fresno.DataLayer.DataAccess.Entities
{
    public class Measurement
    {
        public int Id { get; set; }

        public byte HeartRate { get; set; }

        public float Lactate { get; set; }

        public float Load { get; set; }

        public int StepTestId { get; set; }
    }
}
