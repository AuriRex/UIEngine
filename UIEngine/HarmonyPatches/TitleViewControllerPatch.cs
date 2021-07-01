using HarmonyLib;
using HMUI;
using UIEngine.Managers;
using UnityEngine;

namespace UIEngine.HarmonyPatches
{
    [HarmonyPatch(typeof(TitleViewController))]
    [HarmonyPatch(nameof(TitleViewController.SetText), MethodType.Normal)]
    internal class TitleViewControllerPatch
    {

        static void Postfix(ref TitleViewController __instance)
        {
            GameObject bg = __instance.gameObject.transform.Find("BG")?.gameObject;

            ImageView iv = bg?.GetComponent<ImageView>();

            // TODO reimplement

            /*if(iv != null)
                iv.color = UIEColorManager.instance.IsAdvanced() ? UIEColorManager.instance.bannerTop : UIEColorManager.instance.SimplePrimaryNormal;*/
        }

    }
}
