using LanterneRouge.Fresno.WpfClient.ViewModel;
using System.Collections.Generic;

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

        void GenerateStepTestPdf(StepTestViewModel stepTest);

        void CreateNewStepTest(object userObject);

        bool CanCreateStepTest { get; }

        void CreateNewMeasurement(object stepTestObject);

        bool CanCreateMeasurement { get; }

        void GenerateCalculation(IEnumerable<StepTestViewModel> stepTests);

        void ShowFblcCalculation(StepTestViewModel stepTestVm);

        void ShowFrpbCalculation(StepTestViewModel stepTestVm);

        void ShowLtCalculation(StepTestViewModel stepTestVm);

        void ShowLtLogCalculation(StepTestViewModel stepTestVm);
    }
}
