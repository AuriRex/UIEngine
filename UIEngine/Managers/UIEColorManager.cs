using HMUI;
using UIEngine.Configuration;
using UIEngine.HarmonyPatches;
using System;
using UnityEngine;
using UIEngine.Extensions;
using static UIEngine.Configuration.PluginConfig;
using TMPro;

namespace UIEngine.Managers
{
    public class UIEColorManager
    {
        private PluginConfig _pluginConfig;

        public static UIEColorManager instance;
/*
        private Color _simplePrimaryColor;
        public Color SimplePrimaryNormal
        {
            get => _simplePrimaryColor;
            private set
            {
                simplePrimarySelectedAndHighlight = value;
                simplePrimaryHighlight = value.SaturatedColor(0.95f);
                simplePrimarySelected = value.SaturatedColor(0.9f);
                _simplePrimaryColor = value.SaturatedColor(0.8f);
            }
        }*/
        /*public Color simplePrimaryHighlight;
        public Color simplePrimarySelectedAndHighlight;
        public Color simplePrimarySelected;
        public Color simplePrimaryDisabled = new Color(.1f, .1f, .1f, 0.5f);
        public Color simplePrimaryOff = new Color(.3f, .3f, .3f, 0.5f);
        public Color simplePrimaryOffHighlight = new Color(.4f, .4f, .4f);
        public Color simplePrimaryNotSelected = new Color(1f, 1f, 1f);
        public Color simplePrimaryBackground = new Color(0f, 0f, 0f, 0.5f);

        public AnimatedSwitchView.ColorBlock simplePTon;
        public AnimatedSwitchView.ColorBlock simplePToff;
        public AnimatedSwitchView.ColorBlock simplePTonHighlight;
        public AnimatedSwitchView.ColorBlock simplePToffHighlight;
        public AnimatedSwitchView.ColorBlock simplePTdisabled;

        public AnimatedSwitchView.ColorBlock onColors;
        public AnimatedSwitchView.ColorBlock offColors;
        public AnimatedSwitchView.ColorBlock onHighlightedColors;
        public AnimatedSwitchView.ColorBlock offHighlightedColors;
        public AnimatedSwitchView.ColorBlock disabledColors;*/

        /*public Color songListSelected;
        public Color songListHighlighted;
        public Color songListSelectedAndHighlighted;

        public Color bannerTop;
        public Color backButtonNormal;
        public Color backButtonHighlight;

        public Color segmentTextNormal;
        public Color segmentTextSelected;
        public Color segmentTextHighlighted;
        public Color segmentTextSelectedAndHighlighted;

        public Color segmentIconNormal;
        public Color segmentIconSelected;
        public Color segmentIconHighlighted;
        public Color segmentIconSelectedAndHighlighted;*/

        internal UIEColorManager(PluginConfig pluginConfig)
        {
            _pluginConfig = pluginConfig;
            instance = this;
            RefreshColors();
        }

        [Obsolete]
        public bool IsAdvanced()
        {
            return _pluginConfig.Advanced;
        }

