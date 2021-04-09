using HarmonyLib;
using HMUI;
using UIEngine.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UIEngine.HarmonyPatches
{
    [HarmonyPatch(typeof(AnimatedSwitchView))]
    [HarmonyPatch(nameof(AnimatedSwitchView.Start), MethodType.Normal)]
    internal class AnimatedSwitchViewPatch
    {
		public static HashSet<AnimatedSwitchView> AllAnimatedSwitchViews { get; private set; }

		static void Prefix(ref AnimatedSwitchView __instance,
			ref AnimatedSwitchView.ColorBlock ____onColors,
			ref AnimatedSwitchView.ColorBlock ____offColors,
			ref AnimatedSwitchView.ColorBlock ____onHighlightedColors,
			ref AnimatedSwitchView.ColorBlock ____offHighlightedColors,
			ref AnimatedSwitchView.ColorBlock ____disabledColors)
        {

			UIEColorManager colorManager = UIEColorManager.instance;

			____onColors = colorManager.IsAdvanced() ? colorManager.onColors : colorManager.simplePTon;
			____offColors = colorManager.IsAdvanced() ? colorManager.offColors : colorManager.simplePToff;
			____onHighlightedColors = colorManager.IsAdvanced() ? colorManager.onHighlightedColors : colorManager.simplePTonHighlight;
			____offHighlightedColors = colorManager.IsAdvanced() ? colorManager.offHighlightedColors : colorManager.simplePToffHighlight;
			____disabledColors = colorManager.IsAdvanced() ? colorManager.disabledColors : colorManager.simplePTdisabled;

			/*if (AllAnimatedSwitchViews == null) AllAnimatedSwitchViews = new HashSet<AnimatedSwitchView>();

			if(!AllAnimatedSwitchViews.Contains(__instance))
            {
				AllAnimatedSwitchViews.Add(__instance);
            }*/
		}
    }
}
