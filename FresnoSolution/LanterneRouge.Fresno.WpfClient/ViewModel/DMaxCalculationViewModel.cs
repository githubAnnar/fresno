﻿using LanterneRouge.Fresno.Calculations;
using LanterneRouge.Fresno.Calculations.Base;
using System;
using System.Collections.ObjectModel;

namespace LanterneRouge.Fresno.WpfClient.ViewModel
{
    public class DMaxCalculationViewModel : WorkspaceViewModel, IEquatable<DMaxCalculationViewModel>
    {
        #region Fields

        private DmaxCalculation _dMaxCalculation = null;
        private ObservableCollection<Zone> _dMaxZones = null;

        #endregion

        #region Constructors

        public DMaxCalculationViewModel(StepTestViewModel parentStepTest, Action<WorkspaceViewModel> showWorkspace) : base(parentStepTest, showWorkspace, null)
        { }

        #endregion

        #region Properties

        private StepTestViewModel StepTestParent => Parent as StepTestViewModel;

        public int StepTestId => StepTestParent.Source.Id;

        public string DMaxLactateThresholdText => DMaxCalculation != null ? $"Load Th.: {DMaxCalculation.LoadThreshold:0.0} Heartrate Th.: {DMaxCalculation.HeartRateThreshold:0} @ Lactate: {DMaxCalculation.LactateThreshold:0.00}" : "No Calculation";

        private DmaxCalculation DMaxCalculation => _dMaxCalculation ?? (_dMaxCalculation = StepTestParent.Source.Measurements != null && StepTestParent.Source.Measurements.Count > 0 ? new DmaxCalculation(StepTestParent.Source.Measurements) : null);

        public ObservableCollection<Zone> DMaxZones
        {
            get
            {
                if (_dMaxZones == null && StepTestParent.Source.Measurements != null && StepTestParent.Source.Measurements.Count > 0)
                {
                    if (DMaxCalculation != null)
                    {
                        var z = new PercentOfLTBasedZones(DMaxCalculation, new[] { 0.4, 0.55, 0.75, 0.90, 1.05, 1.2 });
                        _dMaxZones = new ObservableCollection<Zone>(z.Zones);
                    }
                }

                return _dMaxZones;
            }
        }

        #endregion

        #region WorkspaceViewModel Overrides

        public override WorkspaceViewModel SelectedObject => this;

        #endregion

        #region Display Methods

        public override string DisplayName => ToString();

        #endregion

        #region Public Methods

        public override string ToString() => $"DMax Calculation for Step Test ({StepTestId})";

        public override void CreateChild() => throw new NotImplementedException();

        #endregion

        public bool Equals(DMaxCalculationViewModel other) => Equals((object)other);

        public override bool Equals(object obj) => obj is DMaxCalculationViewModel viewModel && GetHashCode().Equals(viewModel.GetHashCode());

        public override int GetHashCode() => ((StepTestViewModel)Parent).GetHashCode();
    }
}
