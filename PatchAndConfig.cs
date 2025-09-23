using System;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;

using Assets.Scripts.UI;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts;
using Assets.Scripts.GridSystem;
using Assets.Scripts.Inventory;
using Assets.Scripts.Objects;
using Assets.Scripts.Objects.Entities;
using Assets.Scripts.Objects.Items;
using Assets.Scripts.UI;
using BepInEx;
using BepInEx.Bootstrap;
using HarmonyLib;
using UnityEngine;


namespace CreativeFreedom
{
    [BepInPlugin("Kastuk.CreativeFreedom", "Creative Freedom", "2025.09.23")]
    public class CreativeFreedom : BaseUnityPlugin
    {
        public static CreativeFreedom Instance;

        public static KeyCode UnlimitHold;// = KeyCode.X;
        public static KeyCode UnlimitSwitch;// = KeyCode.None;
        public static KeyCode ProtectThis;// = KeyCode.B;
        //public static KeyCode UnprotectThis;// = KeyCode.N;

        public static void Log(string line, bool forced = false)
        {
            if (FreedomConfig.Debug || forced)
            Debug.Log("[CreativeFreedom]: " + line);
        }

        private void Awake()
        {
            CreativeFreedom.Instance = this;
            CreativeFreedom.Log("I try to break free...", true);
            try
            {
                Harmony harmony = new Harmony("Kastuk.CreativeFreedom");
                harmony.PatchAll();
                CreativeFreedom.Log("For Liberty of Creativity!", true);
                FreedomConfig.Bind(this);
                KeyManager.OnControlsChanged += new KeyManager.Event(ControlsChangedEvent);
                //BindValidate.ParseKey();
                BindValidate.CheckDoorSep();
            }
            catch (Exception ex)
            {
                CreativeFreedom.Log("There's less freedom these days...", true);
                CreativeFreedom.Log(ex.ToString(), true);
            }
        }

        private void ControlsChangedEvent()
        {
            UnityEngine.Debug.Log("Keybinding Controls changed");

            UnlimitHold = KeyManager.GetKey("Unlimits Hold");
            UnlimitSwitch = KeyManager.GetKey("Unlimits Switch");
            ProtectThis = KeyManager.GetKey("Protect This");
        }
    }

