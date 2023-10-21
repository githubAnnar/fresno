using LanterneRouge.Fresno.Core.Configuration;
using LanterneRouge.Fresno.Core.Interface;
using Microsoft.EntityFrameworkCore;

namespace LanterneRouge.Fresno.Core.Entity
{
    [EntityTypeConfiguration(typeof(UserConfig))]
    public class User : IUserEntity
    {
        private User() { }

        public required Guid Id { get; set; }

        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public string? Street { get; set; }

        public string? PostCode { get; set; }

        public string? PostCity { get; set; } = string.Empty;

        public DateTime? BirthDate { get; set; }

        public int? Height { get; set; }

        public required string Sex { get; set; }

        public required string Email { get; set; }

        public int? MaxHr { get; set; }

        public ICollection<StepTest>? StepTests { get; set; }

        public static User Create() => new()
        {
            Email = string.Empty,
            BirthDate = DateTime.Now,
            FirstName = string.Empty,
            Height = 0,
            Sex = "M",
            Id = Guid.Empty,
            LastName = string.Empty,
            MaxHr = null,
            PostCity = null,
            PostCode = null,
            Street = null,
        };
    }
}
