using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace Stationeers_BasicBepinexPlugin
{
        [BepInPlugin("net.kastuk.stationeers.BasicBepinexPlugin", "Stationeers Basic Bepinex Plugin", "0.1.0.0")]
        public class StationeersBasicBepinexPlugin : BaseUnityPlugin
        {
            public void Log(string line)
            {
                Debug.Log("[BasicBepinexPlugin]: " + line);
            }

        private void Awake()
        {
            StationeersBasicBepinexPlugin.Instance = this;
            this.Log("Hello World");
            try
            {
                Harmony harmony = new Harmony("net.kastuk.stationeers.BasicBepinexPlugin");
                harmony.PatchAll();
                this.Log("Patch succeeded");
            }

            catch (Exception e)
            {
                this.Log("Patch Failed");
                this.Log(e.ToString());
            }
        }
        public static StationeersBasicBepinexPlugin Instance;
    }
 }
