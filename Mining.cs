using System;
using Assets.Scripts.Objects.Items;
using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;

namespace CreativeFreedom
{
    class Mining
    {
        [HarmonyPatch(typeof(MiningDrill), "OnUsePrimary")]
        internal class MiningDrill_Speedup
        {
            [UsedImplicitly]
            private static void Prefix(MiningDrill __instance)
            {
                float time = Mathf.Clamp(FreedomConfig.MineCompletionTime, 0.09f, 0.5f);
                __instance.MineCompletionTime = time;
            }
        }
    }
}
