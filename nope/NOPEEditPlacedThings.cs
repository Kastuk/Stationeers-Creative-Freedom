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
using Assets.Scripts.Vehicles;
using Assets.Scripts.Objects.SpaceShuttle;

using Assets.Scripts.GridSystem;
using Assets.Scripts.Networking;

using Assets.Scripts.Objects.Entities;

using Assets.Scripts.Inventory;
using Objects;
using Objects.Items;
using Assets.Scripts.Objects.Items;

using Assets.Scripts.Util;

///*
//Thanks to guiding of the TurkeyKittin! 
//And other guide of RoboPhred.
//And inspiration from DevCo constructions.
//And Inaki's exercises!
//*/

namespace CreativeFreedom
{

    //[HarmonyPatch(typeof(InventoryManager), "Update")]
    //internal class InventoryManager_Edit_Targeted_Thing
    //{
    //    private static void Postfix()
    //    {
    //        bool flag = GameManager.GameState == GameState.Running;
    //        if (flag)
    //        {
    //            if (CursorManager.CursorThing != null && KeyManager.GetButton(KeyCode.RightControl) && KeyManager.GetButtonUp(KeyCode.LeftArrow))
    //            {
    //                Vector3 targetangles = CursorManager.CursorThing.Rotation.eulerAngles;
    //                targetangles.x += 1f;
    //                CursorManager.CursorThing.ThingTransform.Rotate(targetangles);

    //                //var gridcent = CursorManager.CursorThing.GridPosition;
    //                //CursorManager.CursorThing.ThingTransform.RotateAround(gridcent,  ,5f)

    //                //string text = string.Format("Cursor Thing: {0} at grid coords: {1}", CursorManager.CursorThing.DisplayName, CursorManager.CursorThing.GridPosition);
    //            }

    //            if (CursorManager.CursorThing != null && KeyManager.GetButton(KeyCode.RightControl) && KeyManager.GetButtonUp(KeyCode.UpArrow))
    //            {
    //                Vector3 targetangles = CursorManager.CursorThing.Rotation.eulerAngles;
    //                targetangles.y += 1f;
    //                CursorManager.CursorThing.ThingTransform.Rotate(targetangles);
    //            }
    //            if (CursorManager.CursorThing != null && KeyManager.GetButton(KeyCode.RightControl) && KeyManager.GetButtonUp(KeyCode.DownArrow))
    //            {
    //                Vector3 targetangles = CursorManager.CursorThing.Rotation.eulerAngles;
    //                targetangles.z += 1f;
    //                CursorManager.CursorThing.ThingTransform.Rotate(targetangles);
    //            }
    //            //rotation got problems. Wall got yellow ghost rotaation and then doubling

    //            //position got problems. Cannot move on the same axis two time in a row, need to move into another direction in between
    //            if (CursorManager.CursorThing != null && KeyManager.GetButton(KeyCode.RightShift) && KeyManager.GetButtonUp(KeyCode.LeftArrow))
    //            {
    //                Vector3 coords = CursorManager.CursorThing.CenterPosition;
    //                coords.x += 0.05f; //big grid is 2f, small grid is 0.5f
    //                CursorManager.CursorThing.ThingTransform.position = coords;
    //            }
    //            if (CursorManager.CursorThing != null && KeyManager.GetButton(KeyCode.RightShift) && KeyManager.GetButtonUp(KeyCode.UpArrow))
    //            {
    //                Vector3 coords = CursorManager.CursorThing.CenterPosition;
    //                coords.y += 0.05f;
    //                CursorManager.CursorThing.ThingTransform.position = coords;
    //            }
    //            if (CursorManager.CursorThing != null && KeyManager.GetButton(KeyCode.RightShift) && KeyManager.GetButtonUp(KeyCode.DownArrow))
    //            {
    //                Vector3 coords = CursorManager.CursorThing.CenterPosition;
    //                coords.z += 0.05f;
    //                CursorManager.CursorThing.ThingTransform.position = coords;
    //            }
    //        }

    //    }


        //[HarmonyPatch(typeof(Structure), "Awake")]
        //internal class Structure_TakeLastConstructed
        //{
        //    private static void Postfix(Structure __instance)
        //    {
        //        if (Structure.LastCreatedStructure != null)
        //        {
        //            //do various stuff with last constructed structure
        //        }
        //    }

        //}



        //NOT WORKING ANYMORE

        //    #region Structure

