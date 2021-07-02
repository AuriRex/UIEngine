using HMUI;
using System.Collections.Generic;
using System.Linq;
using UIEngine.Configuration;
using UIEngine.Extensions;
using UIEngine.Utilities;
using UnityEngine;

namespace UIEngine.Managers
{
    public class UIESegmentManager : UIEAnimationElementManagerBase<SelectableCellStaticAnimations>
    {
        public UIESegmentManager(PluginConfig pluginConfig) : base(pluginConfig)
        {

        }

        public override bool ShouldDecorateElement(SelectableCellStaticAnimations element)
        {
            return pluginConfig.SegmentSettings.Enable;
        }

        public override void DecorateElement(SelectableCellStaticAnimations element)
        {
            PluginConfig.Segments.SegmentSetting settings;

            SegmentType segmentType = GetSegmentType(element);

            CurvedTextMeshPro tmp = element.gameObject.GetComponentOnChild<CurvedTextMeshPro>("Text");

            string textContent = tmp?.text ?? string.Empty;

            bool isDefaultSettings = false;

            if (pluginConfig.Advanced)
            {
                if (!TryGetCustomSettings(element.gameObject, textContent, pluginConfig.SegmentSettings.CustomSegmentSettings, out settings))
                {
                    // Use Default Settings instead
                    settings = pluginConfig.SegmentSettings.SegmentSettings;
                    isDefaultSettings = true;
                }
            }
            else
            {
                // Simple Color thing
                settings = UIEAnimationColorUtils.GetSimpleColorSegmentSettings(pluginConfig).SegmentSettings;
                isDefaultSettings = true;
            }

            if(isDefaultSettings && defaultCustomAnimationClips == null)
            {
                defaultCustomAnimationClips = SettingsToAnimationClipTouple(settings, segmentType);
            }

            var animationClips = CreateOrGetAnimationClipsForSettings(element, settings, isDefaultSettings, segmentType);

            AssignClips(element, animationClips);
        }

        private void AssignClips(SelectableCellStaticAnimations element, (AnimationClip normal, AnimationClip highlighted, AnimationClip selected, AnimationClip selectedAndHighlighted) animationClips)
        {

            Accessors.SelectableCellStaticAnimations_normalAnimationClip(ref element) = animationClips.normal;
            Accessors.SelectableCellStaticAnimations_highlightedAnimationClip(ref element) = animationClips.highlighted;
            Accessors.SelectableCellStaticAnimations_selectedAnimationClip(ref element) = animationClips.selected;
            Accessors.SelectableCellStaticAnimations_selectedAndHighlightedAnimationClip(ref element) = animationClips.selectedAndHighlighted;

        }

        internal (AnimationClip, AnimationClip, AnimationClip, AnimationClip) CreateOrGetAnimationClipsForSettings(SelectableCellStaticAnimations element, PluginConfig.Segments.SegmentSetting settings, bool isDefaultSettings, SegmentType segmentType)
        {
            if(animationClipsForElement.TryGetValue(element, out var cachedClips))
            {
                return cachedClips;
            }

            if (isDefaultSettings)
            {
                var defaultClips = defaultCustomAnimationClips.GetValueOrDefault();
                animationClipsForElement.Add(element, defaultClips);
                return defaultClips;
            }

            var clips = SettingsToAnimationClipTouple(settings, segmentType);
            animationClipsForElement.Add(element, clips);
            return clips;
        }

        private (AnimationClip, AnimationClip, AnimationClip, AnimationClip) SettingsToAnimationClipTouple(PluginConfig.Segments.SegmentSetting settings, SegmentType segmentType)
        {
            return (CreateClipForState(settings.Normal, nameof(settings.Normal), segmentType), CreateClipForState(settings.Highlighted, nameof(settings.Highlighted), segmentType), CreateClipForState(settings.Selected, nameof(settings.Selected), segmentType), CreateClipForState(settings.SelectedAndHighlighted, nameof(settings.SelectedAndHighlighted), segmentType));
        }

        private AnimationClip CreateClipForState(PluginConfig.Segments.SegmentState state, string identifier, SegmentType segmentType)
        {
            AnimationClip clip = CreateNewAnimationClip("UIEngine_Segment_" + identifier);

            SetAnimationClipAnimations(clip, state, segmentType);

            return clip;
        }

        internal static void SetAnimationClipAnimations(AnimationClip clip, PluginConfig.Segments.SegmentState state, SegmentType segmentType)
        {
           /* switch (segmentType)
            {
                case SegmentType.Text:
                    
                    break;
                case SegmentType.Icon:
                    
                    break;
            }*/
            UIEAnimationColorUtils.SetAnimationClipColor<CurvedTextMeshPro>(clip, state.BaseColor, "Text", "m_fontColor");
            UIEAnimationColorUtils.SetAnimationClipColor<ImageView>(clip, state.BaseColor, "Icon", "m_Color");
            UIEAnimationColorUtils.SetAnimationClipColor<ImageView>(clip, new Color(state.BackgroundColor.r, state.BackgroundColor.g, state.BackgroundColor.b, state.BackgroundAlpha), "BG", "m_Color", withAlpha: true);

        }

        public static SegmentType GetSegmentType(SelectableCellStaticAnimations scsa)
		{
			Transform buttonTransform = scsa.gameObject.transform;
			List<string> childNames = new List<string>();
			for (int i = 0; i < buttonTransform.childCount; i++)
			{
				Transform child = buttonTransform.GetChild(i);
				childNames.Add(child.name);
			}

			if (Utilities.Utilities.ListContainsAll(childNames, iconSegmentChildren)) return SegmentType.Icon;
			if (Utilities.Utilities.ListContainsAll(childNames, textSegmentChildren)) return SegmentType.Text;

			return SegmentType.Unknown;
		}

		private static List<string> iconSegmentChildren = new string[] { "BG", "Icon" }.ToList();
		private static List<string> textSegmentChildren = new string[] { "BG", "Text" }.ToList();

		public enum SegmentType
		{
			Icon,
			Text,
			Unknown
		}
	}
}
