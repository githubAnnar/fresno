using LanterneRouge.Fresno.Core.Entities;

namespace LanterneRouge.Fresno.Repository.Contracts
{
    public interface IStepTestManager : IDisposable
    {
        List<StepTest> GetAllStepTests(bool refresh = false);

        StepTest GetStepTestById(int id, bool refresh = false);

        void UpsertStepTest(StepTest entity);

        void RemoveStepTest(StepTest entity);
    }
}
