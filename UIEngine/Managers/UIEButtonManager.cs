using HMUI;
using IPA.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using UIEngine.Components;
using UIEngine.Configuration;
using UIEngine.Extensions;
using UIEngine.Utilities;
using UnityEngine;
using static UIEngine.Accessors;
using static UIEngine.Configuration.PluginConfig.Buttons;
using static UIEngine.Utilities.UIEAnimationColorUtils;

namespace UIEngine.Managers
{
    public class UIEButtonManager : UIEAnimationElementManagerBase<ButtonStaticAnimations>
    {
        private static Dictionary<ButtonStaticAnimations, (ButtonType, SpecialType)> _buttonTypeDictionary;
        private static Dictionary<ButtonStaticAnimations, Assembly> _assemblyForModButtons;

        public static event Action<ButtonStaticAnimations> onButtonDecoratedEvent;
        public static event Action<ButtonStaticAnimations, DecorationFailedReason> onButtonDecorationFailedEvent;

        internal UIEButtonManager(PluginConfig pluginConfig) : base(pluginConfig)
        {
            _buttonTypeDictionary = new Dictionary<ButtonStaticAnimations, (ButtonType, SpecialType)>();
            _assemblyForModButtons = new Dictionary<ButtonStaticAnimations, Assembly>();
        }

        public override bool ShouldDecorateElement(ButtonStaticAnimations element)
        {
            if (pluginConfig.Enabled && pluginConfig.Advanced)
                return pluginConfig.ButtonSettings.Enable;

            #region  mightRemoveLaterJank
            ButtonType bType;
            SpecialType sType;

            if (_buttonTypeDictionary.TryGetValue(element, out (ButtonType, SpecialType) types))
            {
                bType = types.Item1;
                sType = types.Item2;
            }
            else
            {
                bType = GetButtonType(element);
                sType = GetSpecialType(element, bType);
                _buttonTypeDictionary.Add(element, (bType, sType));
            }

            Assembly assembly = null;

            if (sType == SpecialType.ModUnderline && !_assemblyForModButtons.TryGetValue(element, out assembly))
            {
                assembly = GetAssemblyForButton(element, bType);

                if (assembly != null && !_assemblyForModButtons.ContainsKey(element))
                {
                    _assemblyForModButtons.Add(element, assembly);
                }
            }

            if (assembly == Assembly.GetExecutingAssembly())
            {
                // our button
                return true;
            }
            #endregion  mightRemoveLaterJank

            return pluginConfig.Enabled;
        }

