using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LanterneRouge.Fresno.WpfClient.ViewModel
{
    public class StepTestsPlotViewModel : WorkspaceViewModel
    {
        #region Fields

        private const string LoadAxisKey = "Load";
        private const string LactateAxisKey = "Lactate";
        private const string HeartrateAxisKey = "Heartrate";

        #endregion

        #region Constructors

        public StepTestsPlotViewModel(IEnumerable<StepTestViewModel> parentStepTests, Action<WorkspaceViewModel> showWorkspace) : base(null, showWorkspace, null)
        {
            StepTestData = parentStepTests.ToList();
        }

        #endregion

        #region Properties

        private List<StepTestViewModel> StepTestData { get; }

        public PlotModel StepTestsModel { get; private set; }

        #endregion

        #region WorkspaceViewModel implementation

        public override WorkspaceViewModel SelectedObject => this;

        #endregion

        #region Display Properties

        public override string DisplayName => $"Step Test plot";

        public override void CreateChild()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
