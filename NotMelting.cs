using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BepInEx;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Networking;
using JetBrains.Annotations;

using Assets.Scripts;
using Assets.Scripts.UI;
using Assets.Scripts.Objects;
using Assets.Scripts.Objects.Structures;

using Assets.Scripts.Inventory;
using Assets.Scripts.Objects.Items;
using Objects.Items;

using Assets.Scripts.Util;
using Assets.Scripts.Voxel;
using Assets.Scripts.Atmospherics;
using Assets.Scripts.GridSystem;
using Assets.Scripts.Networking;
using UnityEngine.Rendering;
using Assets.Scripts.Objects.Clothing;
using Assets.Scripts.Objects.Entities;

namespace CreativeFreedom
{
    class NotMelting
    {
        public static bool WillMelt = true;

        [HarmonyPatch(typeof(Suit), "OnAtmosphericTick")]
        public static class FrozenSuit
        {
            [UsedImplicitly]
            public static void Postfix(Suit __instance)
            {
                if (WorldManager.Instance.GameMode == GameMode.Creative)
                {
                    Human human = __instance.RootParent as Human;
                    if (human)
                    {
                        if (__instance.OutputTemperature <= 10f && WillMelt == true)
                        {
                            WillMelt = false;
                            Debug.Log("Cold suit not let ice melt anywhere.");
                        }
                        else if (__instance.OutputTemperature > 10f && WillMelt == false)
                        {
                            WillMelt = true;
                            Debug.Log("Ice is melting again.");
                        }
                    }
                }
            }
        }

        [HarmonyPatch(typeof(Ice), "CanMelt", MethodType.Getter)]
        public static class MotLetMelt
        {
            [UsedImplicitly]
            public static void Prefix(ref bool __result)
            {
                if (WorldManager.Instance.GameMode == GameMode.Creative && WillMelt == false)
                {
                    __result = false;
                }
            }
        }

        [HarmonyPatch(typeof(GameManager), "LeaveGame")]
        public static class OnLeaveTheSavegame
        {
            [HarmonyPostfix]
            public static void CleanHeader()
            {
                NotMelting.WillMelt = true; //reset state
                                            // DangerousMachinery.GetWindyWings.MovingWings.Clear();
            }
        }
    }
}
