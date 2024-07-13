using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Reflection;

using BepInEx;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Networking;
using JetBrains.Annotations;

using Assets.Scripts;
using Assets.Scripts.UI;
using Assets.Scripts.Objects;
using Assets.Scripts.Objects.Structures;

using Assets.Scripts.Inventory;
using Assets.Scripts.Objects.Items;
using Objects.Items;

using Assets.Scripts.Util;
using Assets.Scripts.Voxel;
using Assets.Scripts.Atmospherics;
using Assets.Scripts.GridSystem;
using Assets.Scripts.Networking;
using UnityEngine.Rendering;


using TMPro;

using System;
using System.Collections;
//using IEnumerator = System.Collections.IEnumerator;


//using System.Diagnostics;

using UnityEngine.Bindings;
using UnityEngineInternal;

using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Experimental.Rendering;


using Assets.Scripts.Objects.Entities;

using Assets.Scripts.Serialization;

using UnityEngine.UI;


namespace CreativeFreedom
{

    public static class BaseMethodCall //got taken from someone there https://github.com/pardeike/Harmony/issues/83
    {
        public static T GetMethodNoOverrides<T>(this MethodInfo method, object callFrom) where T : Delegate
        {
            IntPtr ptr = method.MethodHandle.GetFunctionPointer();
            return (T)Activator.CreateInstance(typeof(T), callFrom, ptr);
        }
    }



    [HarmonyPatch(typeof(VoxelTool), nameof(VoxelTool.Awake))]
    //[HarmonyPatch(nameof(VoxelTool.Awake))]
    public static class VoxelToolCreativeSlot
    {
        [UsedImplicitly]
        [HarmonyPostfix]
        public static void FreeSlotType(VoxelTool __instance)
        {
            if (WorldManager.Instance.GameMode == GameMode.Creative)
            {
                __instance.Slots[1].StringKey = "None";
                __instance.Slots[1].StringHash = Animator.StringToHash(__instance.Slots[1].StringKey);
                __instance.Slots[1].Type = Slot.Class.None;
                __instance.Slots[1].Initialize();
            }
        }
    }
    //isn't it useless if I rewrite methods which call for this checks?
    //[HarmonyPatch("CanRemoveDirt")]
    //[UsedImplicitly]
    //[HarmonyPostfix]
    //public static void SkipCheck(ref bool __result)
    //{
    //    if (WorldManager.Instance.GameMode == GameMode.Creative)
    //    {
    //        __result = true;
    //    }
    //}

    //[HarmonyPatch(typeof(VoxelTool), "RemoveDirtFromDirtBag")]
    //public static class VoxelToolCreative1
    //{
    //    [UsedImplicitly]
    //    [HarmonyPrefix]
    //    public static bool SkipCheck2(ref bool __result)
    //    {
    //        if (WorldManager.Instance.GameMode == GameMode.Creative)
    //        {
    //            __result = true;
    //            return true;
    //        }
    //        else return true;
    //    }
    //}

    //[HarmonyPatch("GetCurrentCollectedDirtAmount")]
    //[UsedImplicitly]
    //[HarmonyPostfix]
    //public static void SkipCheck3(ref float __result)
    //{
    //    if (WorldManager.Instance.GameMode == GameMode.Creative)
    //    {
    //        __result = 0.999f;
    //    }
    //}

