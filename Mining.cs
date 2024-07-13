using System;
using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;

using Assets.Scripts.Inventory;
using Assets.Scripts.Localization2;
using Assets.Scripts;

using Assets.Scripts.Objects;
using Assets.Scripts.Objects.Items;

using System.Globalization;
using System.Text.RegularExpressions;
using Assets.Scripts.Networking;
using Assets.Scripts.UI;
using Assets.Scripts.Util;

using Assets.Scripts.Atmospherics;
using Assets.Scripts.GridSystem;
using Assets.Scripts.Networking;
using Assets.Scripts.Objects.Electrical;
using Assets.Scripts.Objects.Pipes;
using Assets.Scripts.Objects.Motherboards;

using Assets.Scripts.Objects.Entities;

using Assets.Scripts.Serialization;
using Assets.Scripts.Util;

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
                __instance.MineCompletionTime = FreedomConfig.MineCompletionTime.Value;
                //float time = Mathf.Clamp(__instance.MineCompletionTime, 0.08f, 0.5f);
                //__instance.MineCompletionTime = time;
            }
        }

    }

}
