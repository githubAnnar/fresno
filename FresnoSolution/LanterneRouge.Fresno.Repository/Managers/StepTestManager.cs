using LanterneRouge.Fresno.Core.Entity;
using LanterneRouge.Fresno.Core.Interface;
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
            Repository = new StepTestRepository(_connection);
        }

        #endregion

        #region Properties

        private IStepTestRepository Repository { get; }

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

        public async Task<IList<StepTest>> GetAllStepTests(CancellationToken cancellationToken = default) => await Repository.GetAllStepTests(cancellationToken);

        public async Task<IList<StepTest>> GetStepTestsByUser(IUserEntity userEntity, CancellationToken cancellationToken = default) => await Repository.GetStepTestsByUser(userEntity, cancellationToken);

        public async Task<int> GetCountByUser(IUserEntity userEntity, CancellationToken cancellationToken = default) => await Repository.GetCountByUser(userEntity, cancellationToken);

        public async Task<StepTest?> GetStepTestById(Guid id, CancellationToken cancellationToken = default) => await Repository.GetStepTestById(id, cancellationToken);

        public async Task<StepTest?> UpsertStepTest(IStepTestEntity stepTestEntity, CancellationToken cancellationToken = default)
        {
            StepTest? response;
            if (stepTestEntity.Id == Guid.Empty)
            {
                response = await Repository.InsertStepTest(stepTestEntity, cancellationToken);
            }

            else
            {
                response = await Repository.UpdateStepTest(stepTestEntity, cancellationToken);
            }

            return response;
        }

        public async Task DeleteStepTest(Guid id, CancellationToken cancellationToken = default) => await Repository.DeleteStepTest(id, cancellationToken);

        public async Task<bool> IsChanged(IStepTestEntity stepTestEntity, CancellationToken cancellationToken = default) => await Repository.IsChanged(stepTestEntity, cancellationToken);

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
