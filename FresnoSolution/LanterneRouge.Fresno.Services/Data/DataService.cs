using Autofac;
using AutoMapper;
using LanterneRouge.Fresno.Core.Contracts;
using LanterneRouge.Fresno.Core.Infrastructure;
using LanterneRouge.Fresno.Core.Interface;
using LanterneRouge.Fresno.Core.Repositories;
using LanterneRouge.Fresno.Core.Repository;
using LanterneRouge.Fresno.Services.Interfaces;
using LanterneRouge.Fresno.Services.Models;
using log4net;

namespace LanterneRouge.Fresno.Services.Data
{
    public class DataService : IDataService
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(DataService));
        private IUserRepository? _userRepository;
        private IStepTestRepository? _stepTestRepository;
        private IMeasurementRepository? _measurementRepository;
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
                var context = new StepTestContext();
                _userRepository = new UserRepository(context);
                _stepTestRepository = new StepTestRepository(context);
                _measurementRepository = new MeasurementRepository(context);
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
            var users = await _userRepository.GetAllUsers(cancellationToken);
            return users.Select(Mapper.Map<UserModel>).ToList();
        }

        public async Task<UserModel?> GetUser(Guid id, CancellationToken cancellationToken = default)
        {
            if (_userRepository != null)
            {
                var user = await _userRepository.GetUserById(id, cancellationToken);
                return Mapper.Map<UserModel>(user);
            }

            return null;
        }

        public async Task<UserModel?> GetUserByStepTest(IStepTestEntity stepTestEntity, CancellationToken cancellationToken = default)
        {
            if (_userRepository != null)
            {
                var user = await _userRepository.GetUserByStepTest(stepTestEntity, cancellationToken);
                return Mapper.Map<UserModel>(user);
            }

            return null;
        }

        public async Task<UserModel?> SaveUser(IUserEntity userEntity, CancellationToken cancellationToken = default)
        {
            if (_userRepository != null)
            {
                var user = userEntity.Id.Equals(Guid.Empty) ? await _userRepository.InsertUser(userEntity, cancellationToken) : await _userRepository.UpdateUser(userEntity, cancellationToken);
                return Mapper.Map<UserModel>(user);
            }

            return null;
        }

        public async Task DeleteUser(IUserEntity userEntity, CancellationToken cancellationToken = default)
        {
            if (_userRepository != null)
            {
                await _userRepository.DeleteUser(userEntity.Id, cancellationToken);
            }
        }

        public async Task<bool> IsChanged(IUserEntity userEntity, CancellationToken cancellationToken = default) => _userRepository != null && await _userRepository.IsChanged(userEntity, cancellationToken);

        public async Task<bool> IsChanged(IStepTestEntity stepTestEntity, CancellationToken cancellationToken = default) => _stepTestRepository != null && await _stepTestRepository.IsChanged(stepTestEntity, cancellationToken);

        public async Task<bool> IsChanged(IMeasurementEntity measurementEntity, CancellationToken cancellationToken = default) => _measurementRepository != null && await _measurementRepository.IsChanged(measurementEntity, cancellationToken);

        public async Task<IList<StepTestModel>> GetAllStepTests(CancellationToken cancellationToken = default)
        {
            if (_stepTestRepository != null)
            {
                var stepTests = await _stepTestRepository.GetAllStepTests(cancellationToken);
                return stepTests.Select(Mapper.Map<StepTestModel>).ToList();
            }

            return new List<StepTestModel>();
        }

        public async Task<IList<StepTestModel>> GetAllStepTestsByUser(IUserEntity userEntity, CancellationToken cancellationToken = default)
        {
            if (_stepTestRepository != null)
            {
                var stepTests = await _stepTestRepository.GetStepTestsByUser(userEntity, cancellationToken);
                return stepTests.Select(Mapper.Map<StepTestModel>).ToList();
            }

            return new List<StepTestModel>();
        }

        public async Task<int> GetStepTestCountByUser(IUserEntity userEntity, CancellationToken cancellationToken = default) => _stepTestRepository != null ? await _stepTestRepository.GetCountByUser(userEntity, cancellationToken) : 0;

        public async Task<StepTestModel?> SaveStepTest(IStepTestEntity stepTestEntity, CancellationToken cancellationToken = default)
        {
            if (_stepTestRepository != null)
            {
                var stepTest = stepTestEntity.Id.Equals(Guid.Empty) ? await _stepTestRepository.InsertStepTest(stepTestEntity, cancellationToken) : await _stepTestRepository.UpdateStepTest(stepTestEntity, cancellationToken);
                return Mapper.Map<StepTestModel>(stepTest);
            }

            return null;
        }

        public async Task DeleteStepTest(IStepTestEntity entity, CancellationToken cancellationToken = default)
        {
            if (_stepTestRepository != null)
            {
                await _stepTestRepository.DeleteStepTest(entity.Id, cancellationToken);
            }
        }

        public async Task<IList<MeasurementModel>> GetAllMeasurements(CancellationToken cancellationToken = default)
        {
            if (_measurementRepository != null)
            {
                var measurements = await _measurementRepository.GetAllMeasurements(cancellationToken);
                return measurements.Select(Mapper.Map<MeasurementModel>).ToList();
            }

            return new List<MeasurementModel>();
        }

        public async Task<IList<MeasurementModel>> GetAllMeasurementsByStepTest(IStepTestEntity stepTestEntity, CancellationToken cancellationToken)
        {
            if (_measurementRepository != null)
            {
                var measurements = await _measurementRepository.GetMeasurementsByStepTest(stepTestEntity, cancellationToken);
                return measurements.Select(Mapper.Map<MeasurementModel>).ToList();
            }

            return new List<MeasurementModel>();
        }

        public async Task<int> GetMeasurementCountByStepTest(IStepTestEntity stepTestEntity, bool onlyInCalculation = false, CancellationToken cancellationToken = default) => _measurementRepository != null ? await _measurementRepository.GetCountByStepTest(stepTestEntity, onlyInCalculation, cancellationToken) : 0;

        public async Task<MeasurementModel?> SaveMeasurement(IMeasurementEntity measurementEntity, CancellationToken cancellationToken = default)
        {
            if (_measurementRepository != null)
            {
                var measurement = measurementEntity.Id.Equals(Guid.Empty) ? await _measurementRepository.InsertMeasurement(measurementEntity, cancellationToken) : await _measurementRepository.UpdateMeasurement(measurementEntity, cancellationToken);
                return Mapper.Map<MeasurementModel>(measurement);
            }

            return null;
        }

        public async Task DeleteMeasurement(IMeasurementEntity measurementEntity, CancellationToken cancellationToken = default)
        {
            if (_measurementRepository != null)
            {
                await _measurementRepository.DeleteMeasurement(measurementEntity.Id, cancellationToken);
            }
        }
    }
}
