using LanterneRouge.Fresno.Calculations;
using LanterneRouge.Fresno.Calculations.Base;
using System;
using System.Collections.ObjectModel;

namespace LanterneRouge.Fresno.WpfClient.ViewModel
{
    public class LtLogCalculationViewModel : WorkspaceViewModel
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
                        var z = new PercentOfLTBasedZones(LtLogCalculation, new[] { 0.4, 0.55, 0.75, 0.90, 1.05, 1.2 });
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

        public override void CreateChild()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
