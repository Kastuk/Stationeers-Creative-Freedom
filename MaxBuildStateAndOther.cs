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
using UnityEngine.Bindings;

using TMPro;
using UI;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UI;

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



/*
Thanks to guiding of the TurkeyKittin! 
And other guide of RoboPhred.
And inspiration from DevCo constructions.
And Inaki's exercises!
*/

namespace CreativeFreedom
{
    [HarmonyPatch(typeof(InventoryManager), "UsePrimaryComplete")]
    public class MaxBuildState
    {
        [UsedImplicitly]
        public static void Postfix()
        {
            if (InventoryManager.IsAuthoringMode && FreedomConfig.MaxBuildState == true)
            {
                Structure str = Structure.LastCreatedStructure; //THanks to inaki for tip
                int bstate = str.BuildStates.Count - 1;
                str.UpdateBuildStateAndVisualizer(bstate);
                //str.SetCustomColor(AuthorToolColorKeys.colorIndex);
            }
        }
    }
}
    //ADD change color of structure by color of Authoring Tool (switch by keys)
    //[HarmonyPatch(typeof(InventoryManager), "ManagerUpdate")]
    //public class AuthorToolColorKeys
    //{
    //    public static int colorIndex;

    //    [UsedImplicitly]
    //    public static void Postfix()
    //    {
    //        if (InventoryManager.Parent != null)
    //        {
    //            Human hum = InventoryManager.Parent as Human;
    //            if (hum && InventoryManager.IsAuthoringMode)
    //            {
    //                DynamicThing tool = InventoryManager.Instance.ActiveHand.Slot.Occupant;

    //                if (KeyManager.GetButton(KeyCode.RightControl))//Input.GetKeyDown(KeyMap.))
    //                {
    //                    if (KeyManager.GetButtonUp(KeyCode.Plus))
    //                    { tool.SetCustomColor(tool.CustomColor.Index + 1); }
    //                    if (KeyManager.GetButtonUp(KeyCode.Minus))
    //                    { tool.SetCustomColor(tool.CustomColor.Index - 1); }
    //                    colorIndex = tool.CustomColor.Index;
    //                    Debug.Log("Tool color is " + tool.CustomColor.DisplayName);
    //                }
    //            }
    //        }
    //    }
    //}
    // CHange placementsnap mode for smallgridd mountable things to place switchers on tables
    //while GetButton is pressed
//    [HarmonyPatch(typeof(InventoryManager), "PlacementMode")]
//    public class PlacementTypeMemo
//    {
//        public static PlacementSnap def;

//        [UsedImplicitly]
//        public static void Prefix()
//        {
//            if (InventoryManager.ConstructionCursor)
//            {
//               def = InventoryManager.ConstructionCursor.PlacementType;
//            }
//        }
//    }

//    [HarmonyPatch(typeof(InventoryManager), "ManagerUpdate")]//PlacementMode")]
//    public class PlacementSwitcher
//    {
//        public static bool swit = false;

//        [UsedImplicitly]
//        public static void Postfix()
//        {
//            if (InventoryManager.ConstructionCursor != null)
//            {
//                if (KeyManager.GetButtonUp(KeyCode.Z))
//                {
//                    swit = !swit;
//                }
//            }
//        }
//    }
//    [HarmonyPatch(typeof(InventoryManager), "UpdatePlacement")]
//    [HarmonyPatch(new Type[] { typeof(Constructor) })]
//    public class ChangePlacementSnapConstructor
//    {
//        [UsedImplicitly]
//        public static void Prefix()
//        {
//            //MultiConstructor mult = structure as MultiConstructor;
//            //if(mult)
//            if (InventoryManager.ConstructionCursor != null)
//            {
//                if (PlacementSwitcher.swit)
//                {
//                    InventoryManager.ConstructionCursor.PlacementType = PlacementSnap.Grid;
//                }
//                if (!PlacementSwitcher.swit)
//                {
//                    InventoryManager.ConstructionCursor.PlacementType = PlacementTypeMemo.def;
//                }
//            }
//        }
//    }

//    [HarmonyPatch(typeof(InventoryManager), "UpdatePlacement")]
//    [HarmonyPatch(new Type[] { typeof(Structure) })]
//    public class ChangePlacementSnapStructure
//    {
//        [UsedImplicitly]
//        public static void Prefix(ref Structure structure)
//        {
//            //MultiConstructor mult = structure as MultiConstructor;
//            //if(mult)
//            if (structure != null)
//            {
//                if (PlacementSwitcher.swit)
//                {
//                    structure.PlacementType = PlacementSnap.Grid;
//                }
//                if (!PlacementSwitcher.swit)
//                {
//                    structure.PlacementType = PlacementTypeMemo.def;
//                }
//            }
//        }
//    }
//}

    //[HarmonyPatch(typeof(Constructor), "SpawnConstruct"]
    //internal class MaxBuildState2
    //{
    //    [UsedImplicitly]
    //    public static bool Prefix (ref CreateStructureInstance instance)
    //    {
    //        if (GameManager.RunSimulation)
    //        {
    //            Thing.Create<Structure>(instance.Prefab, instance.WorldPosition, instance.WorldRotation, 0L).SetStructureData(instance.LocalRotation, instance.OwnerClientId, instance.LocalGrid, instance.CustomColor);
    //            Structure struc = Structure.LastCreatedStructure;
    //            foreach (var chel in Human.AllHumans) //catch idea of search in lists from liz's AtomicBatteryPatch
    //            {
    //                if (chel.OwnerClientId == struc.OwnerClientId)
    //                {
    //                    if(chel.LeftHandSlot.Occupant as AuthoringTool || chel.RightHandSlot.Occupant as AuthoringTool)
    //                    {
    //                        //too long
    //                    }
    //                }

    //                    return false;
    //        }
    //        if (Assets.Scripts.Networking.NetworkManager.IsClient)
    //        {
    //            new ConstructionCreationMessage(instance).SendToServer();
    //        }
    //    }

    //[HarmonyPatch(typeof(Constructor), "Construct")]
    //[HarmonyPatch(new Type[] { typeof(Grid3), typeof(Quaternion), typeof(bool), typeof(ulong), typeof(Mothership) })] //need args of particular construct method
    //internal class MaxBuildStage
    //{
    //    //let buildstate become max at spawn
    //    //[HarmonyPrefix]
    //    [UsedImplicitly]
    //    public virtual bool Prefix(Constructor __instance, ref Grid3 localPosition, ref Quaternion targetRotation, ref bool authoringMode, ref ulong steamId, ref Mothership mothership = null)
    //    {
    //        if(!authoringMode)
    //        {
    //            return true;
    //        }

    //        CreateStructureInstance createStructureInstance = mothership ? new CreateStructureInstance(__instance.BuildStructure, mothership, localPosition, targetRotation, steamId, -1) : new CreateStructureInstance(__instance.BuildStructure, localPosition, targetRotation, steamId, -1);
    //        if (__instance.PaintableMaterial != null && __instance.CustomColor.Normal != null)
    //        {
    //            createStructureInstance.CustomColor = __instance.CustomColor.Index;
    //        }
    //        Constructor.SpawnConstruct(createStructureInstance);
    //        //change BuildState somehow...
    //        return false;
    //    }
    //}


//        private static void prefix(ref multiconstructor __instance, int optionindex)
//        {
//            __instance.constructables[optionindex].currentbuildstateindex = __instance.constructables[optionindex].buildstates.count - 1;
//        }

//    }
//public virtual void Construct(Grid3 localPosition, Quaternion targetRotation, bool authoringMode, ulong steamId, Mothership mothership = null)
//}
