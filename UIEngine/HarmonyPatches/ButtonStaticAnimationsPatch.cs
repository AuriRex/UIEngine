using HarmonyLib;
using HMUI;
using UIEngine.Managers;
using UnityEngine;

namespace UIEngine.HarmonyPatches
{
	[HarmonyPatch(typeof(ButtonStaticAnimations))]
	[HarmonyPatch(nameof(ButtonStaticAnimations.Awake), MethodType.Normal)]
	internal class ButtonStaticAnimationsPatch
	{

		public const string BSMLBUTTON_CLONE_NAME = "PracticeButton(Clone)";

		static void Prefix(ref ButtonStaticAnimations __instance,
			ref NoTransitionsButton ____button//,
/*			ref AnimationClip ____normalClip,
			ref AnimationClip ____highlightedClip,
			ref AnimationClip ____pressedClip,
			ref AnimationClip ____disabledClip*/)
		{

			ButtonStaticAnimations bsa = __instance;

			if(bsa.name.Equals(BSMLBUTTON_CLONE_NAME))
            {
				__instance.StartCoroutine(Utilities.Utilities.DoAfter(0.1f, () => UIEElementManager.AddButton(bsa)));
			}
			else
            {
				UIEElementManager.AddButton(bsa);
			}

			

			/*ButtonType bType = GetButtonType(__instance);



			UIEColorManager cm = UIEColorManager.instance;

			ImageView bg_iv = __instance.gameObject.transform.Find("BG")?.GetComponent<ImageView>();

			switch (bType)
            {
				case ButtonType.Play:
					UIEColorManager.SetImageViewColors(ref ____normalClip, "BG", cm.IsAdvanced() ? cm.Config.PlayButtonBaseNormal : cm.SimplePrimaryNormal, cm.Config.PlayButtonEnableGradient, cm.Config.PlayButtonBaseNormalGradientOne, cm.Config.PlayButtonBaseNormalGradientTwo, bg_iv);
					UIEColorManager.SetImageViewColors(ref ____highlightedClip, "BG", cm.IsAdvanced() ? cm.Config.PlayButtonBaseHighlighted : cm.simplePrimaryHighlight, cm.Config.PlayButtonEnableGradient, cm.Config.PlayButtonBaseNormalGradientOne, cm.Config.PlayButtonBaseNormalGradientTwo, bg_iv);
					UIEColorManager.SetImageViewColors(ref ____pressedClip, "BG", cm.IsAdvanced() ? cm.Config.PlayButtonBasePressed : cm.simplePrimarySelected, cm.Config.PlayButtonEnableGradient, cm.Config.PlayButtonBaseNormalGradientOne, cm.Config.PlayButtonBaseNormalGradientTwo, bg_iv);
					UIEColorManager.SetImageViewColors(ref ____disabledClip, "BG", cm.IsAdvanced() ? cm.Config.PlayButtonBaseDisabled : cm.simplePrimaryDisabled, cm.Config.PlayButtonEnableGradient, cm.Config.PlayButtonBaseNormalGradientOne, cm.Config.PlayButtonBaseNormalGradientTwo, bg_iv);
					break;
				case ButtonType.Underlined:

					break;

				case ButtonType.BackButton:
					UIEColorManager.SetImageViewColors(ref ____normalClip, "BG", cm.IsAdvanced() ? cm.backButtonNormal : cm.SimplePrimaryNormal);
					UIEColorManager.SetImageViewColors(ref ____highlightedClip, "BG", cm.IsAdvanced() ? cm.backButtonHighlight : cm.simplePrimaryHighlight);
					break;
            }*/

			

		}
	}
}
