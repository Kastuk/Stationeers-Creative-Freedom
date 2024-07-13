using System;
using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;

using Assets.Scripts.Inventory;
using Assets.Scripts.Localization2;
using Assets.Scripts;

using Assets.Scripts.Objects;
using Assets.Scripts.Objects.Items;

using System.Globalization;
using System.Text.RegularExpressions;
using Assets.Scripts.Networking;
using Assets.Scripts.UI;
using Assets.Scripts.Util;

using Assets.Scripts.Atmospherics;
using Assets.Scripts.GridSystem;
using Assets.Scripts.Networking;
using Assets.Scripts.Objects.Electrical;
using Assets.Scripts.Objects.Pipes;
using Assets.Scripts.Objects.Motherboards;
using Assets.Scripts.Objects.Structures;

using System.Collections.Generic;
using System.Linq;


using Assets.Scripts.Objects.Entities;

using Assets.Scripts.Serialization;
using Assets.Scripts.Util;
using Assets.Scripts.Networks;
using Networks;

using TMPro;

namespace CreativeFreedom
{
    class LabellerPlus
    {
        [HarmonyPatch(typeof(Thing), "AttackWith")] //rename PictureFrame by Labeller
                                                    // [HarmonyPatch(new Type[] { typeof(Attack), typeof(bool) })]
        internal class RenameAnything
        {
            [UsedImplicitly]//dunno what is for, something for simpler replacing of the field values.
            [HarmonyPrefix]
            public static bool AttackWithLabeller(Thing __instance, ref Thing.DelayedActionInstance __result, ref Attack attack, ref bool doAction)
            {
                if (FreedomConfig.RenameAll)
                {
                    
                        DynamicThing sourceItem = attack.SourceItem;
                        if (!sourceItem)
                        {
                            return true;
                        }
                        //DynamicThing sourceItem = attack.SourceItem;
                        Labeller labeller = attack.SourceItem as Labeller;
                        if (!labeller)
                        {
                            //__result = Custopics.ThingPatch.AttackWith(__instance, attack, doAction);
                            return true;
                        }

                    if (WorldManager.Instance.GameMode == GameMode.Creative) //__instance is CardboardBox (already in vanilla)
                    {
                        //TODO transpiler to patch all labeller renaming of logic devices as Thing AttackWith is overriden everywhere

                        //if (Input.GetKey(KeyMap.QuantityModifier))
                        //{
                        //    Thing.DelayedActionInstance delayedActionInstance2 = new Thing.DelayedActionInstance
                        //    {
                        //        Duration = 0f,
                        //        ActionMessage = "Set Setting"//GameStrings.DeviceManualInputWindow
                        //    };

                        //    if (WorldManager.Instance.GameMode != GameMode.Creative) //not work as renaming is overwritten in many devices
                        //    {
                        //        if (!labeller.OnOff)
                        //        {
                        //            __result = delayedActionInstance2.Fail(GameStrings.DeviceNotOn);
                        //        }
                        //        if (!labeller.IsOperable)
                        //        {
                        //            __result = delayedActionInstance2.Fail(GameStrings.DeviceNoPower);
                        //        }
                        //    }
                        //    //Device device = __instance as Device;
                        //    //if (!device || !device.CanLogicWrite(LogicType.Setting))
                        //    //{
                        //    //    __result = delayedActionInstance2.Fail("Haven't logic setting");
                        //    //}
                        //    ISetable setable = __instance as ISetable;
                        //    if (setable == null)
                        //    {
                        //        __result = delayedActionInstance2.Fail("Haven't logic setting"); 
                        //        //return false;
                        //    }
                        //    if (!doAction)
                        //    {
                        //        __result = delayedActionInstance2.Succeed();
                        //        return false;
                        //    }

                        //    labeller.Set(setable);
                        //    __result = delayedActionInstance2.Succeed();
                        //    return false;
                        //}
                        ////if (!doAction)
                        ////{
                        ////    __result = delayedActionInstance2;
                        ////    return false;
                        ////}
                        ////LabellerPlus.SetSetting(__instance, labeller);
                        //////Traverse.Create(pict).Field("PicturesToChooseFrom").SetValue(ChangePicture.ChangePicList(pict, list));
                        ////__result = delayedActionInstance2;
                        ////return false;

                        //else
                        //{ //add checks for  things groups and survival mode so ony creative can rename wires and walls
                        Thing.DelayedActionInstance delayedActionInstance = new Thing.DelayedActionInstance
                        {
                            Duration = 0f,
                            ActionMessage = ActionStrings.Rename
                        };

                        //if (WorldManager.Instance.GameMode != GameMode.Creative) //need to write into all overwrites
                        //{
                        if (!labeller.OnOff)
                        {
                            __result = delayedActionInstance.Fail(GameStrings.DeviceNotOn);
                        }
                        if (!labeller.IsOperable)
                        {
                            __result = delayedActionInstance.Fail(GameStrings.DeviceNoPower);
                        }
                        //}
                        if (!doAction)
                        {
                            __result = delayedActionInstance;
                            return false;
                        }
                        labeller.Rename(__instance);
                        //Traverse.Create(pict).Field("PicturesToChooseFrom").SetValue(ChangePicture.ChangePicList(pict, list));
                        __result = delayedActionInstance;
                        return false;

                        //}
                        //__result = __instance.AttackWith(attack, doAction);
                    }

                    //TODO tried to rename networks names, but displayname have no setter
                    //Cable cable = __instance as Cable;
                    //Pipe pipe = __instance as Pipe;
                    //Chute chute = __instance as Chute;
                    //if (cable || pipe || chute)
                    //{
                    //    if (Input.GetKey(KeyCode.LeftShift))
                    //    {
                    //        Thing.DelayedActionInstance delayedActionInstance2 = new Thing.DelayedActionInstance
                    //        {
                    //            Duration = 0f,
                    //            ActionMessage = "Rename network"
                    //        };

                    //        //if (WorldManager.Instance.GameMode != GameMode.Creative) //need to write into all overwrites
                    //        //{
                    //        if (!labeller.OnOff)
                    //        {
                    //            __result = delayedActionInstance2.Fail(GameStrings.DeviceNotOn);
                    //        }
                    //        if (!labeller.IsOperable)
                    //        {
                    //            __result = delayedActionInstance2.Fail(GameStrings.DeviceNoPower);
                    //        }
                    //        //}
                    //        if (!doAction)
                    //        {
                    //            __result = delayedActionInstance2;
                    //            return false;
                    //        }
                    //        StructureNetwork network;
                            
                    //        if (cable) RenameNetwork(cable.CableNetwork, labeller);
                    //        labeller.Rename(__instance);
                    //        //Traverse.Create(pict).Field("PicturesToChooseFrom").SetValue(ChangePicture.ChangePicList(pict, list));
                    //        __result = delayedActionInstance2;
                    //        return false;
                    //    }
                    //}

                }
                return true;
            }

