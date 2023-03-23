using LanterneRouge.Fresno.Core.Entities;

namespace LanterneRouge.Fresno.Repository.Contracts
{
    public interface IStepTestManager : IManagerBase, IDisposable
    {
        List<StepTest> GetAllStepTests();

        List<StepTest> GetStepTestsByUser(User parent);

        StepTest GetStepTestById(int id);

        void UpsertStepTest(StepTest entity);

        void RemoveStepTest(StepTest entity);
    }
}
