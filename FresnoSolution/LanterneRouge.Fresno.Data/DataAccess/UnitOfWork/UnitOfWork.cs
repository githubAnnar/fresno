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

        public IRepository<Measurement, StepTest> MeasurementRepository => _measurementRepository ?? (_measurementRepository = new MeasurementRepository(_transaction));

        public IRepository<StepTest, User> StepTestRepository => _stepTestRepository ?? (_stepTestRepository = new StepTestRepository(_transaction));

        public IRepository<User, object> UserRepository => _userRepository ?? (_userRepository = new UserRepository(_transaction));

        public List<User> CachedUsers => _cachedUsers ?? (_cachedUsers = new List<User>());

        public List<StepTest> CachedStepTests => CachedUsers.SelectMany(u => u.StepTests).ToList();

        public List<Measurement> CachedMeasurements => CachedUsers.SelectMany(u => u.StepTests.SelectMany(s => s.Measurements)).ToList();

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

        public IEnumerable<User> GetAllUsers(bool refresh = false)
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
