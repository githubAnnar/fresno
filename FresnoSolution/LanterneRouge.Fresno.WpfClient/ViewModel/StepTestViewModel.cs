using LanterneRouge.Fresno.Calculations;
using LanterneRouge.Fresno.Core.Entity;
using LanterneRouge.Fresno.Core.Entity.Extentions;
using LanterneRouge.Fresno.Report;
using LanterneRouge.Fresno.Services.Models;
using LanterneRouge.Fresno.Utils.Helpers;
using LanterneRouge.Fresno.WpfClient.View;
using LanterneRouge.Wpf.MVVM;
using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace LanterneRouge.Fresno.WpfClient.ViewModel
{
    public class StepTestViewModel : WorkspaceViewModel, IDataErrorInfo, IEquatable<StepTestViewModel>
    {
        #region Fields

        private static readonly ILog Logger = LogManager.GetLogger(typeof(StepTestViewModel));
        private static readonly string _name = typeof(StepTestViewModel).Name;
        private bool _isSelected = false;
        private ICommand _saveCommand;
        private ICommand _editSelectedCommand;
        private ICommand _showUserCommand;
        private ICommand _showAllMeasurementsCommand;
        private ICommand _addMeasurementCommand;
        private ICommand _showFblcCalculationCommand;
        private ICommand _showFrpbCalculationCommand;
        private ICommand _showLtCalculationCommand;
        private ICommand _showLtLogCalculationCommand;
        private ICommand _showDMaxCalculationCommand;
        private ICommand _createStepTestPdfCommand;

        #endregion

        #region Constructors

        public StepTestViewModel(StepTestModel stepTest, UserViewModel parentUser, MainWindowViewModel rootViewModel) : base(parentUser, rootViewModel, new BitmapImage(new Uri(@"pack://application:,,,/Resources/icons8-diabetes-96.png")))
        {
            Source = stepTest ?? throw new ArgumentNullException(nameof(stepTest));

            // Set up commands
            SubCommands =
            [
                new CommandViewModel("Show User", ShowUserCommand),
                new CommandViewModel("Add Measurement", AddMeasurementCommand),
                new CommandViewModel("Show all Measurements", ShowAllMeasurementsCommand),
                new CommandViewModel("Generate PDF", CreateStepTestPdfCommand),
                new CommandViewModel("FBLC Calculation", ShowFblcCalculationCommand),
                new CommandViewModel("FRPB Calculation", ShowFrpbCalculationCommand),
                new CommandViewModel("LT Calculation", ShowLtCalculationCommand),
                new CommandViewModel("LT Log Calculation", ShowLtLogCalculationCommand),
                new CommandViewModel("DMax Calculation", ShowDMaxCalculationCommand)
            ];

            ContextMenuItemCommands =
            [
                new CommandViewModel("Edit Steptest", EditSelectedCommand),
                new CommandViewModel("Show User", ShowUserCommand),
                new CommandViewModel("Add Measurement", AddMeasurementCommand),
                new CommandViewModel("Show all Measurements", ShowAllMeasurementsCommand),
                new CommandViewModel("Generate PDF", CreateStepTestPdfCommand),
                new CommandViewModel("FBLC Calculation", ShowFblcCalculationCommand),
                new CommandViewModel("FRPB Calculation", ShowFrpbCalculationCommand),
                new CommandViewModel("LT Calculation", ShowLtCalculationCommand),
                new CommandViewModel("LT Log Calculation", ShowLtLogCalculationCommand),
                new CommandViewModel("DMax Calculation", ShowDMaxCalculationCommand)
            ];
        }

        #endregion

        #region Properties

        internal StepTestModel Source { get; private set; }

        public Guid StepTestId => Source.Id;

        public string TestType
        {
            get => Source.TestType;
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
            get => Source.EffortUnit;
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
            get => TimeSpan.FromTicks(Source.StepDuration);
            set
            {
                if (!value.Ticks.Equals(Source.StepDuration))
                {
                    Source.StepDuration = value.Ticks;
                    OnPropertyChanged();
                }
            }
        }

        public float LoadPreset
        {
            get => Source.LoadPreset;
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
            get => Source.Increase;
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
            get => Source.Temperature;
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
            get => Source.Weight;
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
            get => Source.TestDate;
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

        public double DMaxValue => DmaxCalculation != null ? DmaxCalculation.LoadThreshold : 0d;

        private FblcCalculation _fblcCalculation = null;
        private FblcCalculation FblcCalculation => _fblcCalculation ??= DataManager.GetMeasurementCountByStepTest(Source).Result > 0 ? new FblcCalculation([.. DataManager.GetAllMeasurementsByStepTest(Source).Result], 4.0) : null;

        private FrpbCalculation _frpbCalculation = null;
        private FrpbCalculation FrpbCalculation => _frpbCalculation ??= DataManager.GetMeasurementCountByStepTest(Source).Result > 0 ? new FrpbCalculation([.. DataManager.GetAllMeasurementsByStepTest(Source).Result], 1.0) : null;

        private LTCalculation _ltCalculation = null;
        private LTCalculation LtCalculation => _ltCalculation ??= DataManager.GetMeasurementCountByStepTest(Source).Result > 0 ? new LTCalculation([.. DataManager.GetAllMeasurementsByStepTest(Source).Result]) : null;

        private LTLogCalculation _ltLogCalculation = null;

        private LTLogCalculation LtLogCalculation => _ltLogCalculation ??= DataManager.GetMeasurementCountByStepTest(Source).Result > 0 ? new LTLogCalculation([.. DataManager.GetAllMeasurementsByStepTest(Source).Result]) : null;

        private DmaxCalculation _dmaxCalculation;

        private DmaxCalculation DmaxCalculation => _dmaxCalculation ??= DataManager.GetMeasurementCountByStepTest(Source).Result > 0 ? new DmaxCalculation([.. DataManager.GetAllMeasurementsByStepTest(Source).Result], false) : null;

        #endregion

        #region Display Properties

        public override string DisplayName => Source.Id == Guid.Empty ? "New Step Test"/*KayakStrings.Category_New_Singular*/ : ToString();

        public bool IsSelected
        {
            get => _isSelected;
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
            if (DataManager.IsChanged(Source).Result)
            {
                DataManager.SaveStepTest(Source);
                if (Parent is UserViewModel uvm)
                {
                    uvm.SaveToAllStepTests(this);
                }

                OnPropertyChanged(nameof(DisplayName));

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

        private bool CanSave => IsValid && DataManager.IsChanged(Source).Result;

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

        private static readonly string[] ValidatedProperties = [nameof(TestType), nameof(EffortUnit), nameof(StepDurationTimespan), nameof(LoadPreset), nameof(Increase)];

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

        public ICommand EditSelectedCommand => _editSelectedCommand ??= new RelayCommand(EditSelected);

        private void EditSelected(object obj)
        {
            Logger.Debug($"Editing {DisplayName}");
            Show();
        }

        #endregion

        #region ShowUserCommand

        public ICommand SaveCommand => _saveCommand ??= new RelayCommand(param => Save(param), param => CanSave);

        public ICommand ShowUserCommand => _showUserCommand ??= new RelayCommand(ShowUser);

        private void ShowUser(object obj)
        {
            Logger.Debug($"Show {DisplayName}");
            Parent.Show();
        }

        #endregion

        #region ShowAllMeasurementsCommand

        public ICommand ShowAllMeasurementsCommand => _showAllMeasurementsCommand ??= new RelayCommand(param => ShowAllMeasurements(), CanShowAllMeasurements);

        public Predicate<object> CanShowAllMeasurements => (object o) => DataManager.GetAllMeasurementsByStepTest(Source).Result.Any();

        private void ShowAllMeasurements()
        {
            var workspace = new AllMeasurementsViewModel(this, RootViewModel);
            workspace.Show();
            Logger.Debug("Shown all measurements");
        }

        #endregion

        #region AddMeasurementCommand

        public ICommand AddMeasurementCommand => _addMeasurementCommand ??= new RelayCommand(param => CreateChild(), param => IsValid);

        public override void CreateChild()
        {
            Logger.Debug($"Add Measurement on {DisplayName}");
            MeasurementViewModel.Create(this, [.. DataManager.GetAllMeasurementsByStepTest(Source).Result], RootViewModel);
        }

        public void SaveToAllMeasurements(MeasurementViewModel newMeasurement)
        {
            // find AllMeasurements for this steptest if shown
            var workSpace = RootViewModel.Workspaces.FirstOrDefault(w => w.GetType().Equals(typeof(AllMeasurementsViewModel)) && w is AllMeasurementsViewModel allMeasurementsForStepTest && ((StepTestViewModel)allMeasurementsForStepTest.Parent).StepTestId == StepTestId);
            if (workSpace != null && workSpace is AllMeasurementsViewModel measurements)
            {
                if (!measurements.AllMeasurements.Contains(newMeasurement))
                {
                    measurements.AllMeasurements.Add(newMeasurement);
                }
            }
        }

        #endregion

        #region ShowFblcCalculationCommand

        public ICommand ShowFblcCalculationCommand => _showFblcCalculationCommand ??= new RelayCommand(param => ShowFblcCalculation());

        private void ShowFblcCalculation()
        {
            Logger.Debug($"Show FBLC Calculation for {DisplayName}");
            var workspace = new FblcCalculationViewModel(this, RootViewModel);
            workspace.Show();
        }

        #endregion

        #region ShowFrpbCalculationCommand

        public ICommand ShowFrpbCalculationCommand => _showFrpbCalculationCommand ??= new RelayCommand(param => ShowFrpbCalculation());

        private void ShowFrpbCalculation()
        {
            Logger.Debug($"Show FRPB Calculation for {DisplayName}");
            var workspace = new FrpbCalculationViewModel(this, RootViewModel);
            workspace.Show();
        }

        #endregion

        #region ShowLtCalculationCommand

        public ICommand ShowLtCalculationCommand => _showLtCalculationCommand ??= new RelayCommand(ShowLtCalculation);

        private void ShowLtCalculation(object obj)
        {
            Logger.Debug($"Show LT Calculation for {DisplayName}");
            var workspace = new LtCalculationViewModel(this, RootViewModel);
            workspace.Show();
        }

        #endregion

        #region ShowLtLogCalculationCommand

        public ICommand ShowLtLogCalculationCommand => _showLtLogCalculationCommand ??= new RelayCommand(ShowLtLogCalculation);

        private void ShowLtLogCalculation(object obj)
        {
            Logger.Debug($"Show LTLog Calculation for {DisplayName}");
            var workspace = new LtLogCalculationViewModel(this, RootViewModel);
            workspace.Show();
        }

        #endregion

        #region ShowDMaxCalculationCommand

        public ICommand ShowDMaxCalculationCommand => _showDMaxCalculationCommand ??= new RelayCommand(ShowDMaxCalculation);

        private void ShowDMaxCalculation(object obj)
        {
            Logger.Debug($"Show DMax Calculation for {DisplayName}");
            var workspace = new DMaxCalculationViewModel(this, RootViewModel);
            workspace.Show();
        }

        #endregion

        #region ShowPdfCommand

        public ICommand CreateStepTestPdfCommand => _createStepTestPdfCommand ??= new RelayCommand(param => GenerateStepTestPdf(), param => Source.IsValid());

        public void GenerateStepTestPdf()
        {
            ContentWindow modalWindow = null;
            var parentUser = Parent as UserViewModel;
            var selectedList = new List<StepTestViewModel>();
            var viewModel = new UserStepTestListViewModel(parentUser, this, (p, dr) =>
             {
                 if (p != null)
                 {
                     selectedList = p.ToList();
                 }
                 modalWindow.DialogResult = dr;
                 modalWindow.Close();
             }, RootViewModel);

            var view = new UserStepTestListView { DataContext = viewModel };
            modalWindow = new ContentWindow
            {
                Content = view
            };
            modalWindow.ShowDialog();
            if (modalWindow.DialogResult.HasValue && modalWindow.DialogResult.Value)
            {
                var generator = new StepTestReport(Source, selectedList.Select(s => s.Source).ToList());
                var pdfDocument = generator.CreateReport();
                var filename = $"{parentUser.FirstName} {parentUser.LastName} ({Source.Id}).pdf";
                generator.PdfRender(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), filename), pdfDocument);

                MessageBox.Show($"PDF {filename} is generated", "PDF Generation", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        #endregion

        #endregion

        public static void Create(UserViewModel parentUser, MainWindowViewModel rootViewModel)
        {
            var newStepTest = StepTestModel.Create(parentUser.UserId);
            Logger.Info("Created new empty step test entity");
            var workspace = new StepTestViewModel(newStepTest, parentUser, rootViewModel);
            workspace.Show();
            Logger.Debug($"Created new StepTest on {workspace.DisplayName}");
        }

        public bool Equals(StepTestViewModel other) => Equals((object)other);

        public override bool Equals(object obj) => obj is StepTestViewModel viewModel && Source.Equals(viewModel.Source);

        public override int GetHashCode() => Source.GetHashCode();
    }
}
