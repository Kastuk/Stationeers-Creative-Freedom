using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace StationeersCreativeFreedom
{
        [BepInPlugin("net.kastuk.stationeers.CreativeFreedom", "Stationeers Creative Freedom", "0.6.0.0")]
        public class StationeersCreativeFreedom : BaseUnityPlugin
        {
            public void Log(string line)
            {
                Debug.Log("[StationeersCreativeFreedom]: " + line);
            }

        private void Awake()
        {
            StationeersCreativeFreedom.Instance = this;
            this.Log("Trying to patch...");
            try
            {
                Harmony harmony = new Harmony("net.kastuk.stationeers.CreativeFreedom");
                harmony.PatchAll();
                this.Log("Well done!");
            }

            catch (Exception e)
            {
                this.Log("Patch Failed");
                this.Log(e.ToString());
            }
        }
        public static StationeersCreativeFreedom Instance;
    }
 }
