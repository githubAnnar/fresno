using LanterneRouge.Fresno.Core.Entities;
using LanterneRouge.Fresno.WpfClient.MVVM;
using LanterneRouge.Fresno.WpfClient.Utils;
using log4net;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace LanterneRouge.Fresno.WpfClient.ViewModel
{
    public class MeasurementViewModel : WorkspaceViewModel, IDataErrorInfo, IEquatable<MeasurementViewModel>
    {
        #region Fields

        private static readonly ILog Logger = LogManager.GetLogger(typeof(MeasurementViewModel));
        private static readonly string _name = typeof(MeasurementViewModel).Name;
        private bool _isSelected = false;
        private ICommand _saveCommand = null;
        private ICommand _editSelectedCommand;
        private ICommand _showStepTestCommand;
        private ICommand _showUserCommand;

        #endregion

        #region Constructors

        public MeasurementViewModel(Measurement measurement, StepTestViewModel parentStepTest, Action<WorkspaceViewModel> showWorkspace) : base(parentStepTest, showWorkspace, null)
        {
            Source = measurement ?? throw new ArgumentNullException(nameof(measurement));

            // Set up commands
            SubCommands = new ObservableCollection<CommandViewModel>
            {
                new CommandViewModel("Show User", ShowUserCommand),
                new CommandViewModel("Show Steptest", ShowStepTestCommand)
            };

            ContextMenuItemCommands = new ObservableCollection<CommandViewModel>
            {
                new CommandViewModel("Edit Measurement", EditSelectedCommand),
                new CommandViewModel("Show User",ShowUserCommand),
                new CommandViewModel("Show Steptest" ,ShowStepTestCommand)
            };
        }

        #endregion

        #region Properties

        internal Measurement Source { get; private set; }

        public int MeasurementId => Source.Id;

        public int HeartRate
        {
            get { return Source.HeartRate; }
            set
            {
                if (!value.Equals(Source.HeartRate))
                {
                    Source.HeartRate = value;
                    OnPropertyChanged();
                }
            }
        }

        public float Lactate
        {
            get { return Source.Lactate; }
            set
            {
                if (!value.Equals(Source.Lactate))
                {
                    Source.Lactate = value;
                    OnPropertyChanged();
                }
            }
        }

        public float Load
        {
            get { return Source.Load; }
            set
            {
                if (!value.Equals(Source.Load))
                {
                    Source.Load = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Sequence
        {
            get { return Source.Sequence; }
            set
            {
                if (!value.Equals(Source.Sequence))
                {
                    Source.Sequence = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool InCalculation
        {
            get { return Source.InCalculation; }
            set
            {
                if (!value.Equals(Source.InCalculation))
                {
                    Source.InCalculation = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Presentation Properties

        public override string DisplayName => Source.Id == 0 ? "New Measurement" /*KayakStrings.Person_New_Singular*/ : ToString();

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (!value == _isSelected)
                {
                    _isSelected = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand SaveCommand => _saveCommand ?? (_saveCommand = new RelayCommand(param => Save(param), param => CanSave));

        public override WorkspaceViewModel SelectedObject => this;

        #endregion

        #region Public Methods

        public override string ToString() => $"Measurement ({Source.ParentStepTest.Id} / {Sequence})";

        public void Save(object param)
        {
            if (Source.IsChanged)
            {
                if (Source.State == EntityState.New)
                {
                    DataManager.AddMeasurement(Source);
                }

                else
                {
                    DataManager.UpdateMeasurement(Source);
                }

                DataManager.Commit();
                OnPropertyChanged(nameof(DisplayName));

                MessageBox.Show($"{ToString()} saved", "Saving OK", MessageBoxButton.OK, MessageBoxImage.Information);
                Logger.Info($"{ToString()} saved OK");
            }

            if (param is string stringParam && stringParam.Equals("CLOSE", StringComparison.InvariantCultureIgnoreCase))
            {
                Logger.Debug($"Closing MeasurementViewModel for {DisplayName}");
                CloseCommand.Execute(null);
            }
        }

        public static string GetIdentifierName(Measurement measurement) => string.Format("{0}_Measurement_{1}", _name, measurement.Id.Equals(0) ? Guid.NewGuid().ToString() : measurement.Id.ToString());

        #endregion // Public Methods

        #region Private Helpers

        private bool CanSave => IsValid && Source.IsChanged;

        #endregion

        #region IDataErrorInfo Members

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                CommandManager.InvalidateRequerySuggested();
                return GetValidationError(columnName);
            }
        }

        #endregion // IDataErrorInfo Members

        #region Validation

        public bool IsValid => ValidatedProperties.All(p => GetValidationError(p) == null);

        private static readonly string[] ValidatedProperties =
        {
            nameof(Sequence),
            nameof(HeartRate),
            nameof(Lactate),
            nameof(Load)
        };

        private string GetValidationError(string propertyName)
        {
            if (Array.IndexOf(ValidatedProperties, propertyName) < 0)
            {
                return null;
            }

            string error = null;

            switch (propertyName)
            {
                case nameof(Sequence):
                    error = ValidateSequence();
                    break;

                case nameof(HeartRate):
                    error = ValidateHeartRate();
                    break;

                case nameof(Lactate):
                    error = ValidateLactate();
                    break;

                case nameof(Load):
                    error = ValidateLoad();
                    break;

                default:
                    Logger.Error($"Unexpected property being validated on Measurement: {propertyName}");
                    break;
            }

            if (!string.IsNullOrEmpty(error))
            {
                Logger.Warn($"{propertyName} give '{error}'");
            }

            return error;
        }

        private string ValidateSequence() => ValidateHelpers.IsIntLTZero(Sequence) ? "Sequence"/*KayakStrings.Person_Error_MissingFirstName*/ : null;

        private string ValidateHeartRate() => ValidateHelpers.IsIntLTZero(HeartRate) ? "Heart Rate" : null;

        private string ValidateLactate() => ValidateHelpers.IsFloatLTZero(Lactate) ? "Lactate" : null;

        private string ValidateLoad() => ValidateHelpers.IsFloatLTZero(Load) ? "Load" : null;

        #endregion

        #region Commands

        #region EditSelectedCommand

        public ICommand EditSelectedCommand => _editSelectedCommand ?? (_editSelectedCommand = new RelayCommand(EditSelected));

        private void EditSelected(object obj)
        {
            Logger.Debug($"Editing {DisplayName}");
            Show();
        }

        #endregion

        #region ShowStepTestCommand

        public ICommand ShowStepTestCommand => _showStepTestCommand ?? (_showStepTestCommand = new RelayCommand(ShowStepTest));

        private void ShowStepTest(object obj)
        {
            Logger.Debug($"Show {DisplayName}");
            Parent.Show();
        }

        #endregion

        #region ShowUserCommand

        public ICommand ShowUserCommand => _showUserCommand ?? (_showUserCommand = new RelayCommand(ShowUser));

        private void ShowUser(object obj)
        {
            Logger.Debug($"Show user for {DisplayName}");
            Parent.Parent.Show();
        }

        #endregion

        #endregion

        public static void Create(StepTestViewModel parentStepTest, Action<WorkspaceViewModel> showWorkspace)
        {
            var newSequence = parentStepTest.Source.Measurements.Count == 0 ? 1 : parentStepTest.Source.Measurements.Max(m => m.Sequence) + 1;
            var newLoad = parentStepTest.Source.Measurements.Count == 0 ? parentStepTest.Source.LoadPreset : parentStepTest.Source.Measurements.Last().Load + parentStepTest.Source.Increase;

            var newMeasurement = Measurement.Create(newSequence, parentStepTest.StepTestId, 0, 0, newLoad);
            newMeasurement.InCalculation = true;
            newMeasurement.ParentStepTest = parentStepTest.Source;
            newMeasurement.AcceptChanges();
            parentStepTest.Source.Measurements.Add(newMeasurement);
            Logger.Info("New empty measurement created");
            var workspace = new MeasurementViewModel(newMeasurement, parentStepTest, showWorkspace);
            workspace.Show();
        }

        public override void CreateChild()
        {
            Logger.Error("CreateChild is not implemnted!");
            throw new NotImplementedException();
        }

        public bool Equals(MeasurementViewModel other) => Equals((object)other);

        public override bool Equals(object obj) => obj is MeasurementViewModel viewModel && Source.Equals(viewModel.Source);

        public override int GetHashCode() => Source.GetHashCode();
    }
}