            //public static void RenameNetwork(StructureNetwork thing, Labeller labeller)
            //{
            //    //if (!base.ParentSlot.Parent.HasAuthority)
            //    //{
            //    //    return;
            //    //}
            //    if (InputWindow.ShowInputPanel(string.Format(InterfaceStrings.RenameThing, thing.DisplayName), thing.DisplayName))
            //    {
            //        InputWindow.OnSubmit += delegate (string input, string input2)
            //        {
            //            //labeller.InputRenameFinished(input, input2, thing);
            //            thing.DisplayName = input;
            //        };
            //        //base.PlaySound(Labeller.LabelHash, 1f, 1f);
            //        InputWindow.OnCancel += labeller.PlayCancelSound;
            //    }
            //}
        }// TODO OVERWRITE renaming at structure and somewhere else
         // can set setting at walls, cannot at seats and devices

        //static public void SetSetting(Thing thing, Labeller labeller)
        //{
        //    Device device = thing as Device;
        //    string name = thing.CustomName;
        //    if (InputWindow.ShowInputPanel(string.Format(InterfaceStrings.SetThingValue, thing.SourcePrefab.DisplayName), thing.DisplayName, thing, labeller, 32, 0, 600))
        //    {
        //        Assets.Scripts.UI.InputWindow.OnSubmit += delegate (string input, string input2)
        //        {
        //            labeller.InputRenameFinished(input, input2, thing);
        //            double setting = device.GetLogicValue(LogicType.Setting);
        //            double test = 0;
        //            try
        //            {
        //                test = (double)Enum.Parse(typeof(double), thing.CustomName);
        //            }
        //            catch (Exception)
        //            {
        //                test = (double)Animator.StringToHash(thing.CustomName);
        //                UnityEngine.Debug.Log("Value " + thing.CustomName + " is not double for Setting, convert as string to hash: " + test);
        //                //test = (double)Animator.StringToHash(thing.CustomName);
        //                device.SetLogicValue(LogicType.Setting, setting);
        //            }
        //            device.SetLogicValue(LogicType.Setting, test);
        //            thing.CustomName = name;
        //        };
        //        //base.PlaySound(Labeller.LabelHash, 1f, 1f);
        //        //InputWindow.OnCancel += __instance.PlayCancelSound;
        //    }
        //}

