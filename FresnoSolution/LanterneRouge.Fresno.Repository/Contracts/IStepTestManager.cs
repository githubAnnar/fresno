using LanterneRouge.Fresno.Core.Entity;
using LanterneRouge.Fresno.Core.Interface;

namespace LanterneRouge.Fresno.Repository.Contracts
{
    public interface IStepTestManager : IManagerBase, IDisposable
    {
        Task<IList<StepTest>> GetAllStepTests(CancellationToken cancellationToken = default);

        Task<IList<StepTest>> GetStepTestsByUser(IUserEntity userEntity, CancellationToken cancellationToken = default);

        Task<int> GetCountByUser(IUserEntity userEntity, CancellationToken cancellationToken = default);

        Task<StepTest?> GetStepTestById(Guid id, CancellationToken cancellationToken = default);

        Task<StepTest?> UpsertStepTest(IStepTestEntity stepTestEntity, CancellationToken cancellationToken = default);

        Task DeleteStepTest(Guid id, CancellationToken cancellationToken = default);

        Task<bool> IsChanged(IStepTestEntity stepTestEntity, CancellationToken cancellationToken = default);
    }
}
