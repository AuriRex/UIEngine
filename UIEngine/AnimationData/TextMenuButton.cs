using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UIEngine.AnimationData
{
    public static class TextMenuButton
    {
        // TODO
        public static (string, string, float, Type)[] DefaultTextMenuButtonDisabled { get; private set; } = new (string, string, float, Type)[] {
            ("Underline", "m_Color.a", 0.1254902f, typeof(HMUI.ImageView)),
            ("Wrapper/Content/Text", "m_fontColor.a", 0.2509804f, typeof(TMPro.TextMeshProUGUI)),
            ("BG", "m_IsActive", 0f, typeof(UnityEngine.GameObject))
        };
        public static (string, string, float, Type)[] DefaultTextMenuButtonHighlighted { get; private set; } = new (string, string, float, Type)[] {
            ("BG", "m_IsActive", 1f, typeof(UnityEngine.GameObject)),
            ("BG", "m_Color.a", 1f, typeof(HMUI.ImageView)),
            ("Content/Text", "m_fontColor.a", 1f, typeof(TMPro.TextMeshProUGUI)),
            ("Underline", "m_Color.a", 1f, typeof(HMUI.ImageView))
        };
        public static (string, string, float, Type)[] DefaultTextMenuButtonNormal { get; private set; } = new (string, string, float, Type)[] {
            ("Content/Text", "m_fontColor.a", 1f, typeof(TMPro.TextMeshProUGUI)),
            ("Underline", "m_Color.a", 1f, typeof(HMUI.ImageView)),
            ("BG", "m_IsActive", 1f, typeof(UnityEngine.GameObject)),
            ("BG", "m_Color.a", .8f, typeof(HMUI.ImageView))
        };
        public static (string, string, float, Type)[] DefaultTextMenuButtonPressed { get; private set; } = new (string, string, float, Type)[] { };
    }
}
