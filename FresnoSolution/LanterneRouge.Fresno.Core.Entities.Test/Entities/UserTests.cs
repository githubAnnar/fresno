using Bogus;

namespace LanterneRouge.Fresno.Core.Entities
{
    public class UserTests
    {
        private readonly User _user;
        public UserTests()
        {
            _user = new Faker<User>()
                .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName())
                .RuleFor(u => u.LastName, (f, u) => f.Name.LastName())
                .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
                .RuleFor(u => u.Street, f => f.Address.StreetAddress())
                .RuleFor(u => u.PostCity, f => f.Address.City())
                .RuleFor(u => u.PostCode, f => f.Address.ZipCode("####"))
                .RuleFor(u => u.BirthDate, f => f.Person.DateOfBirth)
                .RuleFor(u => u.Height, f => f.Person.Random.Int(150, 210))
                .RuleFor(u => u.Sex, f => f.Person.Gender == Bogus.DataSets.Name.Gender.Male ? "M" : "F")
                .RuleFor(u => u.MaxHr, f => f.Random.Int(150, 210))
                .FinishWith((f, u) =>
                {
                    Console.WriteLine("User Created");
                });
        }

        [Fact]
        public void CreateUserTest()
        {
            var testUser = User.Create(_user.FirstName, _user.LastName, _user.Street, _user.PostCode, _user.PostCity, _user.BirthDate, _user.Height, 0.0f, _user.Sex, _user.Email);
        }
    }
}