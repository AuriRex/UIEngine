using BeatSaberMarkupLanguage;
using HMUI;
using UIEngine.Configuration;
using Zenject;

namespace UIEngine.UI
{
    public class UIEFlowCoordinator : FlowCoordinator
    {
        private PluginConfig _pluginConfig;
        private MainFlowCoordinator _mainFlow;

        private UIEInitialSetupViewController _initialSetupViewController;

        private UIEMainViewController _mainViewController;
        private UIEPreviewViewController _previewViewController;

        [Inject]
        public void Construct(PluginConfig pluginConfig, MainFlowCoordinator mainFlow, UIEInitialSetupViewController initialSetupViewController, UIEMainViewController mainViewController, UIEPreviewViewController previewViewController)
        {
            _pluginConfig = pluginConfig;
            _mainFlow = mainFlow;
            _initialSetupViewController = initialSetupViewController;
            _mainViewController = mainViewController;
            _previewViewController = previewViewController;
        }

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            if(firstActivation)
            {
                SetTitle("UIEngine");
                showBackButton = true;

                if (_pluginConfig.ShowIntroduction)
                {
                    ProvideInitialViewControllers(_initialSetupViewController);
                    return;
                }
                ProvideInitialViewControllers(_mainViewController);
            }
        }

        public void PresentAndRefreshPreviewView(bool show)
        {
            if(show)
            {
                SetRightScreenViewController(_previewViewController, ViewController.AnimationType.In);
                _previewViewController.Refresh();
            }
            else DismissViewController(_previewViewController);
        }

        protected override void DidDeactivate(bool removedFromHierarchy, bool screenSystemDisabling)
        {
            base.DidDeactivate(removedFromHierarchy, screenSystemDisabling);
        }

        protected override void BackButtonWasPressed(ViewController topViewController)
        {
            _mainFlow.DismissFlowCoordinator(this, null);
        }
    }
}
