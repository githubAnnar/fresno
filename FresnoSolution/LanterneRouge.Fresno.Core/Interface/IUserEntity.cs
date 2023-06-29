namespace LanterneRouge.Fresno.Core.Interface
{
    public interface IUserEntity
    {
        int Id { get; set; }

        string FirstName { get; set; }

        string LastName { get; set; }

        string? Street { get; set; }

        string? PostCode { get; set; }

        string? PostCity { get; set; }

        DateTime? BirthDate { get; set; }

        int? Height { get; set; }

        string Sex { get; set; }

        string Email { get; set; }

        int? MaxHr { get; set; }
    }
}