        //    //[HarmonyPatch(typeof(Structure), "Awake")]
        //    //internal class Structure_Awake_Wreckage_Patch
        //    //{
        //    //    private static void Prefix(Structure __instance, ref bool _hasSpawnedWreckage)
        //    //    {
        //    //        __instance._hasSpawnedWreckage = false;
        //    //    }
        //    //}





        //    [HarmonyPatch(typeof(OnServer), "Create", new Type[] { typeof(CreateStructureInstance) })]
        //    internal class OnServer_Create_Patch
        //    {
        //        private static void Postfix(Structure __result)
        //        {

        //            int buildst = __result.BuildStates.Count - 1;
        //            int colorcan = -1; //default color

        //            ///TODOrecognize pipes and cables and then spawn their break versions at buildstate -1 later
        //            ///from BurstPipeEventAction and BreakCableEventAction
        //            Pipe pipe = null;
        //            bool isPipe = (pipe = __result as Pipe);

        //            Cable cable = null;
        //            bool isCable = (cable = __result as Cable);

        //            foreach (var chel in Human.AllHumans) //catch idea of search in lists from liz's AtomicBatteryPatch
        //            {
        //                if (chel.OwnerSteamId == __result.OwnerSteamId)
        //                {
        //                    Human parentHuman = null;
        //                    parentHuman = chel;
        //                    bool suitis = (parentHuman.Suit == null || parentHuman == null);
        //                    if (!suitis)
        //                    {
        //                        //Debug.LogError("Builder catched");

        //                        buildst = (int)(Math.Round(parentHuman.Suit.OutputTemperature - 293.15f)); //temperature +20C = CurrentBuildState 0

        //                        if (buildst >= __result.BuildStates.Count)
        //                        { buildst = __result.BuildStates.Count - 1; }

        //                        if (buildst < -1 && __result.BrokenBuildStates.Count > 0 && !isPipe && !isCable)
        //                        {
        //                            buildst = -1;
        //                            //__result._hasSpawnedWreckage = false;
        //                            Structure.BrokenBuildState brokenstr = __result.BrokenBuildStates[buildst];
        //                            brokenstr.HasBroken = false;
        //                            //  bool val = Traverse.Create(Structure.Get()).Field("_hasSpawnedWreckage").GetValue() as bool;
        //                            //  ____hasSpawnedWreckage = false; //need to disable spawning of wreckages around
        //                        }
        //                        else if (buildst <= -1 && __result.BrokenBuildStates.Count == 0 && !isPipe && !isCable)
        //                        { buildst = 0; }

        //                        else if (buildst <= -1 && isPipe == true)
        //                        {
        //                            buildst = 0;
        //                            UnityMainThreadDispatcher.Instance().Enqueue(pipe.BurstPipe());
        //                        }

        //                        else if (buildst <= -1 && isCable == true)
        //                        {
        //                            buildst = 0;
        //                            //UnityMainThreadDispatcher.Instance().Enqueue(cable.WaitThenBreak());
        //                            cable.Break();
        //                        }

        //                        colorcan = (int)(Math.Round(parentHuman.Suit.OutputSetting - 51f)); //pressure 50 =  CustomColorIndex -1 default
        //                        if (colorcan > 11)
        //                        { colorcan = 11; }
        //                        else if (colorcan < -1)
        //                        { colorcan = -1; }
        //                    }
        //                }
        //            }
        //            __result.UpdateBuildStateAndVisualizer(buildst, 0);
        //            //thanks to inaki for missed lib com.unity.multiplayer-hlapi.Runtime.dll

        //            if (__result.PaintableMaterial != null)
        //            {
        //                OnServer.SetCustomColor(__result, colorcan);
        //            }
        //        }

        //    }



        //    //wrongs here
        //    //for Authoring tool - set final buildstate and custom color by colorcan in second hand
        //    //[HarmonyPatch(typeof(MultiConstructor), "Construct")]
        //    //internal class MultiConstructor_Construct_Patch
        //    //{
        //    //    private static bool Prefix(ref MultiConstructor __instance, CreateStructureInstance createStructureInstance, Grid3 localPosition, Quaternion targetRotation, int optionIndex, Item offhandItem, bool authoringMode, ulong steamId, Mothership mothership, int quantity)
        //    //    {
        //    //        if (WorldManager.Instance.GameMode == GameMode.Creative)
        //    //        {
        //    //            new CreateStructureInstance(__instance.Constructables[optionIndex], localPosition, targetRotation, steamId, -1);

        //    //            createStructureInstance.BuildStates.Count = num1

