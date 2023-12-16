using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using BepInEx;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Networking;
using JetBrains.Annotations;

using Assets.Scripts;
using Assets.Scripts.UI;
using Assets.Scripts.Objects;
using Assets.Scripts.Objects.Structures;

using Assets.Scripts.GridSystem;
using Assets.Scripts.Networking;

using Assets.Scripts.Objects.Electrical;
using Assets.Scripts.Objects.Entities;

using Assets.Scripts.Inventory;

using Assets.Scripts.Util;
using Objects.Items;
using Assets.Scripts.Objects.Items;

namespace CreativeFreedom
{
    //public class Dictionaries
    //{
    //    public static Dictionary<string, RotationAxis> RotAxisLimits = new Dictionary<string, RotationAxis>();
    //    public static Dictionary<string, AllowedRotations> RotAllowLimits = new Dictionary<string, AllowedRotations>();
    //}

    //[HarmonyPatch(typeof(Stationpedia), "PopulateLists")] //mmaybe use Prefab.Register to take every thing as Structure
    //internal class MemorizeAllStructuresLimits
    //{
    //    [UsedImplicitly]//dunno what is for, something for simpler replacing of the field values.
    //    [HarmonyPrefix]
    //    private static void TakeAllStructures()
    //    {
    //        //Structure str = sourcePrefab as Structure;
    //        //if (str != null)
    //        //{
    //        Dictionaries.RotAxisLimits = null;
    //        Dictionaries.RotAllowLimits = null;

    //        foreach (Structure str in Structure.AllStructurePrefabs)
    //        {
    //            try
    //            {
    //                Dictionaries.RotAxisLimits.Add(str.PrefabName, str.RotationAxis);
    //                Dictionaries.RotAllowLimits.Add(str.PrefabName, str.AllowedRotations);
    //            }
    //            catch (Exception e)
    //            {
    //                Debug.Log("CF. Filling dictionary of limits is failed. " + e.Message);
    //            }
    //        }
    //    }
    //}


    //    }
    //}
    //if (Dictionaries.RotAxisLimits != null && Dictionaries.RotAllowLimits != null)
    //{
    //    int count = Dictionaries.RotAxisLimits.Count();
    //    Debug.Log("CF. Dicts of rotation limits is filled. " + count + " structures is here.");
    //}
    //Structure orig = new Structure
    //{
    //    RotationAxis = __instance.RotationAxis,
    //    AllowedRotations = __instance.AllowedRotations
    //};

    //if (FreedomConfig.UnlockRotations)
    //{
    //    __instance.RotationAxis = Assets.Scripts.Objects.RotationAxis.All; //thanks for Kamuchi for idea of using the named enumerator values
    //    __instance.AllowedRotations = Assets.Scripts.Objects.AllowedRotations.All;
    //}
    //if (!FreedomConfig.UnlockRotations)
    //{
    //    if (__instance.RotationAxis == RotationAxis.All && __instance.AllowedRotations == AllowedRotations.All)
    //    {
    //        __instance.RotationAxis = Dictionaries.RotAxisLimits[__instance.PrefabName];
    //        __instance.AllowedRotations = Dictionaries.RotAllowLimits[__instance.PrefabName];
    //    }
    //}

    //TODO key switcher to change precisement of constructions
    //__instance.GridSize = 0.5f; 
    //__instance.PlacementType = PlacementSnap.Grid; //better to use InventoryManager.PlacementMode there

//    [HarmonyPatch(typeof(InventoryManager), "UpdatePlacement")]
//    [HarmonyPatch(new Type[] { typeof(Structure) })]
//    public class RotLimitsReplace
//    {
//        [UsedImplicitly]
//        public static void Prefix(ref Structure __instance)
//        {
//            if (FreedomConfig.UnlockRotations)
//            {
//                __instance.RotationAxis = RotationAxis.All;
//                __instance.AllowedRotations = AllowedRotations.All;
//            }
//            //if (!FreedomConfig.UnlockRotations)
//            //{
//            //    try
//            //    {
//            //        Debug.Log("CF. Reverting rotation limits for " + __instance.DisplayName);
//            //        __instance.RotationAxis = Dictionaries.RotAxisLimits[__instance.PrefabName];
//            //        __instance.AllowedRotations = Dictionaries.RotAllowLimits[__instance.PrefabName];
//            //    }
//            //    catch (Exception e)
//            //    {
//            //        Debug.Log("CF. Problem: " + e);
//            //    }
//            //}
//        }
//    }

//}
//simple old rigid replacing
[HarmonyPatch(typeof(Structure), "Awake")]
    //[HarmonyPatch(new Type[] { typeof(Structure) })]
    public class StructuresRotLimitsReplace
    {
        [UsedImplicitly]
        public static void Postfix(Structure __instance)
        {
            if (FreedomConfig.UnlockRotations)
            {
                __instance.RotationAxis = RotationAxis.All;
                __instance.AllowedRotations = AllowedRotations.All;
            }
        }
    }
}

        //[HarmonyPatch(typeof(InventoryManager), "PlacementMode")]
        //public class RotationsSwitch
        //{
        //    [UsedImplicitly]
        //    public static void Prefix()
        //    {
        //        if (InventoryManager.ConstructionCursor)
        //        {
        //            if (FreedomConfig.UnlockRotations)
        //            {
        //                InventoryManager.ConstructionCursor.RotationAxis = Assets.Scripts.Objects.RotationAxis.All; //thanks to Kamuchi for idea of using the named enumerator values
        //                InventoryManager.ConstructionCursor.AllowedRotations = Assets.Scripts.Objects.AllowedRotations.All;
        //            }
        //            if (!FreedomConfig.UnlockRotations)
        //            {
        //                //if (InventoryManager.ConstructionCursor.RotationAxis == RotationAxis.All && InventoryManager.ConstructionCursor.AllowedRotations == AllowedRotations.All)
        //                //{
        //                try
        //                {
        //                    Debug.Log("CF. Reverting rotation limits for " + InventoryManager.ConstructionCursor.DisplayName);
        //                    InventoryManager.ConstructionCursor.RotationAxis = Dictionaries.RotAxisLimits[InventoryManager.ConstructionCursor.PrefabName];
        //                    InventoryManager.ConstructionCursor.AllowedRotations = Dictionaries.RotAllowLimits[InventoryManager.ConstructionCursor.PrefabName];
        //                }//}
        //                catch (Exception e)
        //                {
        //                    Debug.Log("CF. Problem: " + e);
        //                }
        //            }
        //        }
        //    }


//if (KeyManager.GetButton(KeyMap.QuantityModifier))
//{
//    try
//    {
//        SmartRotate.GetNext(InventoryManager.ConstructionCursor as ISmartRotatable, Quaternion.identity);
//    }
//    catch (System.IndexOutOfRangeException)//Exception e)
//    {
//        Debug.Log(InventoryManager.ConstructionCursor.DisplayName + " is at abnormal angle for SmartRotation. Reset rotation.");
//        InventoryManager.ConstructionCursor.Transform.rotation = Quaternion.identity;
//        //InventoryManager.ConstructionCursor.ThingTransform.rotation = InventoryManager.CurrentRotation;
//    }
//}

//var defaxis = InventoryManager.ConstructionCursor.RotationAxis;
//InventoryManager.ConstructionCursor.RotationAxis = RotationAxis.All;

//if (KeyManager.GetButton(KeyMap.QuantityModifier) && defaxis != RotationAxis.All)
//{
//    InventoryManager.ConstructionCursor.ThingTransform.forward);
//}
