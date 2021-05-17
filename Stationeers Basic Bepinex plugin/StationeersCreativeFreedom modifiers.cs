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

namespace StationeersCreativeFreedom
{


    #region DynamicThing
    [HarmonyPatch(typeof(Entity), "EntityDeath")]
    internal class Entity_Start_Patch
    {
        [UsedImplicitly]
        private static void Prefix(Entity __instance)
        {
            __instance.DecayTimeSecs = 500;
        }
    }

    [HarmonyPatch(typeof(DynamicSkeleton), "Start")]
    internal class DynamicSkeleton_Start_Patch
    {
        [UsedImplicitly]
        private static void Postfix(DynamicSkeleton __instance, ref float ___DestroyTimer)
        {
            ___DestroyTimer = 500f;
        }
    }
    #endregion DynamicThing


    #region Items
    [HarmonyPatch(typeof(MiningDrill), "OnUsePrimary")]
    internal class MiningDrill_Awake_Patch
    {
        [UsedImplicitly]
        private static void Prefix(MiningDrill __instance)
        {
            if (WorldManager.Instance.GameMode == GameMode.Creative)
            {
                __instance.MineCompletionTime = 0.01f; //copied from DeepMine mod of daniellovell
                __instance.MineAmount = 1f;
            }
            //else
            //{ __instance.MineCompletionTime = __instance.MineCompletionTime; 
            //__instance.MineAmount = __instance.MineAmount;}
            // how to return them to default in survival mode?..
        }
    }

    
    #endregion

    #region Movement
    [HarmonyPatch(typeof(MovementController), "SetMovementMode")]
    internal class MovementController_Start_Patch
    {
        [UsedImplicitly]
        private static bool Prefix(MovementController __instance, ref float ___defaultJetpackMaxHeight)
        {
            if (WorldManager.Instance.GameMode == GameMode.Creative)
            { ___defaultJetpackMaxHeight = 500f; }
            return true;
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