        //    //            bool flag2 = __instance.PaintableMaterial != null && __instance.CustomColor.Normal != null;
        //    //            if (flag2)
        //    //            {
        //    //                createStructureInstance.CustomColor = __instance.CustomColor.Index;
        //    //            }
        //    //            OnServer.Create(createStructureInstance);

        //    //            return false;
        //    //        }

        //    //        return true;
        //    //    }
        //    //}
        //    //public class CrativeConstruction
        //    //{
        //    //    private void CatchFreshConstruction()
        //    //    {
        //    //        Structure.OnConstructed += new Thing.Event(this.MessThisStructure);
        //    //    }

        //    //    private void MessThisStructure()
        //    //    {
        //    //        Structure createdStructure = Structure.LastCreatedStructure;
        //    //        if ((UnityEngine.Object)createdStructure != (UnityEngine.Object)null)
        //    //        {


        //    //        }
        //    //    }
        //    //}


        //    #endregion Structure

        //    #region DynamicThing

        //    [HarmonyPatch(typeof(Thing), "CmdSpawnDynamicThingMaxStack")]
        //    internal class Thing_CmdSpawnDynamicThingMaxStack_Patch2
        //    {
        //        private static bool Prefix(NetworkInstanceId parentId, ulong ownerSteamId, string prefabName)
        //        {
        //            DynamicThing dynamicThing = NetworkThing.Find(parentId) as DynamicThing;
        //            DynamicThing dynamicThing2 = (DynamicThing)Thing.FindPrefab(prefabName);
        //            bool flag = !dynamicThing2;
        //            if (!flag)
        //            {
        //                Vector3 vector = dynamicThing.RigidBody.worldCenterOfMass + dynamicThing.ThingTransform.forward * 1f;
        //                Quaternion rotation = Quaternion.AngleAxis(180f, dynamicThing.ThingTransform.up);
        //                DynamicThing dynamicThing3 = OnServer.Create(dynamicThing2, vector, rotation, ownerSteamId, null);
        //                Stackable stackable = dynamicThing3 as Stackable;
        //                bool flag2 = stackable;
        //                if (flag2)
        //                {
        //                    stackable.NetworkQuantity = stackable.MaxQuantity;
        //                }
        //                BatteryCell batteryCell = dynamicThing3 as BatteryCell;
        //                bool flag3 = batteryCell;
        //                if (flag3)
        //                {
        //                    batteryCell.PowerStored = batteryCell.PowerMaximum;
        //                }
        //                Ingot ingot = dynamicThing3 as Ingot;
        //                bool flag4 = ingot;
        //                if (flag4)
        //                {
        //                    ingot.NetworkQuantity = ingot.MaxQuantity;
        //                }
        //                CreditCard creditCard = dynamicThing3 as CreditCard;
        //                bool flag5 = creditCard;
        //                if (flag5)
        //                {
        //                    creditCard.Currency = 8000f;
        //                }
        //                DirtCanister dirtCanister = dynamicThing3 as DirtCanister;
        //                bool flag6 = dirtCanister;
        //                if (flag6)
        //                {
        //                    dirtCanister.AddDirtCheat(8000f);
        //                }

        //               //Creative mode check need

        //               int colorcan = -1;
        //                int buildst2 = 1;
        //                foreach (var chel in Human.AllHumans) //catch idea of search in lists from liz's AtomicBatteryPatch
        //                {
        //                    if (chel.OwnerSteamId == dynamicThing3.OwnerSteamId)
        //                    {
        //                        Human parentHuman = null;
        //                        parentHuman = chel;
        //                        bool suitis = (parentHuman.Suit == null || parentHuman == null);
        //                        if (!suitis)
        //                        {
        //                            //Debug.LogError("Builder catched");
        //                            colorcan = (int)(Math.Round(parentHuman.Suit.OutputSetting - 51f)); //pressure 50 = CustomColorIndex -1 default
        //                            if (colorcan > 11)
        //                            { colorcan = 11; }
        //                            else if (colorcan < -1)
        //                            { colorcan = -1; }

        //                            buildst2 = (int)(Math.Round(parentHuman.Suit.OutputTemperature - 293.15f)); //temperature +20C = CurrentBuildState 0
        //                            if (buildst2 >= 0) { buildst2 = 1; }
        //                            else if (buildst2 < 0) { buildst2 = -1; }
        //                        }

