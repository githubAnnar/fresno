using Autofac;
using AutoMapper;
using LanterneRouge.Fresno.Core.Interface;
using LanterneRouge.Fresno.Repository.Contracts;
using LanterneRouge.Fresno.Repository.Infrastructure;
using LanterneRouge.Fresno.Repository.Managers;
using LanterneRouge.Fresno.Services.Interfaces;
using LanterneRouge.Fresno.Services.Models;
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

        private IMapper Mapper { get; }

        private string? Filename { get; set; }

        public DataService()
        {
            var scope = ServiceLocator.Instance.BeginLifetimeScope();
            Mapper = scope.Resolve<IMapper>();
        }

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

        public async Task<IList<UserModel>> GetAllUsers(CancellationToken cancellationToken = default)
        {
            if (_userManager != null)
            {
                var users = await _userManager.GetAllUsers(cancellationToken);
                return users.Select(Mapper.Map<UserModel>).ToList();
            }

            return new List<UserModel>();
        }

        public async Task<UserModel?> GetUser(Guid id, CancellationToken cancellationToken = default)
        {
            if (_userManager != null)
            {
                var user = await _userManager.GetUserById(id, cancellationToken);
                return Mapper.Map<UserModel>(user);
            }

            return null;
        }

        public async Task<UserModel?> GetUserByStepTest(IStepTestEntity stepTestEntity, CancellationToken cancellationToken = default)
        {
            if (_userManager != null)
            {
                var user = await _userManager.GetUserByStepTest(stepTestEntity, cancellationToken);
                return Mapper.Map<UserModel>(user);
            }

            return null;
        }

        public async Task<UserModel?> SaveUser(IUserEntity userEntity, CancellationToken cancellationToken = default)
        {
            if (_userManager != null)
            {
                var user = await _userManager.UpsertUser(userEntity, cancellationToken);
                return Mapper.Map<UserModel>(user);
            }

            return null;
        }

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

        public async Task<IList<StepTestModel>> GetAllStepTests(CancellationToken cancellationToken = default)
        {
            if (_stepTestManager != null)
            {
                var stepTests = await _stepTestManager.GetAllStepTests(cancellationToken);
                return stepTests.Select(Mapper.Map<StepTestModel>).ToList();
            }

            return new List<StepTestModel>();
        }

        public async Task<IList<StepTestModel>> GetAllStepTestsByUser(IUserEntity userEntity, CancellationToken cancellationToken = default)
        {
            if (_stepTestManager != null)
            {
                var stepTests = await _stepTestManager.GetStepTestsByUser(userEntity, cancellationToken);
                return stepTests.Select(Mapper.Map<StepTestModel>).ToList();
            }

            return new List<StepTestModel>();
        }

        public async Task<int> GetStepTestCountByUser(IUserEntity userEntity, CancellationToken cancellationToken = default) => _stepTestManager != null ? await _stepTestManager.GetCountByUser(userEntity, cancellationToken) : 0;

        public async Task<StepTestModel?> SaveStepTest(IStepTestEntity stepTestEntity, CancellationToken cancellationToken = default)
        {
            if (_stepTestManager != null)
            {
                var stepTest = await _stepTestManager.UpsertStepTest(stepTestEntity, cancellationToken);
                return Mapper.Map<StepTestModel>(stepTest);
            }

            return null;
        }

        public async Task DeleteStepTest(IStepTestEntity entity, CancellationToken cancellationToken = default)
        {
            if (_stepTestManager != null)
            {
                await _stepTestManager.DeleteStepTest(entity.Id, cancellationToken);
            }
        }

        public async Task<IList<MeasurementModel>> GetAllMeasurements(CancellationToken cancellationToken = default)
        {
            if (_measurementManager != null)
            {
                var measurements = await _measurementManager.GetAllMeasurements(cancellationToken);
                return measurements.Select(Mapper.Map<MeasurementModel>).ToList();
            }

            return new List<MeasurementModel>();
        }

        public async Task<IList<MeasurementModel>> GetAllMeasurementsByStepTest(IStepTestEntity stepTestEntity, CancellationToken cancellationToken)
        {
            if (_measurementManager != null)
            {
                var measurements = await _measurementManager.GetMeasurementsByStepTest(stepTestEntity, cancellationToken);
                return measurements.Select(Mapper.Map<MeasurementModel>).ToList();
            }

            return new List<MeasurementModel>();
        }

        public async Task<int> GetMeasurementCountByStepTest(IStepTestEntity stepTestEntity, bool onlyInCalculation = false, CancellationToken cancellationToken = default) => _measurementManager != null ? await _measurementManager.GetCountByStepTest(stepTestEntity, onlyInCalculation, cancellationToken) : 0;

        public async Task<MeasurementModel?> SaveMeasurement(IMeasurementEntity measurementEntity, CancellationToken cancellationToken = default)
        {
            if (_measurementManager != null)
            {
                var measurement = await _measurementManager.UpsertMeasurement(measurementEntity, cancellationToken);
                return Mapper.Map<MeasurementModel>(measurement);
            }
            return null;
        }

        public async Task DeleteMeasurement(IMeasurementEntity measurementEntity, CancellationToken cancellationToken = default)
        {
            if (_measurementManager != null)
            {
                await _measurementManager.DeleteMeasurement(measurementEntity.Id, cancellationToken);
            }
        }
    }
}
