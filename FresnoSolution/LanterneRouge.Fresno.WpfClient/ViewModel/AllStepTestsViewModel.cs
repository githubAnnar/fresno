using LanterneRouge.Fresno.WpfClient.MVVM;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace LanterneRouge.Fresno.WpfClient.ViewModel
{
    public class AllStepTestsViewModel : WorkspaceViewModel
    {
        #region Fields

        private static readonly string Name = typeof(AllStepTestsViewModel).Name;
        private readonly IWorkspaceCommands _wsCommands;
        private ICommand _showDiagramCommand;


        #endregion

        #region Constructors

        protected AllStepTestsViewModel(IWorkspaceCommands mainWorkspaceViewModel)
        {
            _wsCommands = mainWorkspaceViewModel ?? throw new ArgumentNullException("mainWorkspaceViewModel");
        }

        public AllStepTestsViewModel(UserViewModel parentUser, IWorkspaceCommands mainWorkspaceViewModel)
           : this(mainWorkspaceViewModel)
        {
            DisplayName = parentUser == null ? "All Users"/*KayakStrings.Category_All_Categories*/ : $"StepTests: {parentUser.LastName}";
            CreateAllStepTests(parentUser);
        }

        private void CreateAllStepTests(UserViewModel parentUser)
        {
            if (parentUser != null)
            {
                var all = (from stepTest in parentUser.Source.StepTests select new StepTestViewModel(stepTest, _wsCommands)).ToList();
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

        private void ShowDiagram(object obj)
        {
            _wsCommands.GenerateCalculation(AllStepTests.Where(st => st.IsSelected));
        }

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
                foreach (StepTestViewModel categoryVM in e.NewItems)
                {
                    categoryVM.PropertyChanged += OnStepTestViewModelPropertyChanged;
                }
            }

            if (e.OldItems != null && !e.OldItems.Count.Equals(0))
            {
                foreach (StepTestViewModel stepTestVM in e.OldItems)
                {
                    stepTestVM.PropertyChanged -= OnStepTestViewModelPropertyChanged;
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

        public static string GetIdentifierName(StepTestViewModel stepTest)
        {
            return string.Format("{0}_Race_{1}", Name, stepTest.StepTestId);
        }
    }
}
