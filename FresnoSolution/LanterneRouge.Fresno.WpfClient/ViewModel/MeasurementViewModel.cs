using LanterneRouge.Fresno.DataLayer.DataAccess.Entities;
using LanterneRouge.Fresno.WpfClient.MVVM;
using LanterneRouge.Fresno.WpfClient.Utils;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace LanterneRouge.Fresno.WpfClient.ViewModel
{
    public class MeasurementViewModel : WorkspaceViewModel, IDataErrorInfo
    {
        #region Fields

        private static readonly string _name = typeof(MeasurementViewModel).Name;
        private bool _isSelected = false;
        private ICommand _saveCommand = null;
        private readonly IWorkspaceCommands _wsCommands;
        private ICommand _editSelectedCommand;
        private ICommand _showStepTestCommand;
        private ICommand _showUserCommand;

        #endregion

        #region Constructors

        public MeasurementViewModel(Measurement measurement, IWorkspaceCommands mainWorkspaceViewModel)
        {
            Source = measurement ?? throw new ArgumentNullException(nameof(measurement));
            _wsCommands = mainWorkspaceViewModel ?? throw new ArgumentNullException(nameof(mainWorkspaceViewModel));
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

        public override string DisplayName
        {
            get
            {
                return Source.Id == 0 ? "New Measurement" /*KayakStrings.Person_New_Singular*/ : ToString();
            }
        }

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

        public void Save(object param)
        {
            if (Source.IsChanged)
            {
                DataManager.Commit();
                OnPropertyChanged(nameof(DisplayName));

                MessageBox.Show(string.Format("Measurement: {0} saved", Sequence), "Saving OK", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            if (param is string stringParam && stringParam.Equals("CLOSE", StringComparison.InvariantCultureIgnoreCase))
            {
                CloseCommand.Execute(null);
            }
        }

        public static string GetIdentifierName(Measurement measurement)
        {
            return string.Format("{0}_Measurement_{1}", _name, measurement.Id.Equals(0) ? Guid.NewGuid().ToString() : measurement.Id.ToString());
        }

        #endregion // Public Methods

        #region Private Helpers

        private bool CanSave
        {
            get { return IsValid && Source.IsChanged; }
        }

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
                    Debug.Fail($"Unexpected property being validated on Measurement: {propertyName}");
                    break;
            }

            return error;
        }

        private string ValidateSequence()
        {
            return ValidateHelpers.IsIntLTZero(Sequence) ? "Sequence"/*KayakStrings.Person_Error_MissingFirstName*/ : null;
        }

        private string ValidateHeartRate()
        {
            return ValidateHelpers.IsIntLTZero(HeartRate) ? "Heart Rate" : null;
        }

        private string ValidateLactate()
        {
            return ValidateHelpers.IsFloatLTZero(Lactate) ? "Lactate" : null;
        }

        private string ValidateLoad()
        {
            return ValidateHelpers.IsFloatLTZero(Load) ? "Load" : null;
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

        #region ShowStepTestCommand

        public ICommand ShowStepTestCommand => _showStepTestCommand ?? (_showStepTestCommand = new RelayCommand(ShowStepTest));

        private void ShowStepTest(object obj)
        {
            _wsCommands.ShowStepTest(this);
        }

        #endregion

        #region ShowUserCommand

        public ICommand ShowUserCommand => _showUserCommand ?? (_showUserCommand = new RelayCommand(ShowUser));

        private void ShowUser(object obj)
        {
            _wsCommands.ShowUser(this);
        }

        #endregion

        #endregion
    }
}
