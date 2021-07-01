using HarmonyLib;
using HMUI;
using UIEngine.Managers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UIEngine.HarmonyPatches
{
    [HarmonyPatch(typeof(SelectableCellStaticAnimations))]
    [HarmonyPatch(nameof(SelectableCellStaticAnimations.Awake), MethodType.Normal)]
    internal class SelectableCellStaticAnimationsPatch
    {

		static void Postfix(ref SelectableCellStaticAnimations __instance,
            ref AnimationClip ____normalAnimationClip,
            ref AnimationClip ____highlightedAnimationClip,
            ref AnimationClip ____selectedAnimationClip,
			ref AnimationClip ____selectedAndHighlightedAnimationClip)
        {

			UIESegmentManager.AddElement(__instance);
			//SegmentType sType = GetSegmentType(__instance);

			// TODO refactor
			/*UIEColorManager.SetAnimationClipSegmentColor(ref ____normalAnimationClip, sType, UIEColorManager.AnimationState.Normal);
			UIEColorManager.SetAnimationClipSegmentColor(ref ____selectedAnimationClip, sType, UIEColorManager.AnimationState.Selected);
			UIEColorManager.SetAnimationClipSegmentColor(ref ____highlightedAnimationClip, sType, UIEColorManager.AnimationState.Highlighted);
			UIEColorManager.SetAnimationClipSegmentColor(ref ____selectedAndHighlightedAnimationClip, sType, UIEColorManager.AnimationState.SelectedAndHighlighted);*/
		}

		/*private static SegmentType GetSegmentType(SelectableCellStaticAnimations bsa)
		{
			Transform buttonTransform = bsa.gameObject.transform;
			List<string> childNames = new List<string>();
			for (int i = 0; i < buttonTransform.childCount; i++)
			{
				Transform child = buttonTransform.GetChild(i);
				childNames.Add(child.name);
			}

			if (ContainsAll(childNames, iconSegmentChildren)) return SegmentType.Icon;
			if (ContainsAll(childNames, textSegmentChildren)) return SegmentType.Text;

			return SegmentType.Unknown;
		}

		*//*private static bool ContainsAll(List<string> list, List<string> targets)
		{
			foreach (string target in targets)
			{
				if (!list.Contains(target))
				{
					return false;
				}
			}
			return true;
		}*//*

		private static List<string> iconSegmentChildren = new string[] { "BG", "Icon" }.ToList();
		private static List<string> textSegmentChildren = new string[] { "BG", "Text" }.ToList();

		public enum SegmentType
		{
			Icon,
			Text,
			Unknown
		}*/
	}

	/*[SerializeField]
	protected SelectableCell _selectableCell;

	// Token: 0x04000295 RID: 661
	[Space]
	[SerializeField]
	protected AnimationClip _normalAnimationClip;

	// Token: 0x04000296 RID: 662
	[SerializeField]
	protected AnimationClip _highlightedAnimationClip;

	// Token: 0x04000297 RID: 663
	[SerializeField]
	protected AnimationClip _selectedAnimationClip;

	// Token: 0x04000298 RID: 664
	[SerializeField]
	protected AnimationClip _selectedAndHighlightedAnimationClip;*/
}
