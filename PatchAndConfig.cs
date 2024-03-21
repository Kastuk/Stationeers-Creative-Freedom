using System;
using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace CreativeFreedom
{
    [BepInPlugin("CreativeFreedom", "Creative Freedom", "20.03.2024")]
    public class CreativeFreedom : BaseUnityPlugin
    {
        public static CreativeFreedom Instance;

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
                this.Log("For Liberty of Creativity!");
                FreedomConfig.Bind(this);
            }
            catch (Exception ex)
            {
                this.Log("There's less freedom these days...");
                this.Log(ex.ToString());
            }
        }
    }

    public static class FreedomConfig
    {
        public static void Bind(CreativeFreedom cf)
        {
            //FreedomConfig.EnlightenMenu = cf.Config.Bind<bool>("Visual", "EnlightenMenu", false, "Disable main menu scene to reduce memory load.").Value;
            FreedomConfig.SpawnMenuScaleMode = cf.Config.Bind<bool>("Visual", "SpawnMenuScaleMode", true, "Let spawn menu be constant size at wide screens.").Value;
            FreedomConfig.NVLight = cf.Config.Bind<bool>("Visual", "NightVisionLight", true, "Let night vision be clean and free for all in creative mode.").Value;


            //FreedomConfig.SkipBlockedGrid = cf.Config.Bind<bool>("Building", "SkipBlockedRoom", false, "Evade check of blocked grid for full-frame-structures, which usually will remove one of colliding structures after loading the game.").Value;
            FreedomConfig.UnlockRotations = cf.Config.Bind<bool>("Building", "UnlockRotations", false, "Unlock placement rotations. Not work properly with SmartRotation by C for now.").Value;
            FreedomConfig.MaxBuildState = cf.Config.Bind<bool>("Building", "MaxBuildState", true, "Only for Authoring tool. Spawn fully completed constructions.").Value;

            FreedomConfig.MineCompletionTime = cf.Config.Bind<float>("Tools", "MineCompletionTime", 0.12f, "This is time which spend on digging with mining drill. Vanilla value is 0.12, lesser is faster, min clamped to 0.09 to prevent glitching.").Value;

            FreedomConfig.JetpackSwitcher = cf.Config.Bind<bool>("Jetpack", "JetpackSwitcher", true, "Switch jetpack changes.").Value;
            FreedomConfig.JetpackMaxHeight = cf.Config.Bind<float>("Jetpack", "JetpackMaxHeight", 50f, "This is maximum height of jetpack above ground level. Vanilla value is 10.0").Value;
            FreedomConfig.JetpackModSpeed = cf.Config.Bind<float>("Jetpack", "JetpackModSpeed", 5f, "Only for Creative mode. By hold Shift you will modify speed by that value. Stolen idea from FuelJetpack mod.").Value;
            FreedomConfig.InfiniteJetpack = cf.Config.Bind<bool>("Jetpack", "InfiniteJetpack", true, "Only for Creative mode. Let jetpack work without fuel and with zero emmission.").Value;
            //FreedomConfig.DefaultLoadWorld = cf.Config.Bind<bool>("World", "DefaultLoadWorld", true, "If save load did not find the world name, replace it with Mars to load something at least.").Value;

           // FreedomConfig.WindWingDamageMod = cf.Config.Bind<float>("Test", "WindWingDamageMod", 0.5f, "Damage reductor for wind generator wings impact on human.").Value;
        }

        //public static bool EnlightenMenu = false; //some error with SceneManagement at start is bugging me

        public static bool SpawnMenuScaleMode = true;

        public static bool NVLight = true;


        public static bool UnlockRotations = false;

        public static bool MaxBuildState = true;

        //public static bool SkipBlockedGrid = false;


        public static float MineCompletionTime = 0.12f;


        public static bool JetpackSwitcher = true;

        public static float JetpackMaxHeight = 50f;

        public static float JetpackModSpeed = 5f;

        public static bool InfiniteJetpack = true;

        //public static bool DefaultLoadWorld = true;
       // public static float WindWingDamageMod = 0.5f;
    }
}
