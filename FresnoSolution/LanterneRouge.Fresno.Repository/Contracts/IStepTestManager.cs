using LanterneRouge.Fresno.Core.Interface;

namespace LanterneRouge.Fresno.Repository.Contracts
{
    public interface IStepTestManager : IManagerBase, IDisposable
    {
        IList<IStepTestEntity> GetAllStepTests();

        IList<IStepTestEntity> GetStepTestsByUser(IUserEntity parent);

        int StepTestCountByUser(IUserEntity parent, bool onlyInCalculation);

        IStepTestEntity GetStepTestById(int id);

        void UpsertStepTest(IStepTestEntity entity);

        void RemoveStepTest(IStepTestEntity entity);
    }
}
