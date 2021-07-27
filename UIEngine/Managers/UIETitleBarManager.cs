using HMUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIEngine.Components;
using UIEngine.Configuration;

namespace UIEngine.Managers
{
    public class UIETitleBarManager : UIEElementManagerBase<TitleViewController>
    {
        private List<(string, string, Type)> _titleAnimationList;

        public UIETitleBarManager(PluginConfig pluginConfig) : base(pluginConfig)
        {
            _titleAnimationList = new List<(string, string, Type)>()
            {
                ("BG", "color", typeof(ImageView)),
                ("BackButton/BG", "color", typeof(ImageView))
            };
        }

        public override bool ShouldDecorateElement(TitleViewController element)
        {
            return pluginConfig.Enabled && pluginConfig.UnicornPuke;
        }

        public override void DecorateElement(TitleViewController element)
        {

            UnicornPuke unicornPuke = element.gameObject.AddComponent<UnicornPuke>();

            unicornPuke.Init(pluginConfig, _titleAnimationList);
        }
        
    }
}
