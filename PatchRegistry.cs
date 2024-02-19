using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using Assets.Code;
using UnityEngine;
using UnityEngine.UI;
using Action = Assets.Code.Action;

namespace UIImprovements
{
    public class PatchRegistry
    {
        static string harmonyId = "com.mf.sofg_ui_improvements";

        static List<MethodInfo> patchedMethods = new List<MethodInfo>();

        public static void PatchMethod(MethodInfo original, MethodInfo prefix, MethodInfo postfix) {
            var harmony = new HarmonyLib.Harmony(harmonyId);
            harmony.Patch(original, prefix != null ? new HarmonyLib.HarmonyMethod(prefix) : null, postfix != null ? new HarmonyLib.HarmonyMethod(postfix) : null);
            patchedMethods.Add(original);
        }

        public static void RevertAllPatches() {
            var harmony = new HarmonyLib.Harmony(harmonyId);
            foreach (var method in patchedMethods) {
                harmony.Unpatch(method, HarmonyLib.HarmonyPatchType.All, harmonyId);
            }
            patchedMethods.Clear();
        }
        
    }
}
