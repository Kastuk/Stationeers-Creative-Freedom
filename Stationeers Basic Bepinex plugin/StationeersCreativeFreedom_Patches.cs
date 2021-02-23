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

using Assets.Scripts.Util;
using Objects.Items;
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
            __instance.RotationAxis = RotationAxis.All; //thanks for Kamuchi for idea of using the named enumerator values
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

    //for Authoring tool - set final buildstate and custom color by colorcan in second hand
    //[HarmonyPatch(typeof(MultiConstructor), "Construct")]
    //internal class MultiConstructor_Construct_Patch
    //{
    //    private static bool Prefix(ref MultiConstructor __instance, CreateStructureInstance createStructureInstance, Grid3 localPosition, Quaternion targetRotation, int optionIndex, Item offhandItem, bool authoringMode, ulong steamId, Mothership mothership, int quantity)
    //    {
    //        if (WorldManager.Instance.GameMode == GameMode.Creative)
    //        {
    //            new CreateStructureInstance(__instance.Constructables[optionIndex], localPosition, targetRotation, steamId, -1);

    //            createStructureInstance.BuildStates.Count = num1

    //            bool flag2 = __instance.PaintableMaterial != null && __instance.CustomColor.Normal != null;
    //            if (flag2)
    //            {
    //                createStructureInstance.CustomColor = __instance.CustomColor.Index;
    //            }
    //            OnServer.Create(createStructureInstance);

    //            return false;
    //        }

    //        return true;
    //    }
    //}

    [HarmonyPatch(typeof(OnServer), "Create", new Type[] {typeof(CreateStructureInstance)})]
    internal class OnServer_Create_Patch
    {
        private static void Postfix(Structure __result)
        {
            if (WorldManager.Instance.GameMode == GameMode.Creative)
            {
                int maxstate = __result.BuildStates.Count;
                if (maxstate > 1)
                {
                    __result.CurrentBuildStateIndex = maxstate;

                  //  __result.CmdConstructionUpdateRequest();
                  //  __result.UpdateStateVisualizer(false);
                    //or three in the same
                   // __result.UpdateBuildStateAndVisualizer(maxstate, 0);

                }

                if (__result.PaintableMaterial != null)
                {
                    int colorcan = 6;
                    OnServer.SetCustomColor(__result, colorcan);
                }
            }
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
        //thanks to the TurkeyKittin.
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

    #region DynamicThing
    [HarmonyPatch(typeof(Entity), "EntityDeath")]
    internal class Entity_Start_Patch
    {
        [UsedImplicitly]
        private static void Prefix(Entity __instance)
        {
            __instance.DecayTimeSecs = 500;
            //copied from DeepMine mod of daniellovell
        }
    }

    [HarmonyPatch(typeof(DynamicSkeleton), "Start")]
    internal class DynamicSkeleton_Start_Patch
    {
        [UsedImplicitly]
        private static void Postfix(DynamicSkeleton __instance, ref float ___DestroyTimer)
        {
            ___DestroyTimer = 500f;
            //copied from DeepMine mod of daniellovell
        }
    }
    #endregion DynamicThing


    #region Items
    [HarmonyPatch(typeof(MiningDrill), "Awake")]
    internal class MiningDrill_Awake_Patch
    {
        [UsedImplicitly]
        private static void Postfix(MiningDrill __instance)
        {
            __instance.MineCompletionTime = 0.01f;
            __instance.MineAmount = 1f;
            //copied from DeepMine mod of daniellovell
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

    #region Entity

    #endregion Entity
}
