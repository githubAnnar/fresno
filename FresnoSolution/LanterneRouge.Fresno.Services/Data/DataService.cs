using LanterneRouge.Fresno.Core.Entities;
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

        public IEnumerable<User> GetAllUsers() => _userManager.GetAllUsers();

        public User GetUser(int id) => _userManager.GetUserById(id);

        public User GetUserByStepTest(StepTest stepTest) => _userManager.GetUserByStepTest(stepTest);

        public void SaveUser(User entity) => _userManager.UpsertUser(entity);

        public void RemoveUser(User entity) => _userManager.RemoveUser(entity);

        public IEnumerable<StepTest> GetAllStepTests() => _stepTestManager.GetAllStepTests();

        public IEnumerable<StepTest> GetAllStepTestsByUser(User parent) => _stepTestManager.GetStepTestsByUser(parent);

        public int StepTestCountByUser(User entity, bool onlyInCalculation = false) => _stepTestManager.StepTestCountByUser(entity, onlyInCalculation);

        public void SaveStepTest(StepTest entity) => _stepTestManager.UpsertStepTest(entity);

        public void RemoveStepTest(StepTest entity) => _stepTestManager.RemoveStepTest(entity);

        public IEnumerable<Measurement> GetAllMeasurements() => _measurementManager.GetAllMeasurements();

        public IEnumerable<Measurement> GetAllMeasurementsByStepTest(StepTest entity) => _measurementManager.GetMeasurementsByStepTest(entity);

        public int MeasurementsCountByStepTest(StepTest entity, bool onlyInCalculation = false) => _measurementManager.MeasurementsCountByStepTest(entity, onlyInCalculation);

        public void SaveMeasurement(Measurement entity) => _measurementManager.UpsertMeasurement(entity);

        public void RemoveMeasurement(Measurement entity) => _measurementManager.RemoveMeasurement(entity);
    }
}
