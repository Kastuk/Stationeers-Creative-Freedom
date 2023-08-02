using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BepInEx;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Bindings;

using TMPro;
using UI;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UI;

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

using Assets.Scripts.Serialization;

namespace CreativeFreedom
{
    class LightenUp
    {
        //[HarmonyPatch(typeof(Assets.Scripts.UI.MainMenu), "Awake")]
        //public class NoMenuScene
        //{
        //    [UsedImplicitly]
        //    public static void Postfix()
        //    {
        //        if (FreedomConfig.EnlightenMenu)
        //        {GameObject menupiece1 = GameObject.Find("MenuSceneMars01").GetComponentInChildren<MainMenu>(true);
        //            //need to disable animationclips MenuSceneMars01 and LanderMainMenu
        //        }
        //    }
        //}

        [HarmonyPatch(typeof(Stationpedia), "Awake")]
        public class NoStationpedia
        {
            [UsedImplicitly]
            public static bool Prefix()
            {
                if (FreedomConfig.NoStationpedia)
                    return false;
                else return true;
            }
        }
        [HarmonyPatch(typeof(Stationpedia), "Initialize")]
        public class NoStationpedia2
        {
            [UsedImplicitly]
            public static bool Prefix()
            {
                if (FreedomConfig.NoStationpedia)
                    return false;
                else return true;
            }
        }

    }
}
