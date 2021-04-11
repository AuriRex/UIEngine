using UIEngine.Configuration;
using System.Collections.Generic;
using HMUI;
using System;
using UnityEngine;
using System.Linq;
using static UIEngine.Accessors;
using UIEngine.Extensions;
using static UIEngine.Configuration.PluginConfig.Buttons;
using TMPro;

namespace UIEngine.Managers
{
    public class UIEElementManager
    {
        private static PluginConfig _pluginConfig;
        private static UIEColorManager _colorManager;

        private static HashSet<ButtonStaticAnimations> _buttonStaticAnimations;
        private static Dictionary<ButtonStaticAnimations, (ButtonType, SpecialType)> _buttonTypeDictionary;
        private static Dictionary<ButtonStaticAnimations, (AnimationClip, AnimationClip, AnimationClip, AnimationClip)> _newButtonAnimations;

        internal UIEElementManager(PluginConfig pluginConfig, UIEColorManager colorManager)
        {
            _pluginConfig = pluginConfig;
            _colorManager = colorManager;
        }

        internal static void AddButton(ButtonStaticAnimations bsa)
        {
            if (_buttonStaticAnimations == null)
                _buttonStaticAnimations = new HashSet<ButtonStaticAnimations>();

            if (_buttonTypeDictionary == null)
                _buttonTypeDictionary = new Dictionary<ButtonStaticAnimations, (ButtonType, SpecialType)>();

            if (_newButtonAnimations == null)
                _newButtonAnimations = new Dictionary<ButtonStaticAnimations, (AnimationClip, AnimationClip, AnimationClip, AnimationClip)>();

            /*if (_originalbuttonStaticAnimationsClips == null)
                _originalbuttonStaticAnimationsClips = new Dictionary<ButtonStaticAnimations, (AnimationClip, AnimationClip, AnimationClip, AnimationClip)>();*/

            if (!_buttonStaticAnimations.Contains(bsa))
            {
                _buttonStaticAnimations.Add(bsa);
                if (_pluginConfig.Enabled)
                    DecorateButton(bsa, true);
                return;
            }

            if (_pluginConfig.Enabled)
                DecorateButton(bsa);
        }

        internal void RefreshVisuals()
        {
            if (!_pluginConfig.Enabled) return;

            foreach (var bsa in _buttonStaticAnimations)
            {
                DecorateButton(bsa);
            }
        }

        private static void DecorateButton(ButtonStaticAnimations bsa, bool firstTime = false)
        {
            ButtonType bType;
            SpecialType sType;

            if (_buttonTypeDictionary.TryGetValue(bsa, out (ButtonType, SpecialType) types))
            {
                bType = types.Item1;
                sType = types.Item2;
            }
            else
            {
                bType = GetButtonType(bsa);
                sType = GetSpecialType(bsa, bType);
                _buttonTypeDictionary.Add(bsa, (bType, sType));
            }

            /*AnimationClip clipNormal;
            AnimationClip clipHighlighted;
            AnimationClip clipPressed;
            AnimationClip clipDisabled;*/

            string gameObjectName = bsa.gameObject.name;

            UIEColorManager cm = _colorManager;

            //ImageView bg_iv = bsa.gameObject.GetComponentOnChild<ImageView>("BG");



            ref AnimationClip clipNormal = ref ButtonStaticAnimations_normalClip(ref bsa);
            ref AnimationClip clipHighlighted = ref ButtonStaticAnimations_highlightedClip(ref bsa);
            ref AnimationClip clipPressed = ref ButtonStaticAnimations_pressedClip(ref bsa);
            ref AnimationClip clipDisabled = ref ButtonStaticAnimations_disabledClip(ref bsa);

            /*if(firstTime)
            {
                // Back up original clips and clone them
                if(!_originalbuttonStaticAnimationsClips.ContainsKey(bsa))
                    _originalbuttonStaticAnimationsClips.Add(bsa, (clipNormal, clipHighlighted, clipPressed, clipDisabled));

                // does not work, TODO - might not be possible
                //clipNormal = new AnimationClip(clipNormal);
            }*/

            CreateNewDefaultAnimationsIfNeeded(bsa, ref clipNormal, ref clipHighlighted, ref clipPressed, ref clipDisabled, bType);

            Logger.log.Notice("Doing things");

            switch (bType)
            {
                case ButtonType.Play:
                    DecoratePlayButton(bsa, ref clipNormal, ref clipHighlighted, ref clipPressed, ref clipDisabled);
                    break;
                case ButtonType.Underlined:
                    // TODO
                    break;
                case ButtonType.ModeSelection:
                    DecorateModeSelectionButton(bsa, ref clipNormal, ref clipHighlighted, ref clipPressed, ref clipDisabled);
                    break;
                case ButtonType.Back:
                    UIEColorManager.SetAnimationImageViewColors(ref clipNormal, "BG", cm.IsAdvanced() ? cm.backButtonNormal : cm.SimplePrimaryNormal);
                    UIEColorManager.SetAnimationImageViewColors(ref clipHighlighted, "BG", cm.IsAdvanced() ? cm.backButtonHighlight : cm.simplePrimaryHighlight);
                    break;
            }

            bsa.HandleButtonSelectionStateDidChange(NoTransitionsButton.SelectionState.Normal);
        }

