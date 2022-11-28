using System;

namespace LanterneRouge.Fresno.WpfClient.Preferences.ViewModel
{
    public class LactateZonePreferencesViewModel
    {
        public LactateZonePreferencesViewModel(Action close)
        {
            Close = close;
        }

        public Action Close { get; }
    }
}