    public static class FreedomConfig
    {
        public static void Bind(CreativeFreedom cf)
        {
            //FreedomConfig.EnlightenMenu = cf.Config.Bind<bool>("Visual", "EnlightenMenu", false, "Disable main menu scene to reduce memory load.").Value;
            FreedomConfig.UnlockCollisions = cf.Config.Bind<bool>("Building", "UnlockCollisions", true, "Main feature. Unlock collision checks for structures to collide freely (cannot merge full-grid frames and claddings for now). While disabled you can unlock these limits by special keys from ingame control settings").Value;
            //FreedomConfig.KeyUnlockCollisions = cf.Config.Bind<string>("Building", "HoldKeyUnlock", "X", "Key to hold on planning building state to switch collision limits. Default is X (can set to LeftControl and such).").Value;
            FreedomConfig.UnlockRotations = cf.Config.Bind<bool>("Building", "UnlockRotations", false, "Unlock placement rotations. Not work properly with SmartRotation by C for now.").Value;
            //FreedomConfig.SkipBlockedGrid = cf.Config.Bind<bool>("Building", "SkipBlockedRoom", true, "Evade check of blocked grid for full-frame-structures, which usually will remove one of colliding structures after loading the game.").Value;
            FreedomConfig.MaxBuildState = cf.Config.Bind<bool>("Building", "MaxBuildState", true, "Only for Creative mode. Only for Authoring tool. Spawn fully completed constructions.").Value;
            FreedomConfig.DoorSeparator = cf.Config.Bind<string>("Building", "DoorLabelSeparator", "@@", "This string in doorname will separate labels for sides. Default is @@").Value;


            FreedomConfig.RenameAll = cf.Config.Bind<bool>("Tools", "RenameAll", true, "Labeller can rename anything.").Value;

            FreedomConfig.MineCompletionTime = cf.Config.Bind("Tools", "MineCompletionTimeMod", 1f, new ConfigDescription("Modifier for digging time of all drills, 1.0 is no change, lower is faster.", new AcceptableValueRange<float>(0.08f, 5f)));
            //FreedomConfig.MineCompletionTime = cf.Config.Bind("Tools", "MineCompletionTime", new AcceptableValueRange<float>(0.08f, 0.5f) , "Modifier for digging time, lower is faster. 0.08 is low enough to got 0.96 s per voxel. Vanilla is 0.12 .").Value;//"This is time which spend on digging with mining drill. Vanilla value is 0.12, lesser is faster, min clamped to 0.09 to prevent glitching.").Value;
            
            //FreedomConfig.SpawnMenuScaleMode = cf.Config.Bind<bool>("Visual", "SpawnMenuScaleMode", true, "Only for Creative mode. Let spawn menu be constant size at wide screens.").Value;
            FreedomConfig.NVLight = cf.Config.Bind<bool>("Visual", "NightVisionLight", true, "Only for Creative mode. Let built-in night vision (key N) be clean and free for all.").Value;
            FreedomConfig.FOVZoom = cf.Config.Bind<bool>("Visual", "FOVZoom", true, "Only for Creative mode. Let you zoom far or wide with Field of View keys.").Value;
            FreedomConfig.ColoredLight = cf.Config.Bind<bool>("Visual", "Colored Light", true, "Light of the headlamp will be colored.").Value;


            FreedomConfig.JetpackSwitcher = cf.Config.Bind<bool>("Jetpack", "JetpackSwitcher", true, "Switch jetpack changes.").Value;
            FreedomConfig.JetpackMaxHeight = cf.Config.Bind<float>("Jetpack", "JetpackMaxHeight", 50f, "This is maximum height of jetpack above ground level. Vanilla value is 10.0").Value;
            FreedomConfig.JetpackModSpeed = cf.Config.Bind<float>("Jetpack", "JetpackModSpeed", 5f, "Only for Creative mode. By hold Shift you will modify speed by that value. Stolen idea from FuelJetpack mod.").Value;
            FreedomConfig.InfiniteJetpack = cf.Config.Bind<bool>("Jetpack", "InfiniteJetpack", true, "Only for Creative mode. Let jetpack work without fuel and with zero emmission.").Value;

            FreedomConfig.NoBreathSound = cf.Config.Bind<bool>("Other", "NoBreathSound", false, "Disable stress, exertion and jumping breathing sounds.").Value;
            FreedomConfig.AllSpawnable = cf.Config.Bind<bool>("Other", "AllSpawnable", false, "Return old things into spawnables, show em in Stationpedia too.").Value;
            FreedomConfig.Experimental = cf.Config.Bind<bool>("Other", "Experimental", true, "Unfinished features for testing: torpedo rack in furniture kit to use as shelf for tanks.").Value;
            FreedomConfig.Debug = cf.Config.Bind<bool>("Other", "Debug", true, "Turn on additional logs.").Value;

            //FreedomConfig.DefaultLoadWorld = cf.Config.Bind<bool>("World", "DefaultLoadWorld", true, "If save load did not find the world name, replace it with Mars to load something at least.").Value;

            // FreedomConfig.WindWingDamageMod = cf.Config.Bind<float>("Test", "WindWingDamageMod", 0.5f, "Damage reductor for wind generator wings impact on human.").Value;
        }

        //public static bool EnlightenMenu = false; //some error with SceneManagement at start is bugging me

        public static bool SpawnMenuScaleMode = true;

        public static bool NVLight = true;

        public static bool FOVZoom = true;

        public static bool ColoredLight = true;


        //public static string KeyUnlockCollisions = "X";
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

        public static bool NoBreathSound = false;
        public static bool AllSpawnable = false;
        public static bool Experimental = false;

        public static bool Debug = true;

        //public static bool DefaultLoadWorld = true;
        // public static float WindWingDamageMod = 0.5f;
    }

    public static class BindValidate
    {
        //public static KeyCode HoldLimitsKey = KeyCode.X;
        // public static Char DoorSep = '@';
        public static string DoorSep = "@@";
        //public static KeyCode switcher = KeyCode.Z;

        //public static void ParseKey()
        //{
        //    KeyCode key1 = HoldLimitsKey;
        //    //KeyCode key2 = switcher;
        //    try
        //    {
        //        key1 = (KeyCode)Enum.Parse(typeof(KeyCode), FreedomConfig.KeyUnlockCollisions);
        //    }
        //    catch (Exception)
        //    {
        //        Debug.Log("Wrong KeyCode name in Freedom config: " + FreedomConfig.KeyUnlockCollisions + ". Return to default X key");
        //        FreedomConfig.KeyUnlockCollisions = "X";
        //    }
        //    HoldLimitsKey = key1;
        //}

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

