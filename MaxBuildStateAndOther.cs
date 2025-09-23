using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;


using UnityEngine;
using BepInEx;
using HarmonyLib;


using JetBrains.Annotations;

using Assets.Scripts;
using Assets.Scripts.Objects;
using Assets.Scripts.Objects.Items;

using Assets.Scripts.Inventory;

using UnityEngine;




/*
Thanks to guiding of the TurkeyKittin! 
And other guide of RoboPhred.
And inspiration from DevCo constructions.
And Inaki's exercises!
*/

namespace CreativeFreedom
{
    [HarmonyPatch(typeof(InventoryManager), "UsePrimaryComplete")]
    public class MaxBuildState
    {
        [UsedImplicitly]
        public static void Postfix()
        {
            if (InventoryManager.IsAuthoringMode && FreedomConfig.MaxBuildState)
            {
                Structure str = Structure.LastCreatedStructure; //THanks to inaki for tip
                int bstate = str.BuildStates.Count - 1;
                str.UpdateBuildStateAndVisualizer(bstate, 0);
                //str.SetCustomColor(AuthorToolColorKeys.colorIndex);
            } //TODO: add way to make structures destructed, maybe switch targeted thing by key?
        }
    }



    [HarmonyPatch(typeof(Item), nameof(Item.Awake))] //test if it is serializing at saveload or not
    public class AddStructureIntoKit
    {
        [UsedImplicitly]
        public static void Prefix(Item __instance)
        {
            if (FreedomConfig.Experimental)
            {
                if (__instance.PrefabName == "ItemKitFurniture")
                {
                    MultiConstructor cons = __instance as MultiConstructor;
                    CreativeFreedom.Log("Got find kit Furniture item at Awake");

                    Structure rack = Prefab.Find(1473807953) as Structure; //StructureTorpedoRack
                    if (rack != null && !cons.Constructables.Contains(rack))
                    {
                        CreativeFreedom.Log("Rack there: " + rack.PrefabName);
                        cons.Constructables.Add(rack);

                        foreach (Slot slot in rack.Slots)
                        {
                            slot.StringKey = "GasCanister";
                            slot.StringHash = Animator.StringToHash(slot.StringKey);
                            slot.Type = Slot.Class.GasCanister;
                            slot.Initialize();
                        }
                        //may need to be written in right at deconstruction
                        rack.BuildStates[0].Tool.ToolEntry = cons; //return furniture kit at deconstruction
                    }
                }
            }
        }
    }

    //[HarmonyPatch(typeof(Slot), nameof(Slot.CanInsert))] //nope, its been overriding somewhere else
    //public class AllowTanksInRack_Insert
    //{
    //    [UsedImplicitly]
    //    public static void Postfix(ref DynamicThing thing, ref Slot destinationSlot, ref bool __result)
    //    {
    //        if (FreedomConfig.Experimental)
    //        {
    //            if (destinationSlot == null || destinationSlot.IsLocked || !destinationSlot.Occupant)
    //            {
    //                __result = false;
    //            }

    //            if (destinationSlot.Parent.PrefabHash == 1473807953)
    //            {
    //                //if (destinationSlot == null || destinationSlot.IsLocked || !destinationSlot.Occupant)
    //                //{
    //                //    __result = false;
    //                //}
    //                //foreach (Slot destinationSlot2 in destinationSlot.Occupant.Slots)
    //                //{
    //                if (!destinationSlot.Occupant)
    //                {
    //                    if (thing.SlotType == Slot.Class.LiquidCanister || thing.SlotType == Slot.Class.GasCanister || thing.SlotType == Slot.Class.Torpedo)
    //                    {
    //                        __result = true;
    //                    }
    //                }
    //                //else Slot.AllowSwap(thing.ParentSlot, destinationSlot);
    //                //}
    //                //bool flag = (thing.SlotType == Slot.Class.LiquidCanister || thing.SlotType == Slot.Class.GasCanister || thing.SlotType == Slot.Class.Torpedo);
    //                ////bool flag2 = thing is DraggableThing;
    //                //__result = flag;// && !flag2;
    //            }
    //        }
    //    }
    //}

