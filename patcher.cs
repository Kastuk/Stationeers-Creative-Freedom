using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace CreativeFreedom
{
        [BepInPlugin("net.kastuk.stationeers.CreativeFreedom", "Creative Freedom", "0.8.9")]
        public class CreativeFreedom : BaseUnityPlugin
        {
            public void Log(string line)
            {
                Debug.Log("[CreativeFreedom (01/08/2023)]: " + line);
            }



        private void Awake()
        {
            CreativeFreedom.Instance = this;
            this.Log("I try to break free...");
            try
            {
                Harmony harmony = new Harmony("net.kastuk.stationeers.CreativeFreedom");
                harmony.PatchAll();
                this.Log("Freedom!");
                FreedomConfig.Bind(this);
            }

            catch (Exception e)
            {
                this.Log("Freedom is broken.");
                this.Log(e.ToString());
            }
        }

        public static CreativeFreedom Instance;
    }

    public static class FreedomConfig //Copy from Beef's Game Fixes
    {
        //public static bool EnlightenMenu = false;
        //public static bool NoStationpedia = false;

        public static bool MaxBuildState = true;
        public static float MineCompletionTime = 0.12f;
        public static float JetpackMaxHeight = 100.0f;
        public static float JetpackHeavySpeed = 10.0f;

        public static void Bind(CreativeFreedom cf)
        {
            //EnlightenMenu = cf.Config.Bind("General", "EnlightenMenu", false, "Disable main menu scenes to reduce loading.").Value;
           // NoStationpedia = cf.Config.Bind("General", "NoStationpedia", false, "Disable Stationpedia to reduce loading.").Value;
            MaxBuildState = cf.Config.Bind("General", "MaxBuildState", true, "Spawn fully completed constructions with Authoring tool.").Value;
            MineCompletionTime = cf.Config.Bind("General", "MineCompletionTime", 0.12f, "This is time spend on digging with mining drill. Vanilla value is 0.12, lesser is faster, too fast will glitch.").Value;
            JetpackMaxHeight = cf.Config.Bind("General", "JetpackMaxHeight", 100.0f, "This is maximum height of jetpack above ground level. Vanilla value is 10.0").Value;
            JetpackHeavySpeed = cf.Config.Bind("General", "JetpackHeavySpeed", 10.0f, "This is basic speed of hard jetpack.").Value;
        }
    }
}