        [HarmonyPatch(typeof(Door), "SetDoorLabel")] //rename PictureFrame by Labeller
                                                     // [HarmonyPatch(new Type[] { typeof(Attack), typeof(bool) })]
        internal class DoorSideNaming
        {
            [UsedImplicitly]//dunno what is for, something for simpler replacing of the field values.
            [HarmonyPrefix]
            public static bool NamingSides(Door __instance)
            {
                if (__instance.DoorLabels.Count == 0)
                {
                    return false;
                }

                //string labeltext = __instance.DoorLabels[0].text;
                if (__instance.DisplayName.Contains(FreedomConfig.DoorSeparator)) 
                {
                    string[] labelSides = __instance.DisplayName.Split(new string[] { BindValidate.DoorSep }, StringSplitOptions.None); //__instance.DisplayName.Split(BindValidate.DoorSep);
                    List<string> textLabels = new List<string>();
                    foreach (string label in labelSides)
                    {
                        textLabels.Add(DoorSideNaming.FillSide(__instance, label));
                    }

                    foreach (TextMesh textMesh in __instance.DoorLabels)
                    {
                        int i = __instance.DoorLabels.IndexOf(textMesh);
                        textMesh.richText = true;
                        textMesh.text = textLabels[i];
                    }

                    //for (int i = 0; i <= __instance.DoorLabels.Count; i++)
                    //{
                    //    TextMesh textMesh = __instance.DoorLabels[i];
                    //    if (textLabels.Count - 1 <= i)
                    //    {
                    //        textMesh.text = textLabels[i];
                    //        textMesh.richText = true;
                    //    }
                    //    //else textMesh.text = textLabels[textLabels.Count-1];
                    //    //TODO Update labels on old doors too!
                    //}

                    //for (int i = 0; i < __instance.DoorLabels.Count; i++)
                    //{
                    //    TextMesh textMesh = __instance.DoorLabels[i];
                    //    textMesh.richText = true;
                    //    if (textLabels.Count - 1 <= i)
                    //    {
                    //        textMesh.text = textLabels[i];
                    //    }
                    //    //else textMesh.text = textLabels[textLabels.Count-1];
                    //    //TODO Update labels on old doors too!
                    //}
                    return false;
                }
                else return true;
            }

            public static string FillSide(Door __instance, string label)
            {
                string[] array = DoorSideNaming.FormatWordsSides(__instance, label);

                List<string> list = new List<string>();
                string text = "";
                int num = 0;
                for (int i = 0; i < array.Length; i++)
                {
                    if (i + 1 == array.Length && i == 1)
                    {
                        list.Add(text);
                        list.Add(array[i]);
                        text = string.Empty;
                        break;
                    }
                    if (num != 0)
                    {
                        text += " ";
                    }
                    num += array[i].Length;
                    if (num >= 10)
                    {
                        text += array[i];
                        num = 0;
                        list.Add(text);
                        text = string.Empty;
                    }
                    else
                    {
                        text += array[i];
                    }
                }
                if (text != string.Empty)
                {
                    list.Add(text);
                }
                bool flag = false;
                string textLab = string.Empty;
                for (int j = 0; j < list.Count; j++)
                {
                    string text3 = list[j];
                    if (j > 0)
                    {
                        textLab += "\n";
                    }
                    if (!flag && text3.Length <= 7)
                    {
                        textLab += string.Format("<size=90>{0}</size>", text3);
                        flag = true;
                    }
                    else
                    {
                        textLab += text3;
                    }
                }
                return textLab;
            }

            public static string[] FormatWordsSides(Door door, string input)
            {
                List<string> list = input.Substring(0, Mathf.Min(input.Length, door.MaxLabelLength)).ToUpper().Split(' ').ToList<string>();
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    List<string> list2 = new List<string>();
                    string text = list[i];
                    if (text.Length > door.MaxWordLength)
                    {
                        list.RemoveAt(i);
                        while (text.Length > door.MaxWordLength)
                        {
                            string text2 = text.Substring(0, Mathf.Min(text.Length, door.MaxWordLength));
                            list2.Add(text2);
                            text = text.Substring(text2.Length);
                        }
                        list2.Add(text);
                        list.InsertRange(i, list2);
                    }
                }
                return list.ToArray();
            }
        }


