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
    public class AllUsersViewModel : WorkspaceViewModel
    {
        #region Fields

        private static readonly ILog Logger = LogManager.GetLogger(typeof(AllUsersViewModel));
        private static readonly string _name = typeof(AllUsersViewModel).Name;
        private ICommand _addUserCommand;
        private ICommand _addSteptestCommand;
        private ICommand _showUserCommand;
        private ICommand _showAllStepTestCommand;

        #endregion

        #region Constructors

        public AllUsersViewModel(Action<WorkspaceViewModel> showWorkspace) : base(null, showWorkspace, new BitmapImage(new Uri(@"pack://application:,,,/Resources/icons8-user-100.png")))
        {
            DisplayName = "All Users"; /*KayakStrings.Race_All_Races;*/
            CreateAllUsers();
            DataManager.Committed += DataManager_Committed;
            Logger.Debug($"AllUsers loaded");

            // Set up commands
            SubCommands = new ObservableCollection<CommandViewModel>
            {
                new CommandViewModel("Edit User", ShowUserCommand),
                new CommandViewModel("Show all Steptests", ShowAllStepTestsCommand),
                new CommandViewModel("Add User", AddUserCommand),
                new CommandViewModel("Add Steptest", AddStepTestCommand)
            };
        }

        private void DataManager_Committed()
        {
            OnDispose();
            CreateAllUsers();
        }

        private void CreateAllUsers()
        {
            var all = (from user in DataManager.GetAllUsers(true) select new UserViewModel(user, ShowWorkspace)).ToList();
            all.ForEach(a => a.PropertyChanged += OnUserViewModelPropertyChanged);
            AllUsers = new ObservableCollection<UserViewModel>(all);
            AllUsers.CollectionChanged += OnCollectionChanged;
        }

        #endregion

        #region Public Interface

        public ObservableCollection<UserViewModel> AllUsers { get; private set; }

        public UserViewModel Selected => SelectedObject as UserViewModel;

        public static string GetIdentifierName() => _name;

        #endregion

        #region Base Class Overrides

        protected override void OnDispose()
        {
            foreach (UserViewModel userVM in AllUsers)
            {
                userVM.Dispose();
            }

            AllUsers.Clear();
            AllUsers.CollectionChanged -= OnCollectionChanged;
        }

        public override WorkspaceViewModel SelectedObject => AllUsers.FirstOrDefault(item => item.IsSelected);

        public override void CreateChild()
        {
            UserViewModel.Create(ShowWorkspace);
        }

        public ICommand AddUserCommand => _addUserCommand ?? (_addUserCommand = new RelayCommand(param => CreateChild()));

        public void CreateStepTest()
        {
            StepTestViewModel.Create(Selected, ShowWorkspace);
        }

        public bool CanCreateStepTest => Selected != null && Selected.IsValid;

        public ICommand AddStepTestCommand => _addSteptestCommand ?? (_addSteptestCommand = new RelayCommand(param => CreateStepTest(), param => CanCreateStepTest));

        public ICommand ShowUserCommand => _showUserCommand ?? (_showUserCommand = new RelayCommand(param => ShowUser(), param => CanShowUser));

        private void ShowUser()
        {
            Selected.Show();
        }

        public bool CanShowUser => Selected != null && Selected.IsValid;

        public ICommand ShowAllStepTestsCommand => _showAllStepTestCommand ?? (_showAllStepTestCommand = new RelayCommand(param => ShowAllStepTests(), param => CanShowAllStepTests));

        private void ShowAllStepTests()
        {
            var workspace = new AllStepTestsViewModel(Selected, ShowWorkspace);
            workspace.Show();
        }

        public bool CanShowAllStepTests => Selected != null && Selected.IsValid;

        #endregion

        #region Event Handling Methods

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && !e.NewItems.Count.Equals(0))
            {
                foreach (UserViewModel userVM in e.NewItems)
                {
                    userVM.PropertyChanged += OnUserViewModelPropertyChanged;
                    Logger.Debug($"New UserViewModel {userVM.DisplayName}");
                }
            }

            if (e.OldItems != null && !e.OldItems.Count.Equals(0))
            {
                foreach (UserViewModel userVM in e.OldItems)
                {
                    userVM.PropertyChanged -= OnUserViewModelPropertyChanged;
                    Logger.Debug($"Old UserViewModel {userVM.DisplayName}");
                }
            }
        }

        private void OnUserViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            const string isSelected = "IsSelected";

            // Make sure that the property name we're referencing is valid.
            // This is a debugging technique, and does not execute in a Release build.
            if (sender is UserViewModel userViewModel)
            {
                userViewModel.VerifyPropertyName(isSelected);
            }

            // When a customer is selected or unselected, we must let the
            // world know that the TotalSelectedSales property has changed,
            // so that it will be queried again for a new value.
            //if (e.PropertyName.Equals(isSelected))
            //{
            //    this.OnPropertyChanged("TotalSelectedSales");
            //}
        }

        #endregion
    }
}
