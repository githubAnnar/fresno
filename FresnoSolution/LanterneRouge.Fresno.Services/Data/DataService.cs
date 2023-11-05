using LanterneRouge.Fresno.Core.Entity;
using LanterneRouge.Fresno.Core.Interface;
using LanterneRouge.Fresno.Repository.Contracts;
using LanterneRouge.Fresno.Repository.Infrastructure;
using LanterneRouge.Fresno.Repository.Managers;
using LanterneRouge.Fresno.Services.Interfaces;
using log4net;

namespace LanterneRouge.Fresno.Services.Data
{
    public class DataService : IDataService
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(DataService));
        private IUserManager? _userManager;
        private IStepTestManager? _stepTestManager;
        private IMeasurementManager? _measurementManager;
        private ConnectionFactory? _connectionFactory;

        private string? Filename { get; set; }

        public bool LoadDatabase(string filename)
        {
            if (filename is null)
            {
                throw new ArgumentNullException(nameof(filename));
            }

            bool response;
            try
            {
                Filename = filename;
                var localConnectionFactory = _connectionFactory ??= new ConnectionFactory(Filename);
                _userManager = new UserManager(localConnectionFactory);
                _stepTestManager = new StepTestManager(localConnectionFactory);
                _measurementManager = new MeasurementManager(localConnectionFactory);
                response = true;
            }

            catch (Exception e)
            {
                Logger.Error("Unexpected error", e);
                response = false;
            }

            if (response)
            {
                Logger.Info($"Database '{filename}' is loaded");
            }

            else
            {
                Logger.Warn($"Database '{filename}' is not loaded");
            }

            return response;
        }

        public bool CloseDatabase()
        {
            bool response;
            try
            {
                _userManager?.Close();

                _stepTestManager?.Close();

                _measurementManager?.Close();

                _connectionFactory = null;
                response = true;
            }

            catch (Exception e)
            {
                Logger.Error($"Unexpected error when closing database '{Filename}'", e);
                response = false;
            }

            if (response && !string.IsNullOrEmpty(Filename))
            {
                Logger.Info($"Database '{Filename}' is closed");
            }

            else if (!response && !string.IsNullOrEmpty(Filename))
            {
                Logger.Warn($"Database '{Filename}' is not closed");
            }

            return response;
        }

        public async Task<IList<User>> GetAllUsers(CancellationToken cancellationToken = default) => _userManager != null ? await _userManager.GetAllUsers(cancellationToken) : new List<User>();

        public async Task<User?> GetUser(Guid id, CancellationToken cancellationToken = default) => _userManager != null ? await _userManager.GetUserById(id, cancellationToken) : null;

        public async Task<User?> GetUserByStepTest(IStepTestEntity stepTestEntity, CancellationToken cancellationToken = default) => _userManager != null ? await _userManager.GetUserByStepTest(stepTestEntity, cancellationToken) : null;

        public async Task<User?> SaveUser(IUserEntity userEntity, CancellationToken cancellationToken = default) => _userManager != null ? await _userManager.UpsertUser(userEntity, cancellationToken) : null;

        public async Task DeleteUser(IUserEntity userEntity, CancellationToken cancellationToken = default)
        {
            if (_userManager != null)
            {
                await _userManager.DeleteUser(userEntity.Id, cancellationToken);
            }
        }

        public async Task<bool> IsChanged(IUserEntity userEntity, CancellationToken cancellationToken = default) => _userManager != null && await _userManager.IsChanged(userEntity, cancellationToken);

        public async Task<bool> IsChanged(IStepTestEntity stepTestEntity, CancellationToken cancellationToken = default) => _stepTestManager != null && await _stepTestManager.IsChanged(stepTestEntity, cancellationToken);

        public async Task<bool> IsChanged(IMeasurementEntity measurementEntity, CancellationToken cancellationToken = default) => _measurementManager != null && await _measurementManager.IsChanged(measurementEntity, cancellationToken);

        public async Task<IList<StepTest>> GetAllStepTests(CancellationToken cancellationToken = default) => _stepTestManager != null ? await _stepTestManager.GetAllStepTests(cancellationToken) : new List<StepTest>();

        public async Task<IList<StepTest>> GetAllStepTestsByUser(IUserEntity userEntity, CancellationToken cancellationToken = default) => _stepTestManager != null ? await _stepTestManager.GetStepTestsByUser(userEntity, cancellationToken) : new List<StepTest>();

        public async Task<int> GetStepTestCountByUser(IUserEntity userEntity, CancellationToken cancellationToken = default) => _stepTestManager != null ? await _stepTestManager.GetCountByUser(userEntity, cancellationToken) : 0;

        public async Task<StepTest?> SaveStepTest(IStepTestEntity stepTestEntity, CancellationToken cancellationToken = default) => _stepTestManager != null ? await _stepTestManager.UpsertStepTest(stepTestEntity, cancellationToken) : null;

        public async Task DeleteStepTest(IStepTestEntity entity, CancellationToken cancellationToken = default)
        {
            if (_stepTestManager != null)
            {
                await _stepTestManager.DeleteStepTest(entity.Id, cancellationToken);
            }
        }

        public async Task<IList<Measurement>> GetAllMeasurements(CancellationToken cancellationToken = default) => _measurementManager != null ? await _measurementManager.GetAllMeasurements(cancellationToken) : new List<Measurement>();

        public async Task<IList<Measurement>> GetAllMeasurementsByStepTest(IStepTestEntity stepTestEntity, CancellationToken cancellationToken) => _measurementManager != null ? await _measurementManager.GetMeasurementsByStepTest(stepTestEntity, cancellationToken) : new List<Measurement>();

        public async Task<int> GetMeasurementCountByStepTest(IStepTestEntity stepTestEntity, bool onlyInCalculation = false, CancellationToken cancellationToken = default) => _measurementManager != null ? await _measurementManager.GetCountByStepTest(stepTestEntity, onlyInCalculation, cancellationToken) : 0;

        public async Task<Measurement?> SaveMeasurement(IMeasurementEntity measurementEntity, CancellationToken cancellationToken = default) => _measurementManager != null ? await _measurementManager.UpsertMeasurement(measurementEntity, cancellationToken) : null;

        public async Task DeleteMeasurement(IMeasurementEntity measurementEntity, CancellationToken cancellationToken = default)
        {
            if (_measurementManager != null)
            {
                await _measurementManager.DeleteMeasurement(measurementEntity.Id, cancellationToken);
            }
        }
    }
}
