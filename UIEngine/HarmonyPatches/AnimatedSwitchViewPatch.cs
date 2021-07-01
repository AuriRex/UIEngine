using HarmonyLib;
using HMUI;
using UIEngine.Managers;

namespace UIEngine.HarmonyPatches
{
    [HarmonyPatch(typeof(AnimatedSwitchView))]
    [HarmonyPatch(nameof(AnimatedSwitchView.Start), MethodType.Normal)]
    internal class AnimatedSwitchViewPatch
    {
		static void Prefix(ref AnimatedSwitchView __instance)
        {
			UIEToggleManager.AddElement(__instance);
		}
    }
}