        private static void CreateNewDefaultAnimationsIfNeeded(ButtonStaticAnimations bsa, ref AnimationClip clipNormal, ref AnimationClip clipHighlighted, ref AnimationClip clipPressed, ref AnimationClip clipDisabled, ButtonType bType)
        {
            if (_newButtonAnimations.TryGetValue(bsa, out (AnimationClip, AnimationClip, AnimationClip, AnimationClip) clips))
            {
                clipNormal = clips.Item1;
                clipHighlighted = clips.Item2;
                clipPressed = clips.Item3;
                clipDisabled = clips.Item4;
                return;
            }

            switch (bType)
            {
                case ButtonType.Play:
                    AssignNewClips(ref clipNormal, ref clipHighlighted, ref clipPressed, ref clipDisabled);
                    SetDefaultAnimationsForClip(clipNormal, AnimationData.PlayButton.DefaultAnimatedTextButtonNormal);
                    SetDefaultAnimationsForClip(clipHighlighted, AnimationData.PlayButton.DefaultAnimatedTextButtonHighlighted);
                    SetDefaultAnimationsForClip(clipPressed, AnimationData.PlayButton.DefaultAnimatedTextButtonPressed);
                    SetDefaultAnimationsForClip(clipDisabled, AnimationData.PlayButton.DefaultAnimatedTextButtonDisabled);
                    break;
            }

            _newButtonAnimations.Add(bsa, (clipNormal, clipHighlighted, clipPressed, clipDisabled));
        }

        private static void AssignNewClips(ref AnimationClip clipNormal, ref AnimationClip clipHighlighted, ref AnimationClip clipPressed, ref AnimationClip clipDisabled)
        {
            var temp = new AnimationClip();
            temp.legacy = true;
            temp.name = "CustomNormalClip";

            clipNormal = temp;

            temp = new AnimationClip();
            temp.legacy = true;
            temp.name = "CustomHighlightedClip";

            clipHighlighted = temp;

            temp = new AnimationClip();
            temp.legacy = true;
            temp.name = "CustomPressedClip";

            clipPressed = temp;

            temp = new AnimationClip();
            temp.legacy = true;
            temp.name = "CustomDisabledClip";

            clipDisabled = temp;
        }

#nullable enable
        private static void SetDefaultAnimationsForClip(AnimationClip clip, (string, string, float, Type?)[] defaultAnimationData)
        {
            foreach((string, string, float, Type?) values in defaultAnimationData)
            {
                if (values.Item4 == null) continue;
                clip.SetCurve(values.Item1, values.Item4, values.Item2, AnimationCurve.Constant(0, 0, values.Item3));
            }
        }
#nullable restore

        private static readonly Dictionary<string, SpecialType> _gameObjectNameToType = new Dictionary<string, SpecialType> {
            { "SoloButton", SpecialType.SoloMode },
            { "OnlineButton", SpecialType.OnlineMode },
            { "CampaignButton", SpecialType.CampaignMode },
            { "PartyButton", SpecialType.PartyMode }
        };

        private static SpecialType GetSpecialType(ButtonStaticAnimations bsa, ButtonType bType)
        {

            switch(bType)
            {
                case ButtonType.ModeSelection:
                    if (_gameObjectNameToType.TryGetValue(bsa.gameObject.name, out SpecialType value))
                        return value;
                    break;
                case ButtonType.Underlined:
                    // find mod buttons?
                    break;
            }
            

            return SpecialType.Other;
        }

        private static bool GetCustomPlayButtonSettings(string goName, string text, out PlayButton settings)
        {
            foreach(CustomPlayButton s in _pluginConfig.ButtonSettings.CustomPlayButtons)
            {
                if(s.TargetGameObjectName.Equals(goName) || s.TargetTextContent.Equals(text))
                {
                    settings = s;
                    return true;
                }
            }

            settings = null;
            return false;
        }

        private static void DecoratePlayButton(ButtonStaticAnimations bsa, ref AnimationClip clipNormal, ref AnimationClip clipHighlighted, ref AnimationClip clipPressed, ref AnimationClip clipDisabled)
        {

            var textGO = bsa.gameObject.GetChildByName("Content").GetChildByName("Text");

            var textTMP = textGO.GetComponent<CurvedTextMeshPro>();

            string textContent = textTMP.text;

            PlayButton settings;

            if(_pluginConfig.Advanced)
            {
                if (!GetCustomPlayButtonSettings(bsa.gameObject.name, textContent, out settings))
                {
                    // Use Default Settings instead
                    settings = _pluginConfig.ButtonSettings.PlayButtonSettings;
                }
            } 
            else
            {
                // Simple Color thing
                settings = UIEColorManager.GetSimpleColorButtonSettings().PlayButtonSettings;
            }


            // clipNormal is only used as the initial Clip,
            // everytime the button leaves the highlighted state it uses clipPressed instead,
            // so we just set them both to our custom normal state.
            DecoratePlayButtonStateFromSettings(bsa, ref clipNormal, settings.NormalState, textTMP);
            DecoratePlayButtonStateFromSettings(bsa, ref clipPressed, settings.NormalState, textTMP);
            DecoratePlayButtonStateFromSettings(bsa, ref clipHighlighted, settings.HighlightedState, textTMP);
            DecoratePlayButtonStateFromSettings(bsa, ref clipDisabled, settings.DisabledState, textTMP, true);

        }

