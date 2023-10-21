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

        public IEnumerable<IUserEntity> GetAllUsers() => _userManager != null ? _userManager.GetAllUsers() : new List<IUserEntity>();

        public IUserEntity? GetUser(Guid id) => _userManager?.GetUserById(id);

        public IUserEntity? GetUserByStepTest(IStepTestEntity stepTest) => _userManager?.GetUserByStepTest(stepTest);

        public void SaveUser(IUserEntity entity) => _userManager?.UpsertUser(entity);

        public void RemoveUser(IUserEntity entity) => _userManager?.RemoveUser(entity);

        public bool IsChanged(IUserEntity entity) => _userManager != null && _userManager.IsChanged(entity);

        public bool IsChanged(IStepTestEntity entity) => _stepTestManager != null && _stepTestManager.IsChanged(entity);

        public bool IsChanged(IMeasurementEntity entity) => _measurementManager != null && _measurementManager.IsChanged(entity);

        public IEnumerable<IStepTestEntity> GetAllStepTests() => _stepTestManager != null ? _stepTestManager.GetAllStepTests() : new List<IStepTestEntity>();

        public IEnumerable<IStepTestEntity> GetAllStepTestsByUser(IUserEntity parent) => _stepTestManager != null ? _stepTestManager.GetStepTestsByUser(parent) : new List<IStepTestEntity>();

        public int StepTestCountByUser(IUserEntity entity, bool onlyInCalculation = false) => _stepTestManager != null ? _stepTestManager.StepTestCountByUser(entity, onlyInCalculation) : 0;

        public void SaveStepTest(IStepTestEntity entity) => _stepTestManager?.UpsertStepTest(entity);

        public void RemoveStepTest(IStepTestEntity entity) => _stepTestManager?.RemoveStepTest(entity);

        public IEnumerable<IMeasurementEntity> GetAllMeasurements() => _measurementManager != null ? _measurementManager.GetAllMeasurements() : new List<IMeasurementEntity>();

        public IEnumerable<IMeasurementEntity> GetAllMeasurementsByStepTest(IStepTestEntity entity) => _measurementManager != null ? _measurementManager.GetMeasurementsByStepTest(entity) : new List<IMeasurementEntity>();

        public int MeasurementsCountByStepTest(IStepTestEntity entity, bool onlyInCalculation = false) => _measurementManager != null ? _measurementManager.MeasurementsCountByStepTest(entity, onlyInCalculation) : 0;

        public void SaveMeasurement(IMeasurementEntity entity) => _measurementManager?.UpsertMeasurement(entity);

        public void RemoveMeasurement(IMeasurementEntity entity) => _measurementManager?.RemoveMeasurement(entity);
    }
}
