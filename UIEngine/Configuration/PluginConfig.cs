using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using HMUI;
using IPA.Config.Stores;
using IPA.Config.Stores.Attributes;
using IPA.Config.Stores.Converters;
using UIEngine.Managers;
using UIEngine.Utilities;
using UnityEngine;

[assembly: InternalsVisibleTo(GeneratedStore.AssemblyVisibilityTarget)]
namespace UIEngine.Configuration
{
    public class PluginConfig
    {
        public virtual bool Enabled { get; set; } = true;

        [UseConverter(typeof(HexColorConverter))]
        public virtual Color SimplePrimaryColor { get; set; } = new Color(.52f, .12f, 1f);

        public virtual bool Advanced { get; set; } = false;

        public Buttons ButtonSettings { get; set; } = new Buttons();

        public Toggles ToggleSettings { get; set; } = new Toggles();

        public Segments SegmentSettings { get; set; } = new Segments();

        public Filters DecorationExclusions { get; set; } = new Filters();

        #region constructor
        public PluginConfig() { }

        private PluginConfig(Color col)
        {
            SimplePrimaryColor = col;
            ButtonSettings = Buttons.FromSimpleColor(col);
            ToggleSettings = Toggles.FromSimpleColor(col);
            SegmentSettings = Segments.FromSimpleColor(col);
        }

        public static PluginConfig FromSimpleColor(Color col)
        {
            return new PluginConfig(col);
        }
        #endregion constructor
        // Advanced Stuff

        public virtual void Changed()
        {
            /* Force BSIPA to Save */
        }

        public void Save()
        {
            Changed();
        }

        #region buttons
        public class Buttons
        {
            public Buttons() { }

            private Buttons(Color col)
            {
                PlayButtonSettings = PlayButton.FromSimpleColor(col);
                SoloMode = BigMainMenuButton.FromSimpleColor(col);
                OnlineMode = BigMainMenuButton.FromSimpleColor(col);
                PartyMode = BigMainMenuButton.FromSimpleColor(col);
                CampaignMode = BigMainMenuButton.FromSimpleColor(col);
                FallbackBigMenuButton = BigMainMenuButton.FromSimpleColor(col);
                ModUnderlinedButtons = UnderlinedButton.FromSimpleColor(col);
                FallbackUnderlinedButton = UnderlinedButton.FromSimpleColor(col);
            }

            public static Buttons FromSimpleColor(Color col)
            {
                return new Buttons(col);
            }

            public bool Enable { get; set; } = true;

            [NonNullable, UseConverter(typeof(ListConverter<CustomPlayButton>))]
            public List<CustomPlayButton> CustomPlayButtons { get; set; } = new List<CustomPlayButton>() { new CustomPlayButton() };

            public PlayButton PlayButtonSettings { get; set; } = new PlayButton();

            [NonNullable, UseConverter(typeof(ListConverter<CustomBigMainMenuButton>))]
            public List<CustomBigMainMenuButton> CustomBigMainMenuButtons { get; set; } = new List<CustomBigMainMenuButton>();

            public BigMainMenuButton SoloMode { get; set; } = new BigMainMenuButton();
            public BigMainMenuButton OnlineMode { get; set; } = new BigMainMenuButton();
            public BigMainMenuButton PartyMode { get; set; } = new BigMainMenuButton();
            public BigMainMenuButton CampaignMode { get; set; } = new BigMainMenuButton();
            public BigMainMenuButton FallbackBigMenuButton { get; set; } = new BigMainMenuButton();


            [NonNullable, UseConverter(typeof(ListConverter<CustomUnderlinedButton>))]
            public List<CustomUnderlinedButton> CustomUnderlinedButtons { get; set; } = new List<CustomUnderlinedButton>();

            public UnderlinedButton ModUnderlinedButtons { get; set; } = new UnderlinedButton();//ModButtons
            public UnderlinedButton FallbackUnderlinedButton { get; set; } = new UnderlinedButton();//ModButtons

            #region button_play
            public class PlayButton : ISettingsForStateProvider<ISettingsState, NoTransitionsButton.SelectionState>
            {
                public bool Enable { get; set; } = true;

                public PlayButtonState NormalState { get; set; } = new PlayButtonState();
                public PlayButtonState HighlightedState { get; set; } = new PlayButtonState();
                //public PlayButtonState PressedState { get; set; } = new PlayButtonState();
                public PlayButtonState DisabledState { get; set; } = new PlayButtonState();

