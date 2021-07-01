using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UIEngine.Extensions;
using UnityEngine;

namespace UIEngine.Utilities
{
    public class Utilities
    {

        public static IEnumerator DoAfter(float time, Action action)
        {
            float start = Time.fixedTime;
            while (start + time > Time.fixedTime)
                yield return null;
            action?.Invoke();
            yield break;
        }

        public static IEnumerator DoAfterFrames(int frames, Action action)
        {
            while (frames > 0)
            {
                frames--;
                yield return null;
            }
            action?.Invoke();
            yield break;
        }

        public static IEnumerator OnNextFrame(Action action)
        {
            yield return null;
            action?.Invoke();
            yield break;
        }

        public static bool ListContainsAll(List<string> list, List<string> targets)
        {
            foreach (string target in targets)
            {
                if (!list.Contains(target))
                {
                    return false;
                }
            }
            return true;
        }

        public static List<string> GetChildrenAsList(Transform trans)
        {
            List<string> allChildren = new List<string>();
            for (int i = 0; i < trans.childCount; i++)
            {
                allChildren.Add(trans.GetChild(i).name);
            }
            return allChildren;
        }

        private static Type mbvc_type;
        private static MonoBehaviour mbvc;
        public static Assembly GetButtonAssembly(HMUI.ButtonStaticAnimations bsa, string buttonText)
        {
            if(mbvc_type == null)
                mbvc_type = typeof(BeatSaberMarkupLanguage.MenuButtons.MenuButton).Assembly.GetType("BeatSaberMarkupLanguage.MenuButtons.MenuButtonsViewController");

            mbvc = bsa.gameObject.GetNthParent(4).GetComponent(mbvc_type) as MonoBehaviour;

            if (mbvc == null) return null;

            var buttons = mbvc_type.GetField("buttons", BindingFlags.Public | BindingFlags.Instance).GetValue(mbvc) as List<object>;

            foreach(BeatSaberMarkupLanguage.MenuButtons.MenuButton btn in buttons)
            {
                if (btn.Text.Equals(buttonText))
                    return btn.OnClick.Method.DeclaringType.Assembly;
            }

            return null;
        }

    }
}
