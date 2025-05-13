using System.Reflection;
using SPT.Reflection.Patching;
using Audio.AmbientSubsystem;
using UnityEngine;


namespace SPT_QuietWind.Patch
{
    public class WindAmbientBlenderPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod() => typeof(WindAmbientBlender).GetMethod(nameof(WindAmbientBlender.method_7));

        [PatchPrefix]
        static bool Prefix(WindAmbientBlender __instance, ref float __result)
        {
            float windVolumenMult = SPT_QuietWind.WindVolumeMultiplier.Value;
            float clamped = Mathf.Clamp(__instance.method_9(), 0.2f, 1f);

            if (windVolumenMult != 1f) Logger.LogInfo($"[{PluginInfo.GUID} v{PluginInfo.VERSION}] Wind volume multiplied by {windVolumenMult}");

            __result = clamped * windVolumenMult;
            return false; // skip original method
        }
    }
}