    //Add slot injections at AllowSwap and AllowMove to access both gas and liquid canister types
    [HarmonyPatch(typeof(Slot), nameof(Slot.AllowMove))] //nope, its been overriding somewhere else
    public class AllowTanksInRack_Move
    {
        [UsedImplicitly]
        public static void Postfix(ref DynamicThing thing, ref Slot destinationSlot, ref bool __result)
        {
            if (FreedomConfig.Experimental)
            {
                if (!thing || destinationSlot == null)
                {
                    __result = false;
                }
                if (destinationSlot.IsLocked || destinationSlot.Occupant)
                {
                    __result = false;
                }

                if (destinationSlot.Parent.PrefabHash == 1473807953)
                {
                    bool flag = (thing.SlotType == Slot.Class.LiquidCanister || thing.SlotType == Slot.Class.GasCanister || thing.SlotType == Slot.Class.Torpedo);
                    //bool flag2 = thing is DraggableThing;
                    __result = flag;// && !flag2;
                }
            }
        }
    }

    [HarmonyPatch(typeof(Slot), nameof(Slot.AllowSwap), new Type[] { typeof(Slot), typeof(DynamicThing) })] //nope, its been overriding somewhere else
    public class AllowTanksInRack_Swap_1
    {
        [UsedImplicitly]
        public static void Postfix(ref Slot sourceSlot, ref DynamicThing destination, ref bool __result)
        {
            if (FreedomConfig.Experimental)
            {
                if (!sourceSlot.Occupant && !destination)
                {
                    __result = false;
                }

                if (sourceSlot.Parent.PrefabHash == 1473807953)
                {
                    bool flag = (destination.SlotType == Slot.Class.LiquidCanister || destination.SlotType == Slot.Class.GasCanister || destination.SlotType == Slot.Class.Torpedo);
                    //bool flag2 = thing is DraggableThing;
                    __result = flag;// && !flag2;
                }
            }
        }
    }

    [HarmonyPatch(typeof(Slot), nameof(Slot.AllowSwap), new Type[] { typeof(Slot), typeof(Slot) })] //nope, its been overriding somewhere else
    public class AllowTanksInRack_Swap_2
    {
        [UsedImplicitly]
        public static void Postfix(ref Slot sourceSlot, ref Slot destinationSlot, ref bool __result)
        {
            if (FreedomConfig.Experimental)
            {
                if (!sourceSlot.Occupant && !destinationSlot.Occupant)
                {
                    __result = false;
                }

                if (destinationSlot.Parent.PrefabHash == 1473807953)
                {
                    CanEnterResult canEnterResult = sourceSlot.Occupant.CanEnter(destinationSlot);
                    bool flag = (sourceSlot.Occupant.SlotType == Slot.Class.LiquidCanister || sourceSlot.Occupant.SlotType == Slot.Class.GasCanister || sourceSlot.Occupant.SlotType == Slot.Class.Torpedo);
                    //bool flag2 = thing is DraggableThing;
                    //if (canEnterResult && flag)
                    __result = canEnterResult && flag;// && !flag2;
                }
            }
        }
    }

  


    [HarmonyPatch(typeof(Thing), nameof(Thing.SetSlotOccupantTransformData))] //nope, its been overriding somewhere else
    public class AdjustTankInRack
    {
        [UsedImplicitly]
        public static void Postfix(Thing __instance, ref DynamicThing newChild)
        {
            if (FreedomConfig.Experimental)
            {
                if (__instance.PrefabHash == 1473807953 && newChild is GasCanister)
                {
                    newChild.ThingTransformLocalPosition = new Vector3(0f, 0f, 0.33f);
                    newChild.ThingTransformLocalRotation = Quaternion.Euler(new Vector3(90f, 0f, 0f));
                    newChild.ThingTransform.localScale = new Vector3(1.2f, 1.2f, 1.2f);

                    if (newChild.PrefabHash == -668314371) //"ItemGasCanisterSmart")
                    {
                        newChild.ThingTransformLocalPosition = new Vector3(0f, 0f, 0.32f);
                        newChild.ThingTransform.localScale = new Vector3(1.17f, 1.17f, 1.17f);
                    }
                    //DynamicThing dyn = originThing as DynamicThing;
                    //dyn.ScaleToSlot(1);
                    //originThing.ThingTransform.localScale = 
                }
            }
        }
    }

   




    [HarmonyPatch(typeof(Component), nameof(Component.CompareTag))]
    public class AllSpawnable
    {
        [UsedImplicitly]
        public static void Postfix(ref string tag, ref bool __result)
        {
            if (FreedomConfig.AllSpawnable)
            {
                if (tag == "NotSpawnable")
                {
                    __result = false; //thing is shown in spawn menu and stationpedia, but not spawnable!
                }
            }
        }
    }

