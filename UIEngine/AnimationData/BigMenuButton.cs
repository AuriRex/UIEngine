using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIEngine.AnimationData
{
    public static class BigMenuButton
    {
        public static (string, string, float, Type)[] DefaultBigMenuButtonDisabled { get; private set; } = new (string, string, float, Type)[] { };
        public static (string, string, float, Type)[] DefaultBigMenuButtonHighlighted { get; private set; } = new (string, string, float, Type)[] {
            ("Text", "m_fontColor.a", 1f, typeof(TMPro.TextMeshProUGUI)),
            ("Image/Glow", "m_IsActive", 1f, typeof(UnityEngine.GameObject)),
            ("Image/ImageOverlay", "m_Color.a", 1f, typeof(HMUI.ImageView)),
            ("Image/ImageOverlay", "_color0.a", 1f, typeof(HMUI.ImageView)),
            ("Image/ImageOverlay", "_color1.a", 1f, typeof(HMUI.ImageView))
        };
        public static (string, string, float, Type)[] DefaultBigMenuButtonNormal { get; private set; } = new (string, string, float, Type)[] {
            ("Text", "m_fontColor.a", 1f, typeof(TMPro.TextMeshProUGUI)),
            ("Image/Glow", "m_IsActive", 0f, typeof(UnityEngine.GameObject)),
            ("Image/ImageOverlay", "m_Color.a", 0.5f, typeof(HMUI.ImageView)),
            ("Image/ImageOverlay", "_color0.a", 0.5f, typeof(HMUI.ImageView)),
            ("Image/ImageOverlay", "_color1.a", 0.5f, typeof(HMUI.ImageView))
        };
        public static (string, string, float, Type)[] DefaultBigMenuButtonPressed { get; private set; } = new (string, string, float, Type)[] {
            ("Text", "m_fontColor.a", 0.7490196f, typeof(TMPro.TextMeshProUGUI)),
            ("Image/Glow", "m_IsActive", 1f, typeof(UnityEngine.GameObject)),
            ("Image/ImageOverlay", "m_Color.a", 1f, typeof(HMUI.ImageView)),
            ("Image/ImageOverlay", "_color0.a", 1f, typeof(HMUI.ImageView)),
            ("Image/ImageOverlay", "_color1.a", 1f, typeof(HMUI.ImageView))
        };
    }
}