                public class PlayButtonState : ISettingsState
                {
                    [UseConverter(typeof(HexColorConverter))]
                    public Color TextColor { get; set; } = Color.white;
                    [UseConverter(typeof(HexColorConverter))]
                    public Color ShineColor { get; set; } = Color.white;
                    [UseConverter(typeof(HexColorConverter))]
                    public Color RadialGlowColor { get; set; } = Color.white;

                    [NonNullable]
                    public ImageViewSettings BackgroundColors { get; set; } = new ImageViewSettings();

                    [NonNullable]
                    public ImageViewSettings BorderColors { get; set; } = new ImageViewSettings();

                    [NonNullable]
                    public ImageViewSettings OutlineColors { get; set; } = new ImageViewSettings();

                    public PlayButtonState() { }

                    private PlayButtonState(Color col)
                    {
                        BackgroundColors = ImageViewSettings.FromSimpleColor(col);
                        BorderColors = ImageViewSettings.FromSimpleColor(col.SaturatedColor(0.7f));
                        OutlineColors = ImageViewSettings.FromSimpleColor(col);
                    }

                    internal static PlayButtonState FromSimpleColor(Color col)
                    {
                        return new PlayButtonState(col);
                    }
                }

                public PlayButton() { }

                private PlayButton(Color col)
                {
                    NormalState = PlayButtonState.FromSimpleColor(col.SaturatedColor(0.75f));
                    HighlightedState = PlayButtonState.FromSimpleColor(col.SaturatedColor(0.9f));
                    DisabledState = PlayButtonState.FromSimpleColor(Color.black.ColorWithAlpha(0.25f));
                }

                internal static PlayButton FromSimpleColor(Color col)
                {
                    return new PlayButton(col);
                }

                public ISettingsState GetSettingsStateForButtonState(NoTransitionsButton.SelectionState state)
                {
                    switch(state)
                    {
                        case NoTransitionsButton.SelectionState.Disabled:
                            return DisabledState;
                        case NoTransitionsButton.SelectionState.Normal:
                            return NormalState;
                        case NoTransitionsButton.SelectionState.Highlighted:
                        case NoTransitionsButton.SelectionState.Pressed:
                        default:
                            return HighlightedState;
                    }
                }
            }

            public class CustomPlayButton : PlayButton, ICustomElementTarget
            {
                public string TargetMatchingMode { get; set; } = CustomElementTargetMatchingMode.TARGET_MODE_TEXT_CONTENT;
                public string TargetString { get; set; } = string.Empty;
            }
            #endregion button_play

            #region button_big_menu
            public class CustomBigMainMenuButton : BigMainMenuButton, ICustomElementTarget
            {
                public string TargetMatchingMode { get; set; } = CustomElementTargetMatchingMode.TARGET_MODE_TEXT_CONTENT;
                public string TargetString { get; set; } = string.Empty;
            }

            public class BigMainMenuButton
            {

                public BigMainMenuButtonState NormalState { get; set; } = new BigMainMenuButtonState();
                public BigMainMenuButtonState HighlightedState { get; set; } = new BigMainMenuButtonState();

                public class BigMainMenuButtonState
                {
                    [UseConverter(typeof(HexColorConverter))]
                    public Color TextColor { get; set; } = Color.white;
                    [UseConverter(typeof(HexColorConverter))]
                    public Color GlowColor { get; set; } = Color.white;
                    [NonNullable]
                    public ImageViewSettings OverlayColors { get; set; } = new ImageViewSettings();
                    [NonNullable]
                    public ImageViewSettings FillColors { get; set; } = new ImageViewSettings();

                    public BigMainMenuButtonState() { }

                    private BigMainMenuButtonState(Color col)
                    {
                        FillColors = ImageViewSettings.FromSimpleColor(Color.white.ColorWithAlpha(col.a));
                        OverlayColors = ImageViewSettings.FromSimpleColor(col);
                        GlowColor = col;
                    }

                    internal static BigMainMenuButtonState FromSimpleColor(Color col)
                    {
                        return new BigMainMenuButtonState(col);
                    }
                }

                public BigMainMenuButton() { }

                private BigMainMenuButton(Color col)
                {
                    NormalState = BigMainMenuButtonState.FromSimpleColor(col.SaturatedColor(.8f));
                    HighlightedState = BigMainMenuButtonState.FromSimpleColor(col.ColorWithAlpha(0f));
                }

