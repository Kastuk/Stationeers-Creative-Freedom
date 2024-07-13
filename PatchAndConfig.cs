using System;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

namespace CreativeFreedom
{
    [BepInPlugin("Kastuk.CreativeFreedom", "Creative Freedom", "2024.07.11")]
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
                Harmony harmony = new Harmony("Kastuk.CreativeFreedom");
                harmony.PatchAll();
                this.Log("For Liberty of Creativity!");
                FreedomConfig.Bind(this);
                BindValidate.ParseKey();
                BindValidate.CheckDoorSep();
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
            FreedomConfig.UnlockCollisions = cf.Config.Bind<bool>("Building", "UnlockCollisions", true, "Main feature. Unlock collision checks for structures to collide freely (cannot merge full-grid frames and claddings for now). While disabled you can unlock these limits by holding selected key").Value;
            FreedomConfig.KeyUnlockCollisions = cf.Config.Bind<string>("Building", "HoldKeyUnlock", "X", "Key to hold on planning building state to switch collision limits. Default is X (can set to LeftControl and such).").Value;
            FreedomConfig.UnlockRotations = cf.Config.Bind<bool>("Building", "UnlockRotations", false, "Unlock placement rotations. Not work properly with SmartRotation by C for now.").Value;
            //FreedomConfig.SkipBlockedGrid = cf.Config.Bind<bool>("Building", "SkipBlockedRoom", true, "Evade check of blocked grid for full-frame-structures, which usually will remove one of colliding structures after loading the game.").Value;
            FreedomConfig.MaxBuildState = cf.Config.Bind<bool>("Building", "MaxBuildState", true, "Only for Creative mode. Only for Authoring tool. Spawn fully completed constructions.").Value;
            FreedomConfig.DoorSeparator = cf.Config.Bind<string>("Building", "DoorLabelSeparator", "@@", "This symbol in doorname will separate labels for sides. Default is @").Value;


            FreedomConfig.RenameAll = cf.Config.Bind<bool>("Tools", "RenameAll", true, "Labeller can rename anything.").Value;

            FreedomConfig.MineCompletionTime = cf.Config.Bind("Tools", "MineCompletionTime", 0.12f, new ConfigDescription("Modifier for digging time, lower is faster. 0.08 is minimal to got 0.96 s per voxel. Vanilla is 0.12 .", new AcceptableValueRange<float>(0.08f, 0.5f)));
            //FreedomConfig.MineCompletionTime = cf.Config.Bind("Tools", "MineCompletionTime", new AcceptableValueRange<float>(0.08f, 0.5f) , "Modifier for digging time, lower is faster. 0.08 is low enough to got 0.96 s per voxel. Vanilla is 0.12 .").Value;//"This is time which spend on digging with mining drill. Vanilla value is 0.12, lesser is faster, min clamped to 0.09 to prevent glitching.").Value;
            
            FreedomConfig.SpawnMenuScaleMode = cf.Config.Bind<bool>("Visual", "SpawnMenuScaleMode", true, "Only for Creative mode. Let spawn menu be constant size at wide screens.").Value;
            FreedomConfig.NVLight = cf.Config.Bind<bool>("Visual", "NightVisionLight", true, "Only for Creative mode. Let built-in night vision (key N) be clean and free for all.").Value;
            FreedomConfig.FOVZoom = cf.Config.Bind<bool>("Visual", "FOVZoom", true, "Only for Creative mode. Let you zoom far or wide with Field of View keys.").Value;
            FreedomConfig.ColoredLight = cf.Config.Bind<bool>("Visual", "Colored Light", true, "Light of the headlamp will be colored.").Value;


            FreedomConfig.JetpackSwitcher = cf.Config.Bind<bool>("Jetpack", "JetpackSwitcher", true, "Switch jetpack changes.").Value;
            FreedomConfig.JetpackMaxHeight = cf.Config.Bind<float>("Jetpack", "JetpackMaxHeight", 50f, "This is maximum height of jetpack above ground level. Vanilla value is 10.0").Value;
            FreedomConfig.JetpackModSpeed = cf.Config.Bind<float>("Jetpack", "JetpackModSpeed", 5f, "Only for Creative mode. By hold Shift you will modify speed by that value. Stolen idea from FuelJetpack mod.").Value;
            FreedomConfig.InfiniteJetpack = cf.Config.Bind<bool>("Jetpack", "InfiniteJetpack", true, "Only for Creative mode. Let jetpack work without fuel and with zero emmission.").Value;

            FreedomConfig.NoBreathSound = cf.Config.Bind<bool>("Other", "NoBreathSound", true, "Disable stress, exertion and jumping breathing sounds.").Value;

            //FreedomConfig.DefaultLoadWorld = cf.Config.Bind<bool>("World", "DefaultLoadWorld", true, "If save load did not find the world name, replace it with Mars to load something at least.").Value;

            // FreedomConfig.WindWingDamageMod = cf.Config.Bind<float>("Test", "WindWingDamageMod", 0.5f, "Damage reductor for wind generator wings impact on human.").Value;
        }

        //public static bool EnlightenMenu = false; //some error with SceneManagement at start is bugging me

        public static bool SpawnMenuScaleMode = true;

        public static bool NVLight = true;

        public static bool FOVZoom = true;

        public static bool ColoredLight = true;


        public static string KeyUnlockCollisions = "X";
        public static bool UnlockCollisions = true;

        public static bool UnlockRotations = false; //todo switch rotation limits at placement type state with an transpiler

        public static bool MaxBuildState = true;

        public static string DoorSeparator = "@@";
        //public static bool SkipBlockedGrid = true;


        public static bool RenameAll = true;

        public static ConfigEntry<float> MineCompletionTime;// = 0.12f;


        public static bool JetpackSwitcher = true;

        public static float JetpackMaxHeight = 50f;

        public static float JetpackModSpeed = 5f;

        public static bool InfiniteJetpack = true;

        public static bool NoBreathSound = true;

        //public static bool DefaultLoadWorld = true;
        // public static float WindWingDamageMod = 0.5f;
    }

    public static class BindValidate
    {
        public static KeyCode HoldLimitsKey = KeyCode.X;
        // public static Char DoorSep = '@';
        public static string DoorSep = "@@";
        //public static KeyCode switcher = KeyCode.Z;

        public static void ParseKey()
        {
            KeyCode key1 = HoldLimitsKey;
            //KeyCode key2 = switcher;
            try
            {
                key1 = (KeyCode)Enum.Parse(typeof(KeyCode), FreedomConfig.KeyUnlockCollisions);
            }
            catch (Exception)
            {
                Debug.Log("Wrong KeyCode name in Freedom config: " + FreedomConfig.KeyUnlockCollisions + ". Return to default X key");
                FreedomConfig.KeyUnlockCollisions = "X";
            }
            HoldLimitsKey = key1;
        }

        public static void CheckDoorSep()
        {
            
            string sep1 = DoorSep;

            if (FreedomConfig.DoorSeparator != null && FreedomConfig.DoorSeparator.Length > 0)
            {
                sep1 = FreedomConfig.DoorSeparator;//(string)Enum.Parse(typeof(string), FreedomConfig.DoorSeparator);
            }
            else
            {
                Debug.Log("Null or empty separator string in Freedom config. Return to default \"@@\"");
                FreedomConfig.DoorSeparator = "@@";
            }
            DoorSep = sep1;
        }
    }
}
