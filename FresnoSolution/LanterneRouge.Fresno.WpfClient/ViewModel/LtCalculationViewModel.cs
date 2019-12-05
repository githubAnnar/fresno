using LanterneRouge.Fresno.Calculations;
using LanterneRouge.Fresno.Calculations.Base;
using LanterneRouge.Fresno.DataLayer.DataAccess.Entities;
using LanterneRouge.Fresno.WpfClient.MVVM;
using System;
using System.Collections.ObjectModel;

namespace LanterneRouge.Fresno.WpfClient.ViewModel
{
    public class LtCalculationViewModel : WorkspaceViewModel
    {
        #region Fields

        private readonly IWorkspaceCommands _wsCommands;

        #endregion

        #region Constructors

        public LtCalculationViewModel(StepTest category, IWorkspaceCommands mainWorkspaceViewModel)
        {
            Source = category ?? throw new ArgumentNullException(nameof(category));
            _wsCommands = mainWorkspaceViewModel ?? throw new ArgumentNullException(nameof(mainWorkspaceViewModel));
        }

        #endregion

        #region Properties

        internal StepTest Source { get; private set; }

        public int StepTestId => Source.Id;

        public string LTLactateThresholdText => LtCalculation != null ? $"Load Th.: {LtCalculation.LoadThreshold:0.0} Heartrate Th.: {LtCalculation.HeartRateThreshold:0}" : "No Calculation";

        private LTCalculation _ltCalculation = null;
        private LTCalculation LtCalculation => _ltCalculation ?? (_ltCalculation = Source.Measurements != null && Source.Measurements.Count > 0 ? new LTCalculation(Source.Measurements) : null);

        private ObservableCollection<Zone> _LTZones = null;
        public ObservableCollection<Zone> LTZones
        {
            get
            {
                if (_LTZones == null && Source.Measurements != null && Source.Measurements.Count > 0)
                {
                    if (LtCalculation != null)
                    {
                        var z = new PercentOfLTBasedZones(LtCalculation, new[] { 0.4, 0.55, 0.75, 0.90, 1.05, 1.2 });
                        _LTZones = new ObservableCollection<Zone>(z.Zones);
                    }
                }

                return _LTZones;
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

        public override string ToString() => $"LT Calculation for Step Test ({StepTestId})";

        #endregion
    }
}