    [HarmonyPatch(typeof(OnServer), nameof(OnServer.SpawnDynamicThingMaxStack))]
    public class AllSpawnableAtSpawn
    {
        [UsedImplicitly]
        public static void Prefix(ref long parentId, ref string prefabName)
        {
            if (FreedomConfig.AllSpawnable)
            {
                Entity entity = Thing.Find<Entity>(parentId);
                if (entity.tag == "NotSpawnable")
                {
                    entity.tag = "Untagged";
                    //entity.CustomName = entity.CustomName + " *";
                }

                DynamicThing dynamicThing = Prefab.Find<DynamicThing>(prefabName);
                if (dynamicThing.tag == "NotSpawnable")
                {
                    dynamicThing.tag = "Untagged";
                    //dynamicThing.CustomName = dynamicThing.CustomName + " *";
                }
            }
        }
    }

    [HarmonyPatch(typeof(Structure), nameof(Structure.AttackWith))]
    public static class ProtectThisStructure2
    {
        //[HarmonyPatch("AttackWith")]
        [HarmonyPrefix]
        public static bool AttackWithPrefix(Structure __instance, ref Thing.DelayedActionInstance __result, Attack attack, bool doAction = true)
        {
            if (attack.SourceItem is Assets.Scripts.Objects.Items.AuthoringTool)//thanks to CarbonAnanas for shortening
            {
                if (Input.GetKey(CreativeFreedom.ProtectThis))
                {
                    if (__instance.Indestructable != true)
                    {
                        Thing.DelayedActionInstance delayedActionInstance = new Thing.DelayedActionInstance
                        {
                            Duration = 0.2f,
                            ActionMessage = "Protect"
                        };
                        if (!doAction)
                        {
                            __result = delayedActionInstance;
                            return false; // skip original method
                        }
                        delayedActionInstance.ActionMessage = "Protecting";
                        __instance.Indestructable = true;
                        __instance.RenameThing(__instance.DisplayName + "*");
                        //Debug.LogError("Protected" + __instance.DisplayName);
                        __result = delayedActionInstance;
                        return false;
                    }
                    else
                    {
                        Thing.DelayedActionInstance delayedActionInstance2 = new Thing.DelayedActionInstance
                        {
                            Duration = 0.5f,
                            ActionMessage = "Unprotect"
                        };
                        if (!doAction)
                        {
                            __result = delayedActionInstance2;
                            return false; // skip original method
                        }

                        delayedActionInstance2.ActionMessage = "Unprotecting";
                        __instance.Indestructable = false;
                        __instance.RenameThing(Localization.GetThingName(__instance.PrefabName));
                        //Debug.LogError("Unprotected" + __instance.DisplayName);
                        __result = delayedActionInstance2;
                        return false;
                    }
                }
            }
            return true;
        }
    }
}

    //TODO spawn items in current free hand

    //TODO set things spawnable
    //OnServer.SpawnDynamicThingMaxStack
    //entity.tag == Tags.NotSpawnable || dynamicThing.tag == Tags.NotSpawnable




//ADD change color of structure by color of Authoring Tool (switch by keys)
//[HarmonyPatch(typeof(InventoryManager), "ManagerUpdate")]
//public class AuthorToolColorKeys
//{
//    public static int colorIndex;

//    [UsedImplicitly]
//    public static void Postfix()
//    {
//        if (InventoryManager.Parent != null)
//        {
//            Human hum = InventoryManager.Parent as Human;
//            if (hum && InventoryManager.IsAuthoringMode)
//            {
//                DynamicThing tool = InventoryManager.Instance.ActiveHand.Slot.Occupant;

//                if (KeyManager.GetButton(KeyCode.RightControl))//Input.GetKeyDown(KeyMap.))
//                {
//                    if (KeyManager.GetButtonUp(KeyCode.Plus))
//                    { tool.SetCustomColor(tool.CustomColor.Index + 1); }
//                    if (KeyManager.GetButtonUp(KeyCode.Minus))
//                    { tool.SetCustomColor(tool.CustomColor.Index - 1); }
//                    colorIndex = tool.CustomColor.Index;
//                    Debug.Log("Tool color is " + tool.CustomColor.DisplayName);
//                }
//            }
//        }
//    }
//}
// CHange placementsnap mode for smallgridd mountable things to place switchers on tables
//while GetButton is pressed
//    [HarmonyPatch(typeof(InventoryManager), "PlacementMode")]
//    public class PlacementTypeMemo
//    {
//        public static PlacementSnap def;

//        [UsedImplicitly]
//        public static void Prefix()
//        {
//            if (InventoryManager.ConstructionCursor)
//            {
//               def = InventoryManager.ConstructionCursor.PlacementType;
//            }
//        }
//    }

//    [HarmonyPatch(typeof(InventoryManager), "ManagerUpdate")]//PlacementMode")]
//    public class PlacementSwitcher
//    {
//        public static bool swit = false;

