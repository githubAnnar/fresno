using LanterneRouge.Fresno.Core.Entity;
using LanterneRouge.Fresno.Core.Interface;

namespace LanterneRouge.Fresno.Core.Contracts
{
    public interface IUserRepository
    {
        Task<IList<User>> GetAllUsersAsync(CancellationToken cancellationToken = default);

        Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<User?> InsertUserAsync(IUserEntity userEntity, CancellationToken cancellationToken = default);

        Task<User?> UpdateUserAsync(IUserEntity userEntity, CancellationToken cancellationToken = default);

        Task DeleteUserAsync(Guid id, CancellationToken cancellationToken = default);

        Task<bool> IsChangedAsync(IUserEntity userEntity, CancellationToken cancellationToken = default);

        Task<User> GetUserByStepTestIdAsync(Guid stepTestId, CancellationToken cancellationToken = default);
    }
}