                internal static BigMainMenuButton FromSimpleColor(Color col)
                {
                    return new BigMainMenuButton(col);
                }

            }
            #endregion button_big_menu

            public class CustomUnderlinedButton : UnderlinedButton, ICustomElementTarget
            {
                public string TargetMatchingMode { get; set; } = CustomElementTargetMatchingMode.TARGET_MODE_TEXT_CONTENT;
                public string TargetString { get; set; } = string.Empty;
            }

            public class UnderlinedButton
            {

                public UnderlinedButtonState NormalState { get; set; } = new UnderlinedButtonState();
                public UnderlinedButtonState HighlightedState { get; set; } = new UnderlinedButtonState();
                public UnderlinedButtonState DisabledState { get; set; } = new UnderlinedButtonState();

                public class UnderlinedButtonState
                {
                    [UseConverter(typeof(HexColorConverter))]
                    public Color TextColor { get; set; } = Color.white;
                    [NonNullable]
                    public ImageViewSettings BackgroundColors { get; set; } = new ImageViewSettings();
                    [NonNullable]
                    public ImageViewSettings StrokeColors { get; set; } = new ImageViewSettings();

                    public UnderlinedButtonState() { }
                    private UnderlinedButtonState(Color col)
                    {
                        if(UIEAnimationColorUtils.IsLightColor(col))
                        {
                            TextColor = Color.black;
                        }
                        BackgroundColors = ImageViewSettings.FromSimpleColor(new Color(0.2f, 0.2f, 0.2f).ColorWithAlpha(col.a));
                        StrokeColors = ImageViewSettings.FromSimpleColor(Color.white.ColorWithAlpha(col.a));
                    }
                    public static UnderlinedButtonState FromSimpleColor(Color col)
                    {
                        return new UnderlinedButtonState(col);
                    }
                }

                public UnderlinedButton() { }
                private UnderlinedButton(Color col)
                {
                    NormalState = UnderlinedButtonState.FromSimpleColor(col);
                    HighlightedState = UnderlinedButtonState.FromSimpleColor(col);
                    DisabledState = UnderlinedButtonState.FromSimpleColor(col.ColorWithAlpha(.2f));
                }
                public static UnderlinedButton FromSimpleColor(Color col)
                {
                    return new UnderlinedButton(col);
                }
            }

            // TODO
            public class SmallMainMenuButton
            {
                [NonNullable]
                public ImageViewSettings Colors { get; set; } = new ImageViewSettings();
            }

        }
        #endregion buttons

        #region toggles
        public class Toggles
        {
            public Toggles() { }

            private Toggles(Color col)
            {
                ToggleSettings = ToggleSetting.FromSimpleColor(col);
            }

            public static Toggles FromSimpleColor(Color col)
            {
                return new Toggles(col);
            }

            public bool Enable { get; set; } = true;

            [NonNullable, UseConverter(typeof(ListConverter<CustomToggleSetting>))]
            public List<CustomToggleSetting> CustomToggleSettings { get; set; } = new List<CustomToggleSetting>() { new CustomToggleSetting() };

            [NonNullable]
            public ToggleSetting ToggleSettings = new ToggleSetting();

            public class ToggleSetting
            {
                public virtual ToggleState OnColors { get; set; } = new ToggleState(new Color(.2f, .6f, .2f), null);

                public virtual ToggleState OffColors { get; set; } = new ToggleState(new Color(.6f, .2f, .2f), null);

                public virtual ToggleState OnHighlightedColors { get; set; } = new ToggleState(new Color(.1f, 1f, .1f), Color.gray);

                public virtual ToggleState OffHighlightedColors { get; set; } = new ToggleState(new Color(1f, .1f, .1f), Color.gray);

                public virtual ToggleState DisabledColors { get; set; } = new ToggleState(Color.gray, null, 0.25f);

                public ToggleSetting() { }

                private ToggleSetting(Color col)
                {
                    OnColors = new ToggleState(col.SaturatedColor(.9f), null);
                    OffColors = new ToggleState(Color.red.SaturatedColor(.7f), null);
                    OnHighlightedColors = new ToggleState(col, null);
                    OffHighlightedColors = new ToggleState(Color.red.SaturatedColor(.9f), null);
                    DisabledColors = new ToggleState(Color.gray, null, 0.25f);
                }

                public static ToggleSetting FromSimpleColor(Color col)
                {
                    return new ToggleSetting(col);
                }
            }

