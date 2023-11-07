using LanterneRouge.Wpf.MVVM;
using log4net;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LanterneRouge.Fresno.WpfClient.ViewModel
{
    public class AllMeasurementsViewModel : WorkspaceViewModel, IEquatable<AllMeasurementsViewModel>
    {
        #region Fields

        private static readonly ILog Logger = LogManager.GetLogger(typeof(AllMeasurementsViewModel));
        private static readonly string _name = typeof(AllMeasurementsViewModel).Name;
        private ICommand _addMeasurementCommand;
        private ICommand _showMeasurementCommand;
        private ICommand _showStepTestCommand;
        private ICommand _showUserCommand;

        #endregion

        #region Constructors

        public AllMeasurementsViewModel(StepTestViewModel parentStepTest, MainWindowViewModel rootViewModel) : base(parentStepTest, rootViewModel, null)
        {
            Task.Run(CreateAllMeasurements).Wait();

            // Set up subcommands
            SubCommands = new ObservableCollection<CommandViewModel>
            {
                new CommandViewModel("Edit Measurement", ShowMeasurementCommand),
                new CommandViewModel("Add Measurement", AddMeasurementCommand),
                new CommandViewModel("Show Steptest", ShowStepTestCommand),
                new CommandViewModel("Show User", ShowUserCommand)
            };
        }

        private void DataManager_Committed()
        {
            OnDispose();
            Task.Run(CreateAllMeasurements).Wait();
        }

        private async Task CreateAllMeasurements()
        {
            if (Parent is StepTestViewModel parent)
            {
                DisplayName = $"Measurements: {parent.DisplayName}";
                var allMeasurements = await DataManager.GetAllMeasurementsByStepTest(parent.Source);
                var all = (from measurement in allMeasurements select new MeasurementViewModel(measurement, parent, RootViewModel)).ToList();
                all.ForEach(a => a.PropertyChanged += OnMeasurementViewModelPropertyChanged);
                AllMeasurements = new ObservableCollection<MeasurementViewModel>(all);
                OnPropertyChanged(nameof(AllMeasurements));
                AllMeasurements.CollectionChanged += OnCollectionChanged;
                Logger.Debug("AllMeasurements created");
            }
        }

        #endregion

        #region Public Interface

        public MeasurementViewModel Selected => SelectedObject as MeasurementViewModel;

        public ObservableCollection<MeasurementViewModel> AllMeasurements { get; private set; }

        public override WorkspaceViewModel SelectedObject => AllMeasurements.FirstOrDefault(item => item.IsSelected);

        public static string GetIdentifierName(MeasurementViewModel measurement) => $"{_name}_Measurement_{measurement.MeasurementId}";

        #endregion

        #region Base Class Overrides

        protected override void OnDispose()
        {
            foreach (MeasurementViewModel measurementVM in AllMeasurements)
            {
                measurementVM.Dispose();
            }

            AllMeasurements.Clear();
            AllMeasurements.CollectionChanged -= OnCollectionChanged;
        }

        #endregion

        #region Event Handling Methods

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && !e.NewItems.Count.Equals(0))
            {
                foreach (MeasurementViewModel measurementVM in e.NewItems)
                {
                    measurementVM.PropertyChanged += OnMeasurementViewModelPropertyChanged;
                }

                OnPropertyChanged(nameof(AllMeasurements));
            }

            if (e.OldItems != null && !e.OldItems.Count.Equals(0))
            {
                foreach (MeasurementViewModel measurementVM in e.OldItems)
                {
                    measurementVM.PropertyChanged -= OnMeasurementViewModelPropertyChanged;
                }
            }
        }

        private void OnMeasurementViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            const string isSelected = "IsSelected";

            if (sender is MeasurementViewModel measurementViewModel)
            {
                measurementViewModel.VerifyPropertyName(isSelected);
            }
        }

        #endregion

        #region Private Methods

        private static int StandardSort(MeasurementViewModel x, MeasurementViewModel y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    // If x is null and y is null, they're
                    // equal. 
                    return 0;
                }

                // If x is null and y is not null, y
                // is greater. 
                return -1;
            }

            // If x is not null...
            //
            if (y == null)
            // ...and y is null, x is greater.
            {
                return 1;
            }

            return x.Sequence.CompareTo(y.Sequence);
        }

        public ICommand AddMeasurementCommand => _addMeasurementCommand ??= new RelayCommand(param => CreateChild());

        public override void CreateChild() => MeasurementViewModel.Create(Parent as StepTestViewModel, DataManager.GetAllMeasurementsByStepTest((Parent as StepTestViewModel).Source).Result.ToList(), RootViewModel);

        public ICommand ShowStepTestCommand => _showStepTestCommand ??= new RelayCommand(param => Selected.Parent.Show(), param => Selected != null && Selected.IsValid);

        public ICommand ShowUserCommand => _showUserCommand ??= new RelayCommand(param => Selected.Parent.Parent.Show(), param => Selected != null && Selected.IsValid);

        public ICommand ShowMeasurementCommand => _showMeasurementCommand ??= new RelayCommand(param => Selected.Show(), param => Selected != null && Selected.IsValid);

        #endregion

        public bool Equals(AllMeasurementsViewModel other) => Equals((object)other);

        public override bool Equals(object obj) => obj is AllMeasurementsViewModel viewModel && GetHashCode().Equals(viewModel.GetHashCode());

        public override int GetHashCode() => ((StepTestViewModel)Parent).GetHashCode();
    }
}
