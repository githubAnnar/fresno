using LanterneRouge.Fresno.Report.PlotModels;
using LanterneRouge.Fresno.Utils.Extensions;
using LanterneRouge.Fresno.WpfClient.Utils;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LanterneRouge.Fresno.WpfClient.ViewModel
{
    public class StepTestsPlotViewModel : WorkspaceViewModel, IEquatable<StepTestsPlotViewModel>
    {
        #region Fields

        #endregion

        #region Constructors

        public StepTestsPlotViewModel(IEnumerable<StepTestViewModel> parentStepTests, Action<WorkspaceViewModel> showWorkspace) : base(null, showWorkspace, null)
        {
            StepTestData = parentStepTests.ToList();
            StepTestsModel = StepTests.StepTestPlotModel(StepTestData.Select(st => st.Source));
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

        public override void CreateChild() => throw new NotImplementedException();

        #endregion

        public bool Equals(StepTestsPlotViewModel other) => Equals((object)other);

        public override bool Equals(object obj) => obj is StepTestsPlotViewModel viewModel && GetHashCode().Equals(viewModel.GetHashCode());

        public override int GetHashCode() => StepTestData.Select(item => item.GetHashCode()).GetSequenceHashCode();
    }
}
