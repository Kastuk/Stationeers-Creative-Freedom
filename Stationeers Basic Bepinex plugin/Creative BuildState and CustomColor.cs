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
using Assets.Scripts.Objects.Pipes;
using Assets.Scripts.Objects.Electrical;

using Assets.Scripts.GridSystem;
using Assets.Scripts.Networking;

using Assets.Scripts.Objects.Entities;

using Assets.Scripts.Inventory;
using Objects;
using Objects.Items;
using Assets.Scripts.Objects.Items;

using Assets.Scripts.Util;

using System;
using System.Collections.Generic;
using System.Globalization;
using Assets.Scripts.GridSystem;
using Assets.Scripts.Objects;
using UnityEngine;

/*
Thanks to guiding of the TurkeyKittin! 
And other guide of RoboPhred.
And inspiration from DevCo constructions.
And Inaki's exercises!
*/

namespace StationeersCreativeFreedom
{

    #region Structure


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
    //public class CrativeConstruction
    //{
    //    private void CatchFreshConstruction()
    //    {
    //        Structure.OnConstructed += new Thing.Event(this.MessThisStructure);
    //    }

    //    private void MessThisStructure()
    //    {
    //        Structure createdStructure = Structure.LastCreatedStructure;
    //        if ((UnityEngine.Object)createdStructure != (UnityEngine.Object)null)
    //        {


    //        }
    //    }
    //}

    [HarmonyPatch(typeof(OnServer), "Create", new Type[] { typeof(CreateStructureInstance) })]
    internal class OnServer_Create_Patch
    {
        private static void Postfix(Structure __result, ref Structure ____hasSpawnedWreckage)
        {
            if (WorldManager.Instance.GameMode == GameMode.Creative)
            {
                int buildst = __result.BuildStates.Count - 1;
                int colorcan = -1; //default color
                foreach (var chel in Human.AllHumans) //catch idea of search in lists from liz's AtomicBatteryPatch
                {
                    if (chel.OwnerSteamId == __result.OwnerSteamId)
                    {
                        Human parentHuman = null;
                        parentHuman = chel;
                        bool suitis = (parentHuman.Suit == null || parentHuman == null);
                        if (!suitis)
                        {
                            //Debug.LogError("Builder catched");
                           
                                buildst = (int)(Math.Round(parentHuman.Suit.OutputTemperature - 293.15f)); //temperature +20C = CurrentBuildState 0

                                if (buildst >= __result.BuildStates.Count)
                                { buildst = __result.BuildStates.Count - 1; }

                            if (buildst > -1 && __result.BrokenBuildStates.Count > 0)
                            {
                                buildst = -1;
                                //  bool val = Traverse.Create(Structure.Get()).Field("_hasSpawnedWreckage").GetValue() as bool;
                                //  ____hasSpawnedWreckage = false; 
                            }
                            else if (buildst > -1) { buildst = 0; }

                            //Pipe pipe = __result.mission.GetThing(__result) as Pipe;
                            //bool flag2 = pipe == null;
                            ///TODOrecognize pipes and cables and spawn their break versions at -1
                            ///from BurstPipeEventAction and BreakCableEventAction

                            colorcan = (int)(Math.Round(parentHuman.Suit.OutputSetting - 51f)); //pressure 50 =  CustomColorIndex -1 default
                            if (colorcan > 11)
                            { colorcan = 11; }
                            else if (colorcan < -1)
                            { colorcan = -1; }
                        }
                    }
                }
                __result.UpdateBuildStateAndVisualizer(buildst, 0);
                //thanks to inaki for missed lib com.unity.multiplayer-hlapi.Runtime.dll

                if (__result.PaintableMaterial != null)
                {
                    OnServer.SetCustomColor(__result, colorcan);
                }
            }
        }
    }


    #endregion Structure

