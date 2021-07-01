using HMUI;
using System;
using TMPro;
using UIEngine.Configuration;
using UnityEngine;
using static UIEngine.Configuration.PluginConfig;

namespace UIEngine.Utilities
{
    public static class UIEAnimationColorUtils
    {
        private static PluginConfig _simpleColorConfig;

        internal static PluginConfig GetSimpleColorConfig(Color col, bool forceRegenerate = false)
        {
            if(_simpleColorConfig == null || forceRegenerate)
            {
                _simpleColorConfig = PluginConfig.FromSimpleColor(col);
            }

            return _simpleColorConfig;
        }

        internal static PluginConfig GetSimpleColorConfig(PluginConfig pluginConfig, bool forceRegenerate = false)
        {
            return GetSimpleColorConfig(pluginConfig.SimplePrimaryColor, forceRegenerate);
        }

        internal static Buttons GetSimpleColorButtonSettings(PluginConfig pluginConfig)
        {
            return GetSimpleColorConfig(pluginConfig).ButtonSettings;
        }

        internal static Toggles GetSimpleColorToggleSettings(PluginConfig pluginConfig)
        {
            return GetSimpleColorConfig(pluginConfig).ToggleSettings;
        }

        internal static Segments GetSimpleColorSegmentSettings(PluginConfig pluginConfig)
        {
            return GetSimpleColorConfig(pluginConfig).SegmentSettings;
        }

        internal static void SetAnimationClipTubeBloomPrePassLightColor(AnimationClip clip, Color col, string relativePath, bool withAlpha = false)
        {
            SetAnimationClipColor<TubeBloomPrePassLight>(clip, col, relativePath, "_color", withAlpha);
        }

        public static void SetDefaultAnimationsForClip(AnimationClip clip, (string relativePath, string property, float value, Type type)[] defaultAnimationData)
        {
            foreach ((string relativePath, string property, float value, Type type) animationData in defaultAnimationData)
            {
                if (animationData.type == null) continue;
                clip.SetCurve(animationData.relativePath, animationData.type, animationData.property, AnimationCurve.Constant(0, 0, animationData.value));
            }
        }

        public static void SetAnimationClipColor<T>(AnimationClip clip, Color col, string relativePath, string attribute = "m_Color", bool withAlpha = false)
        {
            clip.SetCurve(relativePath, typeof(T), $"{attribute}.r", AnimationCurve.Constant(0, 0, col.r));
            clip.SetCurve(relativePath, typeof(T), $"{attribute}.g", AnimationCurve.Constant(0, 0, col.g));
            clip.SetCurve(relativePath, typeof(T), $"{attribute}.b", AnimationCurve.Constant(0, 0, col.b));
            if (withAlpha)
                clip.SetCurve(relativePath, typeof(T), $"{attribute}.a", AnimationCurve.Constant(0, 0, col.a));
        }

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

        public static ImageView.GradientDirection? GetGradientDirectionFromSettings(ImageViewSettings settings)
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

    }
}
