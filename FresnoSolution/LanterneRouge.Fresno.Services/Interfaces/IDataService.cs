using LanterneRouge.Fresno.Core.Interface;
using LanterneRouge.Fresno.Services.Models;

namespace LanterneRouge.Fresno.Services.Interfaces
{
    public delegate void CommittedHandler();

    public interface IDataService
    {
        Task<IList<UserModel>> GetAllUsersAsync(CancellationToken cancellationToken = default);

        Task<UserModel?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<UserModel?> GetUserByStepTestIdAsync(Guid stepTestId, CancellationToken cancellationToken = default);

        Task<UserModel?> SaveUserAsync(IUserEntity userEntity, CancellationToken cancellationToken = default);

        Task DeleteUserAsync(IUserEntity userEntity, CancellationToken cancellation = default);

        Task<bool> IsChangedAsync(IUserEntity userEntity, CancellationToken cancellationToken = default);

        Task<bool> IsChangedAsync(IStepTestEntity stepTestEntity, CancellationToken cancellationToken = default);

        Task<bool> IsChangedAsync(IMeasurementEntity measurementEntity, CancellationToken cancellationToken = default);

        Task<IList<StepTestModel>> GetAllStepTestsAsync(CancellationToken cancellationToken = default);

        Task<IList<StepTestModel>> GetAllStepTestsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

        Task<int> GetStepTestCountByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

        Task<StepTestModel?> SaveStepTestAsync(IStepTestEntity stepTestEntity, CancellationToken cancellationToken = default);

        Task DeleteStepTestAsync(IStepTestEntity stepTestEntity, CancellationToken cancellationToken = default);

        Task<IList<MeasurementModel>> GetAllMeasurementsAsync(CancellationToken cancellationToken = default);

        Task<IList<MeasurementModel>> GetAllMeasurementsByStepTestIdAsync(Guid stepTestId, CancellationToken cancellationToken = default);

        Task<int> GetMeasurementCountByStepTestIdAsync(Guid stepTestId, bool onlyInCalculation = false, CancellationToken cancellationToken = default);

        Task<MeasurementModel?> SaveMeasurement(IMeasurementEntity measurementEntity, CancellationToken cancellationToken = default);

        Task DeleteMeasurementAsync(IMeasurementEntity measurementEntity, CancellationToken cancellationToken = default);

        bool LoadDatabase(string filename);

        bool CloseDatabase();
    }
}
