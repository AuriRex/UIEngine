using HMUI;
using SiraUtil.Affinity;
using UIEngine.Configuration;
using UIEngine.Managers;

namespace UIEngine.HarmonyPatches
{
    internal class SelectableCellStaticAnimationsPatch : IAffinity
    {
        private readonly PluginConfig _pluginConfig;
        private readonly UIESegmentManager _segmentManager;

        public SelectableCellStaticAnimationsPatch(PluginConfig pluginConfig, UIESegmentManager segmentManager)
        {
            _pluginConfig = pluginConfig;
            _segmentManager = segmentManager;
        }

        [AffinityPostfix]
        [AffinityPatch(typeof(SelectableCellStaticAnimations), nameof(SelectableCellStaticAnimations.Awake))]
        internal void Postfix(ref SelectableCellStaticAnimations __instance)
        {
            _segmentManager.AddElement(__instance);
		}
	}
}