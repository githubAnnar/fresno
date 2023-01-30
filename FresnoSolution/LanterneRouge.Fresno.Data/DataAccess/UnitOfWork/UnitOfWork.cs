using LanterneRouge.Fresno.DataLayer.DataAccess.Entities;
using LanterneRouge.Fresno.DataLayer.DataAccess.Infrastructure;
using LanterneRouge.Fresno.DataLayer.DataAccess.Repositories;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace LanterneRouge.Fresno.DataLayer.DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Fields

        private static readonly ILog Logger = LogManager.GetLogger(typeof(UnitOfWork));
        private IDbConnection _connection;
        private IDbTransaction _transaction;
        private IRepository<Measurement, StepTest> _measurementRepository;
        private IRepository<StepTest, User> _stepTestRepository;
        private IRepository<User, object> _userRepository;
        private bool _disposed;
        private List<User> _cachedUsers;

        #endregion

        #region Constructors

        public UnitOfWork(IConnectionFactory connectionFactory)
        {
            _connection = connectionFactory.GetConnection;
            Logger.Debug($"Connection set: {_connection.ConnectionString}");
            _transaction = _connection.BeginTransaction();
        }

        #endregion

        #region Properties

        private IRepository<Measurement, StepTest> MeasurementRepository => _measurementRepository ?? (_measurementRepository = new MeasurementRepository(_transaction));

        private IRepository<StepTest, User> StepTestRepository => _stepTestRepository ?? (_stepTestRepository = new StepTestRepository(_transaction));

        private IRepository<User, object> UserRepository => _userRepository ?? (_userRepository = new UserRepository(_transaction));

        private List<User> CachedUsers => _cachedUsers ?? (_cachedUsers = new List<User>());

        private List<StepTest> CachedStepTests => CachedUsers.SelectMany(u => u.StepTests).ToList();

        private List<Measurement> CachedMeasurements => CachedUsers.SelectMany(u => u.StepTests.SelectMany(s => s.Measurements)).ToList();

        #endregion

        #region Methods

        public void Commit()
        {
            var updatedUsers = CachedUsers.Where(u => u.State == EntityState.Updated);
            var newUsers = CachedUsers.Where(u => u.State == EntityState.New);
            var updatedStepTests = CachedStepTests.Where(s => s.State == EntityState.Updated);
            var newStepTests = CachedStepTests.Where(s => s.State == EntityState.New);
            var updatedMeasurements = CachedMeasurements.Where(m => m.State == EntityState.Updated);
            var newMeasurements = CachedMeasurements.Where(m => m.State == EntityState.New);

            foreach (var item in newUsers)
            {
                Logger.Info($"Adding new user: {item.LastName}, {item.FirstName}");
                UserRepository.Add(item);
            }

            foreach (var item in newStepTests)
            {
                Logger.Info($"Adding new steptest for user: {item.ParentUser.LastName}, {item.ParentUser.FirstName}");
                StepTestRepository.Add(item);
            }

            foreach (var item in newMeasurements)
            {
                Logger.Info($"Adding new measurement: {item.ParentStepTest.ParentUser.LastName}, {item.ParentStepTest.ParentUser.FirstName}, Steptest: {item.ParentStepTest.Id}");
                MeasurementRepository.Add(item);
            }

            foreach (var item in updatedUsers)
            {
                Logger.Info($"Update user: {item.LastName}, {item.FirstName}");
                UserRepository.Update(item);
            }

            foreach (var item in updatedStepTests)
            {
                Logger.Info($"Update steptest for user: {item.ParentUser.LastName}, {item.ParentUser.FirstName}");
                StepTestRepository.Update(item);
            }

            foreach (var item in updatedMeasurements)
            {
                Logger.Info($"Update measurement: {item.ParentStepTest.ParentUser.LastName}, {item.ParentStepTest.ParentUser.FirstName}, Steptest: {item.ParentStepTest.Id}");
                MeasurementRepository.Update(item);
            }

            try
            {
                _transaction.Commit();
                Logger.Debug("Transaction Committed");
            }

            catch (Exception e)
            {
                _transaction.Rollback();
                Logger.Error("Error in commit, transaction is rolled back!", e);
                throw;
            }

            finally
            {
                _transaction.Dispose();
                _transaction = _connection.BeginTransaction();
                ResetRepositories();
            }
        }

        public void Close()
        {
            // Close db connection
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }

            _connection.Dispose();
            _connection = null;
            Logger.Info("Db connection closed");

            // Reset Caches
            _cachedUsers = null;
            Logger.Info("Cached users is reset");

            // Reset repositories
            _measurementRepository = null;
            _stepTestRepository = null;
            _userRepository = null;
            Logger.Info("Repositories is reset");
        }

        #region User Methods

        public List<User> GetAllUsers(bool refresh = false)
        {
            if (refresh || CachedUsers.Count == 0)
            {
                _cachedUsers = UserRepository.All().ToList();
            }

            return CachedUsers;
        }

        public User GetUserById(int id, bool refresh = false)
        {
            if (refresh || CachedUsers.Count == 0)
            {
                _cachedUsers = UserRepository.All().ToList();
            }

            return _cachedUsers.FirstOrDefault(u => u.Id == id);
        }

        public void UpsertUser(User entity)
        {
            if (entity.State == EntityState.New)
            {
                UserRepository.Add(entity);
            }

            else
            {
                UserRepository.Update(entity);
            }
        }

        public void RemoveUser(User entity) => UserRepository.Remove(entity);

        #endregion

        #region Step Test Methods

        public List<StepTest> GetAllStepTests(bool refresh = false)
        {
            if (refresh || CachedUsers.Count == 0)
            {
                _cachedUsers = UserRepository.All().ToList();
            }

            return CachedStepTests;
        }

        public StepTest GetStepTestById(int id, bool refresh = false)
        {
            if (refresh || CachedUsers.Count == 0)
            {
                _cachedUsers = UserRepository.All().ToList();
            }

            return CachedStepTests.FirstOrDefault(s => s.Id == id);
        }

        public void UpsertStepTest(StepTest entity)
        {
            if (entity.State == EntityState.New)
            {
                StepTestRepository.Add(entity);
            }

            else
            {
                StepTestRepository.Update(entity);
            }
        }

        public void RemoveStepTest(StepTest entity) => StepTestRepository.Remove(entity);

        #endregion

        #region Measurements Methods

        public List<Measurement> GetAllMeasurements(bool refresh = false)
        {
            if (refresh || CachedUsers.Count == 0)
            {
                _cachedUsers = UserRepository.All().ToList();
            }

            return CachedMeasurements;
        }

        public Measurement GetMeasurementById(int id, bool refresh = false)
        {
            if (refresh || CachedUsers.Count == 0)
            {
                _cachedUsers = UserRepository.All().ToList();
            }

            return CachedMeasurements.FirstOrDefault(m => m.Id == id);
        }

        public void UpsertMeasurement(Measurement entity)
        {
            if (entity.State == EntityState.New)
            {
                MeasurementRepository.Add(entity);
            }

            else
            {
                MeasurementRepository.Update(entity);
            }
        }

        public void RemoveMeasurement(Measurement entity) => MeasurementRepository.Remove(entity);

        #endregion

        #endregion

        #region IDisposable Support

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void ResetRepositories()
        {
            _measurementRepository = null;
            _stepTestRepository = null;
            _userRepository = null;
            Logger.Debug("Repositories is resetted");
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_transaction != null)
                    {
                        _transaction.Dispose();
                        _transaction = null;
                    }

                    if (_connection != null)
                    {
                        _connection.Dispose();
                        _connection = null;
                    }
                }

                _disposed = true;
            }
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }

        #endregion
    }
}
