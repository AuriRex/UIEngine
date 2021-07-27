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
using Zenject;

namespace UIEngine.UI
{
    [ViewDefinition("UIEngine.UI.Views.mainView.bsml")]
    [HotReload(RelativePathToLayout = @"Views\mainView.bsml")]
    public class UIEMainViewController : BSMLAutomaticViewController
    {
        private PluginConfig _pluginConfig;


        [Inject]
        public void Construct(PluginConfig pluginConfig)
        {
            _pluginConfig = pluginConfig;
        }

        [UIParams]
        protected BSMLParserParams parserParams = null!;

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {

            base.DidActivate(firstActivation, addedToHierarchy, screenSystemEnabling);
        }

        [UIAction("#post-parse")]
        public void PostParse()
        {

        }

    }
}
