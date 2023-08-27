using LanterneRouge.Fresno.Core.Interface;

namespace LanterneRouge.Fresno.Services.Models
{
    public class UserModel : IUserEntity
    {
        public int Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string FirstName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string LastName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string? Street { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string? PostCode { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string? PostCity { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime? BirthDate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int? Height { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Sex { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Email { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int? MaxHr { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public ICollection<StepTestModel> StepTestModels { get => throw new NotImplementedException(); set { throw new NotImplementedException(); } }
    }
}
