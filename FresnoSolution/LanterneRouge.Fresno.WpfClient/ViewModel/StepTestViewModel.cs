using LanterneRouge.Fresno.Calculations;
using LanterneRouge.Fresno.DataLayer.DataAccess.Entities;
using LanterneRouge.Fresno.WpfClient.MVVM;
using LanterneRouge.Fresno.WpfClient.Utils;
using log4net;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace LanterneRouge.Fresno.WpfClient.ViewModel
{
    public class StepTestViewModel : WorkspaceViewModel, IDataErrorInfo
    {
        #region Fields

        private static readonly ILog Logger = LogManager.GetLogger(typeof(StepTestViewModel));
        private static readonly string _name = typeof(StepTestViewModel).Name;
        private ICommand _saveCommand;
        private bool _isSelected = false;
        private readonly IWorkspaceCommands _wsCommands;
        private ICommand _editSelectedCommand;
        private ICommand _showUserCommand;
        private ICommand _showAllMeasurementsCommand;
        private ICommand _addMeasurementCommand;
        private ICommand _showFlbcCalculationCommand;
        private ICommand _showFrpbCalculationCommand;
        private ICommand _showLtCalculationCommand;
        private ICommand _showLtLogCalculationCommand;

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

        public double LtLogValue => LtLogCalculation != null ? LtLogCalculation.LoadThreshold : 0d;

        private FblcCalculation _fblcCalculation = null;
        private FblcCalculation FblcCalculation => _fblcCalculation ?? (_fblcCalculation = Source.Measurements != null && Source.Measurements.Count > 0 ? new FblcCalculation(Source.Measurements, 4.0) : null);

        private FrpbCalculation _frpbCalculation = null;
        private FrpbCalculation FrpbCalculation => _frpbCalculation ?? (_frpbCalculation = Source.Measurements != null && Source.Measurements.Count > 0 ? new FrpbCalculation(Source.Measurements, 1.0) : null);

        private LTCalculation _ltCalculation = null;
        private LTCalculation LtCalculation => _ltCalculation ?? (_ltCalculation = Source.Measurements != null && Source.Measurements.Count > 0 ? new LTCalculation(Source.Measurements) : null);

        private LTLogCalculation _ltLogCalculation = null;
        private LTLogCalculation LtLogCalculation => _ltLogCalculation ?? (_ltLogCalculation = Source.Measurements != null && Source.Measurements.Count > 0 ? new LTLogCalculation(Source.Measurements) : null);

        #endregion

        #region Display Properties

        public override string DisplayName => Source.Id == 0 ? "New Step Test"/*KayakStrings.Category_New_Singular*/ : ToString();

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

        public override string ToString() => $"StepTest ({StepTestId})";

        public void Save(object param)
        {
            if (Source.IsChanged)
            {
                DataManager.Commit();

                OnPropertyChanged("DisplayName");

                MessageBox.Show($"Step Test: {StepTestId} saved", "Saving OK", MessageBoxButton.OK, MessageBoxImage.Information);
                Logger.Info($"Step Test: {StepTestId} saved OK");
            }

            if (param is string)
            {
                var stringParam = param as string;
                if (stringParam.Equals("CLOSE", StringComparison.InvariantCultureIgnoreCase))
                {
                    Logger.Debug($"Closing {nameof(StepTestViewModel)} for {DisplayName}");
                    CloseCommand.Execute(null);
                }
            }
        }

        public override WorkspaceViewModel SelectedObject => this;

        public static string GetIdentifierName(StepTest stepTest) => string.Format("{0}_StepTest_{1}", _name, stepTest.Id.Equals(-1) ? Guid.NewGuid().ToString() : stepTest.Id.ToString());

        #endregion

        #region Private Helpers

        private bool CanSave => IsValid && Source.IsChanged;

        #endregion

        #region IDataErrorInfo Interface

        public string Error => null;

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
                    Logger.Error($"Unexpected property being validated on StepTest: {propertyName}");
                    break;
            }

            if (!string.IsNullOrEmpty(error))
            {
                Logger.Warn($"{propertyName} give '{error}'");
            }

            return error;
        }

        private string ValidateIncrease() => ValidateHelpers.IsFloatLTZero(Increase) ? "Increase" : null;

        private string ValidateLoadPreset() => ValidateHelpers.IsFloatLTZero(LoadPreset) ? "Load Preset" : null;

        private string ValidateStepDuration() => ValidateHelpers.IsTimeSpanLTZero(StepDurationTimespan) ? "Step Duration" : null;

        private string ValidateEffortUnit() => ValidateHelpers.IsStringMissing(EffortUnit) || (!EffortUnit.Equals("W") && !EffortUnit.Equals("m-s")) ? "Effort Unit" : null;

        private string ValidateTestType() => ValidateHelpers.IsStringMissing(TestType) || (!TestType.Equals("Bike") && !TestType.Equals("Run")) ? "Test Type" : null;

        #endregion

        #region Commands

        #region EditSelectedCommand

        public ICommand EditSelectedCommand => _editSelectedCommand ?? (_editSelectedCommand = new RelayCommand(EditSelected));

        private void EditSelected(object obj)
        {
            Logger.Debug($"Editing {DisplayName}");
            _wsCommands.ShowWorkspace(this);
        }

        #endregion

        #region ShowUserCommand

        public ICommand SaveCommand => _saveCommand ?? (_saveCommand = new RelayCommand(param => Save(param), param => CanSave));

        public ICommand ShowUserCommand => _showUserCommand ?? (_showUserCommand = new RelayCommand(ShowUser));

        private void ShowUser(object obj)
        {
            Logger.Debug($"Show {DisplayName}");
            _wsCommands.ShowUser(this);
        }

        #endregion

        #region ShowAllMeasurementsCommand

        public ICommand ShowAllMeasurementsCommand => _showAllMeasurementsCommand ?? (_showAllMeasurementsCommand = new RelayCommand(ShowAllMeasurements));

        private void ShowAllMeasurements(object obj)
        {
            Logger.Debug($"Show All Measurements for {DisplayName}");
            _wsCommands.ShowAllMeasurements(this);
        }

        #endregion

        #region AddMeasurementCommand

        public ICommand AddMeasurementCommand => _addMeasurementCommand ?? (_addMeasurementCommand = new RelayCommand(AddMeasurement, param => _wsCommands.CanCreateMeasurement));

        private void AddMeasurement(object obj)
        {
            Logger.Debug($"Add Measurement on {DisplayName}");
            _wsCommands.CreateNewMeasurement(this);
        }

        #endregion

        #region ShowFblcCalculationCommand

        public ICommand ShowFblcCalculationCommand => _showFlbcCalculationCommand ?? (_showFlbcCalculationCommand = new RelayCommand(ShowFlbcCalculation));

        private void ShowFlbcCalculation(object obj)
        {
            Logger.Debug($"Show FLBC Calculation for {DisplayName}");
            _wsCommands.ShowFblcCalculation(this);
        }

        #endregion

        #region ShowFrpbCalculationCommand

        public ICommand ShowFrpbCalculationCommand => _showFrpbCalculationCommand ?? (_showFrpbCalculationCommand = new RelayCommand(ShowFrpbCalculation));

        private void ShowFrpbCalculation(object obj)
        {
            Logger.Debug($"Show FRPB Calculation for {DisplayName}");
            _wsCommands.ShowFrpbCalculation(this);
        }

        #endregion

        #region ShowLtCalculationCommand

        public ICommand ShowLtCalculationCommand => _showLtCalculationCommand ?? (_showLtCalculationCommand = new RelayCommand(ShowLtCalculation));

        private void ShowLtCalculation(object obj)
        {
            Logger.Debug($"Show LT Calculation for {DisplayName}");
            _wsCommands.ShowLtCalculation(this);
        }

        #endregion

        #region ShowLtLogCalculationCommand

        public ICommand ShowLtLogCalculationCommand => _showLtLogCalculationCommand ?? (_showLtLogCalculationCommand = new RelayCommand(ShowLtLogCalculation));

        private void ShowLtLogCalculation(object obj)
        {
            Logger.Debug($"Show LTLog Calculation for {DisplayName}");
            _wsCommands.ShowLtLogCalculation(this);
        }

        #endregion

        #endregion
    }
}