        public override void DecorateElement(ButtonStaticAnimations element)
        {
            ButtonType bType;
            SpecialType sType;

            if (_buttonTypeDictionary.TryGetValue(element, out (ButtonType, SpecialType) types))
            {
                bType = types.Item1;
                sType = types.Item2;
            }
            else
            {
                bType = GetButtonType(element);
                sType = GetSpecialType(element, bType);
                _buttonTypeDictionary.Add(element, (bType, sType));
            }

            string gameObjectName = element.gameObject.name;


            Assembly assembly = null;

            if (sType == SpecialType.ModUnderline && !_assemblyForModButtons.TryGetValue(element, out assembly))
            {
                assembly = GetAssemblyForButton(element, bType);

                if (assembly != null && !_assemblyForModButtons.ContainsKey(element))
                {
                    _assemblyForModButtons.Add(element, assembly);
                }
            }

            if(assembly == Assembly.GetExecutingAssembly())
            {
                // our button
                UnicornPuke unicornPuke = element.gameObject.AddComponent<UnicornPuke>();

                unicornPuke.Init(pluginConfig, new List<(string relativePath, string property, Type animationTargetType)>() {
                    ("Underline", "color", typeof(ImageView))
                });
            }

            ref AnimationClip clipNormal = ref ButtonStaticAnimations_normalClip(ref element);
            ref AnimationClip clipHighlighted = ref ButtonStaticAnimations_highlightedClip(ref element);
            ref AnimationClip clipPressed = ref ButtonStaticAnimations_pressedClip(ref element);
            ref AnimationClip clipDisabled = ref ButtonStaticAnimations_disabledClip(ref element);


            PluginConfig.Filters filters = pluginConfig.DecorationExclusions;

            if (filters.FilterRules.Any(x => CancelButtonDecoration(x, bType, sType, element)))
            {
                Logger.log.Debug($"Filtered decoration of button \"{element.name}\" - Not decorating!");
                onButtonDecorationFailedEvent?.Invoke(element, DecorationFailedReason.Filtered);
                return;
            }


            CreateNewDefaultAnimationsIfNeeded(element, ref clipNormal, ref clipHighlighted, ref clipPressed, ref clipDisabled, bType);

            Logger.log.Notice($"Decorating Button \"{element.gameObject.name}\" ({bType}) [{assembly?.GetName().Name}]");

            switch (bType)
            {
                case ButtonType.Play:
                    DecoratePlayButton(element, clipNormal, clipHighlighted, clipPressed, clipDisabled);
                    break;
                case ButtonType.Underlined:
                    DecorateUnderlinedButton(element, clipNormal, clipHighlighted, clipPressed, clipDisabled, sType);
                    break;
                case ButtonType.ModeSelection:
                    DecorateModeSelectionButton(element, clipNormal, clipHighlighted, clipPressed, clipDisabled, sType);
                    break;
                case ButtonType.Back:
                    // TODO implement custom back buttons based on title name
                    /*UIEColorManager.SetAnimationImageViewColors(ref clipNormal, "BG", cm.IsAdvanced() ? cm.backButtonNormal : cm.SimplePrimaryNormal);
                    UIEColorManager.SetAnimationImageViewColors(ref clipHighlighted, "BG", cm.IsAdvanced() ? cm.backButtonHighlight : cm.simplePrimaryHighlight);*/
                    break;
            }

            element.OnEnable();

            onButtonDecoratedEvent?.Invoke(element);
        }

        private static bool CancelButtonDecoration(PluginConfig.FilterTarget filterTarget, ButtonType bType, SpecialType sType, ButtonStaticAnimations bsa)
        {
            if(filterTarget.TargetButtonType.Equals("Any") || filterTarget.TargetButtonType.Equals(bType))
            {
                // Filter applies to this button type

                if(filterTarget.TargetMatchingMode.Equals(PluginConfig.CustomElementTargetMatchingMode.TARGET_MODE_ANY))
                {
                    return true;
                }
            }

            return false;
        }

