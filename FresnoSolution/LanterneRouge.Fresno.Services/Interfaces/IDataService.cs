using LanterneRouge.Fresno.Core.Entity;
using LanterneRouge.Fresno.Core.Interface;

namespace LanterneRouge.Fresno.Services.Interfaces
{
    public delegate void CommittedHandler();

    public interface IDataService
    {
        Task<IList<User>> GetAllUsers(CancellationToken cancellationToken = default);

        Task<User?> GetUser(Guid id, CancellationToken cancellationToken = default);

        Task<User?> GetUserByStepTest(IStepTestEntity stepTestEntity, CancellationToken cancellationToken = default);

        Task<User?> SaveUser(IUserEntity userEntity, CancellationToken cancellationToken = default);

        Task DeleteUser(IUserEntity userEntity, CancellationToken cancellation = default);

        Task<bool> IsChanged(IUserEntity userEntity, CancellationToken cancellationToken = default);

        Task<bool> IsChanged(IStepTestEntity stepTestEntity, CancellationToken cancellationToken = default);

        Task<bool> IsChanged(IMeasurementEntity measurementEntity, CancellationToken cancellationToken = default);

        Task<IList<StepTest>> GetAllStepTests(CancellationToken cancellationToken = default);

        Task<IList<StepTest>> GetAllStepTestsByUser(IUserEntity userEntity, CancellationToken cancellationToken = default);

        Task<int> GetStepTestCountByUser(IUserEntity userEntity, CancellationToken cancellationToken = default);

        Task<StepTest?> SaveStepTest(IStepTestEntity stepTestEntity, CancellationToken cancellationToken = default);

        Task DeleteStepTest(IStepTestEntity stepTestEntity, CancellationToken cancellationToken = default);

        Task<IList<Measurement>> GetAllMeasurements(CancellationToken cancellationToken = default);

        Task<IList<Measurement>> GetAllMeasurementsByStepTest(IStepTestEntity stepTestEntity, CancellationToken cancellationToken = default);

        Task<int> GetMeasurementCountByStepTest(IStepTestEntity stepTestEntity, bool onlyInCalculation = false, CancellationToken cancellationToken = default);

        Task<Measurement?> SaveMeasurement(IMeasurementEntity measurementEntity, CancellationToken cancellationToken = default);

        Task DeleteMeasurement(IMeasurementEntity measurementEntity, CancellationToken cancellationToken = default);

        bool LoadDatabase(string filename);

        bool CloseDatabase();
    }
}
