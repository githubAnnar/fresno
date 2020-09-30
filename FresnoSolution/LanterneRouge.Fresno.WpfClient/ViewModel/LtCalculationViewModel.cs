using LanterneRouge.Fresno.Calculations;
using LanterneRouge.Fresno.Calculations.Base;
using System;
using System.Collections.ObjectModel;

namespace LanterneRouge.Fresno.WpfClient.ViewModel
{
    public class LtCalculationViewModel : WorkspaceViewModel, IEquatable<LtCalculationViewModel>
    {
        #region Fields

        #endregion

        #region Constructors

        public LtCalculationViewModel(StepTestViewModel parentStepTest, Action<WorkspaceViewModel> showWorkspace) : base(parentStepTest, showWorkspace, null)
        { }

        #endregion

        #region Properties

        private StepTestViewModel StepTestParent => Parent as StepTestViewModel;

        public int StepTestId => StepTestParent.Source.Id;

        public string LTLactateThresholdText => LtCalculation != null ? $"Load Th.: {LtCalculation.LoadThreshold:0.0} Heartrate Th.: {LtCalculation.HeartRateThreshold:0}" : "No Calculation";

        private LTCalculation _ltCalculation = null;
        private LTCalculation LtCalculation => _ltCalculation ?? (_ltCalculation = StepTestParent.Source.Measurements != null && StepTestParent.Source.Measurements.Count > 0 ? new LTCalculation(StepTestParent.Source.Measurements) : null);

        private ObservableCollection<Zone> _LTZones = null;
        public ObservableCollection<Zone> LTZones
        {
            get
            {
                if (_LTZones == null && StepTestParent.Source.Measurements != null && StepTestParent.Source.Measurements.Count > 0)
                {
                    if (LtCalculation != null)
                    {
                        var z = new PercentOfLTBasedZones(LtCalculation, new[] { 0.4, 0.55, 0.75, 0.90, 1.05, 1.2 });
                        _LTZones = new ObservableCollection<Zone>(z.Zones);
                    }
                }

                return _LTZones;
            }
        }

        #endregion

        #region WorkspaceViewModel Overrides

        public override WorkspaceViewModel SelectedObject => this;

        #endregion

        #region Display Methods

        public override string DisplayName => ToString();

        #endregion

        #region Public Methods

        public override string ToString() => $"LT Calculation for Step Test ({StepTestId})";

        public override void CreateChild()
        {
            throw new NotImplementedException();
        }

        #endregion

        public bool Equals(LtCalculationViewModel other) => Equals((object)other);

        public override bool Equals(object obj) => obj is LtCalculationViewModel viewModel && GetHashCode().Equals(viewModel.GetHashCode());

        public override int GetHashCode() => ((StepTestViewModel)Parent).GetHashCode();
    }
}
