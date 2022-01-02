using HMUI;
using SiraUtil.Affinity;
using UIEngine.Configuration;
using UIEngine.Managers;

namespace UIEngine.HarmonyPatches
{
    internal class AnimatedSwitchViewPatch : IAffinity
    {
        private readonly PluginConfig _pluginConfig;
        private readonly UIEToggleManager _toggleManager;

        public AnimatedSwitchViewPatch(PluginConfig pluginConfig, UIEToggleManager toggleManager)
        {
            _pluginConfig = pluginConfig;
            _toggleManager = toggleManager;
        }

        [AffinityPrefix]
        [AffinityPatch(typeof(AnimatedSwitchView), nameof(AnimatedSwitchView.Start))]
        internal void Prefix(ref AnimatedSwitchView __instance)
        {
            _toggleManager.AddElement(__instance);
		}
    }
}