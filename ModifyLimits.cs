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
    [HarmonyPatch(typeof(MovementController), "HandleJetpack")]
    internal class Jetpack_HeightLimit
    {
        [UsedImplicitly]
        private static void Prefix(MovementController __instance)//, ref float ___defaultJetpackMaxHeight)
        {
            if (FreedomConfig.JetpackSwitcher)
            {
                __instance.defaultJetpackMaxHeight = FreedomConfig.JetpackMaxHeight; // 10f; 
            }
            //return true;
        }
    }


    [HarmonyPatch(typeof(Jetpack))]
    internal class Jetpack_Modify
    { //Mostly stolen from FuelJetpack of the Thunder
        public static Dictionary<Jetpack, float> OrigSpeed = new Dictionary<Jetpack, float>();

        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        private static void SaveOriginalValues(Jetpack __instance)
        {
            if (FreedomConfig.JetpackSwitcher)
            {
                OrigSpeed[__instance] = __instance.JetPackSpeed;
                Debug.Log("Orig speed of " + __instance.PrefabName + " is " + __instance.JetPackSpeed);
            }
        }


        [HarmonyPatch("LateUpdate")]
        [HarmonyPostfix]
        private static void IncreaseSpeed(Jetpack __instance)
        {
            if (FreedomConfig.JetpackSwitcher && WorldManager.Instance.GameMode == GameMode.Creative)
            {
                if (KeyManager.GetButtonDown(KeyCode.LeftShift))
                {
                    __instance.JetPackSpeed = OrigSpeed[__instance] * FreedomConfig.JetpackModSpeed;
                }
                if (KeyManager.GetButtonUp(KeyCode.LeftShift))
                {
                    __instance.JetPackSpeed = OrigSpeed[__instance];
                }
            }
        }

        [HarmonyPatch("OnAtmosphericTick")]
        [HarmonyPrefix]
        private static bool NoFumes(Jetpack __instance)
        {
            if (FreedomConfig.JetpackSwitcher && FreedomConfig.InfiniteJetpack && WorldManager.Instance.GameMode == GameMode.Creative)
            {
                return false;
            }
            return true;
        }

        [HarmonyPatch("HasPropellent", MethodType.Getter)]
        [HarmonyPostfix]

        private static void AlwaysGotFuel(ref bool __result)
        {
            if (FreedomConfig.JetpackSwitcher && FreedomConfig.InfiniteJetpack && WorldManager.Instance.GameMode == GameMode.Creative)
            {
                __result = true;
            }
        }
    }
    #endregion

    #region Camera
    [HarmonyPatch(typeof(CameraController), "ManagerAwake")]
    internal class Camera_FOV_Limits
    {
        [UsedImplicitly]
        private static void Postfix(CameraController __instance)//, ref float ___defaultJetpackMaxHeight)
        {
            __instance.MinFoV = 3f; //default is 50f
            __instance.MaxFoV = 160f; //default is 130f

            //return true;
        }
    }

    [HarmonyPatch(typeof(CameraController), "FovIncrease")]
    internal class Camera_FOV_Increase
    {
        [UsedImplicitly]
        private static bool Prefix(CameraController __instance)//, ref float ___defaultJetpackMaxHeight)
        {
            if (Input.GetKey(KeyCode.RightControl))
            {
                float num = CameraController.CurrentCamera.fieldOfView;
                if (num < __instance.MaxFoV)
                {
                    if (num >= __instance.MaxFoV - 6f)
                    {
                        num = __instance.MaxFoV;
                    }
                    else num += 6f;
                }
                CameraController.SetFieldOfView(num);
                return false;
            }
            else return true;
        }
    }

    [HarmonyPatch(typeof(CameraController), "FovDecrease")]
    internal class Camera_FOV_Decrease
    {
        [UsedImplicitly]
        private static bool Prefix(CameraController __instance)//, ref float ___defaultJetpackMaxHeight)
        {
            if (Input.GetKey(KeyCode.RightControl))
            {
                float num = CameraController.CurrentCamera.fieldOfView;
                if (num > __instance.MinFoV)
                {
                    if (num <= __instance.MinFoV + 6f)
                    {
                        num = __instance.MinFoV;
                    }
                    else num -= 6f;
                }
                CameraController.SetFieldOfView(num);
                return false;
            }
            else return true;
        }
    }

    #endregion Camera

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
    //TODO disable decay of corpses and skeletons without specific atmos conditions.
    #endregion Entity
}
