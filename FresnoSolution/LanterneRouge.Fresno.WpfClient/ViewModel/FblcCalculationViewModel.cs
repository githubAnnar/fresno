using LanterneRouge.Fresno.Calculations;
using LanterneRouge.Fresno.Calculations.Base;
using System;
using System.Collections.ObjectModel;

namespace LanterneRouge.Fresno.WpfClient.ViewModel
{
    /// <summary>
    /// Fixed Blood Lactate Consentration calculation
    /// </summary>
    public class FblcCalculationViewModel : WorkspaceViewModel, IEquatable<FblcCalculationViewModel>
    {
        #region Fields

        private double _fblcCalculationThreshold = 4d;

        #endregion

        #region Constructors

        public FblcCalculationViewModel(StepTestViewModel parentStepTest, Action<WorkspaceViewModel> showWorkspace) : base(parentStepTest, showWorkspace, null)
        { }

        #endregion

        #region Properties

        public StepTestViewModel StepTestParent => Parent as StepTestViewModel;

        public int StepTestId => StepTestParent.Source.Id;

        public double FblcCalculationThreshold
        {
            get => _fblcCalculationThreshold;
            set
            {
                _fblcCalculationThreshold = value;
                _fblcCalculation = null;
                OnPropertyChanged();
                OnPropertyChanged(nameof(FblcValue));
                OnPropertyChanged(nameof(FBLCZones));
                OnPropertyChanged(nameof(FBLCLactateThresholdText));
            }
        }

        public double FblcValue => FblcCalculation != null ? FblcCalculation.LoadThreshold : 0d;

        public string FBLCLactateThresholdText => FblcCalculation != null ? $"Load Th.: {FblcCalculation.LoadThreshold:0.0} Heartrate Th.: {FblcCalculation.HeartRateThreshold:0}" : "No Calculation";

        private FblcCalculation _fblcCalculation = null;
        private FblcCalculation FblcCalculation => _fblcCalculation ?? (_fblcCalculation = StepTestParent.Source.Measurements != null && StepTestParent.Source.Measurements.Count > 0 ? new FblcCalculation(StepTestParent.Source.Measurements, FblcCalculationThreshold) : null);

        private ObservableCollection<Zone> _FBLCZones = null;
        public ObservableCollection<Zone> FBLCZones
        {
            get
            {
                if (_FBLCZones == null && StepTestParent.Source.Measurements != null && StepTestParent.Source.Measurements.Count > 0)
                {
                    if (FblcCalculation != null)
                    {
                        var z = new LactateBasedZones(FblcCalculation, new[] { 0.8, 1.5, 2.5, 4.0, 6.0, 10.0 });
                        _FBLCZones = new ObservableCollection<Zone>(z.Zones);
                    }
                }

                return _FBLCZones;
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

        public override string ToString() => $"FBLC Calculation for Step Test ({StepTestId})";

        public override void CreateChild()
        {
            throw new NotImplementedException();
        }

        #endregion

        public bool Equals(FblcCalculationViewModel other) => Equals((object)other);

        public override bool Equals(object obj) => obj is FblcCalculationViewModel viewModel && GetHashCode().Equals(viewModel.GetHashCode());

        public override int GetHashCode() => ((StepTestViewModel)Parent).GetHashCode();
    }
}
