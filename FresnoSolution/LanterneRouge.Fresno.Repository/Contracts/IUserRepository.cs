using LanterneRouge.Fresno.Core.Entity;
using LanterneRouge.Fresno.Core.Interface;

namespace LanterneRouge.Fresno.Repository.Contracts
{
    public interface IUserRepository
    {
        Task<IList<User>> GetAllUsers(CancellationToken cancellationToken = default);

        Task<User> GetUserById(Guid id, CancellationToken cancellationToken = default);

        Task<User> InsertUser(IUserEntity userEntity, CancellationToken cancellationToken = default);

        Task<User> UpdateUser(IUserEntity userEntity, CancellationToken cancellationToken = default);

        Task DeleteUser(Guid id, CancellationToken cancellationToken = default);

        Task<bool> IsChanged(IUserEntity userEntity, CancellationToken cancellationToken = default);
    }
}