        //                        Rover rover = dynamicThing3 as Rover;
        //                        DynamicModularRocket rocket = dynamicThing3 as DynamicModularRocket;
        //                        Lander lander = dynamicThing3 as Lander;
        //                        DraggableThing drag = dynamicThing3 as DraggableThing;
        //                        bool toobigdyn = (rover || rocket || lander || drag);

        //                        if (!toobigdyn || parentHuman.LeftHandSlot.Occupant == null || parentHuman.RightHandSlot.Occupant == null)
        //                        {
        //                            Slot emptyslot;
        //                            if (parentHuman.LeftHandSlot.Occupant == null)
        //                            { emptyslot = parentHuman.LeftHandSlot; }
        //                            else
        //                            { emptyslot = parentHuman.RightHandSlot; }

        //                            OnServer.MoveToSlot(dynamicThing3, emptyslot); //move spawned thing into empty hand
        //                        }
        //                        //DraggableThing dragg = dynamicThing3 as DraggableThing;

        //                        //if (dragg == true)
        //                        //{
        //                        //    foreach (var isdragslot in dynamicThing3.Slots) //catch idea of search in lists from liz's AtomicBatteryPatch
        //                        //    {
        //                        //        if (isdragslot.AllowDragging == true)
        //                        //        {
        //                        //            Slot dragslot = isdragslot; //what is it for?
        //                        //            dynamicThing3.InteractWith(parentHuman, dynamicThing3, true); //dunno how to convert thing and parent into this interacting stuff
        //                        //        }
        //                        //    }


        //                        //    dynamicThing3.DragInSlot(parentHuman.Slots[emptyslot], dynamicThing3.Offset, this.InteractionIndex);
        //                        //}
        //                    }

        //                }

        //                if (dynamicThing3.PaintableMaterial != null)
        //                {
        //                    OnServer.SetCustomColor(dynamicThing3, colorcan);
        //                }
        //                DynamicGasCanister gasCanister = dynamicThing3 as DynamicGasCanister;
        //                bool gascan = gasCanister;
        //                if (gascan == true && buildst2 == -1)
        //                {
        //                    bool brokenmesh = gasCanister.BrokenMesh;
        //                    if (brokenmesh)
        //                    {
        //                        gasCanister.Renderers[0].MeshFilter.mesh = gasCanister.BrokenMesh;
        //                        foreach (GameObject gameObject in gasCanister.meshDisable)
        //                        {
        //                            gameObject.SetActive(false);
        //                        }
        //                    }
        //                }
        //                GasCanister gasCanister2 = dynamicThing3 as GasCanister;
        //                bool gascan2 = gasCanister2;
        //                if (gascan2 == true && buildst2 == -1)
        //                {
        //                    bool brokenmesh = gasCanister2.BrokenMesh;
        //                    if (brokenmesh)
        //                    {
        //                        gasCanister2.Renderers[0].MeshFilter.sharedMesh = gasCanister2.BrokenMesh;
        //                    }
        //                }
        //            }

        //            return false;
        //        }
        //    }
        //}
        ////wrongs here
        ////[HarmonyPatch(typeof(Thing), "CmdSpawnDynamicThingMaxStack")]
        ////internal class Thing_CmdSpawnDynamicThingMaxStack_Patch
        ////{
        ////    private static void Postfix(ref DynamicThing __dynamicThing3)
        ////    {
        ////        if (WorldManager.Instance.GameMode == GameMode.Creative)
        ////        {
        ////            int colorcan = -1;
        ////            foreach (var chel in Human.AllHumans) //catch idea of search in lists from liz's AtomicBatteryPatch
        ////            {
        ////                if (chel.OwnerSteamId == __dynamicThing3.OwnerSteamId)
        ////                {
        ////                    Human parentHuman = null;
        ////                    parentHuman = chel;
        ////                    bool suitis = (parentHuman.Suit == null || parentHuman == null);
        ////                    if (!suitis)
        ////                    {
        ////                        //Debug.LogError("Builder catched");
        ////                        colorcan = (int)(Math.Round(parentHuman.Suit.OutputSetting - 49f)); //pressure 50 = CustomColorIndex -1 default
        ////                        if (colorcan > 11)
        ////                        { colorcan = 11; }
        ////                        else if (colorcan < -1)
        ////                        { colorcan = -1; }
        ////                    }
        ////                }
        ////            }
        ////            if (__dynamicThing3.PaintableMaterial != null)
        ////            {
        ////                OnServer.SetCustomColor(__dynamicThing3, colorcan);
        ////            }
        ////        }
        ////    }
        ////

        //#endregion DynamicThing

    //}
}

