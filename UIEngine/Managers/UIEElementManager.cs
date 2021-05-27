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
using UIEngine.Components;
using System.Reflection;
using UnityEngine.Events;

namespace UIEngine.Managers
{
    public class UIEElementManager
    {
        private static PluginConfig _pluginConfig;
        private static UIEColorManager _colorManager;

        private static HashSet<ButtonStaticAnimations> _buttonStaticAnimations;
        private static Dictionary<ButtonStaticAnimations, (ButtonType, SpecialType)> _buttonTypeDictionary;
        private static Dictionary<ButtonStaticAnimations, (AnimationClip, AnimationClip, AnimationClip, AnimationClip)> _newButtonAnimations;
        private static Dictionary<ButtonStaticAnimations, Assembly> _assemblyForModButtons;

        public static event Action<ButtonStaticAnimations> onButtonDecoratedEvent;
        public static event Action<ButtonStaticAnimations, DecorationFailedReason> onButtonDecorationFailedEvent;

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

            if (_assemblyForModButtons == null)
                _assemblyForModButtons = new Dictionary<ButtonStaticAnimations, Assembly>();

            try
            {
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
            catch(Exception ex)
            {
                Logger.log.Error($"Failed to decorate button \"{bsa.gameObject.name}\" : {ex.Message}");
                Logger.log.Error(ex.StackTrace);
                onButtonDecorationFailedEvent?.Invoke(bsa, DecorationFailedReason.Error);
            }

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

            string gameObjectName = bsa.gameObject.name;

            UIEColorManager cm = _colorManager;


            Assembly assembly = null;

            if(sType == SpecialType.ModUnderline && !_assemblyForModButtons.TryGetValue(bsa, out assembly))
            {
                assembly = GetAssemblyForButton(bsa, bType);

                if (assembly != null && !_assemblyForModButtons.ContainsKey(bsa))
                {
                    _assemblyForModButtons.Add(bsa, assembly);
                }
            }

            ref AnimationClip clipNormal = ref ButtonStaticAnimations_normalClip(ref bsa);
            ref AnimationClip clipHighlighted = ref ButtonStaticAnimations_highlightedClip(ref bsa);
            ref AnimationClip clipPressed = ref ButtonStaticAnimations_pressedClip(ref bsa);
            ref AnimationClip clipDisabled = ref ButtonStaticAnimations_disabledClip(ref bsa);


            PluginConfig.Filters filters = _pluginConfig.DecorationExclusions;

            if(filters.FilterRules.Any(x => CancelButtonDecoration(x, bType, sType, bsa)))
            {
                Logger.log.Debug($"Filtered decoration of button \"{bsa.name}\" - Not decorating!");
                onButtonDecorationFailedEvent?.Invoke(bsa, DecorationFailedReason.Filtered);
                return;
            }


            CreateNewDefaultAnimationsIfNeeded(bsa, ref clipNormal, ref clipHighlighted, ref clipPressed, ref clipDisabled, bType);

            Logger.log.Notice($"Decorating Button \"{bsa.gameObject.name}\" ({bType}) [{assembly?.GetName().Name}]");

            switch (bType)
            {
                case ButtonType.Play:
                    DecoratePlayButton(bsa, clipNormal, clipHighlighted, clipPressed, clipDisabled);
                    break;
                case ButtonType.Underlined:
                    DecorateUnderlinedButton(bsa, clipNormal, clipHighlighted, clipPressed, clipDisabled, sType);
                    break;
                case ButtonType.ModeSelection:
                    DecorateModeSelectionButton(bsa, clipNormal, clipHighlighted, clipPressed, clipDisabled, sType);
                    break;
                case ButtonType.Back:
                    UIEColorManager.SetAnimationImageViewColors(ref clipNormal, "BG", cm.IsAdvanced() ? cm.backButtonNormal : cm.SimplePrimaryNormal);
                    UIEColorManager.SetAnimationImageViewColors(ref clipHighlighted, "BG", cm.IsAdvanced() ? cm.backButtonHighlight : cm.simplePrimaryHighlight);
                    break;
            }

            bsa.OnEnable();

            onButtonDecoratedEvent?.Invoke(bsa);
        }

