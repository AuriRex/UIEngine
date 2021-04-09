using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using IPA.Config.Stores;
using IPA.Config.Stores.Attributes;
using IPA.Config.Stores.Converters;
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

        // Advanced Stuff

        // Toggles
        [UseConverter(typeof(HexColorConverter))]
        public virtual Color OnColorsKnob { get; set; } = new Color(.2f, .8f, .2f);
        [UseConverter(typeof(HexColorConverter))]
        public virtual Color OnColorsBG { get; set; } = Color.black;

        [UseConverter(typeof(HexColorConverter))]
        public virtual Color OffColorsKnob { get; set; } = new Color(.8f, .2f, .2f);
        [UseConverter(typeof(HexColorConverter))]
        public virtual Color OffColorsBG { get; set; } = Color.black;

        [UseConverter(typeof(HexColorConverter))]
        public virtual Color OnHighlightedColorsKnob { get; set; } = new Color(.1f, 1f, .1f);
        [UseConverter(typeof(HexColorConverter))]
        public virtual Color OnHighlightedColorsBG { get; set; } = Color.black;

        [UseConverter(typeof(HexColorConverter))]
        public virtual Color OffHighlightedColorsKnob { get; set; } = new Color(1f, .1f, .1f);
        [UseConverter(typeof(HexColorConverter))]
        public virtual Color OffHighlightedColorsBG { get; set; } = Color.black;

        [UseConverter(typeof(HexColorConverter))]
        public virtual Color DisabledColorsKnob { get; set; } = new Color(.3f, .3f, .3f);
        [UseConverter(typeof(HexColorConverter))]
        public virtual Color DisabledColorsBG { get; set; } = Color.black;


        // Songlist
        [UseConverter(typeof(HexColorConverter))]
        public virtual Color SongListSelected { get; set; } = new Color(.52f, .12f, 1f);
        [UseConverter(typeof(HexColorConverter))]
        public virtual Color SongListHighlighted { get; set; } = new Color(1f, .66f, 0f);
        [UseConverter(typeof(HexColorConverter))]
        public virtual Color SongListSelectedAndHighlighted { get; set; } = new Color(.89f, .66f, .31f); // E3A84F


        // Other
        [UseConverter(typeof(HexColorConverter))]
        public virtual Color BannerTop { get; set; } = new Color(.52f, .12f, 1f);
        [UseConverter(typeof(HexColorConverter))]
        public virtual Color BackButtonNormal { get; set; } = new Color(.52f, .12f, 1f);
        [UseConverter(typeof(HexColorConverter))]
        public virtual Color BackButtonHighlight { get; set; } = new Color(.72f, .32f, 1f);


        [UseConverter(typeof(HexColorConverter))]
        public virtual Color SegmentIconNormal { get; set; } = new Color(.7f, .7f, .7f);
        [UseConverter(typeof(HexColorConverter))]
        public virtual Color SegmentIconSelected { get; set; } = new Color(.52f, .12f, 1f);
        [UseConverter(typeof(HexColorConverter))]
        public virtual Color SegmentIconHighlighted { get; set; } = new Color(.72f, .32f, 1f);
        [UseConverter(typeof(HexColorConverter))]
        public virtual Color SegmentIconSelectedAndHighlighted { get; set; } = new Color(.72f, .32f, 1f);

        [UseConverter(typeof(HexColorConverter))]
        public virtual Color SegmentTextNormal { get; set; } = new Color(.7f, .7f, .7f);
        [UseConverter(typeof(HexColorConverter))]
        public virtual Color SegmentTextSelected { get; set; } = new Color(.52f, .12f, 1f);
        [UseConverter(typeof(HexColorConverter))]
        public virtual Color SegmentTextHighlighted { get; set; } = new Color(.72f, .32f, 1f);
        [UseConverter(typeof(HexColorConverter))]
        public virtual Color SegmentTextSelectedAndHighlighted { get; set; } = new Color(.72f, .32f, 1f);

        // Play Button
        public virtual bool PlayButtonEnableGradient { get; set; } = false;
        [UseConverter(typeof(HexColorConverter))]
        public virtual Color PlayButtonBaseNormal { get; set; } = new Color(1f, .66f, 0f);
        [UseConverter(typeof(HexColorConverter))]
        public virtual Color PlayButtonBaseHighlighted { get; set; } = new Color(1f, .66f, 0f);
        [UseConverter(typeof(HexColorConverter))]
        public virtual Color PlayButtonBasePressed { get; set; } = new Color(1f, .66f, 0f);
        [UseConverter(typeof(HexColorConverter))]
        public virtual Color PlayButtonBaseDisabled { get; set; } = new Color(.3f, .3f, .3f);
        // PB G Normal
        [UseConverter(typeof(HexColorConverter))]
        public virtual Color PlayButtonBaseNormalGradientOne { get; set; } = new Color(1f, .66f, 0f);
        [UseConverter(typeof(HexColorConverter))]
        public virtual Color PlayButtonBaseNormalGradientTwo { get; set; } = new Color(1f, .66f, 0f);

        public virtual void Changed()
        {
            /* Force BSIPA to Save */
        }

        public void Save()
        {
            Changed();
        }

        public class Buttons
        {
            public Buttons() { }

            private Buttons(Color col)
            {
                PlayButtonSettings = PlayButton.FromSimpleColor(col);
            }

            public static Buttons FromSimpleColor(Color col)
            {
                return new Buttons(col);
            }

            public bool Enable { get; set; } = true;

            [NonNullable, UseConverter(typeof(ListConverter<CustomPlayButton>))]
            public List<CustomPlayButton> CustomPlayButtons { get; set; } = new List<CustomPlayButton>() { new CustomPlayButton() };

            public PlayButton PlayButtonSettings { get; set; } = new PlayButton();

            public class PlayButton
            {
                public bool Enable { get; set; } = true;

                public PlayButtonState NormalState { get; set; } = new PlayButtonState();
                public PlayButtonState HighlightedState { get; set; } = new PlayButtonState();
                //public PlayButtonState PressedState { get; set; } = new PlayButtonState();
                public PlayButtonState DisabledState { get; set; } = new PlayButtonState();

                public class PlayButtonState
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
                    NormalState = PlayButtonState.FromSimpleColor(col.SaturatedColor(0.8f));
                    HighlightedState = PlayButtonState.FromSimpleColor(col.SaturatedColor(0.9f));
                    //PressedState = PlayButtonState.FromSimpleColor(col);
                    DisabledState = PlayButtonState.FromSimpleColor(Color.black.ColorWithAlpha(0.5f));
                }

                internal static PlayButton FromSimpleColor(Color col)
                {
                    return new PlayButton(col);
                }

            }

            public class CustomPlayButton : PlayButton
            {
                public string TargetGameObjectName { get; set; } = string.Empty;
                public string TargetTextContent { get; set; } = string.Empty;
            }

        }

        public class Toggles
        {
            public bool Enable { get; set; } = true;

            [NonNullable, UseConverter(typeof(ListConverter<CustomToggleSetting>))]
            public List<CustomToggleSetting> CustomToggleSettings { get; set; } = new List<CustomToggleSetting>() { new CustomToggleSetting() };

            [NonNullable]
            public ToggleSetting ToggleSettings = new ToggleSetting();

            public class ToggleSetting
            {
                public virtual ToggleState OnColors { get; set; } = new ToggleState(new Color(.2f, .8f, .2f), null);

                public virtual ToggleState OffColors { get; set; } = new ToggleState(new Color(.8f, .2f, .2f), null);

                public virtual ToggleState OnHighlightedColors { get; set; } = new ToggleState(new Color(.1f, 1f, .1f), null);

                public virtual ToggleState OffHighlightedColors { get; set; } = new ToggleState(new Color(1f, .1f, .1f), null);
            }

            public class CustomToggleSetting : ToggleSetting
            {
                public string TargetGameObjectName { get; set; } = string.Empty;
                public string TargetTextContent { get; set; } = string.Empty;
            }

            public class ToggleState
            {
                public ToggleState()
                {

                }

                public ToggleState(Color? knob, Color? background)
                {
                    KnobColor = knob ?? Color.white;
                    BackgroundColor = background ?? Color.black;
                }

                [UseConverter(typeof(HexColorConverter))]
                public Color KnobColor { get; set; } = Color.white;
                [UseConverter(typeof(HexColorConverter))]
                public Color BackgroundColor { get; set; } = Color.black;
            }
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

        public class SegmentSettings
        {
            [UseConverter(typeof(HexColorConverter))]
            public Color BaseColor { get; set; } = Color.white;
            [UseConverter(typeof(HexColorConverter))]
            public Color BackgroundColor { get; set; } = Color.white;
        }

        /*
         * 
         * {
         *   "buttonType": "ModeSelect",
         *   "name": "SoloButton", OR "name": "*" <-- targets all maybe?
         *   "_<nameOfChildGameObject/an alias of that>": { <--- kinda confusing TODO
         *      "<state(Selected/Highlighted)>": {
         *          "BaseColor": "#colval",
         *          "GradientDirection": "Horizontal/Vertical/None",
         *          "flip": false,
         *          "0": "#colval", <-- top/left
         *          "1": "#colval" <-- bottom/right
         *      }
         *   }
         *   
         * 
         * }
         * 
         */


        // ---
        // ---
        // ---
        internal Color GetColor(string name, float? alpha = null)
        {
            Color col = Color.white;
            
            Color? newCol = (Color) typeof(PluginConfig).GetProperty(name)?.GetValue(this);
            col = newCol ?? col;

            if (alpha.HasValue)
            {
                col.a = alpha.Value;
            }
            else
            {
                if (name.EndsWith("BG")) col.a = .5f;
                else col.a = 1f;
            }

            return col;
        }

        internal void SetColor(string name, Color col)
        {
            typeof(PluginConfig).GetProperty(name)?.SetValue(this, col);
        }
    }
}
