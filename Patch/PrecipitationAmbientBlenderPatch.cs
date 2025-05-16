using System.Reflection;
using SPT.Reflection.Patching;
using Audio.AmbientSubsystem;
using UnityEngine;
using HarmonyLib;
using System.Collections.Generic;
using BepInEx.Configuration;
using System.Reflection.Emit;
using System;


namespace SPT_QuietWind.Patch
{
    public class PrecipitationAmbientBlenderPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            MethodInfo methodInfo = typeof(PrecipitationAmbientBlender).GetMethod(nameof(PrecipitationAmbientBlender.method_2), BindingFlags.Instance | BindingFlags.Public, null, new Type[] { typeof(float), typeof(float) }, null);

            if (methodInfo == null) Logger.LogError($"[{PluginInfo.GUID} v{PluginInfo.VERSION}] No method found");

            return methodInfo;
        }

        [PatchTranspiler]
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
            MethodInfo startFade = AccessTools.Method(typeof(GInterface89), "StartCrossfade", new System.Type[] { typeof(float), typeof(float), typeof(float), typeof(float), typeof(bool), typeof(Action) });
            MethodInfo getSingle0 = AccessTools.Property(typeof(PrecipitationAmbientBlender), nameof(PrecipitationAmbientBlender.Single_0)).GetGetMethod();

            FieldInfo configField = AccessTools.Field(typeof(SPT_QuietWind), nameof(SPT_QuietWind.RainVolumeMultiplier));
            MethodInfo getConfigValue = AccessTools.Property(typeof(ConfigEntry<float>), nameof(ConfigEntry<float>.Value)).GetGetMethod();

            for (int i = 0; i < codes.Count; i++)
            {
                var ci = codes[i];

                // look for call to "StartCrossfade"
                if (ci.opcode == OpCodes.Callvirt && ci.operand as MethodInfo == startFade)
                {
                    // look for previous call to "getSingle0"
                    for (int j = i - 1; j >= 0; j--)
                    {
                        if ((codes[j].opcode == OpCodes.Callvirt || codes[j].opcode == OpCodes.Call) && codes[j].operand as MethodInfo == getSingle0)
                        {
                            // insert our volume multiplier after the call
                            codes.InsertRange(j + 1, new[]
                            {
                                new CodeInstruction(OpCodes.Ldsfld, configField),
                                new CodeInstruction(OpCodes.Callvirt, getConfigValue),
                                new CodeInstruction(OpCodes.Mul),
                            });

                            // skip some calls, so we dont patch our already patched method
                            i += 3;
                            break;
                        }
                    }
                }
            }

            return codes;
        }
    }

    public class PrecipitationAmbientBlenderPatchLogger : ModulePatch
    {
        protected override MethodBase GetTargetMethod() => typeof(PrecipitationAmbientBlender).GetMethod(nameof(PrecipitationAmbientBlender.method_2));

        [PatchPrefix]
        static void Prefix()
        {
            Logger.LogInfo($"[{PluginInfo.GUID} v{PluginInfo.VERSION}] Rain volume has been multiplied by {SPT_QuietWind.WindVolumeMultiplier.Value}");
        }
    }
}