            public class CustomToggleSetting : ToggleSetting, ICustomElementTarget
            {
                public string TargetMatchingMode { get; set; } = CustomElementTargetMatchingMode.TARGET_MODE_TEXT_CONTENT;
                public string TargetString { get; set; } = string.Empty;
            }

            public class ToggleState
            {
                public ToggleState()
                {

                }

                public ToggleState(Color? knob, Color? background, float backgroundAlpha = 0.5f)
                {
                    KnobColor = knob ?? Color.white;
                    BackgroundColor = background ?? Color.black;
                    BackgroundAlpha = backgroundAlpha;
                }

                [UseConverter(typeof(HexColorConverter))]
                public Color KnobColor { get; set; } = Color.white;
                [UseConverter(typeof(HexColorConverter))]
                public Color BackgroundColor { get; set; } = Color.black;
                public float BackgroundAlpha { get; set; } = .5f;
            }
        }
        #endregion toggles

        #region segments
        public class Segments
        {
            public Segments() { }

            private Segments(Color col)
            {
                SegmentSettings = SegmentSetting.FromSimpleColor(col);
            }

            public static Segments FromSimpleColor(Color col)
            {
                return new Segments(col);
            }

            public bool Enable { get; set; } = true;

            [NonNullable, UseConverter(typeof(ListConverter<CustomSegmentSetting>))]
            public List<CustomSegmentSetting> CustomSegmentSettings { get; set; } = new List<CustomSegmentSetting>() { new CustomSegmentSetting() };

            [NonNullable]
            public SegmentSetting SegmentSettings = new SegmentSetting();

            public class SegmentSetting
            {
                public SegmentSetting() { }
                public SegmentSetting(Color col)
                {
                    Normal = new SegmentState(Color.white, null);
                    Highlighted = new SegmentState(col.SaturatedColor(.9f), Color.white);
                    Selected = new SegmentState(col.SaturatedColor(.9f), Color.white, 0.32f);
                    SelectedAndHighlighted = new SegmentState(col, Color.white, 0.35f);
                }

                public SegmentState Normal { get; set; } = new SegmentState(Color.white, null);
                public SegmentState Highlighted { get; set; } = new SegmentState(new Color(0.8f, 0f, 0.8f), Color.white);
                public SegmentState Selected { get; set; } = new SegmentState(new Color(0.8f, 0f, 0.8f), Color.white, 0.32f);
                public SegmentState SelectedAndHighlighted { get; set; } = new SegmentState(new Color(0.9f, 0f, 0.9f), Color.white, 0.35f);

                public static SegmentSetting FromSimpleColor(Color col)
                {
                    return new SegmentSetting(col);
                }
            }

            public class CustomSegmentSetting : SegmentSetting, ICustomElementTarget
            {
                [Ignore]
                public string TargetMatchingMode { get; set; } = CustomElementTargetMatchingMode.TARGET_MODE_TEXT_CONTENT;
                public string TargetString { get; set; } = string.Empty;
            }

            public class SegmentState
            {
                public SegmentState() { }
                public SegmentState(Color col, Color? backgroundColor, float backgroundAlpha = 0.3f)
                {
                    BaseColor = col;
                    BackgroundColor = backgroundColor ?? Color.black;
                    BackgroundAlpha = backgroundAlpha;
                }

                [UseConverter(typeof(HexColorConverter))]
                public Color BaseColor { get; set; } = Color.white;
                [UseConverter(typeof(HexColorConverter))]
                public Color BackgroundColor { get; set; } = Color.white;
                public float BackgroundAlpha { get; set; } = 0.5f;

            }

        }

        #endregion segments

        /// <summary>
        /// Exclude parts of specific buttons or children of objects
        /// </summary>
        public class Filters
        {
            public bool Enabled { get; set; } = true;

