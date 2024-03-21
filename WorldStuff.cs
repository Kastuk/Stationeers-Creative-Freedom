//using System;
//using HarmonyLib;
//using JetBrains.Annotations;
//using UnityEngine;
//using Assets.Scripts;
//using Assets.Scripts.Objects;
//using Assets.Scripts.Objects.Entities;
//using Assets.Scripts.Objects.Items;
//using Assets.Scripts.Objects.Weapons;
//using Assets.Scripts.Inventory;
//using Assets.Scripts.Util;

//namespace CreativeFreedom
//{
//    internal class WorldStuff
//    {
//        [HarmonyPatch(typeof(WorldSetting), "Find", new Type[]
//        {
//            typeof(int)
//        })]
//        [UsedImplicitly]
//        internal class WorldSettings_NotFind_Then_Mars
//        {
//            private static bool Prefix(int stringToHash, ref WorldSetting __result)
//            {
//                bool defaultLoadWorld = FreedomConfig.DefaultLoadWorld;
//                bool result;
//                if (defaultLoadWorld)
//                {
//                    WorldSetting worldSetting;
//                    bool flag = WorldSetting._worldSettingLookup.TryGetValue(stringToHash, out worldSetting);
//                    if (flag)
//                    {
//                        __result = worldSetting;
//                        result = false;
//                    }
//                    else
//                    {
//                        bool flag2 = WorldSetting._worldSettingLookup.TryGetValue(Animator.StringToHash("Mars"), out worldSetting);
//                        if (flag2)
//                        {
//                            Debug.Log("Cannot find worldsetting with name which in this savegame, so loading boring Mars instead.");
//                            Debug.Log("Try to open your world.xml and change worldname to something else of presented worlds, as local worldsettings works no more.");
//                            __result = worldSetting;
//                            result = false;
//                        }
//                        else
//                        {
//                            __result = null;
//                            Debug.Log("There's no Mars?");
//                            result = false;
//                        }
//                    }
//                }
//                else
//                {
//                    result = true;
//                }
//                return result;
//            }
//        }
//    }
//}
