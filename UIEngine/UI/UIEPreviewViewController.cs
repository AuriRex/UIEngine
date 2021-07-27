using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Parser;
using BeatSaberMarkupLanguage.ViewControllers;
using System;
using UIEngine.Configuration;
using UnityEngine;
using Zenject;

namespace UIEngine.UI
{
    [ViewDefinition("UIEngine.UI.Views.previewView.bsml")]
    [HotReload(RelativePathToLayout = @"Views\previewView.bsml")]
    public class UIEPreviewViewController : BSMLAutomaticViewController
    {
        private PluginConfig _pluginConfig;

        [Inject]
        public void Construct(PluginConfig pluginConfig)
        {
            _pluginConfig = pluginConfig;
        }

        private GameObject _previewElements = null;

        [UIParams]
        protected BSMLParserParams parserParams = null!;

        [UIObject("container")]
        protected GameObject container;

        [UIObject("preview-toggle")]
        protected GameObject previewToggle;

        [UIObject("preview-action")]
        protected GameObject previewAction;

        [UIObject("preview-button")]
        protected GameObject previewButton;

        protected override void DidActivate(bool firstActivation, bool addedToHierarchy, bool screenSystemEnabling)
        {

            base.DidActivate(firstActivation, addedToHierarchy, screenSystemEnabling);
        }

        [UIAction("#post-parse")]
        public void PostParse()
        {
            container.SetActive(false);
        }

        internal void Refresh()
        {
            if (container == null) return;
            if(_previewElements != null)
            {
                Destroy(_previewElements);
            }
            _previewElements = GameObject.Instantiate(container);
            _previewElements.transform.SetParent(container.transform.parent, false);
            _previewElements.transform.localPosition = Vector3.zero;
            _previewElements.transform.localScale = Vector3.one;
            _previewElements.SetActive(true);
        }
    }
}