            [NonNullable, UseConverter(typeof(ListConverter<FilterTarget>))]
            public List<FilterTarget> FilterRules { get; set; } = new List<FilterTarget>() {
                new FilterTarget() { // SongBrowser stuff
                    TargetButtonType = UIEButtonManager.ButtonType.Underlined.ToString(),
                    TargetMatchingMode = CustomElementTargetMatchingMode.TARGET_MODE_PARENT_GAMEOBJECT_NAME,
                    TargetString = "SongBrowserViewController"
                },
                new FilterTarget() { // SongBrowser stuff
                    TargetButtonType = UIEButtonManager.ButtonType.Underlined.ToString(),
                    TargetMatchingMode = CustomElementTargetMatchingMode.TARGET_MODE_GAMEOBJECT_NAME,
                    TargetString = "randomButton_hoverHintText",
                    Exclusions = new List<FilterExclusion>() {
                        new FilterExclusion()
                        {
                            ExclusionType = FilterExclusion.EXCLUSION_TYPE_CHILD_GAMEOBJECT,
                            ExclusionTarget = "BG"
                        }
                    }
                },
                new FilterTarget() { // SongBrowser stuff
                    TargetButtonType = UIEButtonManager.ButtonType.Underlined.ToString(),
                    TargetMatchingMode = CustomElementTargetMatchingMode.TARGET_MODE_GAMEOBJECT_NAME,
                    TargetString = "ClearSortAndFilterButton_hoverHintText",
                    Exclusions = new List<FilterExclusion>() {
                        new FilterExclusion()
                        {
                            ExclusionType = FilterExclusion.EXCLUSION_TYPE_CHILD_GAMEOBJECT,
                            ExclusionTarget = "BG"
                        }
                    }
                },
                new FilterTarget() { // SongBrowser stuff
                    TargetButtonType = UIEButtonManager.ButtonType.Underlined.ToString(),
                    TargetMatchingMode = CustomElementTargetMatchingMode.TARGET_MODE_GAMEOBJECT_NAME,
                    TargetString = "playlistExportButton_hoverHintText",
                    Exclusions = new List<FilterExclusion>() {
                        new FilterExclusion()
                        {
                            ExclusionType = FilterExclusion.EXCLUSION_TYPE_CHILD_GAMEOBJECT,
                            ExclusionTarget = "BG"
                        }
                    }
                },
                new FilterTarget() { // Filter the Underline of the SRM button
                    TargetButtonType = UIEButtonManager.ButtonType.Underlined.ToString(),
                    TargetMatchingMode = CustomElementTargetMatchingMode.TARGET_MODE_TEXT_CONTENT,
                    TargetString = "SRM"
                }
            };

            public List<FilterTarget> AllRulesForButtonType(UIEButtonManager.ButtonType buttonType)
            {
                return AllRulesForButtonType(buttonType, FilterRules);
            }

            public static List<FilterTarget> AllRulesForButtonType(UIEButtonManager.ButtonType buttonType, List<FilterTarget> FilterRules)
            {
                List<FilterTarget> list = new List<FilterTarget>();

                foreach(var target in FilterRules)
                {
                    if(target.TargetButtonType.Equals(buttonType.ToString()) || target.TargetButtonType.Equals("Any"))
                    {
                        list.Add(target);
                    }
                }

                return list;
            }

            public static List<FilterTarget> AllMatchingTargets(ButtonStaticAnimations bsa, UIEButtonManager.ButtonType buttonType, List<FilterTarget> FilterRules)
            {
                List<FilterTarget> list = new List<FilterTarget>();

                foreach(var target in FilterRules)
                {

                    switch(target.TargetMatchingMode)
                    {
                        case CustomElementTargetMatchingMode.TARGET_MODE_ANY:
                            list.Add(target);
                            break;
                        case CustomElementTargetMatchingMode.TARGET_MODE_ASSEMBLY_NAME:
                            Assembly assembly = UIEButtonManager.GetAssemblyForButton(bsa, buttonType);
                            if (assembly != null && assembly.GetName().Name.Equals(target.TargetString, StringComparison.CurrentCultureIgnoreCase))
                                list.Add(target);
                            break;
                        case CustomElementTargetMatchingMode.TARGET_MODE_GAMEOBJECT_NAME:
                            if (bsa.gameObject.name.Equals(target.TargetString, StringComparison.CurrentCultureIgnoreCase))
                                list.Add(target);
                            break;
                        case CustomElementTargetMatchingMode.TARGET_MODE_PARENT_GAMEOBJECT_NAME:
                            if ((bsa.gameObject.transform?.parent?.gameObject.name.Equals(target.TargetString, StringComparison.CurrentCultureIgnoreCase)).GetValueOrDefault(false))
                                list.Add(target);
                            break;
                        case CustomElementTargetMatchingMode.TARGET_MODE_TEXT_CONTENT:
                            string textContent = UIEButtonManager.GetTextContentForButtonType(bsa, buttonType);
                            if (textContent != string.Empty && textContent.Equals(target.TargetString, StringComparison.CurrentCultureIgnoreCase))
                                list.Add(target);
                            break;
                    }

                }

                return list;
            }

