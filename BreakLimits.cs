﻿using System;
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



/*
Thanks to guiding of the TurkeyKittin! 
And other guide of RoboPhred.
And inspiration from DevCo constructions.
And Inaki's exercises!
*/

namespace CreativeFreedom
{

    #region Structure

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
    //[HarmonyPatch(typeof(InventoryManager), "PlacementMode")]
    //internal class InventoryManager_PlacementMode_Patch
    //{
    //    private static void Prefix()
    //    {
    //        //  InventoryManager.ConstructionCursor.CanConstruct();
    //        if (WorldManager.Instance.GameMode == GameMode.Creative && KeyManager.GetButton(KeyMap.QuantityModifier))
    //        {
    //            InventoryManager.ConstructionCursor.RotationAxis = RotationAxis.All;
    //            InventoryManager.ConstructionCursor.AllowedRotations = AllowedRotations.All;
    //            InventoryManager.ConstructionCursor.StructureCollisionType = CollisionType.BlockCustom;
    //            InventoryManager.ConstructionCursor.AllowMounting = true;
    //          //InventoryManager.ConstructionCursor._hasSpawnedWreckage = false;
    //        }


    //    }
    //}
    [HarmonyPatch(typeof(CanConstructInfo), "CanConstruct", MethodType.Getter)]
    internal class CanConstructInfo_CanConstruct_Getter
    {
        private static void Postfix(ref bool __result)
        {
            __result = true;
        }
    }

    [HarmonyPatch(typeof(Structure), "CanConstruct")]
    internal class Structure_CanConstruct_Patch
    {
        private static void Postfix(ref CanConstructInfo __result)
        {
            __result = CanConstructInfo.ValidPlacement;
        }
    }
    [HarmonyPatch(typeof(Structure), "CanConstructCell")]
    [UsedImplicitly]
    internal class Structure_CanConstructCell_Patch
    {
        private static void Postfix(ref CanConstructInfo __result)
        {
            __result = CanConstructInfo.ValidPlacement;
        }
    }

    [HarmonyPatch(typeof(CanConstructInfo), "InvalidPlacement")]
    public class InvalidPlacement_Always_Allowed
    {
        //[HarmonyPostfix]
        [UsedImplicitly]
        public static void Postfix(ref CanConstructInfo __result)
        {

            __result = new CanConstructInfo(true, string.Empty); //THANKS to proud2belamer (sth64) for this addition to reanimate this mod!
            return;
        }
    }


    #endregion Structure

    #region SmallGrid

    //[HarmonyPatch(typeof(SmallGrid), "CanConstruct")]
    //internal class SmallGrid_CanConstruct_Patch
    //{
    //    private static void Postfix(ref CanConstructInfo __result)
    //    {
    //        __result = CanConstructInfo.ValidPlacement;
    //    }
    //}

    [HarmonyPatch(typeof(SmallGrid), "_IsCollision")]
    internal class SmallGrid_isCollision_Patch
    {
        private static bool Prefix()
        {
            return false;
        }
    }

    [HarmonyPatch(typeof(SmallGrid), "HasFrameBelow")]
    internal class SmallGrid_HasFrameBelow_Patch
    {
        private static void Postfix(ref bool __result)
        {
            __result = true;
        }
    }

    [HarmonyPatch(typeof(SmallGrid), "HasVoxelBelow")]
    internal class SmallGrid_HasVoxelBelow_Patch
    {
        private static void Postfix(ref bool __result)
        {
            __result = true;
        }
    }

    [HarmonyPatch(typeof(SmallGrid), "CanMountOnWall")]
    internal class SmallGrid_CanMountOnWall_Patch
    {
        private static void Postfix(ref CanMountResult __result)
        {
            __result.result = WallMountResult.Valid; //fix for 27/09/2023
        }
        //thanks to the TurkeyKittin.
    }

    //[HarmonyPatch(typeof(MountedSmallGrid), "CanConstruct")]
    //internal class MountedSmallGrid_CanConstruct_Patch
    //{
    //    private static void Postfix(ref bool __result)
    //    {
    //        __result = true;
    //    }
    //}
    #endregion SmallGrid

    #region Devices
    //[HarmonyPatch(typeof(Airlock), "CanConstruct")]
    //internal class Airlock_CanConstruct_Patch
    //{
    //    private static void Postfix(ref bool __result)
    //    {
    //        __result = true; //evading of CheckSidesBlocked
    //    }
    //}
    #endregion

    #region DynamicThing

    #endregion DynamicThing


    #region Items




    #endregion



    #region Mothership
    ////Need to unlock fixed max angle and remove autostop anchoring at game load
    //[HarmonyPatch(typeof(Mothership), "FixedUpdateEachFrame")]
    //internal class Mothership_OnThreadUpdate_Patch
    //{
    //    [UsedImplicitly]
    //    private static void Prefix(Mothership __instance, ref bool ____isRotationDeviated)
    //    {
    //        ____isRotationDeviated = false;
    //    }
    //}
    #endregion

    #region Entity

    #endregion Entity
}
