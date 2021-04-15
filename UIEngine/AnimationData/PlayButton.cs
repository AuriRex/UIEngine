using System;

namespace UIEngine.AnimationData
{
    public static class PlayButton
    {
        public static (string, string, float, Type)[] DefaultAnimatedTextButtonDisabled { get; private set; } = new (string, string, float, Type)[] {
            ("Border", "m_IsActive", 0f, typeof(UnityEngine.GameObject)),
            ("Content/Text", "m_fontColor.a", 0.2509804f, typeof(TMPro.TextMeshProUGUI)),
            ("BG", "m_IsActive", 0f, typeof(UnityEngine.GameObject)),
            ("BGDisabled", "m_IsActive", 1f, typeof(UnityEngine.GameObject)),
            ("OutlineWrapper/Outline", "m_Enabled", 0f, typeof(HMUI.ImageView))
        };
        public static (string, string, float, Type)[] DefaultAnimatedTextButtonHighlighted { get; private set; } = new (string, string, float, Type)[] {
            ("BG", "_color1.a", 0.5019608f, typeof(HMUI.ImageView)),
            ("BG", "m_IsActive", 1f, typeof(UnityEngine.GameObject)),
            ("Border", "m_IsActive", 1f, typeof(UnityEngine.GameObject)),
            ("Content/Text", "m_fontColor.a", 1f, typeof(TMPro.TextMeshProUGUI)),
            ("BGDisabled", "m_IsActive", 0f, typeof(UnityEngine.GameObject)),
            ("BG", "_color0.a", 1f, typeof(HMUI.ImageView)),
            ("OutlineWrapper/Outline", "m_Enabled", 1f, typeof(HMUI.ImageView))
        };
        public static (string, string, float, Type)[] DefaultAnimatedTextButtonNormal { get; private set; } = new (string, string, float, Type)[] {
            ("BG", "m_IsActive", 1f, typeof(UnityEngine.GameObject)),
            ("Content/Text", "m_fontColor.a", 0.7490196f, typeof(TMPro.TextMeshProUGUI)),
            ("Border", "m_IsActive", 1f, typeof(UnityEngine.GameObject)),
            ("BG", "_color0.a", 1f, typeof(HMUI.ImageView)),
            ("BG", "_color1.a", 0.5019608f, typeof(HMUI.ImageView)),
            ("BGDisabled", "m_IsActive", 0f, typeof(UnityEngine.GameObject)),
            ("OutlineWrapper/Outline", "m_Enabled", 0f, typeof(HMUI.ImageView))
        };
        public static (string, string, float, Type)[] DefaultAnimatedTextButtonPressed { get; private set; } = new (string, string, float, Type)[] {
            ("BG", "m_IsActive", 1f, typeof(UnityEngine.GameObject)),
            ("Content/Text", "m_fontColor.a", 1f, typeof(TMPro.TextMeshProUGUI)),
            ("Border", "m_IsActive", 0f, typeof(UnityEngine.GameObject)),
            ("BGDisabled", "m_IsActive", 0f, typeof(UnityEngine.GameObject))
        };
    }
}
