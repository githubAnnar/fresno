using LanterneRouge.Fresno.Core.Interface;
using LanterneRouge.Fresno.Services.Models;

namespace LanterneRouge.Fresno.Services.Interfaces
{
    public delegate void CommittedHandler();

    public interface IDataService
    {
        Task<IList<UserModel>> GetAllUsers(CancellationToken cancellationToken = default);

        Task<UserModel?> GetUser(Guid id, CancellationToken cancellationToken = default);

        Task<UserModel?> GetUserByStepTest(IStepTestEntity stepTestEntity, CancellationToken cancellationToken = default);

        Task<UserModel?> SaveUser(IUserEntity userEntity, CancellationToken cancellationToken = default);

        Task DeleteUser(IUserEntity userEntity, CancellationToken cancellation = default);

        Task<bool> IsChanged(IUserEntity userEntity, CancellationToken cancellationToken = default);

        Task<bool> IsChanged(IStepTestEntity stepTestEntity, CancellationToken cancellationToken = default);

        Task<bool> IsChanged(IMeasurementEntity measurementEntity, CancellationToken cancellationToken = default);

        Task<IList<StepTestModel>> GetAllStepTests(CancellationToken cancellationToken = default);

        Task<IList<StepTestModel>> GetAllStepTestsByUser(IUserEntity userEntity, CancellationToken cancellationToken = default);

        Task<int> GetStepTestCountByUser(IUserEntity userEntity, CancellationToken cancellationToken = default);

        Task<StepTestModel?> SaveStepTest(IStepTestEntity stepTestEntity, CancellationToken cancellationToken = default);

        Task DeleteStepTest(IStepTestEntity stepTestEntity, CancellationToken cancellationToken = default);

        Task<IList<MeasurementModel>> GetAllMeasurements(CancellationToken cancellationToken = default);

        Task<IList<MeasurementModel>> GetAllMeasurementsByStepTest(IStepTestEntity stepTestEntity, CancellationToken cancellationToken = default);

        Task<int> GetMeasurementCountByStepTest(IStepTestEntity stepTestEntity, bool onlyInCalculation = false, CancellationToken cancellationToken = default);

        Task<MeasurementModel?> SaveMeasurement(IMeasurementEntity measurementEntity, CancellationToken cancellationToken = default);

        Task DeleteMeasurement(IMeasurementEntity measurementEntity, CancellationToken cancellationToken = default);

        bool LoadDatabase(string filename);

        bool CloseDatabase();
    }
}