        private static bool CancelButtonDecoration(PluginConfig.FilterTarget filterTarget, ButtonType bType, SpecialType sType, ButtonStaticAnimations bsa)
        {
            if(filterTarget.TargetButtonType.Equals("Any") || filterTarget.TargetButtonType.Equals(bType))
            {
                // Filter applies to this button type

                if(filterTarget.TargetMatchingMode.Equals(PluginConfig.CustomButtonTargetMatchingMode.TARGET_MODE_ANY))
                {
                    return true;
                }
            }

            return false;
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

            // TODO
            switch (bType)
            {
                case ButtonType.Play:
                    AssignNewClips(ref clipNormal, ref clipHighlighted, ref clipPressed, ref clipDisabled);
                    SetDefaultAnimationsForClip(clipNormal, AnimationData.PlayButton.DefaultAnimatedTextButtonNormal.WithFiltersApplied(_pluginConfig.DecorationExclusions.AllExclusionsForButton(bsa, bType)));
                    SetDefaultAnimationsForClip(clipHighlighted, AnimationData.PlayButton.DefaultAnimatedTextButtonHighlighted.WithFiltersApplied(_pluginConfig.DecorationExclusions.AllExclusionsForButton(bsa, bType)));
                    SetDefaultAnimationsForClip(clipPressed, AnimationData.PlayButton.DefaultAnimatedTextButtonPressed.WithFiltersApplied(_pluginConfig.DecorationExclusions.AllExclusionsForButton(bsa, bType)));
                    SetDefaultAnimationsForClip(clipDisabled, AnimationData.PlayButton.DefaultAnimatedTextButtonDisabled.WithFiltersApplied(_pluginConfig.DecorationExclusions.AllExclusionsForButton(bsa, bType)));
                    break;
                case ButtonType.ModeSelection:
                    AssignNewClips(ref clipNormal, ref clipHighlighted, ref clipPressed, ref clipDisabled);
                    SetDefaultAnimationsForClip(clipNormal, AnimationData.BigMenuButton.DefaultBigMenuButtonNormal.WithFiltersApplied(_pluginConfig.DecorationExclusions.AllExclusionsForButton(bsa, bType)));
                    SetDefaultAnimationsForClip(clipHighlighted, AnimationData.BigMenuButton.DefaultBigMenuButtonHighlighted.WithFiltersApplied(_pluginConfig.DecorationExclusions.AllExclusionsForButton(bsa, bType)));
                    SetDefaultAnimationsForClip(clipPressed, AnimationData.BigMenuButton.DefaultBigMenuButtonPressed.WithFiltersApplied(_pluginConfig.DecorationExclusions.AllExclusionsForButton(bsa, bType)));
                    SetDefaultAnimationsForClip(clipDisabled, AnimationData.BigMenuButton.DefaultBigMenuButtonDisabled.WithFiltersApplied(_pluginConfig.DecorationExclusions.AllExclusionsForButton(bsa, bType)));
                    break;
                case ButtonType.Underlined:
                    AssignNewClips(ref clipNormal, ref clipHighlighted, ref clipPressed, ref clipDisabled);
                    SetDefaultAnimationsForClip(clipNormal, AnimationData.TextMenuButton.DefaultTextMenuButtonNormal.WithFiltersApplied(_pluginConfig.DecorationExclusions.AllExclusionsForButton(bsa, bType)));
                    SetDefaultAnimationsForClip(clipHighlighted, AnimationData.TextMenuButton.DefaultTextMenuButtonHighlighted.WithFiltersApplied(_pluginConfig.DecorationExclusions.AllExclusionsForButton(bsa, bType)));
                    SetDefaultAnimationsForClip(clipPressed, AnimationData.TextMenuButton.DefaultTextMenuButtonPressed.WithFiltersApplied(_pluginConfig.DecorationExclusions.AllExclusionsForButton(bsa, bType)));
                    SetDefaultAnimationsForClip(clipDisabled, AnimationData.TextMenuButton.DefaultTextMenuButtonDisabled.WithFiltersApplied(_pluginConfig.DecorationExclusions.AllExclusionsForButton(bsa, bType)));
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

        private static void SetDefaultAnimationsForClip(AnimationClip clip, (string, string, float, Type)[] defaultAnimationData)
        {
            foreach((string, string, float, Type) values in defaultAnimationData)
            {
                if (values.Item4 == null) continue;
                clip.SetCurve(values.Item1, values.Item4, values.Item2, AnimationCurve.Constant(0, 0, values.Item3));
            }
        }

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
                    // TODO find mod buttons?
                    if(bsa.gameObject.GetNthParent(4).name.Equals("MenuButtonsViewController"))
                    {
                        return SpecialType.ModUnderline;
                    }
                    break;
            }
            

            return SpecialType.Other;
        }

