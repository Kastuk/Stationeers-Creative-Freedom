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
using Assets.Scripts.Voxel;



/*
Thanks to guiding of the TurkeyKittin! 
And other guide of RoboPhred.
And inspiration from DevCo constructions.
And Inaki's exercises!
*/

namespace CreativeFreedom
{


    #region DynamicThing
    //[HarmonyPatch(typeof(Entity), "EntityDeath")]
    //internal class Entity_Start_Patch
    //{
    //    [UsedImplicitly]
    //    private static void Prefix(Entity __instance)
    //    {
    //        __instance.DecayTimeSecs = 500;
    //    }
    //}

    //[HarmonyPatch(typeof(DynamicSkeleton), "Start")]
    //internal class DynamicSkeleton_Start_Patch
    //{
    //    [UsedImplicitly]
    //    private static void Postfix(DynamicSkeleton __instance, ref float ___DestroyTimer)
    //    {
    //        ___DestroyTimer = 500f;
    //    }
    //}
    #endregion DynamicThing


    #region Items
    [HarmonyPatch(typeof(MiningDrill), "OnUsePrimary")]
    internal class MiningDrill_Speedup
    {
        [UsedImplicitly]
        private static void Prefix(MiningDrill __instance)
        {
            __instance.MineCompletionTime = FreedomConfig.MineCompletionTime; //copied from DeepMine mod of daniellovell
              //  __instance.MineAmount = 0.5f;
            
            //else
            //{ __instance.MineCompletionTime = __instance.MineCompletionTime; 
            //__instance.MineAmount = __instance.MineAmount;}
            // how to return them to default in survival mode?..
        }
    }


    #endregion

    #region Movement
        [HarmonyPatch(typeof(MovementController), "SetMovementMode")]
    internal class Jetpack_HeightLimit
    {
        [UsedImplicitly]
        private static void Prefix(MovementController __instance, ref float ___defaultJetpackMaxHeight)
        {
            ___defaultJetpackMaxHeight = FreedomConfig.JetpackMaxHeight; // 10f; 
            
            //return true;
        }
    }
    [HarmonyPatch(typeof(Jetpack), "LateUpdate")]
    internal class Jetpack_Speed
    {
        private static void Postfix (Jetpack __instance)
        {
            if (__instance.PrefabHash == -412551656) //hardjetpack
            {
                __instance.JetPackSpeed = FreedomConfig.JetpackHeavySpeed;
            }
        }
    }

    #endregion

    #region Mothership
    //need to disable rotation limit for z and x axis
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
