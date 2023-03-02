using LanterneRouge.Fresno.Core.Entities;

namespace LanterneRouge.Fresno.Repository.Contracts
{
    public interface IUserManager : IManagerBase, IDisposable
    {
        List<User> GetAllUsers(bool refresh = false);

        User GetUserById(int id, bool refresh = false);

        void UpsertUser(User entity);

        void RemoveUser(User entity);
    }
}