        public static string GetTextContentForButtonType(ButtonStaticAnimations bsa, ButtonType buttonType)
        {
            switch (buttonType)
            {
                case ButtonType.Play:
                case ButtonType.Underlined:
                    var curvedTMP = bsa.gameObject.GetComponentOnChild<CurvedTextMeshPro>("Content/Text");
                    return curvedTMP.text;
                default:
                    return string.Empty;
            }
        }

        public static Assembly GetAssemblyForButton(ButtonStaticAnimations bsa, ButtonType buttonType)
        {
            return Utilities.Utilities.GetButtonAssembly(bsa, GetTextContentForButtonType(bsa, buttonType));
        }

        private static bool GetCustomButtonSettings<T, R>(GameObject go, string text, List<T> inList, out R settings) where T : PluginConfig.ICustomButtonTarget, R
        {
            string goName = go.name;
            foreach(T s in inList)
            {
                switch(s.TargetMatchingMode)
                {
                    case PluginConfig.CustomButtonTargetMatchingMode.TARGET_MODE_GAMEOBJECT_NAME:
                        if (s.TargetString.Equals(goName, StringComparison.CurrentCultureIgnoreCase))
                        {
                            settings = s;
                            return true;
                        }
                        break;
                    case PluginConfig.CustomButtonTargetMatchingMode.TARGET_MODE_TEXT_CONTENT:
                        if(s.TargetString.Equals(text, StringComparison.CurrentCultureIgnoreCase))
                        {
                            settings = s;
                            return true;
                        }
                        break;
                    case PluginConfig.CustomButtonTargetMatchingMode.TARGET_MODE_PARENT_GAMEOBJECT_NAME:
                        if (s.TargetString.Equals(go.transform.parent?.name, StringComparison.CurrentCultureIgnoreCase))
                        {
                            settings = s;
                            return true;
                        }
                        break;
                }
            }

            settings = default;
            return false;
        }

