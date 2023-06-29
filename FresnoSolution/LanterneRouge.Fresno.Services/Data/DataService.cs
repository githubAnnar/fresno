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
        private IUserManager _userManager;
        private IStepTestManager _stepTestManager;
        private IMeasurementManager _measurementManager;
        private ConnectionFactory _connectionFactory;

        private ConnectionFactory LocalConnectionFactory => _connectionFactory ??= new ConnectionFactory(Filename);

        private string Filename { get; set; }

        public bool LoadDatabase(string filename)
        {
            bool response;
            try
            {
                Filename = filename;
                _userManager = new UserManager(LocalConnectionFactory);
                _stepTestManager = new StepTestManager(LocalConnectionFactory);
                _measurementManager = new MeasurementManager(LocalConnectionFactory);
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

        public IEnumerable<IUserEntity> GetAllUsers() => _userManager.GetAllUsers();

        public IUserEntity GetUser(int id) => _userManager.GetUserById(id);

        public IUserEntity GetUserByStepTest(IStepTestEntity stepTest) => _userManager.GetUserByStepTest(stepTest);

        public void SaveUser(IUserEntity entity) => _userManager.UpsertUser(entity);

        public void RemoveUser(IUserEntity entity) => _userManager.RemoveUser(entity);

        public bool IsChanged(IUserEntity entity) => _userManager.IsChanged(entity);

        public bool IsChanged(IStepTestEntity entity) => _stepTestManager.IsChanged(entity);

        public bool IsChanged(IMeasurementEntity entity) => _measurementManager.IsChanged(entity);

        public IEnumerable<IStepTestEntity> GetAllStepTests() => _stepTestManager.GetAllStepTests();

        public IEnumerable<IStepTestEntity> GetAllStepTestsByUser(IUserEntity parent) => _stepTestManager.GetStepTestsByUser(parent);

        public int StepTestCountByUser(IUserEntity entity, bool onlyInCalculation = false) => _stepTestManager.StepTestCountByUser(entity, onlyInCalculation);

        public void SaveStepTest(IStepTestEntity entity) => _stepTestManager.UpsertStepTest(entity);

        public void RemoveStepTest(IStepTestEntity entity) => _stepTestManager.RemoveStepTest(entity);

        public IEnumerable<IMeasurementEntity> GetAllMeasurements() => _measurementManager.GetAllMeasurements();

        public IEnumerable<IMeasurementEntity> GetAllMeasurementsByStepTest(IStepTestEntity entity) => _measurementManager.GetMeasurementsByStepTest(entity);

        public int MeasurementsCountByStepTest(IStepTestEntity entity, bool onlyInCalculation = false) => _measurementManager.MeasurementsCountByStepTest(entity, onlyInCalculation);

        public void SaveMeasurement(IMeasurementEntity entity) => _measurementManager.UpsertMeasurement(entity);

        public void RemoveMeasurement(IMeasurementEntity entity) => _measurementManager.RemoveMeasurement(entity);
    }
}
