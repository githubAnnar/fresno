using LanterneRouge.Fresno.WpfClient.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace LanterneRouge.Fresno.WpfClient.ViewModel
{
    public class AllMeasurementsViewModel : WorkspaceViewModel
    {
        #region Fields

        private static readonly string _name = typeof(AllMeasurementsViewModel).Name;
        private readonly IWorkspaceCommands _wsCommands;

        #endregion

        #region Constructors

        protected AllMeasurementsViewModel(IWorkspaceCommands mainWorkspaceViewModel)
        {
            _wsCommands = mainWorkspaceViewModel ?? throw new ArgumentNullException("mainWorkspaceViewModel");
        }

        public AllMeasurementsViewModel(StepTestViewModel parentStepTest, IWorkspaceCommands mainWorkspaceViewModel)
            : this(mainWorkspaceViewModel)
        {
            CreateAllMeasurements(parentStepTest);
        }

        public AllMeasurementsViewModel(UserViewModel parentUser, IWorkspaceCommands mainWorkspaceViewModel)
            : this(mainWorkspaceViewModel)
        {
            CreateAllMeasurements(parentUser);
        }

        private void CreateAllMeasurements(object parent)
        {
            List<MeasurementViewModel> all;
            if (parent != null)
            {
                if (parent is StepTestViewModel)
                {
                    var stepTest = parent as StepTestViewModel;
                    DisplayName = stepTest == null ? "All Measurements"/*KayakStrings.Craft_All_Crafts*/ : $"Measurements: {stepTest.DisplayName}";
                    all = (from measurement in stepTest.Source.Measurements select new MeasurementViewModel(measurement, _wsCommands)).ToList();
                }

                else
                {
                    all = new List<MeasurementViewModel>();
                }
            }

            else
            {
                all = (from measurement in DataManager.GetAllMeasurements() select new MeasurementViewModel(measurement, _wsCommands)).ToList();
            }

            foreach (var mvm in all)
            {
                mvm.PropertyChanged += OnMeasurementViewModelPropertyChanged;
            }

            AllMeasurements = new ObservableCollection<MeasurementViewModel>(all);
            AllMeasurements.CollectionChanged += OnCollectionChanged;
        }

        #endregion

        #region Public Interface

        public ObservableCollection<MeasurementViewModel> AllMeasurements { get; private set; }

        public override WorkspaceViewModel SelectedObject
        {
            get { return AllMeasurements.FirstOrDefault(item => item.IsSelected); }
        }

        public static string GetIdentifierName(MeasurementViewModel measurement)
        {
            return string.Format("{0}_Category_{1}", _name, measurement.MeasurementId);
        }

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
    }

    #endregion
}
