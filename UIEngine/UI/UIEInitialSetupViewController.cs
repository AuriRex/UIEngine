using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Parser;
using BeatSaberMarkupLanguage.ViewControllers;
using HMUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIEngine.Configuration;
using UIEngine.Utilities;
using UnityEngine;
using Zenject;

namespace UIEngine.UI
{
    [ViewDefinition("UIEngine.UI.Views.setupView.bsml")]
    [HotReload(RelativePathToLayout = @"Views\setupView.bsml")]
    public class UIEInitialSetupViewController : BSMLAutomaticViewController
    {
        private PluginConfig _pluginConfig;
        private UIEFlowCoordinator _ourFlowCoordinator;
        private MenuTransitionsHelper _menuTransitionsHelper;

        [Inject]
        public void Construct(PluginConfig pluginConfig, UIEFlowCoordinator ourFlowCoordinator,MenuTransitionsHelper menuTransitionsHelper)
        {
            _pluginConfig = pluginConfig;
            _ourFlowCoordinator = ourFlowCoordinator;
            _menuTransitionsHelper = menuTransitionsHelper;
        }

        [UIParams]
        protected BSMLParserParams parserParams = null!;

        private string _continueText = kShowPreview;
        [UIValue("continue-text")]
        protected string continueText
        {
            get => _continueText;
            set
            {
                _continueText = value;
                NotifyPropertyChanged(nameof(continueText));
            }
        }

        public const string kContinue = "Apply";
        public const string kShowPreview = "Show Preview";

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {
            // blep


            base.DidActivate(firstActivation, addedToHierarchy, screenSystemEnabling);
        }


        private bool _justSetFlag = false;
        private Color _simpleColor = Color.blue;
        [UIValue("simple-color")]
        protected Color simpleColor
        {
            get => _simpleColor;
            set
            {
                _simpleColor = value;
                NotifyPropertyChanged(nameof(simpleColor));
                _justSetFlag = true;
                continueText = kShowPreview;
            }
        }

        [UIAction("on-continue-clicked")]
        public void OnContinueClicked()
        {
            if(_justSetFlag)
            {
                Logger.log.Debug($"Selected Color: '{ColorUtility.ToHtmlStringRGB(_simpleColor)}'");
                PluginConfig config = UIEAnimationColorUtils.GetSimpleColorConfig(_simpleColor, true);
                config.Enabled = true;
                // config.ShowIntroduction = false;
                _pluginConfig.CopyFrom(config);
                _pluginConfig.Changed();
                _ourFlowCoordinator.PresentAndRefreshPreviewView(true);
                continueText = kContinue;
                _justSetFlag = false;
            }
            else
            {
                _menuTransitionsHelper.RestartGame();
            }
        }

        [UIAction("#post-parse")]
        public void PostParse()
        {
            _simpleColor = _pluginConfig.SimplePrimaryColor;
        }

    }
}
