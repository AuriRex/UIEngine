using HarmonyLib;
using HMUI;
using UIEngine.Managers;

namespace UIEngine.HarmonyPatches
{
    [HarmonyPatch(typeof(ButtonStaticAnimations))]
	[HarmonyPatch(nameof(ButtonStaticAnimations.Awake), MethodType.Normal)]
	internal class ButtonStaticAnimationsPatch
	{
		public const string BSMLBUTTON_CLONE_NAME = "PracticeButton(Clone)";

		static void Prefix(ref ButtonStaticAnimations __instance)
		{
			ButtonStaticAnimations bsa = __instance;

			if(bsa.name.Equals(BSMLBUTTON_CLONE_NAME))
            {
				// Let mod menu buttons initialize first
				__instance.StartCoroutine(Utilities.Utilities.DoAfter(0.1f, () => UIEButtonManager.AddElement(bsa)));
			}
			else
            {
				UIEButtonManager.AddElement(bsa);
			}

		}
	}
}