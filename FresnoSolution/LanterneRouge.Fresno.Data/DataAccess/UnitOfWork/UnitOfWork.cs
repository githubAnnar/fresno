using LanterneRouge.Fresno.DataLayer.DataAccess.Entities;
using LanterneRouge.Fresno.DataLayer.DataAccess.Infrastructure;
using LanterneRouge.Fresno.DataLayer.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace LanterneRouge.Fresno.DataLayer.DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Fields

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
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        #endregion

        #region Properties

        public IRepository<Measurement, StepTest> MeasurementRepository => _measurementRepository ?? (_measurementRepository = new MeasurementRepository(_transaction));

        public IRepository<StepTest, User> StepTestRepository => _stepTestRepository ?? (_stepTestRepository = new StepTestRepository(_transaction));

        public IRepository<User, object> UserRepository => _userRepository ?? (_userRepository = new UserRepository(_transaction));

        public List<User> CachedUsers => _cachedUsers ?? (_cachedUsers = new List<User>());

        public IEnumerable<StepTest> CachedStepTests => CachedUsers.SelectMany(u => u.StepTests);

        public IEnumerable<Measurement> CachedMeasurements => CachedUsers.SelectMany(u => u.StepTests.SelectMany(s => s.Measurements));

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
                UserRepository.Add(item);
            }

            foreach (var item in newStepTests)
            {
                StepTestRepository.Add(item);
            }

            foreach (var item in newMeasurements)
            {
                MeasurementRepository.Add(item);
            }

            foreach (var item in updatedUsers)
            {
                UserRepository.Update(item);
            }

            foreach (var item in updatedStepTests)
            {
                StepTestRepository.Update(item);
            }

            foreach (var item in updatedMeasurements)
            {
                MeasurementRepository.Update(item);
            }

            try
            {
                _transaction.Commit();
            }
            catch
            {
                _transaction.Rollback();
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