        [Obsolete]
        internal void RefreshColors()
        {
           /* if(onColors == null)
            {
                onColors = new AnimatedSwitchView.ColorBlock();
                offColors = new AnimatedSwitchView.ColorBlock();
                onHighlightedColors = new AnimatedSwitchView.ColorBlock();
                offHighlightedColors = new AnimatedSwitchView.ColorBlock();
                disabledColors = new AnimatedSwitchView.ColorBlock();

                simplePTon = new AnimatedSwitchView.ColorBlock();
                simplePToff = new AnimatedSwitchView.ColorBlock();
                simplePTonHighlight = new AnimatedSwitchView.ColorBlock();
                simplePToffHighlight = new AnimatedSwitchView.ColorBlock();
                simplePTdisabled = new AnimatedSwitchView.ColorBlock();
            }*/

            // TODO: Rework the config system LUL

            // Simple primary
            /*SimplePrimaryNormal = _pluginConfig.GetColor(nameof(PluginConfig.SimplePrimaryColor));
            simplePTon.knobColor = SimplePrimaryNormal;
            simplePToff.knobColor = simplePrimaryOff;
            simplePTonHighlight.knobColor = simplePrimaryHighlight;
            simplePToffHighlight.knobColor = simplePrimaryOffHighlight;
            simplePTdisabled.knobColor = simplePrimaryDisabled;

            simplePTon.backgroundColor = simplePrimaryBackground;
            simplePToff.backgroundColor = simplePrimaryBackground;
            simplePTonHighlight.backgroundColor = simplePrimaryBackground;
            simplePToffHighlight.backgroundColor = simplePrimaryBackground;
            simplePTdisabled.backgroundColor = simplePrimaryBackground;*/

            // Toggles
            /*onColors.knobColor = _pluginConfig.GetColor(nameof(PluginConfig.OnColorsKnob));
            onColors.backgroundColor = _pluginConfig.GetColor(nameof(PluginConfig.OnColorsBG));

            offColors.knobColor = _pluginConfig.GetColor(nameof(PluginConfig.OffColorsKnob));
            offColors.backgroundColor = _pluginConfig.GetColor(nameof(PluginConfig.OffColorsBG));

            onHighlightedColors.knobColor = _pluginConfig.GetColor(nameof(PluginConfig.OnHighlightedColorsKnob));
            onHighlightedColors.backgroundColor = _pluginConfig.GetColor(nameof(PluginConfig.OnHighlightedColorsBG));

            offHighlightedColors.knobColor = _pluginConfig.GetColor(nameof(PluginConfig.OffHighlightedColorsKnob));
            offHighlightedColors.backgroundColor = _pluginConfig.GetColor(nameof(PluginConfig.OffHighlightedColorsBG));

            disabledColors.knobColor = _pluginConfig.GetColor(nameof(PluginConfig.DisabledColorsKnob));
            disabledColors.backgroundColor = _pluginConfig.GetColor(nameof(PluginConfig.DisabledColorsBG));

            // Songlist
            songListSelected = _pluginConfig.GetColor(nameof(PluginConfig.SongListSelected), 1f);
            songListHighlighted = _pluginConfig.GetColor(nameof(PluginConfig.SongListHighlighted), 1f);
            songListSelectedAndHighlighted = _pluginConfig.GetColor(nameof(PluginConfig.SongListSelectedAndHighlighted), 1f);

            // Other
            bannerTop = _pluginConfig.GetColor(nameof(PluginConfig.BannerTop), 1f);
            backButtonNormal = _pluginConfig.GetColor(nameof(PluginConfig.BackButtonNormal));
            backButtonHighlight = _pluginConfig.GetColor(nameof(PluginConfig.BackButtonHighlight));

            segmentTextNormal = _pluginConfig.GetColor(nameof(PluginConfig.SegmentTextNormal));
            segmentTextSelected = _pluginConfig.GetColor(nameof(PluginConfig.SegmentTextSelected));
            segmentTextHighlighted = _pluginConfig.GetColor(nameof(PluginConfig.SegmentTextHighlighted));
            segmentTextSelectedAndHighlighted = _pluginConfig.GetColor(nameof(PluginConfig.SegmentTextSelectedAndHighlighted));

            segmentIconNormal = _pluginConfig.GetColor(nameof(PluginConfig.SegmentIconNormal));
            segmentIconSelected = _pluginConfig.GetColor(nameof(PluginConfig.SegmentIconSelected));
            segmentIconHighlighted = _pluginConfig.GetColor(nameof(PluginConfig.SegmentIconHighlighted));
            segmentIconSelectedAndHighlighted = _pluginConfig.GetColor(nameof(PluginConfig.SegmentIconSelectedAndHighlighted));*/
        }

        private static PluginConfig _simpleColorConfig;
        internal static PluginConfig GetSimpleColorConfig(Color? col = null)
        {
            if(_simpleColorConfig == null || col != null)
            {
                _simpleColorConfig = PluginConfig.FromSimpleColor(col ?? instance._pluginConfig.SimplePrimaryColor);
            }

            return _simpleColorConfig;
        }

        internal static Buttons GetSimpleColorButtonSettings()
        {
            return GetSimpleColorConfig().ButtonSettings;
        }

        internal static Toggles GetSimpleColorToggleSettings()
        {
            return GetSimpleColorConfig().ToggleSettings;
        }

        internal static void SetAnimationClipTubeBloomPrePassLightColor(AnimationClip clip, Color col, string relativePath, bool withAlpha = false)
        {
            SetAnimationClipColor<TubeBloomPrePassLight>(clip, col, relativePath, "_color", withAlpha);
        }

