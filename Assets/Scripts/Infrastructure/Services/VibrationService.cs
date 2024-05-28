using Client.Data.Core;
using Lofelt.NiceVibrations;

namespace Client.Infrastructure.Services
{
    public class VibrationService
    {
        private SharedData _data;

        public VibrationService(SharedData data)
        {
            _data = data;
        }

        public void Vibrate(NiceHaptic.PresetType HapticPreset)
        {
            if (_data.PlayerData.IsVibrationOn)
                NiceHaptic.PlayPreset(HapticPreset);
        }
    }
}