
using Assets.Scripts.Objects.Items;
using HarmonyLib;
using UnityEngine;

using Assets.Scripts;

using JetBrains.Annotations;

namespace CreativeFreedom
{

    internal class JetpackSpeed
    {//taken from FuelJetpack created by Thunder
        public static float CheckSpeed(Jetpack jetpack)
        {
            float baseJetpackSpeed = 3f;
           // Debug.Log("Check speed of jetpack");
            switch (jetpack.PrefabHash)
            {
                case 1969189000:
                    baseJetpackSpeed = 3f; // Jetpack Basic
                    break;
                case -1260618380:
                    baseJetpackSpeed = 5f; // Spacepack
                    break;
                case -412551656:
                    baseJetpackSpeed = 8f; // Hardsuit Jetpack
                    break;
                default:
                    baseJetpackSpeed = 3f;
                    break;
            }

            return baseJetpackSpeed;
        }
    }


    [HarmonyPatch(typeof(MovementController), "HandleJetpack")]
    internal class Jetpack_HeightLimit
    {
        [UsedImplicitly]
        private static void Prefix(MovementController __instance)
        {
            //bool jetpackSwitcher = ;
            if (FreedomConfig.JetpackSwitcher)
            {
                __instance.defaultJetpackMaxHeight = FreedomConfig.JetpackMaxHeight;
                //Debug.Log("Jetpack height limit is " + FreedomConfig.JetpackMaxHeight);
            }
        }
    }



    //[HarmonyPatch(typeof(Jetpack))]
    //[HarmonyPatch(typeof(Jetpack),nameof(Jetpack.Awake))]
    //internal class Jetpack_Modify
    //{
    //    public static Dictionary<Jetpack, float> OrigSpeed = new Dictionary<Jetpack, float>();

    //    //[HarmonyPatch(nameof(Jetpack.Awake))]
    //    [UsedImplicitly]
    //    [HarmonyPostfix]
    //    private static void SaveOriginalValues(Jetpack __instance)
    //    {
    //        bool jetpackSwitcher = FreedomConfig.JetpackSwitcher;
    //        if (jetpackSwitcher)
    //        {
    //            if (!Jetpack_Modify.OrigSpeed.ContainsKey(__instance))
    //                Jetpack_Modify.OrigSpeed.Add(__instance,__instance.JetPackSpeed);
    //            //else Jetpack_Modify.OrigSpeed[__instance] = 5f;
    //            //Debug.Log(string.Concat(new object[]
    //            //{
    //            //    "Orig speed of ",
    //            //    __instance.PrefabName,
    //            //    " is ",
    //            //    __instance.JetPackSpeed
    //            //}));
    //        }
    //    }

    [HarmonyPatch(typeof(Jetpack), "LateUpdate")]
    internal class Jetpack_Breaks1
    {
        [UsedImplicitly]
        [HarmonyPostfix]
        private static void IncreaseSpeed(Jetpack __instance)
        {
            //bool flag = ;
            float speed;
            if (FreedomConfig.JetpackSwitcher && WorldManager.Instance.GameMode == GameMode.Creative)
            {
                speed = JetpackSpeed.CheckSpeed(__instance);

                bool buttonDown = KeyManager.GetButtonDown(KeyCode.LeftShift);
                if (buttonDown)
                {
                    __instance.JetPackSpeed = speed * FreedomConfig.JetpackModSpeed;
                }
                bool buttonUp = KeyManager.GetButtonUp(KeyCode.LeftShift);
                if (buttonUp)
                {
                    __instance.JetPackSpeed = speed;
                }
            }
        }
    }

    [HarmonyPatch(typeof(Jetpack), "OnAtmosphericTick")]
    internal class Jetpack_Breaks2
    {
        [UsedImplicitly]
        [HarmonyPrefix]
        private static bool NoFumes(Jetpack __instance)
        {
            bool flag = (FreedomConfig.JetpackSwitcher && FreedomConfig.InfiniteJetpack && WorldManager.Instance.GameMode == GameMode.Creative);
            return !flag;
        }
    }

    [HarmonyPatch(typeof(Jetpack), "HasPropellent", MethodType.Getter)]
    internal class Jetpack_Breaks3
    {
        [UsedImplicitly]
        [HarmonyPostfix]
        private static void AlwaysGotFuel(ref bool __result)
        {
            //bool flag = ;
            if (FreedomConfig.JetpackSwitcher && FreedomConfig.InfiniteJetpack && WorldManager.Instance.GameMode == GameMode.Creative)
            {
                __result = true;
            }
        }
    }
}
