using LanterneRouge.Fresno.Calculations;
using LanterneRouge.Fresno.Calculations.Base;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace LanterneRouge.Fresno.WpfClient.ViewModel
{
    public class FrpbCalculationViewModel : WorkspaceViewModel, IEquatable<FrpbCalculationViewModel>
    {
        #region Fields

        private double _frpbCalculationThreshold = 1d;

        #endregion

        #region Constructors

        public FrpbCalculationViewModel(StepTestViewModel parentStepTest, MainWindowViewModel rootViewModel) : base(parentStepTest, rootViewModel, null)
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
        private FrpbCalculation FrpbCalculation => _frpbCalculation ??= DataManager.MeasurementsCountByStepTest(StepTestParent.Source) > 0 ? new FrpbCalculation(DataManager.GetAllMeasurementsByStepTest(StepTestParent.Source), FrpbCalculationThreshold) : null;

        private ObservableCollection<Zone> _FRPBZones = null;
        public ObservableCollection<Zone> FRPBZones
        {
            get
            {
                if (_FRPBZones == null && DataManager.MeasurementsCountByStepTest(StepTestParent.Source) > 0)
                {
                    if (FrpbCalculation != null)
                    {
                        var z = new PercentOfLTBasedZones(FrpbCalculation, ApplicationSettingsManager.ZoneSettingsValue.GetZoneSetting(nameof(FrpbCalculation)).Limits.ToArray());
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

        public override void CreateChild() => throw new NotImplementedException();

        #endregion

        public bool Equals(FrpbCalculationViewModel other) => Equals((object)other);

        public override bool Equals(object obj) => obj is FrpbCalculationViewModel viewModel && GetHashCode().Equals(viewModel.GetHashCode());

        public override int GetHashCode() => ((StepTestViewModel)Parent).GetHashCode();
    }
}