    [HarmonyPatch(typeof(GameManager), nameof(GameManager.Update))]//, new Type[] { typeof(bool) })]
    class CheckKeysPressed
    {
        // static bool warn = true;
        // static bool warn2 = true;
        static public bool UnSwitch = false;

        static void Postfix()
        {

            if (KeyManager.GetButtonDown(CreativeFreedom.UnlimitSwitch))
            {
                UnSwitch = !UnSwitch;
            }
        }
    }


    class Controls
    {
        /* Custom shortcut key binding injection is done after KeyManager.SetupKeyBindings() method is 
     * called; this way, we can get our custom new bindings saved/load by the game during the 
     * controls initialisation without needing any extra file access.
     */
        [HarmonyPatch(typeof(KeyManager), "SetupKeyBindings")]
        class ControlsInjectBindingGroup
        {
            static void Postfix()
            {
                // We need to add a custom control group for the keys to be attached to, and create 
                // the Lookout reference.
                UnityEngine.Debug.Log("Adding custom Controls group of Creative Freedom");
                ControlsGroup controlsGroup111 = new ControlsGroup("Creative Freedom");
                KeyManager.AddGroupLookup(controlsGroup111);

                // We will add the custom keys with default values to the KeyItem list using our new
                // created control group, however this method -due to accesibility of the class method-
                // will change the current ControlGroup name.

                ControlsInjectBindingGroup.AddKey("Unlimits Hold", KeyCode.X, controlsGroup111, false);
                ControlsInjectBindingGroup.AddKey("Unlimits Switch", KeyCode.None, controlsGroup111, false);
                ControlsInjectBindingGroup.AddKey("Protect This", KeyCode.B, controlsGroup111, false);

                //ShortcutInjectBindingGroup.AddKey("Zoop hold", KeyCode.Z, controlsGroup1, false);
                //ShortcutInjectBindingGroup.AddKey("Zoop switch", KeyCode.Mouse2, controlsGroup1, false);


                // TODO ADD other tools
                //ShortcutInjectBindingGroup.AddKey("Weapon", KeyCode.None, controlsGroup1, false);
                //ShortcutInjectBindingGroup.AddKey("Water", KeyCode.None, controlsGroup1, false);


                // We need to restore the name of the control group back to its correct string
                //controlsGroup1.Name = "Zoop";
                //if (KeyManager.OnControlsChanged != null)
                //{
                //  KeyManager.OnControlsChanged();
                //}
                ControlsAssignment.RefreshState();
            }

            /* Custom method to add keys to a ControlGroup. We 'hijack' the control group lookup function 
             * that will also save the name of they key in the list for us
             */
            private static void AddKey(string assignmentName,
                KeyCode keyCode,
                ControlsGroup controlsGroup,
                bool hidden = false
                )
            {
                // This is just because of the accessibility to change the assigned name, we use 
                // the control group for that.
                //controlsGroup.Name = assignmentName;
                //KeyManager.AddGroupLookup(controlsGroup);
                //var foo = Traverse.Create<KeyManager>().Field("_controlsGroupLookup").GetValue<Dictionary<string, ControlsGroup>>();
                var controlsLookupList = Traverse.Create(typeof(KeyManager)).Field("_controlsGroupLookup").GetValue() as Dictionary<string, ControlsGroup>; //read static field
                controlsLookupList[assignmentName] = controlsGroup;
                //Traverse.Create<KeyManager>().Field("_controlsGroupLookup").SetValue(foo);
                Traverse.Create(typeof(KeyManager)).Field("_controlsGroupLookup").SetValue(controlsLookupList); //for static fields

                // Now Create the key, add its looup string name, and save it in the allkeys list, to ensure
                // is being saved/load by the game config initialisation function.
                KeyItem keyItem = new KeyItem(assignmentName, keyCode, hidden);
                KeyManager.KeyItemLookup[assignmentName] = keyItem;
                //KeyManager.KeyItemLookup.Add(assignmentName, keyItem);
                KeyManager.AllKeys.Add(keyItem);
                // Debug.Log("Added key " + assignmentName);
            }
        }
    }

}
