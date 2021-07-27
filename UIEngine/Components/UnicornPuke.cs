using HarmonyLib;
using HMUI;
using IPA.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UIEngine.Configuration;
using UIEngine.Extensions;
using UIEngine.Utilities;
using UnityEngine;

namespace UIEngine.Components
{
    public class UnicornPuke : MonoBehaviour
    {

        public static UnicornPuke PukeMaster = null;

        public Color CurrentMainColor { get; private set; } = Color.white;

        private PluginConfig _pluginConfig;
        private List<(string relativePath, string property, Type animationTargetType)> _animationTargets;
        private Dictionary<(string relativePath, string property, Type animationTargetType), Component> _components = new Dictionary<(string relativePath, string property, Type animationTargetType), Component>();
        private Dictionary<Type, (PropertyInfo, MethodBase)> _reflectionGarbo = new Dictionary<Type, (PropertyInfo, MethodBase)>();

        public void Init(PluginConfig pluginConfig, List<(string relativePath, string property, Type animationTargetType)> animationTargets)
        {
            _pluginConfig = pluginConfig;
            _animationTargets = animationTargets;
        }

        public void Awake()
        {
            if (PukeMaster == null)
                PukeMaster = this;
        }

        public void Update()
        {
            if(PukeMaster.isActiveAndEnabled && PukeMaster == this)
            {
                CurrentMainColor = GetMainColor();
            }
            else if(!PukeMaster.isActiveAndEnabled)
            {
                PukeMaster = this;
                CurrentMainColor = GetMainColor();
            }
        }

        public Color GetMainColor()
        {
            return Color.HSVToRGB((Time.realtimeSinceStartup / 10f) % 1f, 1, 1);
        }

        public void LateUpdate()
        {
            if (PukeMaster != this)
            {
                CurrentMainColor = PukeMaster?.CurrentMainColor ?? Color.white;
            }

            ColorElements();
        }

        private void ColorElements()
        {
            Component comp;
            PropertyInfo prop;
            MethodBase methodBase;
            (PropertyInfo, MethodBase) garbo;
            foreach ((string relativePath, string property, Type animationTargetType) target in _animationTargets)
            {

                if(!_components.TryGetValue(target, out comp))
                {
                    comp = gameObject.GetComponentOnChild(target.relativePath, target.animationTargetType);
                    _components.Add(target, comp);
                }

                if(!_reflectionGarbo.TryGetValue(target.animationTargetType, out garbo))
                {
                    prop = target.animationTargetType.GetProperty(target.property, AccessTools.all);
                    methodBase = target.animationTargetType.GetMethod("OnEnable", AccessTools.all);

                    _reflectionGarbo.Add(target.animationTargetType, (prop, methodBase));
                }
                else
                {
                    prop = garbo.Item1;
                    methodBase = garbo.Item2;
                }

                prop?.SetValue(comp, CurrentMainColor);
                methodBase?.Invoke(comp, null);

            }

        }
    }
}