//        [UsedImplicitly]
//        public static void Postfix()
//        {
//            if (InventoryManager.ConstructionCursor != null)
//            {
//                if (KeyManager.GetButtonUp(KeyCode.Z))
//                {
//                    swit = !swit;
//                }
//            }
//        }
//    }
//    [HarmonyPatch(typeof(InventoryManager), "UpdatePlacement")]
//    [HarmonyPatch(new Type[] { typeof(Constructor) })]
//    public class ChangePlacementSnapConstructor
//    {
//        [UsedImplicitly]
//        public static void Prefix()
//        {
//            //MultiConstructor mult = structure as MultiConstructor;
//            //if(mult)
//            if (InventoryManager.ConstructionCursor != null)
//            {
//                if (PlacementSwitcher.swit)
//                {
//                    InventoryManager.ConstructionCursor.PlacementType = PlacementSnap.Grid;
//                }
//                if (!PlacementSwitcher.swit)
//                {
//                    InventoryManager.ConstructionCursor.PlacementType = PlacementTypeMemo.def;
//                }
//            }
//        }
//    }

//    [HarmonyPatch(typeof(InventoryManager), "UpdatePlacement")]
//    [HarmonyPatch(new Type[] { typeof(Structure) })]
//    public class ChangePlacementSnapStructure
//    {
//        [UsedImplicitly]
//        public static void Prefix(ref Structure structure)
//        {
//            //MultiConstructor mult = structure as MultiConstructor;
//            //if(mult)
//            if (structure != null)
//            {
//                if (PlacementSwitcher.swit)
//                {
//                    structure.PlacementType = PlacementSnap.Grid;
//                }
//                if (!PlacementSwitcher.swit)
//                {
//                    structure.PlacementType = PlacementTypeMemo.def;
//                }
//            }
//        }
//    }
//}

//[HarmonyPatch(typeof(Constructor), "SpawnConstruct"]
//internal class MaxBuildState2
//{
//    [UsedImplicitly]
//    public static bool Prefix (ref CreateStructureInstance instance)
//    {
//        if (GameManager.RunSimulation)
//        {
//            Thing.Create<Structure>(instance.Prefab, instance.WorldPosition, instance.WorldRotation, 0L).SetStructureData(instance.LocalRotation, instance.OwnerClientId, instance.LocalGrid, instance.CustomColor);
//            Structure struc = Structure.LastCreatedStructure;
//            foreach (var chel in Human.AllHumans) //catch idea of search in lists from liz's AtomicBatteryPatch
//            {
//                if (chel.OwnerClientId == struc.OwnerClientId)
//                {
//                    if(chel.LeftHandSlot.Occupant as AuthoringTool || chel.RightHandSlot.Occupant as AuthoringTool)
//                    {
//                        //too long
//                    }
//                }

//                    return false;
//        }
//        if (Assets.Scripts.Networking.NetworkManager.IsClient)
//        {
//            new ConstructionCreationMessage(instance).SendToServer();
//        }
//    }

//[HarmonyPatch(typeof(Constructor), "Construct")]
//[HarmonyPatch(new Type[] { typeof(Grid3), typeof(Quaternion), typeof(bool), typeof(ulong), typeof(Mothership) })] //need args of particular construct method
//internal class MaxBuildStage
//{
//    //let buildstate become max at spawn
//    //[HarmonyPrefix]
//    [UsedImplicitly]
//    public virtual bool Prefix(Constructor __instance, ref Grid3 localPosition, ref Quaternion targetRotation, ref bool authoringMode, ref ulong steamId, ref Mothership mothership = null)
//    {
//        if(!authoringMode)
//        {
//            return true;
//        }

//        CreateStructureInstance createStructureInstance = mothership ? new CreateStructureInstance(__instance.BuildStructure, mothership, localPosition, targetRotation, steamId, -1) : new CreateStructureInstance(__instance.BuildStructure, localPosition, targetRotation, steamId, -1);
//        if (__instance.PaintableMaterial != null && __instance.CustomColor.Normal != null)
//        {
//            createStructureInstance.CustomColor = __instance.CustomColor.Index;
//        }
//        Constructor.SpawnConstruct(createStructureInstance);
//        //change BuildState somehow...
//        return false;
//    }
//}


//        private static void prefix(ref multiconstructor __instance, int optionindex)
//        {
//            __instance.constructables[optionindex].currentbuildstateindex = __instance.constructables[optionindex].buildstates.count - 1;
//        }

//    }
//public virtual void Construct(Grid3 localPosition, Quaternion targetRotation, bool authoringMode, ulong steamId, Mothership mothership = null)
//}
