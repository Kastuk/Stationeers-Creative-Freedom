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



/*
Thanks to guiding of the TurkeyKittin! 
And other guide of RoboPhred.
And inspiration from DevCo constructions.
And Inaki's exercises!
*/

namespace CreativeFreedom //copy from dnSpy DECOMPILED last version as I lost last changed sources by clicking revert on commit
                          //commit push did got error:
{//Error encountered while pushing to the remote repository: rejected Updates were rejected because the remote contains work that you do not have locally. This is usually caused by another repository pushing to the same ref. You may want to first integrate the remote changes before pushing again.

    #region Structure

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


    [HarmonyPatch(typeof(Structure),nameof(Structure.Awake))]
    
    public static class Structure_Rotation_Unlock
    {
        //[HarmonyPatch("Awake")]
        [UsedImplicitly]//dunno what is for, something for simpler replacing of the field values.
        [HarmonyPostfix]
        public static void LetAnyRotation(Structure __instance)
        {
            if (FreedomConfig.UnlockRotations)
            {
                __instance.RotationAxis = RotationAxis.All; //thanks for Kamuchi for idea of using the named enumerator values
                __instance.AllowedRotations = AllowedRotations.All;
            }
            //TODO key switcher to change precisement of constructions
            //__instance.GridSize = 0.5f; 
            //__instance.PlacementType = PlacementSnap.Grid;
        }
    }

    public static class Structure_Skip_CanConstruct
    {
        [HarmonyPatch("CanConstruct")]
        [UsedImplicitly]//dunno what is for, something for simpler replacing of the field values.
        [HarmonyPostfix]
        public static void SkipCanConstruct(ref CanConstructInfo __result)
        {
            __result = CanConstructInfo.ValidPlacement;
        }
    }
        //[HarmonyPatch(typeof(Structure))]
        //[HarmonyPatch("CanConstructCell")]
        //internal class Structure_Cell_Unlock
        //{
        //    [UsedImplicitly]
        //    [HarmonyPostfix]
        //    private static void LetBuildInAnyCell(ref CanConstructInfo __result)
        //    {
        //        if (FreedomConfig.SkipBlockedGrid)
        //        {
        //            __result = CanConstructInfo.ValidPlacement;
        //        }
        //    }
        //}

    [HarmonyPatch(typeof(CanConstructInfo),nameof(CanConstructInfo.CanConstruct), MethodType.Getter)]
    public static class CanConstructInfo_CanConstruct_Getter
    {
        //[HarmonyPatch("CanConstruct", MethodType.Getter)] //Thaats how to inject into GET methods
        [HarmonyPostfix]
        [UsedImplicitly]
        public static void ReturnCanConstructTrue(ref bool __result)
        {
            __result = true;
        }
    }

    public static class InvalidPlacement_Always_Allowed
    {
        [HarmonyPatch("InvalidPlacement")]
        [HarmonyPostfix]
        [UsedImplicitly]
        public static void CanConstructInfoTrue(ref CanConstructInfo __result)
        {

            __result = new CanConstructInfo(true, string.Empty); //THANKS to proud2belamer (sth64) for this addition to reanimate this mod!
            return;
        }
    }

    
    //NOT WORK, still error at load and removing of colliding structures!!!

    //THIS MUST disable autoremoving (at game load) of collising frames and other full-grid things, like merged cladding.
    //Savegames with such collising structures still will autoremove things without the mod!
    //[HarmonyPatch(typeof(GridController),nameof(GridController.IsBlockedGrid), new Type[]{typeof(WorldGrid)})]

    //public static class AllowBlockedBigGrid
    //    {  
    //    [UsedImplicitly]
    //    [HarmonyPostfix]
    //    public static void UnblockeBigGrid(ref bool __result)
    //    {
    //        if (FreedomConfig.SkipBlockedGrid)
    //        {
    //            __result = false;
    //        }
    //    }
    //}
    //[HarmonyPatch(typeof(Structure), "CanConstruct")]
    //internal class Structure_CanConstruct_Patch
    //{
    //    private static void Postfix(ref CanConstructInfo __result)
    //    {
    //       __result = CanConstructInfo.ValidPlacement;
    //    }
    //}
    //[HarmonyPatch(typeof(Structure), "CanConstructCell")]
    // [UsedImplicitly]
    //  internal class Structure_CanConstructCell_Patch
    //{
    //    private static void Postfix(ref CanConstructInfo __result)
    //    {
    //        __result = CanConstructInfo.ValidPlacement;
    //    }
    //}

   


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

    [HarmonyPatch(typeof(SmallGrid),nameof(SmallGrid._IsCollision))]

    public static class SmallGrid_isCollision_False
    {
        //[HarmonyPatch("_IsCollision")]
        [UsedImplicitly]
        public static void Postfix(ref bool __result)
        {
            __result = false;
            //return false;
        }
    }

    public static class SmallGrid_HasFrameBelow_True
    {
        [HarmonyPatch("HasFrameBelow")]
        [UsedImplicitly]
        public static void Postfix(ref bool __result)
        {
            __result = true;
        }
    }

    public static class SmallGrid_HasVoxelBelow_True
    {
        [HarmonyPatch("HasVoxelBelow")]
        [UsedImplicitly]
        public static void Postfix(ref bool __result)
        {
            __result = true;
        }
    }


    internal class SmallGrid_CanMountOnWall_Valid
    {
        [HarmonyPatch("CanMountOnWall")]
        [UsedImplicitly]
        private static void Postfix(ref CanMountResult __result)
        {
            __result.result = WallMountResult.Valid;
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

   // #region Devices
    //[HarmonyPatch(typeof(Airlock), "CanConstruct")]
    //internal class Airlock_CanConstruct_Patch
    //{
    //    private static void Postfix(ref bool __result)
    //    {
    //        __result = true; //evading of CheckSidesBlocked
    //    }
    //}
  //  #endregion Devices

   // #region DynamicThing

  //  #endregion DynamicThing


   // #region Items

  //  #endregion Items



    //#region Mothership //no more Motherships T-T
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
    //#endregion

  //  #region Entity

  //  #endregion Entity
}