        private void CreateNewDefaultAnimationsIfNeeded(ButtonStaticAnimations bsa, ref AnimationClip clipNormal, ref AnimationClip clipHighlighted, ref AnimationClip clipPressed, ref AnimationClip clipDisabled, ButtonType bType)
        {
            if (animationClipsForElement.TryGetValue(bsa, out (AnimationClip, AnimationClip, AnimationClip, AnimationClip) clips))
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
                    SetDefaultAnimationsForClip(clipNormal, AnimationData.PlayButton.DefaultAnimatedTextButtonNormal.WithFiltersApplied(pluginConfig.DecorationExclusions.AllExclusionsForButton(bsa, bType)));
                    SetDefaultAnimationsForClip(clipHighlighted, AnimationData.PlayButton.DefaultAnimatedTextButtonHighlighted.WithFiltersApplied(pluginConfig.DecorationExclusions.AllExclusionsForButton(bsa, bType)));
                    SetDefaultAnimationsForClip(clipPressed, AnimationData.PlayButton.DefaultAnimatedTextButtonPressed.WithFiltersApplied(pluginConfig.DecorationExclusions.AllExclusionsForButton(bsa, bType)));
                    SetDefaultAnimationsForClip(clipDisabled, AnimationData.PlayButton.DefaultAnimatedTextButtonDisabled.WithFiltersApplied(pluginConfig.DecorationExclusions.AllExclusionsForButton(bsa, bType)));
                    break;
                case ButtonType.ModeSelection:
                    AssignNewClips(ref clipNormal, ref clipHighlighted, ref clipPressed, ref clipDisabled);
                    SetDefaultAnimationsForClip(clipNormal, AnimationData.BigMenuButton.DefaultBigMenuButtonNormal.WithFiltersApplied(pluginConfig.DecorationExclusions.AllExclusionsForButton(bsa, bType)));
                    SetDefaultAnimationsForClip(clipHighlighted, AnimationData.BigMenuButton.DefaultBigMenuButtonHighlighted.WithFiltersApplied(pluginConfig.DecorationExclusions.AllExclusionsForButton(bsa, bType)));
                    SetDefaultAnimationsForClip(clipPressed, AnimationData.BigMenuButton.DefaultBigMenuButtonPressed.WithFiltersApplied(pluginConfig.DecorationExclusions.AllExclusionsForButton(bsa, bType)));
                    SetDefaultAnimationsForClip(clipDisabled, AnimationData.BigMenuButton.DefaultBigMenuButtonDisabled.WithFiltersApplied(pluginConfig.DecorationExclusions.AllExclusionsForButton(bsa, bType)));
                    break;
                case ButtonType.Underlined:
                    AssignNewClips(ref clipNormal, ref clipHighlighted, ref clipPressed, ref clipDisabled);
                    SetDefaultAnimationsForClip(clipNormal, AnimationData.TextMenuButton.DefaultTextMenuButtonNormal.WithFiltersApplied(pluginConfig.DecorationExclusions.AllExclusionsForButton(bsa, bType)));
                    SetDefaultAnimationsForClip(clipHighlighted, AnimationData.TextMenuButton.DefaultTextMenuButtonHighlighted.WithFiltersApplied(pluginConfig.DecorationExclusions.AllExclusionsForButton(bsa, bType)));
                    SetDefaultAnimationsForClip(clipPressed, AnimationData.TextMenuButton.DefaultTextMenuButtonPressed.WithFiltersApplied(pluginConfig.DecorationExclusions.AllExclusionsForButton(bsa, bType)));
                    SetDefaultAnimationsForClip(clipDisabled, AnimationData.TextMenuButton.DefaultTextMenuButtonDisabled.WithFiltersApplied(pluginConfig.DecorationExclusions.AllExclusionsForButton(bsa, bType)));
                    break;
            }
            
            animationClipsForElement.Add(bsa, (clipNormal, clipHighlighted, clipPressed, clipDisabled));
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

