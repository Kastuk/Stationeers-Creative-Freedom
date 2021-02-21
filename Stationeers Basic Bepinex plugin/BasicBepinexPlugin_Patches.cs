using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BepInEx;
using HarmonyLib;
using UnityEngine;
using JetBrains.Annotations;

using Assets.Scripts.Objects;

namespace Stationeers_BasicBepinexPlugin
{
    [HarmonyPatch(typeof(Structure), "Awake")] //inject into the Awake method from Structure class

    internal class Structure_Awake_Patch
    {
        [UsedImplicitly]//dunno what is for, something for simpler replacing of the field values.
        private static void Postfix(Structure __instance) //Postfix is starting after original method did make his work
        {//__instance.Structure is the same stuff as this.Structure
            __instance.RotationAxis = RotationAxis.All; //replace default parameters of given structure thing
            __instance.AllowedRotations = AllowedRotations.All; //this enumerator can take names of states
        }
    }
    
    [HarmonyPatch(typeof(SmallGrid), "_IsCollision")]
    internal class SmallGrid_isCollision_Patch
    {
        private static bool Prefix() //run before original method
        {
            return false; //disable original method
        }
    }

    [HarmonyPatch(typeof(Structure), "CanConstruct")]
    internal class Structure_CanConstruct_Patch
    {
        private static void Postfix(ref bool __result)
        {
            __result = true; //just changing results
        }
    }

    [HarmonyPatch(typeof(SmallGrid), "CanMountOnWall")]
    internal class SmallGrid_CanMountOnWall_Patch
    {
        private static void Postfix(ref Structure.CanMountResult __result)
        {
            __result.result = Structure.WallMountResult.Valid; //same changing of result, but for referenced class
        }
    }


}