        internal static void SetAnimationClipColor<T>(AnimationClip clip, Color col, string relativePath, string attribute = "m_Color", bool withAlpha = false)
        {
            clip.SetCurve(relativePath, typeof(T), $"{attribute}.r", AnimationCurve.Constant(0, 0, col.r));
            clip.SetCurve(relativePath, typeof(T), $"{attribute}.g", AnimationCurve.Constant(0, 0, col.g));
            clip.SetCurve(relativePath, typeof(T), $"{attribute}.b", AnimationCurve.Constant(0, 0, col.b));
            if (withAlpha)
                clip.SetCurve(relativePath, typeof(T), $"{attribute}.a", AnimationCurve.Constant(0, 0, col.a));
        }

       /* internal static void SetAnimationClipSegmentColor(ref AnimationClip clip, SelectableCellStaticAnimationsPatch.SegmentType segmentType, AnimationState animState)
        {
            Color c = instance.IsAdvanced() ? GetColorForSegmentState(segmentType, animState) : SimpleColorForAnimationState(animState);

            switch (segmentType)
            {
                case SelectableCellStaticAnimationsPatch.SegmentType.Text:
                    SetAnimationClipColor<CurvedTextMeshPro>(clip, c, "Text", "m_fontColor");
                    break;
                case SelectableCellStaticAnimationsPatch.SegmentType.Icon:
                    SetAnimationClipColor<ImageView>(clip, c, "Icon", "m_Color");
                    *//*clip.SetCurve("Icon", typeof(ImageView), "m_Color.r", AnimationCurve.Constant(0, 0, c.r));
                    clip.SetCurve("Icon", typeof(ImageView), "m_Color.g", AnimationCurve.Constant(0, 0, c.g));
                    clip.SetCurve("Icon", typeof(ImageView), "m_Color.b", AnimationCurve.Constant(0, 0, c.b));*//*
                    break;
            }
            
        }*/

        public static void SetTextColor(CurvedTextMeshPro tmp, Color? color)
        {
            if (tmp == null) return;
            if (!color.HasValue)
                color = Color.white;
            tmp.color = color.Value;
        }

        public static void SetAnimationTextColor<T>(AnimationClip clip, TextMeshProUGUI TMP, Color col, string relativePath, bool withAlpha = false) where T : TextMeshProUGUI
        {
            clip.SetCurve(relativePath, typeof(T), "m_fontColor.r", AnimationCurve.Constant(0, 0, col.r));
            clip.SetCurve(relativePath, typeof(T), "m_fontColor.g", AnimationCurve.Constant(0, 0, col.g));
            clip.SetCurve(relativePath, typeof(T), "m_fontColor.b", AnimationCurve.Constant(0, 0, col.b));
            if(withAlpha)
                clip.SetCurve(relativePath, typeof(T), "m_fontColor.a", AnimationCurve.Constant(0, 0, col.a));
        }
       
        public static void SetAnimationFromImageViewSettings(AnimationClip clip, ImageView imageView, ImageViewSettings settings, string relativePath = "BG")
        {
            ImageView.GradientDirection? gradientDirection = GetGradientDirectionFromSettings(settings);

            SetAnimationImageViewColors(ref clip,
                relativePath,
                settings.BaseColor,
                gradientDirection.HasValue,
                settings.GradientColor0,
                settings.GradientColor1,
                imageView,
                settings.FlipGradient,
                gradientDirection);
        }

        private static ImageView.GradientDirection? GetGradientDirectionFromSettings(ImageViewSettings settings)
        {
            switch(settings.GradientDirection)
            {
                case ImageViewSettings.GRADIENT_DIRECTION_VERTICAL:
                    return ImageView.GradientDirection.Vertical;
                case ImageViewSettings.GRADIENT_DIRECTION_HORIZONTAL:
                    return ImageView.GradientDirection.Horizontal;
                default:
                case ImageViewSettings.GRADIENT_DIRECTION_NONE:
                    return null;
            }
        }

