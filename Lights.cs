using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

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

using Assets.Scripts.Objects.Entities;

namespace CreativeFreedom
{
    class Lights
    {
        [HarmonyPatch(typeof(Headlamp), nameof(Headlamp.SetCustomColor))]
        public static class ColoredLight
        {
            //[HarmonyPatch(nameof(StackableLight.Awake))]
            [UsedImplicitly]
            [HarmonyPostfix]
            public static void ColoredHeadSpotLight(Headlamp __instance)
            {
                if (FreedomConfig.ColoredLight)
                {
                    foreach (ThingLight thingLight in __instance.Lights)
                    {
                        thingLight.Light.color = __instance.CustomColor.Light;
                    }
                }
            }
        }



        [HarmonyPatch(typeof(CameraController), nameof(CameraController.ManagerAwake))]
        public static class WhiteNV
        {
            //[HarmonyPatch(nameof(CameraController.SetNightVision))]
            //[HarmonyPatch(nameof(StackableLight.Awake))]
            [UsedImplicitly]
            [HarmonyPostfix]
            public static void WhiteNVision()
            { 
                if (WorldManager.Instance.GameMode == GameMode.Creative && FreedomConfig.NVLight)
                {
                    var nvlight = CameraController.Instance.NightVisionLight.gameObject.GetComponent<Light>();
                    nvlight.color = Color.white;
                }
            }
        }

        [HarmonyPatch(typeof(Human),nameof(Human.ToggleNightVision))]
        public static class CreativeNightVision1
        {
            //[HarmonyPatch(nameof(StackableLight.Awake))]
            [UsedImplicitly]
            [HarmonyPrefix]
            public static bool AnyNVision(Human __instance)
            {
                if (WorldManager.Instance.GameMode == GameMode.Creative && FreedomConfig.NVLight)
                {
                    if (__instance.IsUnresponsive || __instance.IsSleeping)
                    {
                        return false;
                    }
                    CreativeNightVision3.CleanLight(!Human.CurrentlyUsingNightVision);
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(Human), "UpdateNightVision")] //private now?
        public static class CreativeNightVision2
        {
            //[HarmonyPatch(nameof(StackableLight.Awake))]
            [UsedImplicitly]
            [HarmonyPrefix]
            public static bool NoUpdateNVision(Human __instance)
            {
                if (WorldManager.Instance.GameMode == GameMode.Creative && FreedomConfig.NVLight)
                {
                    return false;
                }
                return true;
            }
        }

        public class CreativeNightVision3
        {
            public static void CleanLight(bool show)
            {
                if (GameManager.IsBatchMode)
                {
                    return;
                }
                //LeanTween.cancel(CameraController.Instance.gameObject);
               

                if (show)
                {
                    // CameraController.Instance.NightVisionFX.Preset = CameraFilterPack_NightVisionFX.preset.Night_Vision_Full;
                    // CameraController.Instance.NightVisionFX.enabled = true;
                    CameraController.Instance.NightVisionLight.gameObject.SetActive(true);
                    //CameraController.Instance.NightVisionFX.OnOff = 0f;
                }
                else
                {
                    CameraController.Instance.NightVisionLight.gameObject.SetActive(false);
                }
                Human.CurrentlyUsingNightVision = show;
            }
        }
                  //  return false;
                //}
               // return true;
           // }
        //}


        //[HarmonyPatch(typeof(RoadFlare),nameof(RoadFlare.Awake))]
        //public static class CreativeRoadFlare
        //{
        //    //[HarmonyPatch(nameof(RoadFlare.Awake))] //way to evade misprints at strings of method names
        //    [UsedImplicitly]
        //    [HarmonyPostfix]
        //    public static void LongBurn(RoadFlare __instance)
        //    {
        //        if (WorldManager.Instance.GameMode == GameMode.Creative)
        //        {
        //            __instance._actualLifetime = 5000f;
        //        }
        //    }
        //}
        //not sure why cannot change same for ChemLight
        //[HarmonyPatch(typeof(StackableLight),nameof(StackableLight.Awake))]
        //public static class CreativeAllStackingLights
        //{
        //    //[HarmonyPatch(nameof(StackableLight.Awake))]
        //    [UsedImplicitly]
        //    [HarmonyPostfix]
        //    public static void LongBurn(StackableLight __instance)
        //    {
        //        if (WorldManager.Instance.GameMode == GameMode.Creative)
        //        {
        //            //__instance._actualLifetime = 5000f;
        //            typeof(StackableLight).GetField("_actualLifetime", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(__instance, 5000f);

        //            __instance.ActualLifetime = 5000f;
        //        }
        //    }
        //}
    }
}
