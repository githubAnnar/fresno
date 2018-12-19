using LanterneRouge.Fresno.DataLayer.DataAccess.Entities;
using LanterneRouge.Fresno.DataLayer.DataAccess.Infrastructure;
using LanterneRouge.Fresno.DataLayer.DataAccess.Repositories;
using System;
using System.Data;

namespace LanterneRouge.Fresno.DataLayer.DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbConnection _connection;
        private IDbTransaction _transaction;
        private IRepository<Measurement, StepTest> _measurementRepository;
        private IRepository<StepTest, User> _stepTestRepository;
        private IRepository<User, object> _userRepository;
        private bool _disposed;

        public UnitOfWork(IConnectionFactory connectionFactory)
        {
            _connection = connectionFactory.GetConnection;
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        public IRepository<Measurement, StepTest> MeasurementRepository => _measurementRepository ?? (_measurementRepository = new MeasurementRepository(_transaction));

        public IRepository<StepTest, User> StepTestRepository => _stepTestRepository ?? (_stepTestRepository = new StepTestRepository(_transaction));

        public IRepository<User, object> UserRepository => _userRepository ?? (_userRepository = new UserRepository(_transaction));

        public void Commit()
        {
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
    }
}
