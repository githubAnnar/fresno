using LanterneRouge.Fresno.WpfClient.ViewModel;

namespace LanterneRouge.Fresno.WpfClient.MVVM
{
    public interface IWorkspaceCommands
    {
        void ShowWorkspace(WorkspaceViewModel view);

        void ShowAllStepTests(UserViewModel user);

        void ShowAllMeasurements(StepTestViewModel stepTest);

        void ShowUser(UserViewModel user);

        void ShowUser(StepTestViewModel stepTest);

        void ShowUser(MeasurementViewModel measurement);

        void ShowStepTest(StepTestViewModel stepTest);

        void ShowStepTest(MeasurementViewModel measurement);

        void ShowMeasurement(MeasurementViewModel measurement);

        void CreateNewStepTest(object userObject);

        bool CanCreateStepTest { get; }

        void CreateNewMeasurement(object stepTestObject);

        bool CanCreateMeasurement { get; }

        void GenerateCalculation(StepTestViewModel stepTest);
    }
}
