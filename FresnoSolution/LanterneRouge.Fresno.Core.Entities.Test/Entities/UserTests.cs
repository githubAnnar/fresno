using Bogus;
using static Bogus.DataSets.Name;

namespace LanterneRouge.Fresno.Core.Entity
{
    public class UserTests
    {
        private class UserGender
        {
            public Gender Ug { get; set; }
        }

        private readonly User _user;
        private readonly UserGender _gender;

        public UserTests()
        {
            _gender = new Faker<UserGender>()
                .RuleFor(g => g.Ug, f => f.Person.Gender);

            _user = new Faker<User>()
                .RuleFor(u => u.Id, f => f.Random.Guid())
                .RuleFor(u => u.FirstName, (f, u) => f.Name.FirstName(_gender.Ug))
                .RuleFor(u => u.LastName, (f, u) => f.Name.LastName(_gender.Ug))
                .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName))
                .RuleFor(u => u.Street, f => f.Address.StreetAddress())
                .RuleFor(u => u.PostCity, f => f.Address.City())
                .RuleFor(u => u.PostCode, f => f.Address.ZipCode("####"))
                .RuleFor(u => u.BirthDate, f => f.Person.DateOfBirth)
                .RuleFor(u => u.Height, f => f.Person.Random.Int(150, 210))
                .RuleFor(u => u.Sex, f => _gender.Ug == Gender.Male ? "M" : "F")
                .RuleFor(u => u.MaxHr, f => f.Random.Int(150, 210))
                .FinishWith((f, u) =>
                {
                    Console.WriteLine($"User {u.FirstName} {u.LastName} Created");
                });
        }

        [Fact]
        public void CreateUserTest()
        {
            //var testUser = User.Create(_user.FirstName, _user.LastName, _user.Street, _user.PostCode, _user.PostCity, _user.BirthDate, _user.Height, _user.MaxHr, _user.Sex, _user.Email);

            //Assert.Equal(_user.FirstName, testUser.FirstName);
            //Assert.Equal(_user.LastName, testUser.LastName);
            //Assert.Equal(_user.Email, testUser.Email);
            //Assert.Equal(_user.Street, testUser.Street);
            //Assert.Equal(_user.PostCity, testUser.PostCity);
            //Assert.Equal(_user.PostCode, testUser.PostCode);
            //Assert.Equal(_user.BirthDate, testUser.BirthDate);
            //Assert.Equal(_user.Height, testUser.Height);
            //Assert.Equal(_user.Sex, testUser.Sex);
            //Assert.True(testUser.Sex.Equals("M", StringComparison.InvariantCultureIgnoreCase) || testUser.Sex.Equals("F", StringComparison.InvariantCultureIgnoreCase));
            //Assert.Equal(_user.MaxHr, testUser.MaxHr);
            //Assert.True(testUser.IsLoaded);
        }
    }
}