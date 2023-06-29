using LanterneRouge.Fresno.Core.Contracts;
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
            StepTestRepository = new StepTestRepository(_connection);
        }

        #endregion

        #region Properties

        private IRepository<IStepTestEntity, IUserEntity> StepTestRepository { get; }

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

        public List<IStepTestEntity> GetAllStepTests() => StepTestRepository.All().ToList();

        public List<IStepTestEntity> GetStepTestsByUser(IUserEntity parent) => StepTestRepository.FindByParentId(parent).ToList();

        public int StepTestCountByUser(IUserEntity parent, bool onlyInCalculation) => StepTestRepository.GetCountByParentId(parent, onlyInCalculation);

        public IStepTestEntity GetStepTestById(int id) => StepTestRepository.FindSingle(id);

        public void UpsertStepTest(IStepTestEntity entity)
        {
            if (entity.Id == 0)
            {
                StepTestRepository.Add(entity);
            }

            else
            {
                StepTestRepository.Update(entity);
            }
        }

        public void RemoveStepTest(IStepTestEntity entity) => StepTestRepository.Remove(entity);

        public bool IsChanged(IStepTestEntity entity) => StepTestRepository.IsChanged(entity);

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
