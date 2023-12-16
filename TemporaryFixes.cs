using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using JetBrains.Annotations;

using Assets.Scripts;
using Assets.Scripts.UI;
using Assets.Scripts.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Internal;
using UnityEngine.Playables;
using UnityEngine.Scripting;
using UnityEngine;

namespace CreativeFreedom
{
    class TemporaryFixes
    {
        [HarmonyPatch(typeof(WorldSetting), "Find", new Type[] { typeof(int) })]
        [UsedImplicitly]
        internal class WorldSettings_Find_Patch
        {
            private static bool Prefix(int stringToHash, ref WorldSetting __result)
            {
                if (FreedomConfig.DefaultLoadWorld)
                {
                    WorldSetting result;
                    if (WorldSetting._worldSettingLookup.TryGetValue(stringToHash, out result))
                    {
                        __result = result;
                        return false;
                    }
                    else if (WorldSetting._worldSettingLookup.TryGetValue(Animator.StringToHash("Mars"), out result))
                    {
                        Debug.Log("Cannot find worldsetting with name which in this savegame, so loading boring Mars instead.");
                        Debug.Log("Try to open your world.xml and change worldname to something else of presented worlds, as local worldsettings works no more.");

                        __result = result;
                        return false;
                    }
                    else {
                        __result = null;
                        Debug.Log("There's no Mars?");
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
