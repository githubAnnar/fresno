namespace LanterneRouge.Fresno.Core.Entities
{
    public class User : BaseEntity<User>
    {
        public User() : base()
        {
            IsLoaded = false;
        }

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Street { get; set; } = string.Empty;

        public string PostCode { get; set; } = string.Empty;

        public string PostCity { get; set; } = string.Empty;

        public DateTime BirthDate { get; set; }

        public int Height { get; set; }

        public string Sex { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public int MaxHr { get; set; }

        public override bool IsValid => true;

        public bool IsLoaded { get; set; }

        public static User Create(string firstName, string lastName, string street, string postCode, string postCity, DateTime birthDate, int height, int maxHr, string sex, string email)
        {
            var newUser = new User { FirstName = firstName, LastName = lastName, Street = street, PostCode = postCode, PostCity = postCity, BirthDate = birthDate, Height = height, Sex = sex, Email = email, IsLoaded = true, MaxHr = maxHr };
            newUser.AcceptChanges();
            return newUser;
        }

        public static User Empty => new();
    }
}
