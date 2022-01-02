using HMUI;
using SiraUtil.Affinity;
using UIEngine.Configuration;
using UIEngine.Managers;

namespace UIEngine.HarmonyPatches
{
    internal class ButtonStaticAnimationsPatch : IAffinity
	{
		public const string BSMLBUTTON_CLONE_NAME = "PracticeButton(Clone)";

		private readonly PluginConfig _pluginConfig;
		private readonly UIEButtonManager _buttonManager;

		public ButtonStaticAnimationsPatch(PluginConfig pluginConfig, UIEButtonManager buttonManager)
        {
			_pluginConfig = pluginConfig;
			_buttonManager = buttonManager;
		}

		[AffinityPrefix]
		[AffinityPatch(typeof(ButtonStaticAnimations), nameof(ButtonStaticAnimations.Awake))]
		internal void Prefix(ref ButtonStaticAnimations __instance)
		{
			ButtonStaticAnimations bsa = __instance;

			if(bsa.name.Equals(BSMLBUTTON_CLONE_NAME))
            {
				// Let mod menu buttons initialize first
				__instance.StartCoroutine(Utilities.Utilities.DoAfter(0.1f, () => _buttonManager.AddElement(bsa)));
			}
			else
            {
				_buttonManager.AddElement(bsa);
			}

		}
	}
}