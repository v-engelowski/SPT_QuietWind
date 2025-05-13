using Audio.AmbientSubsystem;
using BepInEx;
using BepInEx.Configuration;
using SPT_QuietWind.Patch;
using System;


namespace SPT_QuietWind
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.NAME, PluginInfo.VERSION)]
    public class SPT_QuietWind : BaseUnityPlugin
    {
        public static ConfigEntry<float> WindVolumeMultiplier;

        private void Awake()
        {
            WindVolumeMultiplier = Config.Bind("Audio", "Wind volume multiplier", 1.0f, new ConfigDescription("Scales wind ambient volume. Applies when wind condition changes.", new AcceptableValueRange<float>(0f, 2f)));

            new WindAmbientBlenderPatch().Enable();
            Logger.LogInfo($"[{PluginInfo.GUID} v{PluginInfo.VERSION}] Loaded plugin!");
        }
    }
}