        private static void DecoratePlayButtonStateFromSettings(ButtonStaticAnimations bsa, ref AnimationClip clip, PlayButton.PlayButtonState state, TextMeshProUGUI TMP, bool disabledState = false)
        {
            // TODO: Shine and Radial Color
            UIEColorManager.SetAnimationTextColor(ref clip, TMP, state.TextColor, "Content/Text");
            UIEColorManager.SetAnimationFromImageViewSettings(ref clip, disabledState ? bsa.gameObject.GetComponentOnChild<ImageView>("BGDisabled")  : bsa.gameObject.GetComponentOnChild<ImageView>("BG"), state.BackgroundColors, disabledState ? "BGDisabled" : "BG");
            UIEColorManager.SetAnimationFromImageViewSettings(ref clip, bsa.gameObject.GetComponentOnChild<ImageView>("Border"), state.BorderColors, "Border");
            UIEColorManager.SetAnimationFromImageViewSettings(ref clip, bsa.gameObject.GetChildByName("OutlineWrapper")?.GetComponentOnChild<ImageView>("Outline"), state.OutlineColors, "OutlineWrapper/Outline");
        }

        private static void DecorateModeSelectionButton(ButtonStaticAnimations bsa, ref AnimationClip clipNormal, ref AnimationClip clipHighlighted, ref AnimationClip clipPressed, ref AnimationClip clipDisabled)
        {
            // TODO
        }

        private static ButtonType GetButtonType(ButtonStaticAnimations bsa)
        {
            Transform buttonTransform = bsa.gameObject.transform;

            List<string> childNames = GetChildrenAsList(buttonTransform);

            if (ListContainsAll(childNames, playButtonChildren)) return ButtonType.Play;
            if (ListContainsAll(childNames, underlinedButtonChildren)) return ButtonType.Underlined;
            if (bsa.gameObject.name.Equals("BackButton") && ListContainsAll(childNames, backButtonChildren)) return ButtonType.Back;

            if(ListContainsAll(childNames, modeButtonChildren))
            {
                if(buttonTransform.childCount > 0)
                {
                    List<string> childrenOfChild0 = GetChildrenAsList(buttonTransform.GetChild(0));

                    if(ListContainsAll(childrenOfChild0, modeButtonImageChildren))
                    {
                        return ButtonType.ModeSelection;
                    }
                }
            }

            if(ListContainsAll(childNames, menuSmallButtonChildren))
            {
                if (buttonTransform.childCount > 0)
                {
                    List<string> childrenOfChild0 = GetChildrenAsList(buttonTransform.GetChild(0));

                    if (ListContainsAll(childrenOfChild0, menuSmallButtonImageChildren))
                    {
                        return ButtonType.MainMenuSmall;
                    }
                }
            }

            return ButtonType.Unknown;
        }

        public static List<string> GetChildrenAsList(Transform trans)
        {
            List<string> allChildren = new List<string>();
            for (int i = 0; i < trans.childCount; i++)
            {
                allChildren.Add(trans.GetChild(i).name);
            }
            return allChildren;
        }

        // TODO Move to Util class idk
        public static bool ListContainsAll(List<string> list, List<string> targets)
        {
            foreach (string target in targets)
            {
                if (!list.Contains(target))
                {
                    return false;
                }
            }
            return true;
        }

        private static List<string> playButtonChildren = new string[] { "BG", "BGDisabled", "Content", "Border", "OutlineWrapper" }.ToList();
        private static List<string> underlinedButtonChildren = new string[] { "BG", "Underline", "Content" }.ToList();
        private static List<string> backButtonChildren = new string[] { "BG", "Icon" }.ToList();

        private static List<string> modeButtonChildren = new string[] { "Image", "Text" }.ToList();
        private static List<string> modeButtonImageChildren = new string[] { "Glow", "Image0", "ImageOverlay" }.ToList();

        private static List<string> menuSmallButtonChildren = new string[] { "Image" }.ToList();
        private static List<string> menuSmallButtonImageChildren = new string[] { "Image0" }.ToList();

        enum ButtonType
        {
            Play, // The fancy one / play button
            Underlined, // Every Button with an underline
            Back, // Back button
            ModeSelection, // Solo / Online / Campaign / Party
            MainMenuSmall, // Help, Settings, Tutorial
            Unknown
        }

        enum SpecialType
        {
            SoloMode,
            OnlineMode,
            PartyMode,
            CampaignMode,
            ModUnderline,
            Custom,
            Other
        }
    }
}
