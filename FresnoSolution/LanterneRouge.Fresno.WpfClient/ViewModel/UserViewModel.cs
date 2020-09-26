﻿using LanterneRouge.Fresno.DataLayer.DataAccess.Entities;
using LanterneRouge.Fresno.WpfClient.MVVM;
using LanterneRouge.Fresno.WpfClient.Utils;
using log4net;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace LanterneRouge.Fresno.WpfClient.ViewModel
{
    public class UserViewModel : WorkspaceViewModel, IDataErrorInfo
    {
        #region Fields

        private ILog Logger = LogManager.GetLogger(typeof(UserViewModel));
        private static readonly string _name = typeof(UserViewModel).Name;
        private ICommand _saveCommand = null;
        private bool _isSelected = false;
        private readonly IWorkspaceCommands _wsCommands;
        private User _source;
        private ICommand _editSelectedCommand;
        private ICommand _showAllStepTestsCommand;
        private ICommand _addStepTestCommand;

        #endregion

        #region Constructors

        public UserViewModel(User user, IWorkspaceCommands mainWorkspaceViewModel)
        {
            Source = user ?? throw new ArgumentNullException(nameof(user));
            _wsCommands = mainWorkspaceViewModel ?? throw new ArgumentNullException(nameof(mainWorkspaceViewModel));
            ItemIcon = new BitmapImage(new Uri(@"pack://application:,,,/Resources/icons8-user-100.png"));
        }

        #endregion

        #region Properties

        internal User Source
        {
            get => _source.IsLoaded ? _source : DataManager.GetAllUsers().Where(u => u.Id == _source.Id).FirstOrDefault();
            private set => _source = value;
        }

        public int UserId => Source.Id;

        public string FirstName
        {
            get { return Source.FirstName; }
            set
            {
                if (!value.Equals(Source.FirstName))
                {
                    Source.FirstName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string LastName
        {
            get { return Source.LastName; }
            set
            {
                if (!value.Equals(Source.LastName))
                {
                    Source.LastName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Street
        {
            get { return Source.Street; }
            set
            {
                if (!value.Equals(Source.Street))
                {
                    Source.Street = value;
                    OnPropertyChanged();
                }
            }
        }

        public string PostCode
        {
            get { return Source.PostCode; }
            set
            {
                if (!value.Equals(Source.PostCode))
                {
                    Source.PostCode = value;
                    OnPropertyChanged();
                }
            }
        }

        public string PostCity
        {
            get { return Source.PostCity; }
            set
            {
                if (!value.Equals(Source.PostCity))
                {
                    Source.PostCity = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime BirthDate
        {
            get { return Source.BirthDate; }
            set
            {
                if (!value.Equals(Source.BirthDate))
                {
                    Source.BirthDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Height
        {
            get { return Source.Height; }
            set
            {
                if (!value.Equals(Source.Height))
                {
                    Source.Height = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Sex
        {
            get { return Source.Sex; }
            set
            {
                if (!value.Equals(Source.Sex))
                {
                    Source.Sex = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Email
        {
            get { return Source.Email; }
            set
            {
                if (!value.Equals(Source.Email))
                {
                    Source.Email = value;
                    OnPropertyChanged();
                }
            }
        }

        public int MaxHr
        {
            get { return Source.MaxHr; }
            set
            {
                if (!value.Equals(Source.MaxHr))
                {
                    Source.MaxHr = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Display Properties

        public override string DisplayName => Source.Id == 0 ? "New User" /*KayakStrings.Race_New_Singular*/ : ToString();

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

        public ICommand SaveCommand => _saveCommand ?? (_saveCommand = new RelayCommand(param => Save(param), param => CanSave));

        #endregion

        #region Public Methods

        public override string ToString() => $"{LastName ?? "User" /*KayakStrings.Race_Singular*/} ({UserId})";

        public void Save(object param)
        {
            if (Source.IsChanged)
            {
                if (Source.State == EntityState.New)
                {
                    DataManager.AddUser(Source);
                }

                else
                {
                    DataManager.UpdateUser(Source);
                }

                DataManager.Commit();
                DataManager.GetAllUsers(true);
                OnPropertyChanged(nameof(DisplayName));

                MessageBox.Show($"User: {LastName} saved", "Saving OK", MessageBoxButton.OK, MessageBoxImage.Information);
                Logger.Info($"User: {LastName} saved OK");
            }

            if (param is string stringParam && stringParam.Equals("CLOSE", StringComparison.InvariantCultureIgnoreCase))
            {
                Logger.Debug($"Closing {nameof(UserViewModel)} for {DisplayName}");
                CloseCommand.Execute(null);
            }
        }

        public override WorkspaceViewModel SelectedObject => this;

        public static string GetIdentifierName(User user)
        {
            var id = user.Id.Equals(-1) ? Guid.NewGuid().ToString() : user.Id.ToString();
            return $"{_name}_Race_{id}";
        }

        #endregion

        #region Private Helpers

        private bool CanSave => Source.IsValid && Source.IsChanged;

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

        public bool IsValid => !ValidatedProperties.Any(vp => GetValidationError(vp) != null);

        private static readonly string[] ValidatedProperties =
        {
            nameof(FirstName),
            nameof(LastName),
            nameof(Email),
            nameof(Sex)
        };

        /// <summary>
        /// Gets the validation error.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        private string GetValidationError(string propertyName)
        {
            if (Array.IndexOf(ValidatedProperties, propertyName) < 0)
            {
                return null;
            }

            string error = null;

            switch (propertyName)
            {
                case nameof(FirstName):
                    error = ValidateFirstName();
                    break;

                case nameof(LastName):
                    error = ValidateLastName();
                    break;

                case nameof(Email):
                    error = ValidateEmail();
                    break;

                case nameof(Sex):
                    error = ValidateSex();
                    break;

                default:
                    Logger.Warn($"Unexpected property being validated on User: { propertyName}");
                    break;
            }

            if (!string.IsNullOrEmpty(error))
            {
                Logger.Warn($"{propertyName} give '{error}'");
            }

            return error;
        }

        private string ValidateFirstName() => ValidateHelpers.IsStringMissing(FirstName) ? "Missing First Name"/*KayakStrings.Race_Error_MissingName*/ : null;

        private string ValidateLastName() => ValidateHelpers.IsStringMissing(LastName) ? "Missing Last Name"/*KayakStrings.Race_Error_MissingName*/ : null;

        private string ValidateEmail() => ValidateHelpers.IsStringMissing(Email) ? "Missing Email"/*KayakStrings.Race_Error_MissingName*/ : null;

        private string ValidateSex()
        {
            string error = ValidateHelpers.IsStringMissing(Sex) ? "Missing Sex"/*KayakStrings.Race_Error_MissingName*/ : null;
            error = !Sex.Equals("M") && !Sex.Equals("F") ? "Wrong Sex" : null;

            return error;
        }

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

        #region ShowAllStepTestsCommand

        public ICommand ShowAllStepTestsCommand => _showAllStepTestsCommand ?? (_showAllStepTestsCommand = new RelayCommand(ShowAllStepTests));

        private void ShowAllStepTests(object obj)
        {
            Logger.Debug($"Show All SteTests for {DisplayName}");
            _wsCommands.ShowAllStepTests(this);
        }

        #endregion

        #region AddStepTestCommand

        public ICommand AddStepTestCommand => _addStepTestCommand ?? (_addStepTestCommand = new RelayCommand(AddStepTest, param => _wsCommands.CanCreateStepTest));

        private void AddStepTest(object obj)
        {
            Logger.Debug($"Add StepTest on {DisplayName}");
            _wsCommands.CreateNewStepTest(this);
        }

        #endregion

        #endregion
    }
}
