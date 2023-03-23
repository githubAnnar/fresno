using LanterneRouge.Fresno.Core.Entities;

namespace LanterneRouge.Fresno.Repository.Contracts
{
    public interface IUserManager : IManagerBase, IDisposable
    {
        List<User> GetAllUsers();

        User GetUserById(int id);

        User GetUserByStepTest(StepTest stepTest);

        void UpsertUser(User entity);

        void RemoveUser(User entity);
    }
}