        public static void SetAnimationImageViewColors(ref AnimationClip clip, string relativePathOfGameObject = "BG", Color? baseColor = null, bool enabledGradient = false, Color? gradient0 = null, Color? gradient1 = null, ImageView imageView = null, bool flipGradientColors = false, ImageView.GradientDirection? gradientDirection = ImageView.GradientDirection.Vertical)
        {
            if (!baseColor.HasValue) baseColor = Color.white;
/*
            if (imageView != null)
            {
                imageView.SetGradientEnabled(enabledGradient);
                imageView.SetFlipGradient(flipGradientColors);
                if (!gradientDirection.HasValue)
                    gradientDirection = ImageView.GradientDirection.Horizontal;
                imageView.SetGradientDirection(gradientDirection.Value);
            }*/

            clip.SetCurve(relativePathOfGameObject, typeof(ImageView), "_gradient", AnimationCurve.Constant(0, 0, enabledGradient ? 1 : 0));
            clip.SetCurve(relativePathOfGameObject, typeof(ImageView), "_flipGradientColors", AnimationCurve.Constant(0, 0, flipGradientColors ? 1 : 0));
            if(gradientDirection.HasValue)
                clip.SetCurve(relativePathOfGameObject, typeof(ImageView), "_gradientDirection", AnimationCurve.Constant(0, 0, (int) gradientDirection.Value));

            if (enabledGradient)
            {
                if(gradient0.HasValue)
                {
                    clip.SetCurve(relativePathOfGameObject, typeof(ImageView), "_color0.r", AnimationCurve.Constant(0, 0, gradient0.Value.r));
                    clip.SetCurve(relativePathOfGameObject, typeof(ImageView), "_color0.g", AnimationCurve.Constant(0, 0, gradient0.Value.g));
                    clip.SetCurve(relativePathOfGameObject, typeof(ImageView), "_color0.b", AnimationCurve.Constant(0, 0, gradient0.Value.b));
                }
                if (gradient1.HasValue)
                {
                    clip.SetCurve(relativePathOfGameObject, typeof(ImageView), "_color1.r", AnimationCurve.Constant(0, 0, gradient1.Value.r));
                    clip.SetCurve(relativePathOfGameObject, typeof(ImageView), "_color1.g", AnimationCurve.Constant(0, 0, gradient1.Value.g));
                    clip.SetCurve(relativePathOfGameObject, typeof(ImageView), "_color1.b", AnimationCurve.Constant(0, 0, gradient1.Value.b));
                }
            }

            clip.SetCurve(relativePathOfGameObject, typeof(ImageView), "m_Color.r", AnimationCurve.Constant(0, 0, baseColor.Value.r));
            clip.SetCurve(relativePathOfGameObject, typeof(ImageView), "m_Color.g", AnimationCurve.Constant(0, 0, baseColor.Value.g));
            clip.SetCurve(relativePathOfGameObject, typeof(ImageView), "m_Color.b", AnimationCurve.Constant(0, 0, baseColor.Value.b));
        }

        /*private static Color SimpleColorForAnimationState(AnimationState animState, bool normalIsColorless = true)
        {
            switch (animState)
            {
                case AnimationState.Normal:
                    if(normalIsColorless)
                        return instance.simplePrimaryNotSelected;
                    return instance.SimplePrimaryNormal;
                case AnimationState.Selected:
                    return instance.simplePrimarySelected;
                case AnimationState.Highlighted:
                    return instance.simplePrimaryHighlight;
                case AnimationState.SelectedAndHighlighted:
                    return instance.simplePrimarySelectedAndHighlight;
            }
            return Color.white;
        }*/

        // I don't like this
       /* [Obsolete("blepp")]
        private static Color GetColorForSegmentState(SelectableCellStaticAnimationsPatch.SegmentType segmentType, AnimationState animState)
        {
            switch(segmentType)
            {
                case SelectableCellStaticAnimationsPatch.SegmentType.Text:
                    switch (animState)
                    {
                        case AnimationState.Normal:
                            return instance.segmentTextNormal;
                        case AnimationState.Selected:
                            return instance.segmentTextSelected;
                        case AnimationState.Highlighted:
                            return instance.segmentTextHighlighted;
                        case AnimationState.SelectedAndHighlighted:
                            return instance.segmentTextSelectedAndHighlighted;
                    }
                    break;
                case SelectableCellStaticAnimationsPatch.SegmentType.Icon:
                    switch (animState)
                    {
                        case AnimationState.Normal:
                            return instance.segmentIconNormal;
                        case AnimationState.Selected:
                            return instance.segmentIconSelected;
                        case AnimationState.Highlighted:
                            return instance.segmentIconHighlighted;
                        case AnimationState.SelectedAndHighlighted:
                            return instance.segmentIconSelectedAndHighlighted;
                    }
                    break;
            }
            return Color.white;
        }*/

        public enum AnimationState
        {
            Normal,
            Selected,
            Highlighted,
            SelectedAndHighlighted
        }

        internal void Dispose()
        {
            //AnimatedSwitchViewPatch.ResetAll();
        }

    }
}
