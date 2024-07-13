using System;
using System.Reflection;
using Assets.Scripts.UI;
using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Objects.Entities;

    namespace CreativeFreedom
{
    class DecorStuff
    {
        //[HarmonyPatch(typeof(MainMenu), "Awake")] //trying to reduce memory load by disabling main menu scene
        //public class LiteMainMenu
        //{
        //    [UsedImplicitly]
        //    [HarmonyPrefix]
        //    private static void DestroyTheScenery(MainMenu __instance)
        //    {
        //        if (FreedomConfig.EnlightenMenu)
        //        {
        //            Debug.Log("CF. Trying to disable the main menu scene.");
        //            GameObject obj = GameObject.Find("MenuSceneMars01");
        //            GameObject obj2 = GameObject.Find("Dust");
        //            UnityEngine.Object.Destroy(obj);
        //            UnityEngine.Object.Destroy(obj2);
        //        }
        //    }
        //}

        //[HarmonyPatch] //stolen from XRepairs of AlexStz
        //public class ScenariosOn
        //{
        //    [HarmonyPatch(typeof(MainMenu), nameof(MainMenu.OnEnable))]
        //    [HarmonyPostfix]
        //    public static void Start_patch(MainMenu __instance)
        //    {
        //        Button btnScenarios = Traverse.Create(__instance).Field("_scenarioButton").GetValue<Button>();
        //        btnScenarios.interactable = true;
        //        btnScenarios.enabled = true;
        //    }
        //}


            [HarmonyPatch(typeof(Human), "PlayBreathAudio")]
        public static class Human_Mute
        {
            //[HarmonyPatch(typeof("PlayBreathAudio"))]
            [UsedImplicitly]
            [HarmonyPrefix]
            public static bool NOBreath1()
            {
                if (FreedomConfig.NoBreathSound)
                {
                    return false;
                }
                else return true;
            }

            [HarmonyPatch(typeof(Human), "PlayJumpBreathAudio")]
            [UsedImplicitly]
            [HarmonyPrefix]
            public static bool NoBreath2()
            {
                if (FreedomConfig.NoBreathSound)
                {
                    return false;
                }
                else return true;
            }
        }

        [HarmonyPatch(typeof(DynamicInvPanel), "Initialize")] //make spawn menu bigger for unusual monitors
        public class ChangeDynamicInvPanelSize2
        {
            private static CanvasScaler canv;

            [UsedImplicitly]
            public static void Postfix(DynamicInvPanel __instance)
            {
                if (ChangeDynamicInvPanelSize2.canv == null)
                {
                    var _canv = typeof(DynamicInvPanel).GetField("_canvas", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(__instance);
                    Canvas iscanv = _canv as Canvas;
                    if (iscanv) ChangeDynamicInvPanelSize2.canv = iscanv.GetComponent<CanvasScaler>();//__instance._canvas.GetComponent<CanvasScaler>();
                }
                if (FreedomConfig.SpawnMenuScaleMode)
                {
                    Debug.Log("CF. Switch scale mode of Spawn menu to: Constant Pixel Size.");
                    ChangeDynamicInvPanelSize2.canv.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
                }
                else
                {
                    if (!FreedomConfig.SpawnMenuScaleMode)
                    {
                        Debug.Log("CF. Scale mode of Spawn menu is default: Scale With Screen Size.");
                        ChangeDynamicInvPanelSize2.canv.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                    }
                }
            }
        }

    }
}
