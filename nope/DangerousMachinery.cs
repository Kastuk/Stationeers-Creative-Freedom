//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Linq;
//using System.Text;

//using System.Runtime.CompilerServices;
//using System.Reflection;

//using BepInEx;
//using HarmonyLib;
//using UnityEngine;
//using UnityEngine.Networking;
//using JetBrains.Annotations;

//using Assets.Scripts;
//using Assets.Scripts.UI;
//using Assets.Scripts.Objects;
//using Assets.Scripts.Objects.Structures;

//using Assets.Scripts.GridSystem;
//using Assets.Scripts.Networking;

//using Assets.Scripts.Objects.Electrical;
//using Assets.Scripts.Objects.Entities;

//using Assets.Scripts.Inventory;

//using Assets.Scripts.Util;
//using Objects.Items;
//using Assets.Scripts.Objects.Items;

//using System;
//using Assets.Scripts;
//using Assets.Scripts.GridSystem;
//using Assets.Scripts.Localization2;
//using Assets.Scripts.Networking;
//using Assets.Scripts.Networks;
//using Assets.Scripts.Objects;
//using Assets.Scripts.Objects.Motherboards;
//using Assets.Scripts.Objects.Pipes;
//using Assets.Scripts.Sound;
//using Assets.Scripts.Util;
//using UnityEngine;
//using Weather;
//using Objects;

//namespace CreativeFreedom
//{
//    class DangerousMachinery
//    {
//        //this is for standalone survival-related mod
//        //TODO get switcher for wings rotation by powering up turbine (Device, Connection checks)

//        [HarmonyPatch(typeof(WindTurbineGenerator),nameof(WindTurbineGenerator.Awake))]
//        public static class GetWindyWings
//        {
//            public static Dictionary<Collider, WindTurbineGenerator> MovingWings = new Dictionary<Collider, WindTurbineGenerator>();

//            //private static Rigidbody Wings;

//           // [HarmonyPatch("Awake")]
//            [UsedImplicitly]
//            [HarmonyPostfix]
//            public static void TakeCollider(WindTurbineGenerator __instance)
//            {
//                Collider collider = __instance.bladesTransform.GetComponent<Collider>();//__instance.bladesTransform.GetChild(0).GetComponent<Collider>();
//                if (collider != null && !MovingWings.ContainsKey(collider))
//                {
//                    MovingWings.Add(collider,__instance);
//                }
//            }

//            //[HarmonyPatch("UpdateEachFrame")]
//            //[UsedImplicitly]
//            //[HarmonyPostfix]
//            //public static void MakeDamageToThings(WindTurbineGenerator __instance)
//            //{
//            //    OnCollisionAnything.
//            //    if (Wings)
//            //}
//        }

//        [HarmonyPatch(typeof(Human), nameof(Human.OnCollisionEnter))]
//        public static class DamageByWings
//        {
//            //[HarmonyPatch("OnCollisionEnter")]
//            [UsedImplicitly]
//            [HarmonyPostfix]
//            public static void CollideWithWing(Human __instance, ref Collision collision)
//            {
//                if (GetWindyWings.MovingWings.Keys.Count != 0)
//                {
//                    if (GameManager.GameState != GameState.Running)
//                    {
//                        return;
//                    }
//                    if (__instance.RootParent.HasAuthority)
//                    {
//                        if (__instance.Invulnerability > 0f)
//                        {
//                            return;
//                        }
//                        Thing thing = Thing.Find(collision.collider);
//                        WindTurbineGenerator windy = thing as WindTurbineGenerator;
//                        if (windy)
//                        {
//                            if (windy == GetWindyWings.MovingWings[collision.collider])
//                            {
//                                float speed = windy.GenerationRate;
//                                float num = (speed / Time.deltaTime / 60f) * 0.1f;
//                                Debug.Log("Speed of wind wing per second (set as impact magnitude) is " + num);
//                                if (num < __instance.MinSuitDamageVelocity)
//                                {
//                                    return;
//                                }
//                                Vector3 contactPoint = collision.contacts[0].point + collision.contacts[0].normal * FreedomConfig.WindWingDamageMod;
//                                if (GameManager.RunSimulation)
//                                {
//                                    __instance.OnNetworkCollision(contactPoint, collision.relativeVelocity, thing, num);
//                                    return;
//                                }
//                            }
//                        }
//                    }
//                }
//            }
//        }

//        //[HarmonyPatch(typeof(GameManager), "LeaveGame")] //already in NotMelting
//        //public static class OnLeaveTheSavegame
//        //{
//        //    [HarmonyPostfix]
//        //    public static void CleanHeader()
//        //    {
//        //        GetWindyWings.MovingWings.Clear(); //clean dictionary
//        //    }
//        //}
//    }
//}
