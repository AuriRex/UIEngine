using HarmonyLib;
using HMUI;
using UIEngine.Managers;

namespace UIEngine.HarmonyPatches
{
    [HarmonyPatch(typeof(SelectableCellStaticAnimations))]
    [HarmonyPatch(nameof(SelectableCellStaticAnimations.Awake), MethodType.Normal)]
    internal class SelectableCellStaticAnimationsPatch
    {
		static void Postfix(ref SelectableCellStaticAnimations __instance)
        {
			UIESegmentManager.AddElement(__instance);
		}
	}
}