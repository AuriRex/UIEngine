using HMUI;
using System;
using System.Collections.Generic;
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

        public static bool IsLightColor(Color col)
        {
            int r = (int) (col.r * 255f);
            int g = (int) (col.g * 255f);
            int b = (int) (col.b * 255f);

            int brightness = ((r * 299) + (g * 587) + (b * 114)) / 1000;

            return brightness > 125;
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
       
        public static void SetAnimationFromImageViewSettings(AnimationClip clip, ImageView imageView, ImageViewSettings settings, string relativePath = "BG", bool setAlphaValues = false)
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
                gradientDirection,
                setAlphaValues);
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

        public static void SetAnimationImageViewColors(ref AnimationClip clip, string relativePathOfGameObject = "BG", Color? baseColor = null, bool enabledGradient = false, Color? gradient0 = null, Color? gradient1 = null, ImageView imageView = null, bool flipGradientColors = false, ImageView.GradientDirection? gradientDirection = ImageView.GradientDirection.Vertical, bool setAlphaValues = false)
        {
            if (!baseColor.HasValue) baseColor = Color.white;

            clip.SetCurve(relativePathOfGameObject, typeof(ImageView), "_gradient", AnimationCurve.Constant(0, 0, enabledGradient ? 1 : 0));
            clip.SetCurve(relativePathOfGameObject, typeof(ImageView), "_flipGradientColors", AnimationCurve.Constant(0, 0, flipGradientColors ? 1 : 0));

            if(gradientDirection.HasValue)
                clip.SetCurve(relativePathOfGameObject, typeof(ImageView), "_gradientDirection", AnimationCurve.Constant(0, 0, (int) gradientDirection.Value));

            if (gradient0.HasValue)
            {
                clip.SetCurve(relativePathOfGameObject, typeof(ImageView), "_color0.r", AnimationCurve.Constant(0, 0, gradient0.Value.r));
                clip.SetCurve(relativePathOfGameObject, typeof(ImageView), "_color0.g", AnimationCurve.Constant(0, 0, gradient0.Value.g));
                clip.SetCurve(relativePathOfGameObject, typeof(ImageView), "_color0.b", AnimationCurve.Constant(0, 0, gradient0.Value.b));
                if(setAlphaValues)
                    clip.SetCurve(relativePathOfGameObject, typeof(ImageView), "_color0.a", AnimationCurve.Constant(0, 0, gradient0.Value.a));
            }

            if (gradient1.HasValue)
            {
                clip.SetCurve(relativePathOfGameObject, typeof(ImageView), "_color1.r", AnimationCurve.Constant(0, 0, gradient1.Value.r));
                clip.SetCurve(relativePathOfGameObject, typeof(ImageView), "_color1.g", AnimationCurve.Constant(0, 0, gradient1.Value.g));
                clip.SetCurve(relativePathOfGameObject, typeof(ImageView), "_color1.b", AnimationCurve.Constant(0, 0, gradient1.Value.b));
                if (setAlphaValues)
                    clip.SetCurve(relativePathOfGameObject, typeof(ImageView), "_color1.a", AnimationCurve.Constant(0, 0, gradient1.Value.a));
            }

            clip.SetCurve(relativePathOfGameObject, typeof(ImageView), "m_Color.r", AnimationCurve.Constant(0, 0, baseColor.Value.r));
            clip.SetCurve(relativePathOfGameObject, typeof(ImageView), "m_Color.g", AnimationCurve.Constant(0, 0, baseColor.Value.g));
            clip.SetCurve(relativePathOfGameObject, typeof(ImageView), "m_Color.b", AnimationCurve.Constant(0, 0, baseColor.Value.b));
            if (setAlphaValues)
                clip.SetCurve(relativePathOfGameObject, typeof(ImageView), "m_Color.a", AnimationCurve.Constant(0, 0, baseColor.Value.a));
        }

        private static Dictionary<Texture2D, Texture2D> _readableTexturesCache = new Dictionary<Texture2D, Texture2D>();
        private static Dictionary<Texture2D, Texture2D> _monochromeTexturesCache = new Dictionary<Texture2D, Texture2D>();
        public static Sprite MonochromifySprite(Sprite original)
        {
            Texture2D modifyableTexture;

            if(!_readableTexturesCache.TryGetValue(original.texture, out modifyableTexture))
            {
                RenderTexture out_renderTexture = new RenderTexture(original.texture.width, original.texture.height, 0);
                out_renderTexture.enableRandomWrite = true;
                RenderTexture previousActiveRT = RenderTexture.active;
                RenderTexture.active = out_renderTexture;
                // Copy your texture ref to the render texture
                Graphics.Blit(original.texture, out_renderTexture);

                Texture2D newTexture = new Texture2D(original.texture.width, original.texture.height, TextureFormat.RGBA32, false);
                newTexture.ReadPixels(new Rect(0, 0, original.texture.width, original.texture.height), 0, 0, false);

                newTexture.Apply();

                modifyableTexture = newTexture;
                _readableTexturesCache.Add(original.texture, newTexture);
                RenderTexture.active = previousActiveRT;
            }



            Texture2D monochromeTexture;

            if(!_monochromeTexturesCache.TryGetValue(modifyableTexture, out monochromeTexture))
            {

                monochromeTexture = new Texture2D(modifyableTexture.width, modifyableTexture.height, modifyableTexture.format, mipChain: false);
                var pixels = modifyableTexture.GetPixels();

                for (int i = 0; i < pixels.Length; i++)
                {
                    Color oldColor = pixels[i];
                    float gsv = oldColor.grayscale;
                    float alpha = oldColor.a;
                    pixels[i] = new Color(gsv, gsv, gsv, alpha);
                }

                monochromeTexture.SetPixels(pixels);
                monochromeTexture.Apply();

                _monochromeTexturesCache.Add(modifyableTexture, monochromeTexture);
            }

            return Sprite.Create(monochromeTexture, original.rect, original.pivot, original.pixelsPerUnit, 0, SpriteMeshType.Tight, original.border);
        }
    }
}
