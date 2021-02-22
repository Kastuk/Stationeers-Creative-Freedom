using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BepInEx;
using HarmonyLib;
using UnityEngine;
using JetBrains.Annotations;

using Assets.Scripts;
using Assets.Scripts.Objects;
using Assets.Scripts.Objects.Structures;

using Assets.Scripts.Objects.Items;

/*
Thanks to guiding of the TurkeyKittin! 
And other guide of RoboPhred.
And inspiration from DevCo constructions.
*/

namespace StationeersCreativeFreedom
{

    #region Structure

    [HarmonyPatch(typeof(Structure), "Awake")]
    internal class Structure_Awake_Patch
    {
        [UsedImplicitly]//dunno what is for, something for simpler replacing of the field values.
        private static void Postfix(Structure __instance)
        {
            __instance.RotationAxis = RotationAxis.All;
            __instance.AllowedRotations = AllowedRotations.All;

            //TODO key switcher to change precisement of constructions
            //__instance.GridSize = 0.5f; 
            //__instance.PlacementType = PlacementSnap.Grid;
        }
    }

    [HarmonyPatch(typeof(Structure), "CanConstruct")]
    internal class Structure_CanConstruct_Patch
    {
        private static void Postfix(ref bool __result)
        {
            __result = true;
        }
    }

    #endregion Structure

    #region SmallGrid

    [HarmonyPatch(typeof(SmallGrid), "CanConstruct")]
    internal class SmallGrid_CanConstruct_Patch
    {
        private static void Postfix(ref bool __result)
        {
            __result = true;
        }
    }

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
        private static void Postfix(ref Structure.CanMountResult __result)
        {
            __result.result = Structure.WallMountResult.Valid;
        }
    }

    [HarmonyPatch(typeof(MountedSmallGrid), "CanConstruct")]
    internal class MountedSmallGrid_CanConstruct_Patch
    {
        private static void Postfix(ref bool __result)
        {
            __result = true;
        }
    }
    #endregion SmallGrid
    
    #region Devices
    [HarmonyPatch(typeof(Airlock), "CanConstruct")]
    internal class Airlock_CanConstruct_Patch
    {
        private static void Postfix(ref bool __result)
        {
            __result = true; //evading of CheckSidesBlocked
        }
    }
    #endregion

    #region Items
    [HarmonyPatch(typeof(MiningDrill), "Awake")]
    internal class MiningDrill_Awake_Patch
    {
        [UsedImplicitly]
        private static void Postfix(MiningDrill __instance)
        {
            __instance.MineCompletionTime = 0.01f;
            __instance.MineAmount = 1f;
        }
    }
    #endregion

    #region Movement
    [HarmonyPatch(typeof(MovementController), "Start")]
    internal class MovementController_Start_Patch
    {
        [UsedImplicitly]
        private static bool Prefix(MovementController __instance, ref float ___defaultJetpackMaxHeight)
        {
            ___defaultJetpackMaxHeight = 500f;
            return true;
        }
    }
    #endregion

    #region Mothership
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
}
