using LanterneRouge.Fresno.Calculations;
using LanterneRouge.Fresno.Calculations.Base;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace LanterneRouge.Fresno.WpfClient.ViewModel
{
    public class LtLogCalculationViewModel : WorkspaceViewModel, IEquatable<LtLogCalculationViewModel>
    {
        #region Fields

        #endregion

        #region Constructors

        public LtLogCalculationViewModel(StepTestViewModel parentStepTest, Action<WorkspaceViewModel> showWorkspace) : base(parentStepTest, showWorkspace, null)
        { }

        #endregion

        #region Properties

        private StepTestViewModel StepTestParent => Parent as StepTestViewModel;

        public int StepTestId => StepTestParent.Source.Id;

        public string LTLogLactateThresholdText => LtLogCalculation != null ? $"Load Th.: {LtLogCalculation.LoadThreshold:0.0} Heartrate Th.: {LtLogCalculation.HeartRateThreshold:0}" : "No Calculation";

        private LTLogCalculation _ltLogCalculation = null;
        private LTLogCalculation LtLogCalculation => _ltLogCalculation ?? (_ltLogCalculation = StepTestParent.Source.Measurements != null && StepTestParent.Source.Measurements.Count > 0 ? new LTLogCalculation(StepTestParent.Source.Measurements) : null);

        private ObservableCollection<Zone> _LTLogZones = null;
        public ObservableCollection<Zone> LTLogZones
        {
            get
            {
                if (_LTLogZones == null && StepTestParent.Source.Measurements != null && StepTestParent.Source.Measurements.Count > 0)
                {
                    if (LtLogCalculation != null)
                    {
                        var z = new PercentOfLTBasedZones(LtLogCalculation, ApplicationSettingsManager.ZoneSettingsValue.GetZoneSetting(nameof(LTLogCalculation)).Limits.ToArray());
                        _LTLogZones = new ObservableCollection<Zone>(z.Zones);
                    }
                }

                return _LTLogZones;
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

        public override string ToString() => $"LT Log Calculation for Step Test ({StepTestId})";

        public override void CreateChild() => throw new NotImplementedException();

        #endregion

        public bool Equals(LtLogCalculationViewModel other) => Equals((object)other);

        public override bool Equals(object obj) => obj is LtLogCalculationViewModel viewModel && GetHashCode().Equals(viewModel.GetHashCode());

        public override int GetHashCode() => ((StepTestViewModel)Parent).GetHashCode();
    }
}
