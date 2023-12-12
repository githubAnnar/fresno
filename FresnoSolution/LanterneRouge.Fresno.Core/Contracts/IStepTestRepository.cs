using LanterneRouge.Fresno.Core.Entity;
using LanterneRouge.Fresno.Core.Interface;

namespace LanterneRouge.Fresno.Core.Contracts
{
    public interface IStepTestRepository
    {
        Task<IList<StepTest>> GetAllStepTests(CancellationToken cancellationToken = default);

        Task<StepTest?> GetStepTestById(Guid id, CancellationToken cancellationToken = default);

        Task<StepTest?> InsertStepTest(IStepTestEntity stepTestEntity, CancellationToken cancellationToken = default);

        Task<StepTest?> UpdateStepTest(IStepTestEntity stepTestEntity, CancellationToken cancellationToken = default);

        Task DeleteStepTest(Guid id, CancellationToken cancellationToken = default);

        Task<IList<StepTest>> GetStepTestsByUser(IUserEntity userEntity, CancellationToken cancellationToken = default);

        Task<int> GetCountByUser(IUserEntity userEntity, CancellationToken cancellationToken = default);

        Task<bool> IsChanged(IStepTestEntity stepTestEntity, CancellationToken cancellationToken = default);
    }
}