        private static bool GetCustomButtonSettings<T, R>(GameObject go, string text, List<T> inList, out R settings) where T : PluginConfig.ICustomElementTarget, R
        {
            string goName = go.name;
            foreach(T s in inList)
            {
                switch(s.TargetMatchingMode)
                {
                    case PluginConfig.CustomElementTargetMatchingMode.TARGET_MODE_GAMEOBJECT_NAME:
                        if (s.TargetString.Equals(goName, StringComparison.CurrentCultureIgnoreCase))
                        {
                            settings = s;
                            return true;
                        }
                        break;
                    case PluginConfig.CustomElementTargetMatchingMode.TARGET_MODE_TEXT_CONTENT:
                        if(s.TargetString.Equals(text, StringComparison.CurrentCultureIgnoreCase))
                        {
                            settings = s;
                            return true;
                        }
                        break;
                    case PluginConfig.CustomElementTargetMatchingMode.TARGET_MODE_PARENT_GAMEOBJECT_NAME:
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

        private void DecoratePlayButton(ButtonStaticAnimations bsa, AnimationClip clipNormal, AnimationClip clipHighlighted, AnimationClip clipPressed, AnimationClip clipDisabled)
        {

            var curvedTMP = bsa.gameObject.GetComponentOnChild<CurvedTextMeshPro>("Content/Text");

            string textContent = curvedTMP.text;

            PlayButton settings;

            if(pluginConfig.Advanced)
            {
                if (!GetCustomButtonSettings<CustomPlayButton, PlayButton>(bsa.gameObject, textContent, pluginConfig.ButtonSettings.CustomPlayButtons, out settings))
                {
                    // Use Default Settings instead
                    settings = pluginConfig.ButtonSettings.PlayButtonSettings;
                }
            } 
            else
            {
                // Simple Color thing
                settings = UIEAnimationColorUtils.GetSimpleColorButtonSettings(pluginConfig).PlayButtonSettings;
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
            UIEAnimationColorUtils.SetAnimationTextColor<CurvedTextMeshPro>(clip, TMP, state.TextColor, "Content/Text");
            UIEAnimationColorUtils.SetAnimationFromImageViewSettings(clip, disabledState ? bsa.gameObject.GetComponentOnChild<ImageView>("BGDisabled")  : bsa.gameObject.GetComponentOnChild<ImageView>("BG"), state.BackgroundColors, disabledState ? "BGDisabled" : "BG");
            UIEAnimationColorUtils.SetAnimationFromImageViewSettings(clip, bsa.gameObject.GetComponentOnChild<ImageView>("Border"), state.BorderColors, "Border");
            UIEAnimationColorUtils.SetAnimationFromImageViewSettings(clip, bsa.gameObject.GetChildByName("OutlineWrapper")?.GetComponentOnChild<ImageView>("Outline"), state.OutlineColors, "OutlineWrapper/Outline");
        }

        private void DecorateModeSelectionButton(ButtonStaticAnimations bsa, AnimationClip clipNormal, AnimationClip clipHighlighted, AnimationClip clipPressed, AnimationClip clipDisabled, SpecialType sType)
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
            if (pluginConfig.Advanced)
            {
                if (!GetCustomButtonSettings<CustomBigMainMenuButton, BigMainMenuButton>(bsa.gameObject, textContent, pluginConfig.ButtonSettings.CustomBigMainMenuButtons, out settings))
                {
                    // Use Default Settings instead
                    settings = GetFromType(pluginConfig, sType);
                }
            }
            else
            {
                // Simple Color thing
                settings = GetFromType(UIEAnimationColorUtils.GetSimpleColorConfig(pluginConfig), sType);
            }

            DecorateModeButtonForState(bsa, clipNormal, settings.NormalState, curvedTMP);
            DecorateModeButtonForState(bsa, clipHighlighted, settings.HighlightedState, curvedTMP);
            DecorateModeButtonForState(bsa, clipPressed, settings.HighlightedState, curvedTMP);
            //DecorateModeButtonForState(bsa, clipDisabled, settings.NormalState, curvedTMP);
        }

        private static void DecorateModeButtonForState(ButtonStaticAnimations bsa, AnimationClip clip, BigMainMenuButton.BigMainMenuButtonState state, TextMeshProUGUI TMP)
        {
            UIEAnimationColorUtils.SetAnimationTextColor<CurvedTextMeshPro>(clip, TMP, state.TextColor, "Text");
            UIEAnimationColorUtils.SetAnimationClipTubeBloomPrePassLightColor(clip, state.GlowColor, "Image/Glow");


            ImageView fill_imageView = bsa.gameObject.GetComponentOnChild<ImageView>("Image/Image0");

            /*var swap = bsa.gameObject.GetComponent<ButtonSpriteSwap>();

            Sprite _normal = swap.GetField<Sprite, ButtonSpriteSwap>("_normalStateSprite");
            Sprite _hover = swap.GetField<Sprite, ButtonSpriteSwap>("_highlightStateSprite");

            _normal = UIEAnimationColorUtils.MonochromifySprite(_normal);
            _hover = UIEAnimationColorUtils.MonochromifySprite(_hover);

            swap.SetField("_disabledStateSprite", _normal);
            swap.SetField("_normalStateSprite", _normal);
            swap.SetField("_highlightStateSprite", _hover);
            //Unneeded?
            swap.SetField("_pressedStateSprite", _hover);
            
*/

            UIEAnimationColorUtils.SetAnimationFromImageViewSettings(clip, fill_imageView, state.FillColors, "Image/Image0", setAlphaValues: true);
            UIEAnimationColorUtils.SetAnimationFromImageViewSettings(clip, bsa.gameObject.GetComponentOnChild<ImageView>("Image/ImageOverlay"), state.OverlayColors, "Image/ImageOverlay");
        }

        private void DecorateUnderlinedButton(ButtonStaticAnimations bsa, AnimationClip clipNormal, AnimationClip clipHighlighted, AnimationClip clipPressed, AnimationClip clipDisabled, SpecialType sType)
        {
            // TODO

            var curvedTMP = bsa.gameObject.GetComponentOnChild<CurvedTextMeshPro>("Content/Text");

            string textContent = curvedTMP?.text ?? string.Empty;

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
            if (pluginConfig.Advanced)
            {
                if (!GetCustomButtonSettings<CustomUnderlinedButton, UnderlinedButton>(bsa.gameObject, textContent, pluginConfig.ButtonSettings.CustomUnderlinedButtons, out settings))
                {
                    // Use Default Settings instead
                    settings = GetFromType(pluginConfig, sType);
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
                settings = GetFromType(UIEAnimationColorUtils.GetSimpleColorConfig(pluginConfig), sType);
            }

            DecorateUnderlinedButtonForState(bsa, clipNormal, settings.NormalState, curvedTMP);
            DecorateUnderlinedButtonForState(bsa, clipHighlighted, settings.HighlightedState, curvedTMP);
            DecorateUnderlinedButtonForState(bsa, clipPressed, settings.NormalState, curvedTMP);
            DecorateUnderlinedButtonForState(bsa, clipDisabled, settings.DisabledState, curvedTMP);
        }

        private void DecorateUnderlinedButtonForState(ButtonStaticAnimations bsa, AnimationClip clip, UnderlinedButton.UnderlinedButtonState state, TextMeshProUGUI TMP)
        {
            List<PluginConfig.FilterExclusion> list = pluginConfig.DecorationExclusions.AllExclusionsForButton(bsa, ButtonType.Underlined);

            /*Logger.log.Notice($"{bsa.name} - Parent: {bsa.transform?.parent?.name}:");
            foreach(var e in list)
            {
                Logger.log.Critical($"{bsa.name} -> {e.ExclusionType} | {e.ExclusionTarget}");
            }*/

            if(!list.Any(x => x.IsGameObjectExclusion && x.ExclusionTarget.Equals("Content/Text")))
                UIEAnimationColorUtils.SetAnimationTextColor<CurvedTextMeshPro>(clip, TMP, state.TextColor, "Content/Text");
            if (!list.Any(x => x.IsGameObjectExclusion && x.ExclusionTarget.Equals("BG")))
                UIEAnimationColorUtils.SetAnimationFromImageViewSettings(clip, bsa.gameObject.GetComponentOnChild<ImageView>("BG"), state.BackgroundColors, "BG");
            if (!list.Any(x => x.IsGameObjectExclusion && x.ExclusionTarget.Equals("Underline")))
                UIEAnimationColorUtils.SetAnimationFromImageViewSettings(clip, bsa.gameObject.GetComponentOnChild<ImageView>("Underline"), state.StrokeColors, "Underline");
        }

        private static ButtonType GetButtonType(ButtonStaticAnimations bsa)
        {
            Transform buttonTransform = bsa.gameObject.transform;

            List<string> childNames = Utilities.Utilities.GetChildrenAsList(buttonTransform);

            if (Utilities.Utilities.ListContainsAll(childNames, playButtonChildren)) return ButtonType.Play;
            if (Utilities.Utilities.ListContainsAll(childNames, underlinedButtonChildren)) return ButtonType.Underlined;
            if (bsa.gameObject.name.Equals("BackButton") && Utilities.Utilities.ListContainsAll(childNames, backButtonChildren)) return ButtonType.Back;

            if(Utilities.Utilities.ListContainsAll(childNames, modeButtonChildren))
            {
                if(buttonTransform.childCount > 0)
                {
                    List<string> childrenOfChild0 = Utilities.Utilities.GetChildrenAsList(buttonTransform.GetChild(0));

                    if(Utilities.Utilities.ListContainsAll(childrenOfChild0, modeButtonImageChildren))
                    {
                        return ButtonType.ModeSelection;
                    }
                }
            }

            if(Utilities.Utilities.ListContainsAll(childNames, menuSmallButtonChildren))
            {
                if (buttonTransform.childCount > 0)
                {
                    List<string> childrenOfChild0 = Utilities.Utilities.GetChildrenAsList(buttonTransform.GetChild(0));

                    if (Utilities.Utilities.ListContainsAll(childrenOfChild0, menuSmallButtonImageChildren))
                    {
                        return ButtonType.MainMenuSmall;
                    }
                }
            }

            return ButtonType.Unknown;
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
