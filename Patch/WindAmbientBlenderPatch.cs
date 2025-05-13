using System.Reflection;
using SPT.Reflection.Patching;
using Audio.AmbientSubsystem;
using UnityEngine;


namespace SPT_QuietWeather.Patch
{
    public class WindAmbientBlenderPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod() => typeof(WindAmbientBlender).GetMethod(nameof(WindAmbientBlender.method_7));

        [PatchPrefix]
        static bool Prefix(WindAmbientBlender __instance, ref float __result)
        {
            float clamped = Mathf.Clamp(__instance.method_9(), 0.2f, 1f);
            __result = clamped * QuietWeather.WindVolumeMultiplier.Value;

            return false; // skip original method
        }
    }
}
