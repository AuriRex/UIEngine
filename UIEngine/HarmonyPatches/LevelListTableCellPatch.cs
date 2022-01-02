using SiraUtil.Affinity;
using UIEngine.Configuration;
using UnityEngine;

namespace UIEngine.HarmonyPatches
{
    internal class LevelListTableCellPatch : IAffinity
    {
        private readonly PluginConfig _pluginConfig;

        public LevelListTableCellPatch(PluginConfig pluginConfig)
        {
            _pluginConfig = pluginConfig;
        }

        [AffinityPostfix]
        [AffinityPatch(typeof(LevelListTableCell), nameof(LevelListTableCell.SetDataFromLevelAsync))]
        static void Postfix(ref LevelListTableCell __instance,
            ref Color ____highlightBackgroundColor,
            ref Color ____selectedBackgroundColor,
            ref Color ____selectedAndHighlightedBackgroundColor)
        {

            // TODO: reimplement
           /* UIEColorManager colorManager = UIEColorManager.instance;

            ____highlightBackgroundColor = colorManager.IsAdvanced() ? colorManager.songListHighlighted : colorManager.simplePrimaryHighlight;
            ____selectedBackgroundColor = colorManager.IsAdvanced() ? colorManager.songListSelected : colorManager.simplePrimarySelected;
            ____selectedAndHighlightedBackgroundColor = colorManager.IsAdvanced() ? colorManager.songListSelectedAndHighlighted : colorManager.simplePrimarySelectedAndHighlight;*/
        }

    }
}
