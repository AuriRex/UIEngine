using HMUI;
using UnityEngine;
using static UIEngine.Accessors;

namespace UIEngine.Extensions
{
    public static class ImageViewExtensions
    {

        public static int PID_ShineColor = Shader.PropertyToID("_ShineColor");
        public static int PID_RadialColor = Shader.PropertyToID("_RadialColor");

        public static void SetShineColor(this ImageView imageView, Color col)
        {
            imageView.material.SetColor(PID_ShineColor, col);
        }

        public static void SetRadialColor(this ImageView imageView, Color col)
        {
            imageView.material.SetColor(PID_RadialColor, col);
        }

        public static void SetGradient(this ImageView imageView, ImageView.GradientDirection gradientDirection, Color gradient0, Color gradient1, bool flip = false, bool enabled = true)
        {
            imageView.SetGradientEnabled(enabled);
            imageView.SetFlipGradient(flip);
            imageView.SetGradientDirection(gradientDirection);
            imageView.color0 = gradient0;
            imageView.color1 = gradient1;
        }

        public static void SetGradientDirection(this ImageView imageView, ImageView.GradientDirection gradientDirection)
        {
            ImageView_gradientDirectionAccessor(ref imageView) = gradientDirection;
        }

        public static ImageView.GradientDirection GetGradientDirection(this ImageView imageView)
        {
            return ImageView_gradientDirectionAccessor(ref imageView);
        }

        public static void SetFlipGradient(this ImageView imageView, bool flip)
        {
            ImageView_flipGradientColorsAccessor(ref imageView) = flip;
        }

        public static bool GetFlipGradient(this ImageView imageView)
        {
            return ImageView_flipGradientColorsAccessor(ref imageView);
        }

        public static void SetGradientEnabled(this ImageView imageView, bool enabled)
        {
            ImageView_gradientAccessor(ref imageView) = enabled;
        }

        public static bool GetGradientEnabled(this ImageView imageView)
        {
            return ImageView_gradientAccessor(ref imageView);
        }

    }
}