    [HarmonyPatch(typeof(VoxelTool), nameof(VoxelTool.LateUpdate))]
    internal class VoxelToolCreativeUpdate
    {
        [UsedImplicitly]
        [HarmonyPrefix]
        public static bool BreakVoxelPositionLimit(VoxelTool __instance)
        {
            if (WorldManager.Instance.GameMode == GameMode.Creative)
            {
                //TODO break voxel blueprint position limit
                //recover that base.LateUpdateEachFrame(); first
                // Debug.Log("Voxel tool got creative check in LateUpdate");

                MethodInfo dynThing_base = AccessTools.DeclaredMethod(typeof(DynamicThing), "LateUpdateEachFrame");
                dynThing_base.GetMethodNoOverrides<Action>(__instance).Invoke(); //base.LateUpdateEachFrame();

                if (__instance.OnOff)// && __instance.Powered)
                {
                    // Debug.Log("Voxel tool got On in LateUpdate");
                    if (InventoryManager.ActiveHandSlot != null && InventoryManager.ActiveHandSlot.Occupant == __instance)
                    {
                        if (!__instance.VoxelBlueprint.gameObject.activeSelf)
                        {
                            __instance.VoxelBlueprint.gameObject.SetActive(true);
                        }
                        // Vector3 vector = (__instance.AllowForwardCursor ? CursorManager.CursorPositionForward : CursorManager.CursorPlanePosition).ToGrid(1f, 0f).ToVector3();
                        Vector3 vector = CursorManager.CursorPositionForward.ToGrid(1f, 0f).ToVector3();

                        Asteroid asteroid = ChunkController.World.GetChunk(vector) as Asteroid;
                        //if (asteroid)
                        //{
                        Vector3 localPosition = asteroid.GetLocalPosition(vector);
                        //var _color = typeof(VoxelTool).GetField("_canPlaceColor", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(__instance);
                        //Color iscolor = _color as Color;
                        Color origcopy = new Color(0f, 1f, 0f, 0.3f);

                        __instance.CubeWireFrame.BlueprintRenderer.material.color = origcopy;
                        


                        //}
                        //else
                        //{
                        //    __instance.CubeWireFrame.BlueprintRenderer.material.color = __instance._cantPlaceColor;
                        //}
                        __instance.VoxelBlueprint.transform.position = vector;
                        return false;
                    }
                }
                if (__instance.VoxelBlueprint.gameObject.activeSelf)
                {
                    __instance.VoxelBlueprint.gameObject.SetActive(false);
                }

                return false;
            }
            else return true;
        }
    }

