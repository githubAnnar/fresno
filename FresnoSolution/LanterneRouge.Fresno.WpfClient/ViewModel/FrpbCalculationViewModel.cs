using LanterneRouge.Fresno.Calculations;
using LanterneRouge.Fresno.Calculations.Base;
using System;
using System.Collections.ObjectModel;

namespace LanterneRouge.Fresno.WpfClient.ViewModel
{
    public class FrpbCalculationViewModel : WorkspaceViewModel
    {
        #region Fields

        private double _frpbCalculationThreshold = 1d;

        #endregion

        #region Constructors

        public FrpbCalculationViewModel(StepTestViewModel parentStepTest, Action<WorkspaceViewModel> showWorkspace) : base(parentStepTest, showWorkspace, null)
        { }

        #endregion

        #region Properties

        private StepTestViewModel StepTestParent => Parent as StepTestViewModel;

        public int StepTestId => StepTestParent.Source.Id;

        public double FrpbCalculationThreshold
        {
            get => _frpbCalculationThreshold;
            set
            {
                _frpbCalculationThreshold = value;
                _frpbCalculation = null;
                _FRPBZones = null;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FrpbValue));
                OnPropertyChanged(nameof(FRPBZones));
                OnPropertyChanged(nameof(FRPBLactateThresholdText));
            }
        }

        public double FrpbValue => FrpbCalculation != null ? FrpbCalculation.LoadThreshold : 0d;

        public string FRPBLactateThresholdText => FrpbCalculation != null ? $"Load Th.: {FrpbCalculation.LoadThreshold:0.0} Heartrate Th.: {FrpbCalculation.HeartRateThreshold:0}" : "No Calculation";

        private FrpbCalculation _frpbCalculation = null;
        private FrpbCalculation FrpbCalculation => _frpbCalculation ?? (_frpbCalculation = StepTestParent.Source.Measurements != null && StepTestParent.Source.Measurements.Count > 0 ? new FrpbCalculation(StepTestParent.Source.Measurements, FrpbCalculationThreshold) : null);

        private ObservableCollection<Zone> _FRPBZones = null;
        public ObservableCollection<Zone> FRPBZones
        {
            get
            {
                if (_FRPBZones == null && StepTestParent.Source.Measurements != null && StepTestParent.Source.Measurements.Count > 0)
                {
                    if (FrpbCalculation != null)
                    {
                        var z = new PercentOfLTBasedZones(FrpbCalculation, new[] { 0.4, 0.55, 0.75, 0.90, 1.05, 1.2 });
                        _FRPBZones = new ObservableCollection<Zone>(z.Zones);
                    }
                }

                return _FRPBZones;
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

        public override string ToString() => $"FRPB Calculation for Step Test ({StepTestId})";

        #endregion
    }
}