    #region DynamicThing
    //[HarmonyPatch(typeof(Thing), "CmdSpawnDynamicThingMaxStack")]
    //internal class Thing_CmdSpawnDynamicThingMaxStack_Patch
    //{
    //    private static void Postfix(ref DynamicThing __dynamicThing3)
    //    {
    //        if (WorldManager.Instance.GameMode == GameMode.Creative)
    //        {
    //            int colorcan = -1;
    //            foreach (var chel in Human.AllHumans) //catch idea of search in lists from liz's AtomicBatteryPatch
    //            {
    //                if (chel.OwnerSteamId == __dynamicThing3.OwnerSteamId)
    //                {
    //                    Human parentHuman = null;
    //                    parentHuman = chel;
    //                    bool suitis = (parentHuman.Suit == null || parentHuman == null);
    //                    if (!suitis)
    //                    {
    //                        //Debug.LogError("Builder catched");
    //                        colorcan = (int)(Math.Round(parentHuman.Suit.OutputSetting - 49f)); //pressure 50 = CustomColorIndex -1 default
    //                        if (colorcan > 11)
    //                        { colorcan = 11; }
    //                        else if (colorcan < -1)
    //                        { colorcan = -1; }
    //                    }
    //                }
    //            }
    //            if (__dynamicThing3.PaintableMaterial != null)
    //            {
    //                OnServer.SetCustomColor(__dynamicThing3, colorcan);
    //            }
    //        }
    //    }
    //}
    [HarmonyPatch(typeof(Thing), "CmdSpawnDynamicThingMaxStack")]
    internal class Thing_CmdSpawnDynamicThingMaxStack_Patch2
    {
        private static bool Prefix(NetworkInstanceId parentId, ulong ownerSteamId, string prefabName)
        {
            DynamicThing dynamicThing = NetworkThing.Find(parentId) as DynamicThing;
            DynamicThing dynamicThing2 = (DynamicThing)Thing.FindPrefab(prefabName);
            bool flag = !dynamicThing2;
            if (!flag)
            {
                Vector3 vector = dynamicThing.RigidBody.worldCenterOfMass + dynamicThing.ThingTransform.forward * 1f;
                Quaternion rotation = Quaternion.AngleAxis(180f, dynamicThing.ThingTransform.up);
                DynamicThing dynamicThing3 = OnServer.Create(dynamicThing2, vector, rotation, ownerSteamId, null);
                Stackable stackable = dynamicThing3 as Stackable;
                bool flag2 = stackable;
                if (flag2)
                {
                    stackable.NetworkQuantity = stackable.MaxQuantity;
                }
                BatteryCell batteryCell = dynamicThing3 as BatteryCell;
                bool flag3 = batteryCell;
                if (flag3)
                {
                    batteryCell.PowerStored = batteryCell.PowerMaximum;
                }
                Ingot ingot = dynamicThing3 as Ingot;
                bool flag4 = ingot;
                if (flag4)
                {
                    ingot.NetworkQuantity = ingot.MaxQuantity;
                }
                CreditCard creditCard = dynamicThing3 as CreditCard;
                bool flag5 = creditCard;
                if (flag5)
                {
                    creditCard.Currency = 8000f;
                }
                DirtCanister dirtCanister = dynamicThing3 as DirtCanister;
                bool flag6 = dirtCanister;
                if (flag6)
                {
                    dirtCanister.AddDirtCheat(8000f);
                }

                //Creative mode addition
                if (WorldManager.Instance.GameMode == GameMode.Creative)
                {
                    int colorcan = -1;
                    foreach (var chel in Human.AllHumans) //catch idea of search in lists from liz's AtomicBatteryPatch
                    {
                        if (chel.OwnerSteamId == dynamicThing3.OwnerSteamId)
                        {
                            Human parentHuman = null;
                            parentHuman = chel;
                            bool suitis = (parentHuman.Suit == null || parentHuman == null);
                            if (!suitis)
                            {
                                //Debug.LogError("Builder catched");
                                colorcan = (int)(Math.Round(parentHuman.Suit.OutputSetting - 51f)); //pressure 50 = CustomColorIndex -1 default
                                if (colorcan > 11)
                                { colorcan = 11; }
                                else if (colorcan < -1)
                                { colorcan = -1; }
                            }
                        }
                    }
                    if (dynamicThing3.PaintableMaterial != null)
                    {
                        OnServer.SetCustomColor(dynamicThing3, colorcan);
                    }
                }
            }

           
            return false;
        }
    }
}
    #endregion DynamicThing

