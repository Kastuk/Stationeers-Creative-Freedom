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

namespace StationeersCreativeFreedom
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




    [HarmonyPatch(typeof(VoxelTool), "UpdateEachFrame")]
    internal class VoxelTool_UpdateEachFrame_Patch
    {
        private static void Postfix(VoxelTool __instance)
        {
            __instance.UsageCooldown = 0.1f;

            Human sculptor = __instance.RootParentHuman;

            if (sculptor != null)
            {
                int itname = 0;

                if (sculptor.LeftHandSlot.Occupant != null && sculptor.LeftHandSlot.Occupant != __instance)
                {
                    itname = sculptor.LeftHandSlot.Occupant.PrefabHash;
                }
                if (sculptor.RightHandSlot.Occupant != null && sculptor.RightHandSlot.Occupant != __instance)
                {
                    itname = sculptor.RightHandSlot.Occupant.PrefabHash;
                }

                switch (itname)
                {
                    //default:
                    //    __instance.VoxelPaintType = MineableType.Stone;
                    //    break;
                    case 0:
                        __instance.VoxelPaintType = MinableType.Stone;
                        //Debug.LogError("Boring stone");
                        break;

                    case 1758427767://"ItemIronOre":
                        __instance.VoxelPaintType = MinableType.Iron;
                        //Debug.LogError("Such irony");
                        break;
                    case 1724793494: //"ItemCoalOre":
                        __instance.VoxelPaintType = MinableType.Coal;
                        break;
                    case -70730784:// "ItemCopperOre":
                        __instance.VoxelPaintType = MinableType.Copper;
                        break;
                    case -916518678:// "ItemSilverOre":
                        __instance.VoxelPaintType = MinableType.Silver;
                        break;
                    case -1348105509:// "ItemGoldOre":
                        __instance.VoxelPaintType = MinableType.Gold;
                        break;
                    case -1516581844:// "ItemUraniumOre":
                        __instance.VoxelPaintType = MinableType.Uranium;
                        break;
                    case -190236170:// "ItemLeadOre":
                        __instance.VoxelPaintType = MinableType.Lead;
                        break;
                    case 1830218956:// "ItemNickelOre":
                        __instance.VoxelPaintType = MinableType.Nickel;
                        break;
                    case 1103972403:// "ItemSiliconOre":
                        __instance.VoxelPaintType = MinableType.Silicon;
                        break;
                    case -983091249:// "ItemCobaltOre":
                        __instance.VoxelPaintType = MinableType.Cobalt;
                        break;

                    case 1217489948:// "ItemIce": //water
                        __instance.VoxelPaintType = MinableType.Ice;
                        break;
                    case -1805394113:// "ItemOxite":
                        __instance.VoxelPaintType = MinableType.Oxite;
                        break;
                    case 1253102035:// "ItemVolatiles":
                        __instance.VoxelPaintType = MinableType.Volatiles;
                        break;

                    case 1578288856:// "ItemFireExtinguisher":
                        __instance.VoxelPaintType = MinableType.GeyserHydrogen;
                        Debug.LogError("Poof!");
                        break;
                    //case "ItemWasteIngot":
                    //    __instance.VoxelPaintType = MineableType.Bedrock;
                    //    break;
                    //case "ItemGrenade":
                    //    __instance.VoxelPaintType = MineableType.Crater;
                    //    break;
                }
            }
        }
    }

    [HarmonyPatch(typeof(VoxelTool), "PaintVoxel")]
    internal class VoxelTool_PaintVoxel_Patch
    {
        private static bool Prefix(VoxelTool __instance, ref Vector3 worldLocation)
        {
            if (__instance.VoxelPaintType != MineableType.Stone)
            {
                ChunkObject chunk = ChunkController.World.GetChunk(worldLocation) as ChunkObject;
                chunk.SetVoxel((byte)__instance.VoxelPaintType, 1f, (short)worldLocation.x, (short)worldLocation.y, (short)worldLocation.z, true);

                // Asteroid.PlaceVoxel(worldLocation, __instance.VoxelPaintType, __instance.VoxelPaintDensity); //just placing terrain, can be reused for events

                Asteroid aster = ChunkController.World.GetChunk(worldLocation) as Asteroid;
                MineableObject mineviz = MiningManager.Instance.GetMineableVisualiser(__instance.VoxelPaintType, worldLocation);
                mineviz.SetVisible(true);
                aster.MineableVisualizers.Add(worldLocation.ToGrid(1f, 0f), mineviz);
                //aster.RequestRegenerateMesh(true, true);
                //TODO find a way to make ore collidable and mineable! TRY TO USE AsteroidVoxelMessage !
                //ALSO break the neighbour voxels checks to place ore anywhere (inside terrain too)

                Debug.LogError("Pop!");
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