        //already in vanilla
        //[HarmonyPatch(typeof(LogicDial), "InteractWith")] //rename PictureFrame by Labeller
        //                                                  // [HarmonyPatch(new Type[] { typeof(Attack), typeof(bool) })]
        //internal class SetDialSetting
        //{
        //    [UsedImplicitly]//dunno what is for, something for simpler replacing of the field values.
        //    [HarmonyPrefix]
        //    public static bool InteractDialKnob(LogicDial __instance, ref Thing.DelayedActionInstance __result, ref Interactable interactable, ref Interaction interaction, ref bool doAction)
        //    {
        //        Labeller labeller = interaction.SourceSlot.Occupant as Labeller;
        //        if (!labeller)
        //        {
        //            //__result = Custopics.ThingPatch.AttackWith(__instance, attack, doAction);
        //            return true;
        //        }
        //        if (interactable.Action != InteractableType.Button3 && interactable.Action != InteractableType.Button4)
        //        {
        //            return true;
        //        }
        //        //Labeller labeller = interaction.SourceSlot.Occupant as Labeller;

        //        Thing.DelayedActionInstance delayedActionInstance3 = new Thing.DelayedActionInstance
        //        {
        //            Duration = 0f,
        //            ActionMessage = ActionStrings.Set
        //        };
        //        delayedActionInstance3.AppendStateMessage(GameStrings.DeviceManualInputWindow);

        //        if (!__instance.IsAuthorized(interaction.SourceThing))
        //        {
        //            __result = delayedActionInstance3.Fail(GameStrings.AccessCardUnableToInteract);
        //        }
        //        if (__instance.Mode == 0)
        //        {
        //            __result = delayedActionInstance3.Succeed();
        //        }

        //        if (!labeller.OnOff)
        //        {
        //            __result = delayedActionInstance3.Fail(GameStrings.DeviceNotOn);
        //        }
        //        if (!labeller.IsOperable)
        //        {
        //            __result = delayedActionInstance3.Fail(GameStrings.DeviceNoPower);

        //        }
        //        if (!doAction)
        //        {
        //            __result = delayedActionInstance3;//.Succeed();
        //            return false;
        //        }

        //        SetDialSetting.SetPlus(__instance, labeller);
        //        __instance.Setting = (double)Mathf.Min((int)__instance.Setting + (interaction.AltKey ? 10 : 1), __instance.Mode);
        //        //__instance.knob.PlayKnobSound((int)__instance.Setting, true, __instance.Mode);
        //        __instance.OnSettingChanged();
        //        __result = Thing.DelayedActionInstance.Success(interactable.ContextualName);

        //        return false;
        //    }

        //    public static void SetPlus(Device setable, Labeller label)
        //    {
        //        //if (!base.ParentSlot.Parent.HasAuthority)
        //        //{
        //        //    return;
        //        //}
        //        if (InputWindow.ShowInputPanel(string.Format(InterfaceStrings.SetThingValue, setable.DisplayName), setable.GetLogicValue(LogicType.Setting).ToStringExact(), setable, label, 32, TMP_InputField.ContentType.DecimalNumber, 600))
        //        {
        //            InputWindow.OnSubmit += delegate (string input, string input2)
        //            {
        //                SetDialSetting.InputSettingPlus(input, setable);
        //            };
        //            //base.PlaySound(Labeller.LabelHash, 1f, 1f);
        //            InputWindow.OnCancel += label.PlayCancelSound;
        //        }
        //    }
        //    public static void InputSettingPlus(string value, Device settable)
        //    {
        //        if (settable != null)
        //        {
        //            double maxValue;
        //            double.TryParse(value, NumberStyles.Number, CultureInfo.CurrentCulture, out maxValue);
        //            if (double.IsPositiveInfinity(maxValue))
        //            {
        //                maxValue = double.MaxValue;
        //            }
        //            if (NetworkManager.IsClient)
        //            {
        //                new SetLogicFromClient
        //                {
        //                    LogicId = settable.NetworkId,
        //                    Value = maxValue
        //                }.SendToServer();
        //            }
        //            else
        //            {
        //                settable.SetLogicValue(LogicType.Setting, maxValue);
        //            }
        //            //base.PlaySound(Labeller.LabelConfirmHash, 1f, 1f);
        //        }
        //    }
        //}
    }
}