            public List<FilterExclusion> AllExclusionsForButton(ButtonStaticAnimations bsa, UIEButtonManager.ButtonType buttonType)
            {
                List<FilterExclusion> list = new List<FilterExclusion>();

                List<FilterTarget> FilterRules = AllRulesForButtonType(buttonType);

                FilterRules = AllMatchingTargets(bsa, buttonType, FilterRules);

                foreach(var target in FilterRules)
                {
                    foreach(var exclusion in target.Exclusions)
                    {
                        list.Add(exclusion);
                    }
                }

                return list;
            }
        }

        public class FilterTarget
        {
            // TODO: use on the other types of elements as well maybe?
            // public string TargetElement { get; set; } = "Button"; // "Toggle" / "Segment"
            public string TargetButtonType { get; set; } = UIEButtonManager.ButtonType.Underlined.ToString();
            public string TargetMatchingMode { get; set; } = CustomElementTargetMatchingMode.TARGET_MODE_GAMEOBJECT_NAME;
            public string TargetString { get; set; } = string.Empty;

            [NonNullable, UseConverter(typeof(ListConverter<FilterExclusion>))]
            public List<FilterExclusion> Exclusions { get; set; } = new List<FilterExclusion>()
            {
                new FilterExclusion() {
                    ExclusionType = FilterExclusion.EXCLUSION_TYPE_CHILD_GAMEOBJECT,
                    ExclusionTarget = "Underline"
                }
            };
        }

        public class FilterExclusion
        {
            [Ignore]
            public const string EXCLUSION_TYPE_PROPERTY = "Property";
            [Ignore]
            public const string EXCLUSION_TYPE_CHILD_GAMEOBJECT = "ChildGameObject";

            [Ignore]
            public bool IsPropertyExclusion {
                get
                {
                    return ExclusionType.Equals(EXCLUSION_TYPE_PROPERTY);
                }
            }

            [Ignore]
            public bool IsGameObjectExclusion
            {
                get
                {
                    return ExclusionType.Equals(EXCLUSION_TYPE_CHILD_GAMEOBJECT);
                }
            }

            public string ExclusionType { get; set; } = EXCLUSION_TYPE_CHILD_GAMEOBJECT;
            public string ExclusionTarget { get; set; } = string.Empty;
            
        }

        public class ImageViewSettings
        {
            [Ignore]
            public const string GRADIENT_DIRECTION_NONE = "None";
            [Ignore]
            public const string GRADIENT_DIRECTION_VERTICAL = "Vertical";
            [Ignore]
            public const string GRADIENT_DIRECTION_HORIZONTAL = "Horizontal";

            [UseConverter(typeof(HexColorConverter))]
            public Color BaseColor { get; set; } = Color.white;

            public string GradientDirection { get; set; } = GRADIENT_DIRECTION_NONE; // None, Vertical, Horizontal

            public bool FlipGradient { get; set; } = false;


            [UseConverter(typeof(HexColorConverter))]
            public Color GradientColor0 { get; set; } = Color.white;
            [UseConverter(typeof(HexColorConverter))]
            public Color GradientColor1 { get; set; } = Color.white;

            public ImageViewSettings() { }

            private ImageViewSettings(Color col)
            {
                BaseColor = col;
                GradientDirection = GRADIENT_DIRECTION_NONE;
            }

            public static ImageViewSettings FromSimpleColor(Color col)
            {
                return new ImageViewSettings(col);
            }
        }

        public interface ICustomElementTarget
        {
            public string TargetMatchingMode { get; set; }
            public string TargetString { get; set; }
        }

        public interface ISettingsForStateProvider<T, BS>
        {
            public T GetSettingsStateForButtonState(BS state);
        }

        public interface ISettingsState
        {

        }

        public static class CustomElementTargetMatchingMode {
            public const string TARGET_MODE_ANY = "Any";
            public const string TARGET_MODE_ASSEMBLY_NAME = "Assembly";
            public const string TARGET_MODE_GAMEOBJECT_NAME = "GameObject";
            public const string TARGET_MODE_TEXT_CONTENT = "ElementText";
            public const string TARGET_MODE_PARENT_GAMEOBJECT_NAME = "ParentGameObject";
        }

    }
}
