using Audio.AmbientSubsystem;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using SPT_QuietWind.Patch;
using System;


namespace SPT_QuietWind
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.NAME, PluginInfo.VERSION)]
    public class SPT_QuietWind : BaseUnityPlugin
    {
        public static ConfigEntry<float> WindVolumeMultiplier;
        public static ConfigEntry<float> RainVolumeMultiplier;

        private void Awake()
        {
            WindVolumeMultiplier = Config.Bind("Audio", "Wind volume multiplier", 1.0f, new ConfigDescription("Scales wind ambient volume. Applies when wind condition changes.", new AcceptableValueRange<float>(0f, 1f)));
            RainVolumeMultiplier = Config.Bind("Audio", "Rain volume multiplier", 1.0f, new ConfigDescription("Scales ambient rain volume. Applies when rain condition changes.", new AcceptableValueRange<float>(0f, 1f)));
            
            new WindAmbientBlenderPatch().Enable();
            new PrecipitationAmbientBlenderPatch().Enable();
            new PrecipitationAmbientBlenderPatchLogger().Enable();

            Logger.LogInfo($"[{PluginInfo.GUID} v{PluginInfo.VERSION}] Loaded plugin!");
        }
    }
}
