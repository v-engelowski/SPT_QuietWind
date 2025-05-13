using Audio.AmbientSubsystem;
using BepInEx;
using BepInEx.Configuration;
using SPT_QuietWeather.Patch;
using System;


namespace SPT_QuietWeather
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.NAME, PluginInfo.VERSION)]
    public class QuietWeather : BaseUnityPlugin
    {
        public static ConfigEntry<float> WindVolumeMultiplier;

        private void Awake()
        {
            WindVolumeMultiplier = Config.Bind("Audio", "Wind volume multiplier", 1.0f, new ConfigDescription("Scales wind ambient volume.", new AcceptableValueRange<float>(0f, 2f)));
            WindVolumeMultiplier.SettingChanged += OnWindVolumeChanged;

            new WindAmbientBlenderPatch().Enable();
            Logger.LogInfo($"[{PluginInfo.GUID} v{PluginInfo.VERSION}] Loaded plugin!");
        }

        private void OnWindVolumeChanged(object sender, EventArgs e)
        {
            var blender = FindObjectOfType<WindAmbientBlender>();

            if (blender != null)
            {
                blender.method_3();
                Logger.LogInfo($"[{PluginInfo.GUID} v{PluginInfo.VERSION}] Wind volume set!");
            }
            else
            {
                Logger.LogInfo($"[{PluginInfo.GUID} v{PluginInfo.VERSION}] Wind volume not set, no WindAmbientBlender (are you in a raid?)");
            }
        }
    }
}