        private static void DecoratePlayButton(ButtonStaticAnimations bsa, AnimationClip clipNormal, AnimationClip clipHighlighted, AnimationClip clipPressed, AnimationClip clipDisabled)
        {

            var curvedTMP = bsa.gameObject.GetComponentOnChild<CurvedTextMeshPro>("Content/Text");

            string textContent = curvedTMP.text;

            PlayButton settings;

            if(_pluginConfig.Advanced)
            {
                if (!GetCustomButtonSettings<CustomPlayButton, PlayButton>(bsa.gameObject, textContent, _pluginConfig.ButtonSettings.CustomPlayButtons, out settings))
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

            var bmcc = bsa.gameObject.GetOrAddComponent<ButtonMaterialPropertyChanger>();

            bmcc.InitWithState<PlayButton.PlayButtonState>(settings);

            bmcc.AddMaterialPath("BG", "_ShineColor", nameof(PlayButton.PlayButtonState.ShineColor));
            bmcc.AddMaterialPath("Border", "_RadialColor", nameof(PlayButton.PlayButtonState.RadialGlowColor));
            bmcc.AddMaterialPath("OutlineWrapper/Outline", "_RadialColor", nameof(PlayButton.PlayButtonState.RadialGlowColor));

            // clipNormal is only used as the initial Clip,
            // everytime the button leaves the highlighted state it uses clipPressed instead,
            // so we just set them both to our custom normal state.
            DecoratePlayButtonStateFromSettings(bsa, clipNormal, settings.NormalState, curvedTMP);
            DecoratePlayButtonStateFromSettings(bsa, clipPressed, settings.NormalState, curvedTMP);
            DecoratePlayButtonStateFromSettings(bsa, clipHighlighted, settings.HighlightedState, curvedTMP);
            DecoratePlayButtonStateFromSettings(bsa, clipDisabled, settings.DisabledState, curvedTMP, true);

        }

        private static void DecoratePlayButtonStateFromSettings(ButtonStaticAnimations bsa, AnimationClip clip, PlayButton.PlayButtonState state, TextMeshProUGUI TMP, bool disabledState = false)
        {
            UIEColorManager.SetAnimationTextColor<CurvedTextMeshPro>(clip, TMP, state.TextColor, "Content/Text");
            UIEColorManager.SetAnimationFromImageViewSettings(clip, disabledState ? bsa.gameObject.GetComponentOnChild<ImageView>("BGDisabled")  : bsa.gameObject.GetComponentOnChild<ImageView>("BG"), state.BackgroundColors, disabledState ? "BGDisabled" : "BG");
            UIEColorManager.SetAnimationFromImageViewSettings(clip, bsa.gameObject.GetComponentOnChild<ImageView>("Border"), state.BorderColors, "Border");
            UIEColorManager.SetAnimationFromImageViewSettings(clip, bsa.gameObject.GetChildByName("OutlineWrapper")?.GetComponentOnChild<ImageView>("Outline"), state.OutlineColors, "OutlineWrapper/Outline");
        }

        private static void DecorateModeSelectionButton(ButtonStaticAnimations bsa, AnimationClip clipNormal, AnimationClip clipHighlighted, AnimationClip clipPressed, AnimationClip clipDisabled, SpecialType sType)
        {
            var curvedTMP = bsa.gameObject.GetComponentOnChild<CurvedTextMeshPro>("Content/Text");

            string textContent = curvedTMP.text;

            BigMainMenuButton GetFromType(PluginConfig config, SpecialType specialtype)
            {
                switch (sType)
                {
                    case SpecialType.SoloMode:
                        return config.ButtonSettings.SoloMode;
                    case SpecialType.OnlineMode:
                        return config.ButtonSettings.OnlineMode;
                    case SpecialType.PartyMode:
                        return config.ButtonSettings.PartyMode;
                    case SpecialType.CampaignMode:
                        return config.ButtonSettings.CampaignMode;
                    default:
                        return config.ButtonSettings.FallbackBigMenuButton;
                }
            }

            BigMainMenuButton settings;
            if (_pluginConfig.Advanced)
            {
                if (!GetCustomButtonSettings<CustomBigMainMenuButton, BigMainMenuButton>(bsa.gameObject, textContent, _pluginConfig.ButtonSettings.CustomBigMainMenuButtons, out settings))
                {
                    // Use Default Settings instead
                    settings = GetFromType(_pluginConfig, sType);
                }
            }
            else
            {
                // Simple Color thing
                settings = GetFromType(UIEColorManager.GetSimpleColorConfig(), sType);
            }

            DecorateModeButtonForState(bsa, clipNormal, settings.NormalState, curvedTMP);
            DecorateModeButtonForState(bsa, clipHighlighted, settings.HighlightedState, curvedTMP);
            DecorateModeButtonForState(bsa, clipPressed, settings.HighlightedState, curvedTMP);
            //DecorateModeButtonForState(bsa, clipDisabled, settings.NormalState, curvedTMP);
        }

        private static void DecorateModeButtonForState(ButtonStaticAnimations bsa, AnimationClip clip, BigMainMenuButton.BigMainMenuButtonState state, TextMeshProUGUI TMP)
        {
            UIEColorManager.SetAnimationTextColor<CurvedTextMeshPro>(clip, TMP, state.TextColor, "Text");
            UIEColorManager.SetAnimationClipTubeBloomPrePassLightColor(clip, state.GlowColor, "Image/Glow");
            UIEColorManager.SetAnimationFromImageViewSettings(clip, bsa.gameObject.GetComponentOnChild<ImageView>("Image/Image0"), state.FillColors, "Image/Image0");
            UIEColorManager.SetAnimationFromImageViewSettings(clip, bsa.gameObject.GetComponentOnChild<ImageView>("Image/ImageOverlay"), state.OverlayColors, "Image/ImageOverlay");
        }

        private static void DecorateUnderlinedButton(ButtonStaticAnimations bsa, AnimationClip clipNormal, AnimationClip clipHighlighted, AnimationClip clipPressed, AnimationClip clipDisabled, SpecialType sType)
        {
            // TODO

            var curvedTMP = bsa.gameObject.GetComponentOnChild<CurvedTextMeshPro>("Content/Text");

            string textContent = curvedTMP.text;

            UnderlinedButton GetFromType(PluginConfig config, SpecialType specialtype)
            {
                switch (specialtype)
                {
                    case SpecialType.ModUnderline:
                        return config.ButtonSettings.ModUnderlinedButtons;
                    default:
                        return config.ButtonSettings.FallbackUnderlinedButton;
                }
            }

            UnderlinedButton settings;
            if (_pluginConfig.Advanced)
            {
                if (!GetCustomButtonSettings<CustomUnderlinedButton, UnderlinedButton>(bsa.gameObject, textContent, _pluginConfig.ButtonSettings.CustomUnderlinedButtons, out settings))
                {
                    // Use Default Settings instead
                    settings = GetFromType(_pluginConfig, sType);
                    Logger.log.Notice($"Using default settings for \"{textContent}\"");
                }
                else
                {
                    Logger.log.Notice($"Gotten custom settings for \"{textContent}\"");
                }
            }
            else
            {
                // Simple Color thing
                settings = GetFromType(UIEColorManager.GetSimpleColorConfig(), sType);
            }

            DecorateUnderlinedButtonForState(bsa, clipNormal, settings.NormalState, curvedTMP);
            DecorateUnderlinedButtonForState(bsa, clipHighlighted, settings.HighlightedState, curvedTMP);
            DecorateUnderlinedButtonForState(bsa, clipPressed, settings.NormalState, curvedTMP);
            DecorateUnderlinedButtonForState(bsa, clipDisabled, settings.DisabledState, curvedTMP);
        }

        private static void DecorateUnderlinedButtonForState(ButtonStaticAnimations bsa, AnimationClip clip, UnderlinedButton.UnderlinedButtonState state, TextMeshProUGUI TMP)
        {
            List<PluginConfig.FilterExclusion> list = _pluginConfig.DecorationExclusions.AllExclusionsForButton(bsa, ButtonType.Underlined);

            Logger.log.Notice($"{bsa.name} - Parent: {bsa.transform?.parent?.name}:");
            foreach(var e in list)
            {
                Logger.log.Critical($"{bsa.name} -> {e.ExclusionType} | {e.ExclusionTarget}");
            }

            if(!list.Any(x => x.IsGameObjectExclusion && x.ExclusionTarget.Equals("Content/Text")))
                UIEColorManager.SetAnimationTextColor<CurvedTextMeshPro>(clip, TMP, state.TextColor, "Content/Text");
            if (!list.Any(x => x.IsGameObjectExclusion && x.ExclusionTarget.Equals("BG")))
                UIEColorManager.SetAnimationFromImageViewSettings(clip, bsa.gameObject.GetComponentOnChild<ImageView>("BG"), state.BackgroundColors, "BG");
            if (!list.Any(x => x.IsGameObjectExclusion && x.ExclusionTarget.Equals("Underline")))
                UIEColorManager.SetAnimationFromImageViewSettings(clip, bsa.gameObject.GetComponentOnChild<ImageView>("Underline"), state.StrokeColors, "Underline");
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

        public enum ButtonType
        {
            Play, // The fancy one / play button
            Underlined, // Every Button with an underline
            Back, // Back button
            ModeSelection, // Solo / Online / Campaign / Party
            MainMenuSmall, // Help, Settings, Tutorial
            Unknown
        }

        public enum SpecialType
        {
            SoloMode,
            OnlineMode,
            PartyMode,
            CampaignMode,
            ModUnderline,
            Custom,
            Other
        }

        public enum DecorationFailedReason
        {
            Filtered,
            Error,
            Other
        }
    }
}