    [HarmonyPatch(typeof(VoxelTool), nameof(VoxelTool.OnUsePrimary))]
    internal class VoxelToolCreativeUsePrimary
    {
        [UsedImplicitly]
        [HarmonyPrefix]
        public static bool OnUsePrimaryNoLimits(VoxelTool __instance, ref Vector3 targetLocation, ref Quaternion targetRotation, ref ulong steamId, ref bool authoringMode)
        {
            if (WorldManager.Instance.GameMode == GameMode.Creative && __instance.OnOff)
            {
                //Debug.Log("Voxel tool got creative check in OnUsePrimary");

                MethodInfo item_base = AccessTools.DeclaredMethod(typeof(Item), "OnUsePrimary");
                item_base.GetMethodNoOverrides<Action<Vector3, Quaternion, ulong, bool>>(__instance).Invoke(targetLocation, targetRotation, steamId, authoringMode);
                //base.OnUsePrimary(targetLocation, targetRotation, steamId, authoringMode);

                //if (!__instance._voxelToolInUse && __instance.OnOff)// && __instance.Powered)
                //{
                //    Debug.Log("Voxel tool got On and not in use at OnUsePrimary");
                //    MethodInfo monob_base = AccessTools.DeclaredMethod(typeof(MonoBehaviour), "StartCoroutine");
                //    monob_base.GetMethodNoOverrides<Action<IEnumerator>>(__instance).Invoke(__instance.UseVoxelTool());
                //    //base.StartCoroutine(__instance.UseVoxelTool());
                //}
                var _timer = typeof(VoxelTool).GetField("_usageCoolDown", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(__instance);
                float istimer = (float)_timer;

                if (istimer > 0f)// || !__instance.Powered)
                {
                    return false;
                }
                Vector3 vector = targetLocation.GridCenter(1f, 0f);
                //Asteroid asteroid = ChunkController.World.GetChunk(vector) as Asteroid;
                //if (!asteroid)
                //{
                //    return false;
                //}
                //Vector3 localPosition = asteroid.GetLocalPosition(vector);

                //__instance.PaintVoxel(vector);
                var parameters = new object[] { vector };
                typeof(VoxelTool).GetMethod("PaintVoxel", BindingFlags.NonPublic | BindingFlags.Instance).Invoke(__instance, parameters);//new PrivateMethodClass(), null);
               
                //if (GameManager.RunSimulation)
                //{
                //    __instance.RemoveDirtFromDirtBag();
                //}
                istimer = __instance.UsageCooldown;
                typeof(VoxelTool).GetField("_usageCoolDown", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(__instance, istimer);


                return false;
            }
            else return true;
        }
    }


    //[HarmonyPatch(typeof(VoxelTool), nameof(VoxelTool.UseVoxelTool))]
    //internal class VoxelToolCreativeUsing
    //{
    //    [UsedImplicitly]
    //    [HarmonyPrefix]
    //    public static IEnumerator VoxelUsedNoLimits(VoxelTool __instance)
    //    {
    //        if (WorldManager.Instance.GameMode == GameMode.Creative)
    //        {
    //            Debug.Log("Voxel tool got creative check in UseVoxelTool");
    //            __instance._voxelToolInUse = true;
    //            MethodInfo thing_base = AccessTools.DeclaredMethod(typeof(Thing), nameof(Thing.Interact));
    //            thing_base.GetMethodNoOverrides<Action<InteractableType, int>>(__instance).Invoke(InteractableType.Activate, 1);
    //            //base.Interact(InteractableType.Activate, 1);

    //            while (KeyManager.GetMouse("Primary") && !KeyManager.GetButton(KeyMap.SwapHands))
    //            {
    //                Debug.Log("Voxel tool got UseVoxelTool endofframe");
    //                yield return Yielders._endOfFrame;//Yielders.EndOfFrame;
    //            }
    //            thing_base.GetMethodNoOverrides<Action<InteractableType, int>>(__instance).Invoke(InteractableType.Activate, 0);
    //            //base.Interact(InteractableType.Activate, 0);
    //            __instance._voxelToolInUse = false;
    //            yield break;
    //            //return false;
    //        }
    //        //else return true;
    //    }
    //}
    //[HarmonyPatch] // patch Verse.Widgets.IsPartiallyOrFullyTypedNumber
    //class VoxelToolCreativeDraw
    //{
    //    static System.Reflection.MethodBase TargetMethod()
    //    {
    //        // refer to C# reflection documentation:  https://github.com/pardeike/Harmony/issues/121
    //        return typeof(VoxelTool).GetMethod("PaintVoxel",
    //                       BindingFlags.NonPublic | BindingFlags.Static).MakeGenericMethod(typeof(Vector3));
    //    }
    //    //{ //take from https://github.com/pardeike/Harmony/issues/121
    //    //    return typeof(VoxelTool)
    //    //        .GetMethod("PaintVoxel",
    //    //                   BindingFlags.NonPublic | BindingFlags.Static)
    //    //        .MakeGenericMethod(typeof(Vector3));
    //    //}

    //   // public static GasMixture GeyserMix = new GasMixture(); //FIRSTLY fix the geysers exhaust

    //    [UsedImplicitly]
    //    [HarmonyPrefix]
    //    static void DrawMineablesAndGeysers(VoxelTool __instance, ref Vector3 worldLocation)//, out Vector3 __state)
    //    {
    //        //__state = worldLocation;

    //        if (WorldManager.Instance.GameMode == GameMode.Creative)
    //        {
    //            // Debug.Log("Voxel tool got creative check in PaintVoxel");
    //            //find player slots, check slots, set ore to sculpt by voxel tool
    //            Slot slot = __instance.DirtCanisterSlot;
    //            // GasCanister gascan = slot.Occupant as GasCanister; //FIRSTLY fix the geysers exhaust
    //            //lets spawn geysers which spawning gas based on canister contains in VoxelTool slot!

    //            try
    //            {
    //                //if (gascan && !gascan.IsEmpty)  //FIRSTLY fix the geysers exhaust
    //                //{
    //                //    GeyserMix = gascan.InternalAtmosphere.GasMixture;
    //                //    if (!GeyserMix.IsValid)
    //                //    {
    //                //        Debug.Log("GeyserMix is not valid");
    //                //    }
    //                //    __instance.VoxelPaintType = MinableType.GeyserHydrogen;
    //                //}
    //                Ore oreball = slot.Occupant as Ore;
    //                if (oreball)
    //                {
    //                    //Debug.Log("Voxel tool got Ore as slot occupant");
    //                    switch (oreball.PrefabName)
    //                    {
    //                        //switch by thing in canister slot
    //                        case "ItemCoalOre":
    //                            __instance.VoxelPaintType = MinableType.Coal;
    //                            break;
    //                        case "ItemIronOre":
    //                            __instance.VoxelPaintType = MinableType.Iron;
    //                            break;
    //                        case "ItemCopperOre":
    //                            __instance.VoxelPaintType = MinableType.Copper;
    //                            break;
    //                        case "ItemSilverOre":
    //                            __instance.VoxelPaintType = MinableType.Silver;
    //                            break;
    //                        case "ItemGoldOre":
    //                            __instance.VoxelPaintType = MinableType.Gold;
    //                            break;
    //                        case "ItemLeadOre":
    //                            __instance.VoxelPaintType = MinableType.Lead;
    //                            break;
    //                        case "ItemNickelOre":
    //                            __instance.VoxelPaintType = MinableType.Nickel;
    //                            break;
    //                        case "ItemUraniumOre":
    //                            __instance.VoxelPaintType = MinableType.Uranium;
    //                            break;
    //                        case "ItemSiliconOre":
    //                            __instance.VoxelPaintType = MinableType.Silicon;
    //                            break;
    //                        case "ItemCobaltOre":
    //                            __instance.VoxelPaintType = MinableType.Cobalt;
    //                            break;
    //                        //must give a way to set ice without melting
    //                        //disable melting at creative mode? Problems for experiments
    //                        case "ItemIce":
    //                        case "ItemPureIce":
    //                        case "ItemPureIceSteam":
    //                            __instance.VoxelPaintType = MinableType.Ice;
    //                            break;
    //                        case "ItemVolatiles":
    //                        case "ItemPureIceVolatiles":
    //                        case "ItemPureIceLiquidVolatiles":
    //                            __instance.VoxelPaintType = MinableType.Volatiles;
    //                            break;
    //                        case "ItemOxite":
    //                        case "ItemPureIceOxygen":
    //                        case "ItemPureIceLiquidOxygen":
    //                            __instance.VoxelPaintType = MinableType.Oxite;
    //                            break;
    //                        case "ItemNitrice":
    //                        case "ItemPureIceNitrogen":
    //                        case "ItemPureIceNitrous":
    //                        case "ItemPureIceLiquidNitrogen":
    //                        case "ItemPureIceLiquidNitrous":
    //                            __instance.VoxelPaintType = MinableType.Nitrice;
    //                            break;

    //                        default:
    //                            __instance.VoxelPaintType = MinableType.Stone;
    //                            break;
    //                    }
    //                }
    //            }

    //            catch (Exception e)
    //            {
    //                Debug.Log("Error at VoxelTool.PaintVoxel: " + e.ToString());
    //            }
    //            //return;
    //        }
    //        //return false;
    //    }
    //    //if (__instance.VoxelPaintType == MinableType.Stone)
    //    //{
    //    //    return true;
    //    //}
    //    [HarmonyPostfix]
    //    static void SetMineableVoxel(VoxelTool __instance, Vector3 __state)
    //    {
    //        if (WorldManager.Instance.GameMode == GameMode.Creative)
    //        {
    //            //find a way to run vizualizer of mineables like in deserializing of asteroids or something
    //            //as only after saveload mineables is appear properly
    //            //except geysers, which not give any gas

    //            Vector3 worldLocation = __state;
    //        if (__instance.VoxelPaintType <= MinableType.Stone || __state == null)
    //        {
    //            return;
    //        }

    //        Asteroid asteroid = ChunkController.World.GetChunk(worldLocation) as Asteroid;
    //            if (asteroid)
    //            {
    //                ChunkController.World.Register(asteroid);

    //                //Debug.Log("Asteroid params: " + asteroid.name + ", " + asteroid.IsInitialized);
    //                //Minables mineable = asteroid.GetMineable(worldLocation);
    //                //if (mineable != null)
    //                //{
    //                //    Debug.Log("Mineable params: " + mineable.IsMineable + ", " + mineable.VoxelType);
    //                //    return false;
    //                //}
    //                //asteroid.PopulateMinable(worldLocation, __instance.VoxelPaintType);

    //                //Minables mineable = asteroid.GetMineable(worldLocation);
    //                //if (mineable != null)
    //                //{
    //                //    if (mineable.VoxelType == __instance.VoxelPaintType)
    //                //    {
    //                //        return false;
    //                //    }
    //                //    asteroid._allMineables.Remove(mineable.Position); //this may be out of bounds if mineable is not in dictionary allmineables
    //                //}
    //                try
    //                {
    //                    Minables minables = new Minables(MiningManager.FindMineableType(__instance.VoxelPaintType), worldLocation * ChunkObject.VoxelSize + asteroid.Position, asteroid)
    //                    {
    //                        LocalPosition = BitConverter.GetBytes(asteroid.ChunkController.Vector2Offset[(int)worldLocation.x, (int)worldLocation.y, (int)worldLocation.z])
    //                    };
    //                    //if (!asteroid._allMineables.ContainsKey(worldLocation))
    //                    //{
    //                    //    asteroid._allMineables.Add(worldLocation, minables);
    //                    //}
    //                    asteroid.VoxelVisual[(int)BitConverter.ToInt16(minables.LocalPosition, 0)] = true;
    //                    asteroid.SetMineableRenderers(true);
    //                    //asteroid._allMineables[worldLocation] = minables; //THERE?
    //                    //PROBLEM is it got out of bounds cause that chunk already filled?

    //                    MineableObject mineableVisualiser = MiningManager.Instance.GetMineableVisualiser(minables.VoxelType, minables.Position);
    //                    if (mineableVisualiser)
    //                    {
    //                        mineableVisualiser.SetVisible(true);
    //                        asteroid.MineableVisualizers.Add(minables.Position.ToGrid(1f, 0f), mineableVisualiser);
    //                    }

    //                    MeshRenderer meshRenderer;
    //                    MiningManager.MineableGoggleVisualizers.TryGetValue(minables, out meshRenderer);
    //                    Mesh mesh = null;
    //                    Material material = null;
    //                    if (meshRenderer != null)
    //                    {
    //                        MeshFilter component = meshRenderer.GetComponent<MeshFilter>();
    //                        mesh = ((component != null) ? component.sharedMesh : null);
    //                        material = meshRenderer.sharedMaterial;
    //                    }
    //                    InstancedIndirectDrawCall instancedIndirectDrawCall;
    //                    if (mesh == null || material == null)
    //                    {
    //                        instancedIndirectDrawCall = InstancedIndirectDrawCall.FindOrAddDrawCall(asteroid.MineableVisualizerDrawCalls, MiningManager.Instance.defaultMineableVisualizerMaterial, MiningManager.Instance.defaultMineableVisualizerMesh, 0, 0);
    //                    }
    //                    else
    //                    {
    //                        instancedIndirectDrawCall = InstancedIndirectDrawCall.FindOrAddDrawCall(asteroid.MineableVisualizerDrawCalls, material, mesh, 0, 0);
    //                    }
    //                    Matrix4x4 matrix4x = Matrix4x4.TRS(minables.Position, Quaternion.identity, new Vector3(1f, 1f, 1f));
    //                    minables.InstancedIndirectDrawCall = instancedIndirectDrawCall;
    //                    minables.VisualizerTransformMatrix = matrix4x;
    //                    instancedIndirectDrawCall.AddInstance(matrix4x);


    //                    asteroid.SetMineableRenderers(true);
    //                }
    //                catch (Exception e)
    //                {
    //                    Debug.Log("Error at custom populate mineable: " + e.ToString());
    //                }
    //            }
    //        }
    //    }
    //}

    //    //TODO: Create asteroid!

    //    Asteroid asteroid = ChunkController.World.GetChunk(worldLocation) as Asteroid;
    //    //asteroid.PopulateMinable(__state, __instance.VoxelPaintType);
    //    if (asteroid != null)
    //    {
    //        //taken from insides of Asteroid.PopulateMinable
    //        Minables mineable = asteroid.GetMineable(worldLocation);
    //        if (mineable != null)
    //        {
    //            if (mineable.VoxelType == __instance.VoxelPaintType)
    //            {
    //                return false;
    //            }
    //            asteroid._allMineables.Remove(mineable.Position);
    //        }

    //        Minables minables = new Minables(MiningManager.FindMineableType(__instance.VoxelPaintType), worldLocation * ChunkObject.VoxelSize + asteroid.Position, asteroid)
    //        {
    //            LocalPosition = BitConverter.GetBytes(asteroid.ChunkController.Vector2Offset[(int)worldLocation.x, (int)worldLocation.y, (int)worldLocation.z])
    //        };
    //        asteroid._allMineables[worldLocation] = minables;
    //        //PROBLEM is it got out of bounds cause that chunk already filled?

    //        MineableObject mineableVisualiser = MiningManager.Instance.GetMineableVisualiser(minables.VoxelType, minables.Position);
    //        if (mineableVisualiser)
    //        {
    //            mineableVisualiser.SetVisible(true);
    //            asteroid.MineableVisualizers.Add(minables.Position.ToGrid(1f, 0f), mineableVisualiser);
    //        }

    //        MeshRenderer meshRenderer;
    //        MiningManager.MineableGoggleVisualizers.TryGetValue(minables, out meshRenderer);
    //        Mesh mesh = null;
    //        Material material = null;
    //        if (meshRenderer != null)
    //        {
    //            MeshFilter component = meshRenderer.GetComponent<MeshFilter>();
    //            mesh = ((component != null) ? component.sharedMesh : null);
    //            material = meshRenderer.sharedMaterial;
    //        }
    //        InstancedIndirectDrawCall instancedIndirectDrawCall;
    //        if (mesh == null || material == null)
    //        {
    //            instancedIndirectDrawCall = InstancedIndirectDrawCall.FindOrAddDrawCall(asteroid.MineableVisualizerDrawCalls, MiningManager.Instance.defaultMineableVisualizerMaterial, MiningManager.Instance.defaultMineableVisualizerMesh, 0, 0);
    //        }
    //        else
    //        {
    //            instancedIndirectDrawCall = InstancedIndirectDrawCall.FindOrAddDrawCall(asteroid.MineableVisualizerDrawCalls, material, mesh, 0, 0);
    //        }
    //        Matrix4x4 matrix4x = Matrix4x4.TRS(minables.Position, Quaternion.identity, new Vector3(1f, 1f, 1f));
    //        minables.InstancedIndirectDrawCall = instancedIndirectDrawCall;
    //        minables.VisualizerTransformMatrix = matrix4x;
    //        instancedIndirectDrawCall.AddInstance(matrix4x);

    //        return false;
    //    }
    //    return false;
    //}
    //        return false;
    //    }
    //    return true;
    //}


    //WHAt is it thing... Ah, its got __state from beginning of this PaintVoxel method to let placed mineable be diggable 
    //[HarmonyPostfix]
    //static void SetMineableVoxel(VoxelTool __instance, Vector3 __state)
    //{
    //    if (__instance.VoxelPaintType <= MinableType.Stone || __state == null)
    //    {
    //        return;
    //    }

    //    Asteroid asteroid = ChunkController.World.GetChunk(__state) as Asteroid;
    //    //asteroid.PopulateMinable(__state, __instance.VoxelPaintType);
    //    if (asteroid != null)
    //    {
    //        //taken from insides of Asteroid.PopulateMinable
    //        Minables mineable = asteroid.GetMineable(__state);
    //        if (mineable != null)
    //        {
    //            if (mineable.VoxelType == __instance.VoxelPaintType)
    //            {
    //                return;
    //            }
    //            asteroid._allMineables.Remove(mineable.Position);
    //        }

    //        Minables minables = new Minables(MiningManager.FindMineableType(__instance.VoxelPaintType), __state * ChunkObject.VoxelSize + asteroid.Position, asteroid)
    //        {
    //            LocalPosition = BitConverter.GetBytes(asteroid.ChunkController.Vector2Offset[(int)__state.x, (int)__state.y, (int)__state.z])
    //        };
    //        asteroid._allMineables[__state] = minables;
    //        //PROBLEM is it got out of bounds cause that chunk already filled?

    //        MineableObject mineableVisualiser = MiningManager.Instance.GetMineableVisualiser(minables.VoxelType, minables.Position);
    //        if (mineableVisualiser)
    //        {
    //            mineableVisualiser.SetVisible(true);
    //            asteroid.MineableVisualizers.Add(minables.Position.ToGrid(1f, 0f), mineableVisualiser);
    //        }

    //        MeshRenderer meshRenderer;
    //        MiningManager.MineableGoggleVisualizers.TryGetValue(minables, out meshRenderer);
    //        Mesh mesh = null;
    //        Material material = null;
    //        if (meshRenderer != null)
    //        {
    //            MeshFilter component = meshRenderer.GetComponent<MeshFilter>();
    //            mesh = ((component != null) ? component.sharedMesh : null);
    //            material = meshRenderer.sharedMaterial;
    //        }
    //        InstancedIndirectDrawCall instancedIndirectDrawCall;
    //        if (mesh == null || material == null)
    //        {
    //            instancedIndirectDrawCall = InstancedIndirectDrawCall.FindOrAddDrawCall(asteroid.MineableVisualizerDrawCalls, MiningManager.Instance.defaultMineableVisualizerMaterial, MiningManager.Instance.defaultMineableVisualizerMesh, 0, 0);
    //        }
    //        else
    //        {
    //            instancedIndirectDrawCall = InstancedIndirectDrawCall.FindOrAddDrawCall(asteroid.MineableVisualizerDrawCalls, material, mesh, 0, 0);
    //        }
    //        Matrix4x4 matrix4x = Matrix4x4.TRS(minables.Position, Quaternion.identity, new Vector3(1f, 1f, 1f));
    //        minables.InstancedIndirectDrawCall = instancedIndirectDrawCall;
    //        minables.VisualizerTransformMatrix = matrix4x;
    //        instancedIndirectDrawCall.AddInstance(matrix4x);
    //    }
    //}


    //change gasmix for fresh spawned geyser
    //[HarmonyPatch(typeof(GeyserObject), nameof(GeyserObject.CalculateGas))]
    //internal class GeyserCustomized
    //{
    //    [UsedImplicitly]
    //    [HarmonyPrefix]
    //    public static bool SwitchGeyserGasMix(GeyserObject __instance)
    //    {
    //        __instance.MaximumPressure = 10000f; //1013.25f;

    //        if (WorldManager.Instance.GameMode == GameMode.Creative && VoxelToolCreativeDraw.GeyserMix.TotalMolesGassesAndLiquids > 0f)
    //        {
    //            try
    //            {
    //                __instance._spawnGas = GasMixtureHelper.Create();
    //                __instance._spawnGas.Add(VoxelToolCreativeDraw.GeyserMix);
    //                VoxelToolCreativeDraw.GeyserMix.Cleanup();
    //            }
    //            catch (Exception e)
    //            {
    //                Debug.Log("Error at GeyserObject.CalculateGas: " + e.ToString());
    //            }
    //            return false;
    //        }
    //        else return true;
    //    }
    //}
}

