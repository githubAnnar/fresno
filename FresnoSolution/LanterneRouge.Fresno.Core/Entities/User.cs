namespace LanterneRouge.Fresno.Core.Entities
{
    public class User : BaseEntity<User>
    {
        public User() : base()
        {
            StepTests = new List<StepTest>();
            IsLoaded = false;
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Street { get; set; }

        public string PostCode { get; set; }

        public string PostCity { get; set; }

        public DateTime BirthDate { get; set; }

        public int Height { get; set; }

        public string Sex { get; set; }

        public string Email { get; set; }

        public int MaxHr { get; set; }

        public List<StepTest> StepTests { get; set; }

        public override bool IsValid => true;

        public bool IsLoaded { get; set; }

        public static User Create(string firstName, string lastName, string street, string postCode, string postCity, DateTime birthDate, int height, float weight, string sex, string email)
        {
            var newUser = new User { FirstName = firstName, LastName = lastName, Street = street, PostCode = postCode, PostCity = postCity, BirthDate = birthDate, Height = height, Sex = sex, Email = email, IsLoaded = true };
            newUser.AcceptChanges();
            return newUser;
        }
    }
}
