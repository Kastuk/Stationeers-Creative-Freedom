using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BepInEx;
using HarmonyLib;
using UnityEngine;
using Assets.Scripts;
using Assets.Scripts.Util;
using Assets.Scripts.Serialization;

namespace CreativeFreedom
{
        [BepInPlugin("CreativeFreedom", "Creative Freedom", "16.12.2023")]
        public class CreativeFreedom : BaseUnityPlugin
        {
            public void Log(string line)
            {
                Debug.Log("[CreativeFreedom]: " + line);
            }



        private void Awake()
        {
            CreativeFreedom.Instance = this;
            this.Log("I try to break free...");
            try
            {
                Harmony harmony = new Harmony("CreativeFreedom");
                harmony.PatchAll();
                //Singleton<GameManager>.Instance.MenuCutscene.MenuLite(FreedomConfig.EnlightenMenu);
                //MenuCutscene.MenuLite(FreedomConfig.EnlightenMenu);
                this.Log("For Liberty of Creativity!");
                FreedomConfig.Bind(this);
            }

            catch (Exception e)
            {
                this.Log("No freedom.");
                this.Log(e.ToString());
            }
        }

        public static CreativeFreedom Instance;
    }

    public static class FreedomConfig //Copy from Beef's Game Fixes
    {
        public static bool EnlightenMenu = false;
        //public static bool NoStationpedia = false;
        //public static bool NoFlares = true;
        public static bool SpawnMenuScaleMode = true;

        public static bool UnlockRotations = false;
        public static bool MaxBuildState = true;
        public static float MineCompletionTime = 0.12f;

        public static bool JetpackSwitcher = true;
        public static float JetpackMaxHeight = 50.0f;
        public static float JetpackModSpeed = 5.0f;
        public static bool InfiniteJetpack = true;

        public static bool DefaultLoadWorld = true;

        public static void Bind(CreativeFreedom cf)
        {

            EnlightenMenu = cf.Config.Bind("Visual", "EnlightenMenu", false, "Disable main menu scene to reduce memory load.").Value;
            // NoStationpedia = cf.Config.Bind("General", "NoStationpedia", false, "Disable Stationpedia to reduce loading.").Value;
            SpawnMenuScaleMode = cf.Config.Bind("Visual", "SpawnMenuScaleMode", true, "Let spawn menu be constant size at wide screens.").Value;
            //SpawnMenuScaleMode = cf.Config.Bind("Visual", "NoFlares", true, "Clean up sun flares effects like colored ghost balls and dust specks.").Value;


            UnlockRotations = cf.Config.Bind("Building", "UnlockRotations", false, "Unlock placement rotations. Not work with SmartRotation by C for now.").Value;
            MaxBuildState = cf.Config.Bind("Building", "MaxBuildState", true, "Only for Authoring tool. Spawn fully completed constructions.").Value;

            MineCompletionTime = cf.Config.Bind("Tools", "MineCompletionTime", 0.12f, "This is time spend on digging with mining drill. Vanilla value is 0.12, lesser is faster, too fast will glitch.").Value;

            JetpackSwitcher = cf.Config.Bind("Jetpack", "JetpackSwitcher", true, "Switch jetpack changes.").Value;
            JetpackMaxHeight = cf.Config.Bind("Jetpack", "JetpackMaxHeight", 50.0f, "This is maximum height of jetpack above ground level. Vanilla value is 10.0").Value;
            JetpackModSpeed = cf.Config.Bind("Jetpack", "JetpackModSpeed", 5.0f, "Only for Creative mode. By hold Shift you will modify speed by that value. Stolen idea from FuelJetpack mod.").Value;
            InfiniteJetpack = cf.Config.Bind("Jetpack", "InfiniteJetpack", true, "Only for Creative mode. Let jetpack work without fuel and with zero emmission.").Value;

            DefaultLoadWorld = cf.Config.Bind("World", "DefaultLoadWorld", true, "If save load did not find the world name, replace it with Mars to load something at least.").Value;
        }
    }
}
