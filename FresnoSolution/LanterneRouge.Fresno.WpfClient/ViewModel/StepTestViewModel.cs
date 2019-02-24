using LanterneRouge.Fresno.Calculations;
using LanterneRouge.Fresno.Calculations.Base;
using LanterneRouge.Fresno.DataLayer.DataAccess.Entities;
using LanterneRouge.Fresno.WpfClient.MVVM;
using LanterneRouge.Fresno.WpfClient.Utils;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace LanterneRouge.Fresno.WpfClient.ViewModel
{
    public class StepTestViewModel : WorkspaceViewModel, IDataErrorInfo
    {
        #region Fields

        private static readonly string _name = typeof(StepTestViewModel).Name;
        private ICommand _saveCommand;
        private bool _isSelected = false;
        private readonly IWorkspaceCommands _wsCommands;
        private ICommand _editSelectedCommand;
        private ICommand _showUserCommand;
        private ICommand _showAllMeasurementsCommand;
        private ICommand _addMeasurementCommand;

        #endregion

        #region Constructors

        public StepTestViewModel(StepTest category, IWorkspaceCommands mainWorkspaceViewModel)
        {
            Source = category ?? throw new ArgumentNullException(nameof(category));
            _wsCommands = mainWorkspaceViewModel ?? throw new ArgumentNullException(nameof(mainWorkspaceViewModel));
        }

        #endregion

        #region Properties

        internal StepTest Source { get; private set; }

        public int StepTestId => Source.Id;

        public string TestType
        {
            get { return Source.TestType; }
            set
            {
                if (!value.Equals(Source.TestType))
                {
                    Source.TestType = value;
                    OnPropertyChanged();
                }
            }
        }

        public string EffortUnit
        {
            get { return Source.EffortUnit; }
            set
            {
                if (!value.Equals(Source.EffortUnit))
                {
                    Source.EffortUnit = value;
                    OnPropertyChanged();
                }
            }
        }

        public TimeSpan StepDurationTimespan
        {
            get { return TimeSpan.FromTicks(Source.StepDuration); }
            set
            {
                if (!value.Ticks.Equals(Source.StepDuration))
                {
                    Source.StepDuration = value.Ticks;
                    OnPropertyChanged();
                }
            }
        }

        //public long StepDuration
        //{
        //    get { return Source.StepDuration; }
        //    set
        //    {
        //        if (!value.Equals(Source.StepDuration))
        //        {
        //            Source.StepDuration = value;
        //            OnPropertyChanged();
        //        }
        //    }
        //}

        public float LoadPreset
        {
            get { return Source.LoadPreset; }
            set
            {
                if (!value.Equals(Source.LoadPreset))
                {
                    Source.LoadPreset = value;
                    OnPropertyChanged();
                }
            }
        }

        public float Increase
        {
            get { return Source.Increase; }
            set
            {
                if (!value.Equals(Source.Increase))
                {
                    Source.Increase = value;
                    OnPropertyChanged();
                }
            }
        }

        public float Temperature
        {
            get { return Source.Temperature; }
            set
            {
                if (!value.Equals(Source.Temperature))
                {
                    Source.Temperature = value;
                    OnPropertyChanged();
                }
            }
        }

        public float Weight
        {
            get { return Source.Weight; }
            set
            {
                if (!value.Equals(Source.Weight))
                {
                    Source.Weight = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime TestDate
        {
            get { return Source.TestDate; }
            set
            {
                if (!value.Equals(Source.TestDate))
                {
                    Source.TestDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public double FblcValue => FblcCalculation != null ? FblcCalculation.LoadThreshold : 0d;

        public double FrpbValue => FrpbCalculation != null ? FrpbCalculation.LoadThreshold : 0d;

        public double LtValue => LtCalculation != null ? LtCalculation.LoadThreshold : 0d;


        public string FBLCLactateThresholdText => FblcCalculation != null ? $"Load Th.: {FblcCalculation.LoadThreshold:0.0} Heartrate Th.: {FblcCalculation.HeartRateThreshold:0}" : "No Calculation";

        public string FRPBLactateThresholdText => FrpbCalculation != null ? $"Load Th.: {FrpbCalculation.LoadThreshold:0.0} Heartrate Th.: {FrpbCalculation.HeartRateThreshold:0}" : "No Calculation";

        public string LTLactateThresholdText => LtCalculation != null ? $"Load Th.: {LtCalculation.LoadThreshold:0.0} Heartrate Th.: {LtCalculation.HeartRateThreshold:0}" : "No Calculation";

        private FblcCalculation _fblcCalculation = null;
        private FblcCalculation FblcCalculation => _fblcCalculation ?? (_fblcCalculation = Source.Measurements != null && Source.Measurements.Count > 0 ? new FblcCalculation(Source.Measurements, 4.0) : null);

        private FrpbCalculation _frpbCalculation = null;
        private FrpbCalculation FrpbCalculation => _frpbCalculation ?? (_frpbCalculation = Source.Measurements != null && Source.Measurements.Count > 0 ? new FrpbCalculation(Source.Measurements, 1.0) : null);

        private LTCalculation _ltCalculation = null;
        private LTCalculation LtCalculation => _ltCalculation ?? (_ltCalculation = Source.Measurements != null && Source.Measurements.Count > 0 ? new LTCalculation(Source.Measurements) : null);

        private ObservableCollection<Zone> _FBLCZones = null;
        public ObservableCollection<Zone> FBLCZones
        {
            get
            {
                if (_FBLCZones == null && Source.Measurements != null && Source.Measurements.Count > 0)
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

        private ObservableCollection<Zone> _FRPBZones = null;
        public ObservableCollection<Zone> FRPBZones
        {
            get
            {
                if (_FRPBZones == null && Source.Measurements != null && Source.Measurements.Count > 0)
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

        private ObservableCollection<Zone> _percentZones = null;
        public ObservableCollection<Zone> PercentZones
        {
            get
            {
                if (_percentZones == null)
                {
                    if (FblcCalculation != null)
                    {
                        var z = new PercentOfLTBasedZones(FblcCalculation, new[] { 0.4, 0.55, 0.75, 0.90, 1.05, 1.2 });
                        _percentZones = new ObservableCollection<Zone>(z.Zones);
                    }
                }

                return _percentZones;
            }
        }

        private ObservableCollection<Zone> _ltPercentZones = null;
        public ObservableCollection<Zone> LtPercentZones
        {
            get
            {
                if (_ltPercentZones == null)
                {
                    if (LtCalculation != null)
                    {
                        var z = new PercentOfLTBasedZones(LtCalculation, new[] { 0.4, 0.55, 0.75, 0.90, 1.05, 1.2 });
                        _ltPercentZones = new ObservableCollection<Zone>(z.Zones);
                    }
                }

                return _ltPercentZones;
            }
        }

        #endregion

        #region Display Properties

        public override string DisplayName
        {
            get
            {
                return Source.Id == 0 ? "New Step Test"/*KayakStrings.Category_New_Singular*/ : ToString();
            }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (!value.Equals(_isSelected))
                {
                    _isSelected = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return $"Step Test ({StepTestId})";
        }

        public void Save(object param)
        {
            if (Source.IsChanged)
            {
                DataManager.Commit();

                OnPropertyChanged("DisplayName");

                MessageBox.Show(string.Format("Step Test: {0} saved", StepTestId), "Saving OK", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            if (param is string)
            {
                var stringParam = param as string;
                if (stringParam.Equals("CLOSE", StringComparison.InvariantCultureIgnoreCase))
                {
                    CloseCommand.Execute(null);
                }
            }
        }

        public override WorkspaceViewModel SelectedObject => this;

        public static string GetIdentifierName(StepTest stepTest)
        {
            return string.Format("{0}_StepTest_{1}", _name, stepTest.Id.Equals(-1) ? Guid.NewGuid().ToString() : stepTest.Id.ToString());
        }

        #endregion

        #region Private Helpers

        private bool CanSave
        {
            get { return IsValid && Source.IsChanged; }
        }

        #endregion

        #region IDataErrorInfo Interface

        public string Error
        {
            get { return null; }
        }

        public string this[string columnName]
        {
            get
            {
                CommandManager.InvalidateRequerySuggested();
                return GetValidationError(columnName);
            }
        }

        #endregion

        #region Validation

        public bool IsValid => ValidatedProperties.All(p => GetValidationError(p) == null);

        private static readonly string[] ValidatedProperties = { nameof(TestType), nameof(EffortUnit), nameof(StepDurationTimespan), nameof(LoadPreset), nameof(Increase) };

        private string GetValidationError(string propertyName)
        {
            if (Array.IndexOf(ValidatedProperties, propertyName) < 0)
            {
                return null;
            }

            string error = null;

            switch (propertyName)
            {
                case nameof(TestType):
                    error = ValidateTestType();
                    break;

                case nameof(EffortUnit):
                    error = ValidateEffortUnit();
                    break;

                case nameof(StepDurationTimespan):
                    error = ValidateStepDuration();
                    break;

                case nameof(LoadPreset):
                    error = ValidateLoadPreset();
                    break;

                case nameof(Increase):
                    error = ValidateIncrease();
                    break;

                default:
                    Debug.Fail($"Unexpected property being validated on StepTest: {propertyName}");
                    break;
            }

            return error;
        }

        private string ValidateIncrease()
        {
            return ValidateHelpers.IsFloatLTZero(Increase) ? "Increase" : null;
        }

        private string ValidateLoadPreset()
        {
            return ValidateHelpers.IsFloatLTZero(LoadPreset) ? "Load Preset" : null;
        }

        private string ValidateStepDuration()
        {
            return ValidateHelpers.IsTimeSpanLTZero(StepDurationTimespan) ? "Step Duration" : null;
        }

        private string ValidateEffortUnit()
        {
            return ValidateHelpers.IsStringMissing(EffortUnit) || (!EffortUnit.Equals("W") && !EffortUnit.Equals("m-s")) ? "Effort Unit" : null;
        }

        private string ValidateTestType()
        {
            return ValidateHelpers.IsStringMissing(TestType) || (!TestType.Equals("Bike") && !TestType.Equals("Run")) ? "Test Type" : null;
        }

        #endregion

        #region Commands

        #region EditSelectedCommand

        public ICommand EditSelectedCommand => _editSelectedCommand ?? (_editSelectedCommand = new RelayCommand(EditSelected));

        private void EditSelected(object obj)
        {
            _wsCommands.ShowWorkspace(this);
        }

        #endregion

        #region ShowUserCommand

        public ICommand SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand = new RelayCommand(param => Save(param), param => CanSave));
            }
        }


        public ICommand ShowUserCommand => _showUserCommand ?? (_showUserCommand = new RelayCommand(ShowUser));

        private void ShowUser(object obj)
        {
            _wsCommands.ShowUser(this);
        }

        #endregion

        #region ShowAllMeasurementsCommand

        public ICommand ShowAllMeasurementsCommand => _showAllMeasurementsCommand ?? (_showAllMeasurementsCommand = new RelayCommand(ShowAllMeasurements));

        private void ShowAllMeasurements(object obj)
        {
            _wsCommands.ShowAllMeasurements(this);
        }

        #endregion

        #region AddMeasurementCommand

        public ICommand AddMeasurementCommand => _addMeasurementCommand ?? (_addMeasurementCommand = new RelayCommand(AddMeasurement, param => _wsCommands.CanCreateMeasurement));

        private void AddMeasurement(object obj)
        {
            _wsCommands.CreateNewMeasurement(this);
        }

        #endregion

        #endregion
    }
}
