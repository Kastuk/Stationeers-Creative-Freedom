using System;
using Assets.Scripts;
using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;

namespace CreativeFreedom
{
    public class TelescopicFOV
    {
        [HarmonyPatch(typeof(CameraController), nameof(CameraController.ManagerAwake))]

        internal class Camera_FOV_Limits
        {
            //[HarmonyPatch(nameof(CameraController.ManagerAwake))]
            [UsedImplicitly]
            [HarmonyPostfix]
            private static void MinMaxFov(CameraController __instance)
            {
                if (FreedomConfig.FOVZoom)
                {
                    if (WorldManager.Instance.GameMode == GameMode.Creative)
                    {
                        __instance.MinFoV = 3f; //to not collide with glasses zoom in Survivalities
                    }
                    __instance.MaxFoV = 160f;
                }
            }
        }

        internal class Camera_FOV_Increase_Speed
        {
            [HarmonyPatch(nameof(CameraController.FovIncrease))]
            [UsedImplicitly]
            [HarmonyPrefix]
            private static bool ModifierKeyZoomPlus(CameraController __instance)
            {
                if (FreedomConfig.FOVZoom)
                {
                    bool key = Input.GetKey(KeyCode.RightControl);
                    bool result;
                    if (key)
                    {
                        float num = CameraController.CurrentCamera.fieldOfView;
                        bool flag = num < __instance.MaxFoV;
                        if (flag)
                        {
                            bool flag2 = num >= __instance.MaxFoV - 6f;
                            if (flag2)
                            {
                                num = __instance.MaxFoV;
                            }
                            else
                            {
                                num += 6f;
                            }
                        }
                        CameraController.SetFieldOfView(num);
                        result = false;
                    }
                    else
                    {
                        result = true;
                    }
                    return result;
                }
                else return true;
            }
        }
        internal class Camera_FOV_Decrease_Speed
        {
            [HarmonyPatch(nameof(CameraController.FovDecrease))]
            [UsedImplicitly]
            [HarmonyPrefix]
            private static bool ModifierKeyZoomMinus(CameraController __instance)
            {
                if (FreedomConfig.FOVZoom)
                {
                    bool key = Input.GetKey(KeyCode.RightControl);
                    bool result;
                    if (key)
                    {
                        float num = CameraController.CurrentCamera.fieldOfView;
                        bool flag = num > __instance.MinFoV;
                        if (flag)
                        {
                            bool flag2 = num <= __instance.MinFoV + 6f;
                            if (flag2)
                            {
                                num = __instance.MinFoV;
                            }
                            else
                            {
                                num -= 6f;
                            }
                        }
                        CameraController.SetFieldOfView(num);
                        result = false;
                    }
                    else
                    {
                        result = true;
                    }
                    return result;
                }
                else return true;
            }
        }
    }
}
