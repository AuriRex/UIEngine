using HarmonyLib;
using UIEngine.Managers;
using System;
using UnityEngine;

namespace UIEngine.HarmonyPatches
{
    [HarmonyPatch(typeof(LevelListTableCell))]
    [HarmonyPatch(nameof(LevelListTableCell.SetDataFromLevelAsync), MethodType.Normal)]
    internal class LevelListTableCellPatch
    {
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
