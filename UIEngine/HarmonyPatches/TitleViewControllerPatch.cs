using HMUI;
using SiraUtil.Affinity;
using UIEngine.Configuration;
using UIEngine.Managers;

namespace UIEngine.HarmonyPatches
{
    internal class TitleViewControllerPatch : IAffinity
    {
        private readonly PluginConfig _pluginConfig;
        private readonly UIETitleBarManager _titleBarManager;

        public TitleViewControllerPatch(PluginConfig pluginConfig, UIETitleBarManager titleBarManager)
        {
            _pluginConfig = pluginConfig;
            _titleBarManager = titleBarManager;
        }

        [AffinityPostfix]
        [AffinityPatch(typeof(TitleViewController), nameof(TitleViewController.SetText))]
        internal void Postfix(ref TitleViewController __instance)
        {
            /*GameObject bg = __instance.gameObject.transform.Find("BG")?.gameObject;

            ImageView iv = bg?.GetComponent<ImageView>();*/

            _titleBarManager.AddElement(__instance);
        }

    }
}
