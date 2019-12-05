using LanterneRouge.Fresno.Calculations;
using LanterneRouge.Fresno.Calculations.Base;
using LanterneRouge.Fresno.DataLayer.DataAccess.Entities;
using LanterneRouge.Fresno.WpfClient.MVVM;
using System;
using System.Collections.ObjectModel;

namespace LanterneRouge.Fresno.WpfClient.ViewModel
{
    public class LtLogCalculationViewModel : WorkspaceViewModel
    {
        #region Fields

        private readonly IWorkspaceCommands _wsCommands;

        #endregion

        #region Constructors

        public LtLogCalculationViewModel(StepTest category, IWorkspaceCommands mainWorkspaceViewModel)
        {
            Source = category ?? throw new ArgumentNullException(nameof(category));
            _wsCommands = mainWorkspaceViewModel ?? throw new ArgumentNullException(nameof(mainWorkspaceViewModel));
        }

        #endregion

        #region Properties

        internal StepTest Source { get; private set; }

        public int StepTestId => Source.Id;

        public string LTLogLactateThresholdText => LtLogCalculation != null ? $"Load Th.: {LtLogCalculation.LoadThreshold:0.0} Heartrate Th.: {LtLogCalculation.HeartRateThreshold:0}" : "No Calculation";

        private LTLogCalculation _ltLogCalculation = null;
        private LTLogCalculation LtLogCalculation => _ltLogCalculation ?? (_ltLogCalculation = Source.Measurements != null && Source.Measurements.Count > 0 ? new LTLogCalculation(Source.Measurements) : null);

        private ObservableCollection<Zone> _LTLogZones = null;
        public ObservableCollection<Zone> LTLogZones
        {
            get
            {
                if (_LTLogZones == null && Source.Measurements != null && Source.Measurements.Count > 0)
                {
                    if (LtLogCalculation != null)
                    {
                        var z = new PercentOfLTBasedZones(LtLogCalculation, new[] { 0.4, 0.55, 0.75, 0.90, 1.05, 1.2 });
                        _LTLogZones = new ObservableCollection<Zone>(z.Zones);
                    }
                }

                return _LTLogZones;
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

        public override string ToString() => $"LT Log Calculation for Step Test ({StepTestId})";

        #endregion
    }
}
