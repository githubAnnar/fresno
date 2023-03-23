using LanterneRouge.Fresno.Core.Contracts;
using LanterneRouge.Fresno.Core.Entities;
using LanterneRouge.Fresno.Repository.Contracts;
using LanterneRouge.Fresno.Repository.Repositories;
using System.Data;

namespace LanterneRouge.Fresno.Repository.Managers
{
    public class StepTestManager : ManagerBase, IStepTestManager
    {
        #region Constructors
        public StepTestManager(IConnectionFactory connectionFactory) : base(connectionFactory)
        {
            StepTestRepository = new StepTestRepository(_transaction);
        }

        #endregion

        #region Properties

        private IRepository<StepTest> StepTestRepository { get; }

        #endregion

        #region Methods

        public override void Close()
        {
            // Close db connection
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }

            _connection.Dispose();
            _connection = null!;
            Logger.Info("Db connection closed");
        }

        public List<StepTest> GetAllStepTests() => StepTestRepository.All().ToList();

        public List<StepTest> GetStepTestsByUser(User parent) => StepTestRepository.FindByParentId(parent).ToList();


        public StepTest GetStepTestById(int id) => StepTestRepository.FindSingle(id);

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

        #region IDisposable Support

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
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
                        _transaction = null!;
                    }

                    if (_connection != null)
                    {
                        _connection.Dispose();
                        _connection = null!;
                    }
                }

                _disposed = true;
            }
        }


        ~StepTestManager()
        {
            Dispose(false);
        }

        #endregion
    }
}
