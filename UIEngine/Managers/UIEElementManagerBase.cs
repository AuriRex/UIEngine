﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIEngine.Configuration;
using UnityEngine;

namespace UIEngine.Managers
{
    public abstract class UIEElementManagerBase<TElement> : IDisposable where TElement : MonoBehaviour
    {
        private static UIEElementManagerBase<TElement> _instance;
        internal static UIEElementManagerBase<TElement> Instance
        {
            get
            {
                if (_instance == null) throw new Exception();
                return _instance;
            }
            private set
            {
                _instance = value;
            }
        }
        public static bool IsConstructed
        {
            get
            {
                return _instance != null;
            }
        }

        protected PluginConfig pluginConfig;

        protected HashSet<TElement> elementSet;

        public UIEElementManagerBase(PluginConfig pluginConfig)
        {
            this.pluginConfig = pluginConfig;

            elementSet = new HashSet<TElement>();

            _instance = this;
        }

        public void Dispose()
        {
            _instance = null;
        }


        public abstract void DecorateElement(TElement element);

        internal static void AddElement(TElement element)
        {
            if (element == null) return;
            if(!Instance.elementSet.Contains(element))
            {
                Instance.elementSet.Add(element);
                try
                {
                    Instance.DecorateElement(element);
                }
                catch(Exception ex)
                {
                    Logger.log.Error($"Failed decorating UI element \"{element.name}\": {ex.Message}");
                    Logger.log.Error($"{ex.StackTrace}");
                }
                
            }
            else
            {
                Logger.log.Warn($"Trying to decorate the element \"{element.name}\" twice, this should not happen.");
            }
        }

        public static bool TryGetCustomSettings<TCustomSetting, TBaseSetting>(GameObject go, string text, List<TCustomSetting> inList, out TBaseSetting settings) where TCustomSetting : PluginConfig.ICustomElementTarget, TBaseSetting
        {
            string goName = go.name;
            foreach (TCustomSetting s in inList)
            {
                switch (s.TargetMatchingMode)
                {
                    case PluginConfig.CustomElementTargetMatchingMode.TARGET_MODE_GAMEOBJECT_NAME:
                        if (s.TargetString.Equals(goName, StringComparison.CurrentCultureIgnoreCase))
                        {
                            settings = s;
                            return true;
                        }
                        break;
                    case PluginConfig.CustomElementTargetMatchingMode.TARGET_MODE_TEXT_CONTENT:
                        if (s.TargetString.Equals(text, StringComparison.CurrentCultureIgnoreCase))
                        {
                            settings = s;
                            return true;
                        }
                        break;
                    case PluginConfig.CustomElementTargetMatchingMode.TARGET_MODE_PARENT_GAMEOBJECT_NAME:
                        if (s.TargetString.Equals(go.transform.parent?.name, StringComparison.CurrentCultureIgnoreCase))
                        {
                            settings = s;
                            return true;
                        }
                        break;
                }
            }

            settings = default;
            return false;
        }

    }

    public abstract class UIEAnimationElementManagerBase<TElement> : UIEElementManagerBase<TElement> where TElement : MonoBehaviour
    {
        protected (AnimationClip, AnimationClip, AnimationClip, AnimationClip)? defaultCustomAnimationClips;
        protected Dictionary<TElement, (AnimationClip, AnimationClip, AnimationClip, AnimationClip)> animationClipsForElement;

        public UIEAnimationElementManagerBase(PluginConfig pluginConfig) : base(pluginConfig)
        {
            animationClipsForElement = new Dictionary<TElement, (AnimationClip, AnimationClip, AnimationClip, AnimationClip)>();
        }

        public static AnimationClip CreateNewAnimationClip(string identifier = "UIEngine_Unknown")
        {
            var clip = new AnimationClip();
            clip.legacy = true;
            clip.name = "CustomClip_" + identifier;
            return clip;
        }

        public static void AssignClip(ref AnimationClip clipToOverwrite, AnimationClip newClip)
        {
            clipToOverwrite = newClip;
        }
    }
}