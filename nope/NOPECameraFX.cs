using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using BepInEx;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Networking;
using JetBrains.Annotations;

using Assets.Scripts;
using Assets.Scripts.UI;
using Assets.Scripts.Objects;
using Assets.Scripts.Objects.Structures;
using Assets.Scripts.Objects.Pipes;
using Assets.Scripts.Objects.Electrical;

using Assets.Scripts.GridSystem;
using Assets.Scripts.Networking;

using Assets.Scripts.Objects.Entities;

using Assets.Scripts.Inventory;
using Objects;
using Objects.Items;
using Assets.Scripts.Objects.Items;

using Assets.Scripts.Util;

using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;
using System.Collections;

namespace StationeersCreativeFreedom
{
    [HarmonyPatch(typeof(CameraFilterPack_Atmosphere_Rain_Pro_3D), "Update")]
    internal class Rain_Patch
    {
        [UsedImplicitly]
        private static void Prefix(CameraFilterPack_Atmosphere_Rain_Pro_3D __instance)
        {
            if (KeyManager.GetButtonUp(KeyMap.FovReset))
            {
                __instance.enabled = true;
            }
            else { __instance.enabled = false; }
            //else
            //{ __instance.MineCompletionTime = __instance.MineCompletionTime; 
            //__instance.MineAmount = __instance.MineAmount;}

        }
    }

}
