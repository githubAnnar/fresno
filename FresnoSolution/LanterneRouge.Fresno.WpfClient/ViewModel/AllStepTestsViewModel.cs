using LanterneRouge.Fresno.WpfClient.MVVM;
using log4net;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace LanterneRouge.Fresno.WpfClient.ViewModel
{
    public class AllStepTestsViewModel : WorkspaceViewModel
    {
        #region Fields

        private static readonly ILog Logger = LogManager.GetLogger(typeof(AllStepTestsViewModel));
        private static readonly string Name = typeof(AllStepTestsViewModel).Name;
        private ICommand _showDiagramCommand;

        #endregion

        #region Constructors

        public AllStepTestsViewModel(UserViewModel parentUser, Action<WorkspaceViewModel> showWorkspace) : base(parentUser, showWorkspace, new BitmapImage(new Uri(@"pack://application:,,,/Resources/icons8-diabetes-96.png")))
        {
            DisplayName = parentUser == null ? "All Users"/*KayakStrings.Category_All_Categories*/ : $"StepTests: {parentUser.LastName}";
            CreateAllStepTests();
            DataManager.Committed += DataManager_Committed;
            Logger.Debug($"AllStepests for user {parentUser.LastName} loaded");
        }

        private void DataManager_Committed()
        {
            OnDispose();
            CreateAllStepTests();
        }

        private void CreateAllStepTests()
        {
            if (Parent is UserViewModel parent)
            {
                var all = (from stepTest in parent.Source.StepTests select new StepTestViewModel(stepTest, parent, ShowWorkspace)).ToList();
                all.ForEach(cvm => cvm.PropertyChanged += OnStepTestViewModelPropertyChanged);
                AllStepTests = new ObservableCollection<StepTestViewModel>(all);
                AllStepTests.CollectionChanged += OnCollectionChanged;
            }
        }

        #endregion

        #region Public Interface

        public ObservableCollection<StepTestViewModel> AllStepTests { get; private set; }

        #region ShowDiagram Command

        public ICommand ShowDiagramCommand => _showDiagramCommand ?? (_showDiagramCommand = new RelayCommand(ShowDiagram, param => CanShowDiagram));

        public bool CanShowDiagram => AllStepTests.Any(at => at.IsSelected);

        private void ShowDiagram(object obj) => _wsCommands.GenerateCalculation(AllStepTests.Where(st => st.IsSelected));

        #endregion

        #endregion

        #region Base Class Overrides

        /// <summary>
        /// Child classes can override this method to perform
        /// clean-up logic, such as removing event handlers.
        /// </summary>
        protected override void OnDispose()
        {
            foreach (var stepTestVM in AllStepTests)
            {
                Logger.Debug($"Disposing {stepTestVM.DisplayName}");
                stepTestVM.Dispose();
            }

            AllStepTests.Clear();
            AllStepTests.CollectionChanged -= OnCollectionChanged;
        }

        #endregion

        #region Event Handling Methods

        /// <summary>
        /// Called when [collection changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="NotifyCollectionChangedEventArgs"/> instance containing the event data.</param>
        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && !e.NewItems.Count.Equals(0))
            {
                foreach (StepTestViewModel stepTestVM in e.NewItems)
                {
                    stepTestVM.PropertyChanged += OnStepTestViewModelPropertyChanged;
                    Logger.Debug($"New StepTestViewModel {stepTestVM.DisplayName}");
                }
            }

            if (e.OldItems != null && !e.OldItems.Count.Equals(0))
            {
                foreach (StepTestViewModel stepTestVM in e.OldItems)
                {
                    stepTestVM.PropertyChanged -= OnStepTestViewModelPropertyChanged;
                    Logger.Debug($"Old StepTestViewModel {stepTestVM.DisplayName}");
                }
            }
        }

        private void OnStepTestViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            const string isSelected = "IsSelected";

            if (sender is StepTestViewModel stepTestViewModel)
            {
                stepTestViewModel.VerifyPropertyName(isSelected);
            }
        }

        #endregion

        /// <summary>
        /// Gets the selected object.
        /// </summary>
        public override WorkspaceViewModel SelectedObject => AllStepTests.FirstOrDefault(item => item.IsSelected);

        public static string GetIdentifierName(StepTestViewModel stepTest) => $"{Name}_StepTest_{stepTest.StepTestId}";
    }
}
