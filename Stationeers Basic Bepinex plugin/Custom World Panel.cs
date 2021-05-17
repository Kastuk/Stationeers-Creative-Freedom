//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Linq;
//using System.Text;

//using BepInEx;
//using HarmonyLib;
//using UnityEngine;
//using UnityEngine.Networking;
//using JetBrains.Annotations;

//using Assets.Scripts;
//using Assets.Scripts.UI;
//using Assets.Scripts.Objects;
//using Assets.Scripts.Objects.Structures;
//using Assets.Scripts.Objects.Pipes;
//using Assets.Scripts.Objects.Electrical;

//using Assets.Scripts.GridSystem;
//using Assets.Scripts.Networking;

//using Assets.Scripts.Objects.Entities;

//using Assets.Scripts.Inventory;
//using Objects;
//using Objects.Items;
//using Assets.Scripts.Objects.Items;

//using Assets.Scripts.Util;

//using System.Runtime.CompilerServices;
//using System.Text.RegularExpressions;
//using TMPro;
//using UnityEngine.Events;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;

//using System;
//using System.Diagnostics;
//using System.Globalization;
//using System.Runtime.InteropServices;
//using System.Security.Permissions;
//using System.Threading;
//using System.Collections;

///*
//Thanks to guiding of the TurkeyKittin! 
//And other guide of RoboPhred.
//And inspiration from DevCo constructions.
//And Inaki's exercises!
//*/

//namespace StationeersCreativeFreedom
//{

//    [HarmonyPatch(typeof(PanelCustomWorld), "GetTerrainGenerationData")]
//    internal class PanelCustomWorld_GetTerrainGenerationData_Patch
//    {
//        [UsedImplicitly]
//        private static void Prefix(PanelCustomWorld __instance, ref PanelCustomWorld __TerrainParemeters)
//        {
//            bool textishere = __instance.WorldDescriptionField.text == null;
//            if (!textishere)
//            {
//                var twodim = false;
//                float freq = 1f;
//                float ampl = 1f;
//                float size = 0f;

//                string desctext = ("1,0.4,1.6,2.4"); //__instance.WorldDescriptionField.text; 
//                try
//                {
//                    float parsed = float.Parse(desctext); //1,0.4,1.6,2.4 (twodim (1/0), freq, ampl, size)
//                }
//                catch
//                {
//                    twodim = false;
//                }
//                //var stringArray = new string[];
//                //var list = new List(stringArray);
//                // public List<TerrainHeighMapParameters> TerrasinParameters = new List<TerrainHeighMapParameters>();
//                //terrainPars.TwoDimensions =
//                //__instance.CustomWorldData.TerrainParemeters.InsertRange();
//                //public List<TerrainHeighMapParameters> terrainPars = new List<TerrainHeighMapParameters>();
//                PanelCustomWorld.CustomWorldData.TerrainParemeters.Clear();
//                PanelCustomWorld.terrainPars.Insert(0, twodim);


//                foreach (WorldManager.TerrainGenerationParameters terPars in WorldManager.Instance.TerrainHeighMapParameters)
//                {
//                    PanelCustomWorld.CustomWorldData.TerrainParemeters.Clear();
//                    terPars.Insert(1, 0);
//                    PanelCustomWorld.CustomWorldData.TerrainParemeters.Insert(0, twodim);
//                    PanelCustomWorld.CustomWorldData.TerrainParemeters.InsertRange(2, new float[] { freq, ampl, size });


//                }
//            }
//        }
//    }

//    [HarmonyPatch(typeof(PanelCustomWorld), "CreatePreview")]
//    internal class PanelCustomWorld_CreatePreview_Patch
//    {
//        [UsedImplicitly]
//        private static void Prefix(PanelCustomWorld __instance, ref PanelCustomWorld __TerrainParemeters)
//        {
//            bool textishere = __instance.WorldDescriptionField.text == null;
//            if (!textishere)
//            {
//               string desctext = __instance.WorldDescriptionField.text;//("false,0.4,1.6,2.4"); 

//                string[] terrapars = desctext.Split(',');
//                var twodim = Convert.ToBoolean(terrapars[0]); //1 to true?
//                var freq = Convert.ToSingle(terrapars[1]);
//                var ampl = Convert.ToSingle(terrapars[2]);
//                var size = Convert.ToSingle(terrapars[3]);

//                //var stringArray = new string[];
//                //var list = new List(stringArray);
//                // public List<TerrainHeighMapParameters> TerrasinParameters = new List<TerrainHeighMapParameters>();
//                //terrainPars.TwoDimensions =
//                //__instance.CustomWorldData.TerrainParemeters.InsertRange();
//                //public List<TerrainHeighMapParameters> terrainPars = new List<TerrainHeighMapParameters>();

//                //List<TerrainHeighMapParameters> TerrainParemeters = new List<TerrainHeighMapParameters>();
//                //PanelCustomWorld.CustomWorldData.TerrainParemeters.Clear();
//                PanelCustomWorld.CustomWorldData.TerrainParemeters.TwoDimensions = false;

//                //PanelCustomWorld.terrainPars.Insert(0, twodim);


//                foreach (WorldManager.TerrainGenerationParameters terPars in WorldManager.Instance.TerrainHeighMapParameters)
//                {
//                    PanelCustomWorld.CustomWorldData.TerrainParemeters.Clear();
//                    terPars.Insert(1, 0);
//                    PanelCustomWorld.CustomWorldData.TerrainParemeters.Insert(0, twodim);
//                    PanelCustomWorld.CustomWorldData.TerrainParemeters.InsertRange(2, new float[] { freq, ampl, size });


//                }
//            }
//        }
//    }

//    //[Serializable]
//    //[StructLayout(LayoutKind.Sequential)]
//    //public sealed class TerrainHeighMapParameters : ValueType
//    //{
//    //public bool TwoDimensions;

//    //// Token: 0x04001BD2 RID: 7122
//    //public float Frequency;

//    //// Token: 0x04001BD3 RID: 7123
//    //public float Amplitude;

//    //// Token: 0x04001BD4 RID: 7124
//    //public float Size;
//    //}

//}
