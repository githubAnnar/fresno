using LanterneRouge.Fresno.Core.Entity;
using LanterneRouge.Fresno.Core.Interface;

namespace LanterneRouge.Fresno.Core.Contracts
{
    public interface IStepTestRepository
    {
        Task<IList<StepTest>> GetAllStepTestsAsync(CancellationToken cancellationToken = default);

        Task<StepTest?> GetStepTestByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<StepTest?> InsertStepTestAsync(IStepTestEntity stepTestEntity, CancellationToken cancellationToken = default);

        Task<StepTest?> UpdateStepTestAsync(IStepTestEntity stepTestEntity, CancellationToken cancellationToken = default);

        Task DeleteStepTestAsync(Guid id, CancellationToken cancellationToken = default);

        Task<IList<StepTest>> GetStepTestsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

        Task<int> GetCountByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

        Task<bool> IsChangedAsync(IStepTestEntity stepTestEntity, CancellationToken cancellationToken = default);
    }
}
