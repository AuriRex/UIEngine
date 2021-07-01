using HMUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIEngine.Configuration;
using static UIEngine.Extensions.GameObjectExtensions;

namespace UIEngine.Managers
{
    public class UIEToggleManager : UIEElementManagerBase<AnimatedSwitchView>
    {

        private (AnimatedSwitchView.ColorBlock, AnimatedSwitchView.ColorBlock, AnimatedSwitchView.ColorBlock, AnimatedSwitchView.ColorBlock, AnimatedSwitchView.ColorBlock)? _customDefaultColorBlocks = null;

        private Dictionary<AnimatedSwitchView, (AnimatedSwitchView.ColorBlock, AnimatedSwitchView.ColorBlock, AnimatedSwitchView.ColorBlock, AnimatedSwitchView.ColorBlock, AnimatedSwitchView.ColorBlock)> _colorBlocks;

        public UIEToggleManager(PluginConfig pluginConfig) : base(pluginConfig)
        {
            _colorBlocks = new Dictionary<AnimatedSwitchView, (AnimatedSwitchView.ColorBlock, AnimatedSwitchView.ColorBlock, AnimatedSwitchView.ColorBlock, AnimatedSwitchView.ColorBlock, AnimatedSwitchView.ColorBlock)>();
        }

        public override void DecorateElement(AnimatedSwitchView element)
        {
            if (!pluginConfig.ToggleSettings.Enable) return;

            PluginConfig.Toggles.ToggleSetting settings;

            CurvedTextMeshPro tmp = element.transform.parent?.gameObject.GetComponentOnChild<CurvedTextMeshPro>("NameText");

            string textContent = tmp?.text ?? string.Empty;

            bool isDefaultSettings = false;

            if (pluginConfig.Advanced)
            {
                if (!TryGetCustomSettings(element.gameObject, textContent, pluginConfig.ToggleSettings.CustomToggleSettings, out settings))
                {
                    // Use Default Settings instead
                    settings = pluginConfig.ToggleSettings.ToggleSettings;
                    isDefaultSettings = true;
                }
            }
            else
            {
                // Simple Color thing
                settings = UIEColorManager.GetSimpleColorToggleSettings().ToggleSettings;
                isDefaultSettings = true;
            }

            
            if(isDefaultSettings && _customDefaultColorBlocks == null)
            {

                _customDefaultColorBlocks = SettingsToColorBlockTouple(settings);

            }

            var colorBlocks = CreateOrGetColorBlocksForSettings(element, settings, isDefaultSettings);

            AssignColorBlocks(element, colorBlocks);
        }

        private void AssignColorBlocks(AnimatedSwitchView element, (AnimatedSwitchView.ColorBlock on, AnimatedSwitchView.ColorBlock off, AnimatedSwitchView.ColorBlock onHighlight, AnimatedSwitchView.ColorBlock offHighlight, AnimatedSwitchView.ColorBlock disabled) colorBlocks)
        {

            Accessors.AnimatedSwitchView_onColors(ref element) = colorBlocks.on;
            Accessors.AnimatedSwitchView_offColors(ref element) = colorBlocks.off;
            Accessors.AnimatedSwitchView_onHighlightedColors(ref element) = colorBlocks.onHighlight;
            Accessors.AnimatedSwitchView_offHighlightedColors(ref element) = colorBlocks.offHighlight;
            Accessors.AnimatedSwitchView_disabledColors(ref element) = colorBlocks.disabled;

        }

        internal (AnimatedSwitchView.ColorBlock, AnimatedSwitchView.ColorBlock, AnimatedSwitchView.ColorBlock, AnimatedSwitchView.ColorBlock, AnimatedSwitchView.ColorBlock) CreateOrGetColorBlocksForSettings(AnimatedSwitchView element, PluginConfig.Toggles.ToggleSetting settings, bool isDefaultSettings)
        {
            if(_colorBlocks.TryGetValue(element, out var cachedBlocks))
            {
                return cachedBlocks;
            }

            if(isDefaultSettings)
            {
                var defaultBlocks = _customDefaultColorBlocks.GetValueOrDefault();
                _colorBlocks.Add(element, defaultBlocks);
                return defaultBlocks;
            }

            var colorBlocks = SettingsToColorBlockTouple(settings);
            _colorBlocks.Add(element, colorBlocks);
            return colorBlocks;
        }

        private (AnimatedSwitchView.ColorBlock, AnimatedSwitchView.ColorBlock, AnimatedSwitchView.ColorBlock, AnimatedSwitchView.ColorBlock, AnimatedSwitchView.ColorBlock) SettingsToColorBlockTouple(PluginConfig.Toggles.ToggleSetting settings)
        {
            return (StateToColorBlock(settings.OnColors), StateToColorBlock(settings.OffColors), StateToColorBlock(settings.OnHighlightedColors), StateToColorBlock(settings.OffHighlightedColors), StateToColorBlock(settings.DisabledColors));
        }

        private AnimatedSwitchView.ColorBlock StateToColorBlock(PluginConfig.Toggles.ToggleState state)
        {
            var colorBlock = new AnimatedSwitchView.ColorBlock();

            colorBlock.knobColor = state.KnobColor;
            colorBlock.backgroundColor = new UnityEngine.Color(state.BackgroundColor.r, state.BackgroundColor.g, state.BackgroundColor.b, state.BackgroundAlpha);


            return colorBlock;
        }
    }
}
