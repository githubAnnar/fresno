using LanterneRouge.Fresno.WpfClient.MVVM;

namespace LanterneRouge.Fresno.WpfClient.UserControls.ViewModel
{
    public class ZoneSettingViewModel : ViewModelBase
    {
        private double _zoneValue = 0d;
        private string _zoneName = "Zone";

        public double ZoneValue { get => _zoneValue; set { _zoneValue = value; OnPropertyChanged(); } }

        public string ZoneName { get => _zoneName; set { _zoneName = value; OnPropertyChanged(); } }

        public int Maximum { get; set; } = 20;

        public double TickFrequency { get; set; } = 1d;
    }
}
