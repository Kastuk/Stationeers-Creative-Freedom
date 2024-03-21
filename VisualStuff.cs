using System;
using Assets.Scripts.UI;
using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

namespace CreativeFreedom
{
    class VisualStuff
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

        [HarmonyPatch(typeof(DynamicInvPanel), "Initialize")] //make spawn menu bigger for unusual monitors
        public class ChangeDynamicInvPanelSize2
        {
            private static CanvasScaler canv;

            [UsedImplicitly]
            public static void Postfix(DynamicInvPanel __instance)
            {
                if (ChangeDynamicInvPanelSize2.canv == null)
                {
                    ChangeDynamicInvPanelSize2.canv = __instance._canvas.GetComponent<CanvasScaler>();
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
