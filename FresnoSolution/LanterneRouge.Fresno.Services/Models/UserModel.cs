using LanterneRouge.Fresno.Core.Interface;

namespace LanterneRouge.Fresno.Services.Models
{
    public class UserModel : IUserEntity
    {
        public Guid Id { get; set; }
        public required string FirstName { get ; set ; }
        public required string LastName { get ; set ; }
        public string? Street { get; set; }
        public string? PostCode { get; set; }
        public string? PostCity { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? Height { get; set; }
        public required string Sex { get ; set ; }
        public required string Email { get; set; }
        public int? MaxHr { get; set; }

        public ICollection<StepTestModel>? StepTestModels { get => throw new NotImplementedException(); set { throw new NotImplementedException(); } }

        public static UserModel Create() => new()
        {
            Id = Guid.NewGuid(),
            Email = string.Empty,
            BirthDate = DateTime.Now,
            FirstName = string.Empty,
            Height = 0,
            Sex = "M",
            LastName = string.Empty,
            MaxHr = null,
            PostCity = null,
            PostCode = null,
            Street = null,
        };

        public override int GetHashCode() => Id.GetHashCode();
    }
}
