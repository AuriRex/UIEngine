using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.MenuButtons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIEngine.Configuration;
using UIEngine.UI;
using Zenject;

namespace UIEngine.Managers
{
    internal class MenuButtonManager : IInitializable, IDisposable
    {
        private MenuButton _menuButton;

        private readonly PluginConfig _pluginConfig;
        private readonly MainFlowCoordinator _mainFlowCoordinator;

        private readonly UIEFlowCoordinator _UIEflowCoordinator;

        public MenuButtonManager(PluginConfig pluginConfig, MainFlowCoordinator mainFlowCoordinator, UIEFlowCoordinator flowCoordinator)
        {
            _pluginConfig = pluginConfig;
            _mainFlowCoordinator = mainFlowCoordinator;
            _UIEflowCoordinator = flowCoordinator;
            _menuButton = new MenuButton("UIEngine", "Colorful", PresentFlowCoordinator);
        }

        

        public void Initialize()
        {
            MenuButtons.instance.RegisterButton(_menuButton);
        }

        public void Dispose()
        {
            if(MenuButtons.IsSingletonAvailable)
            {
                MenuButtons.instance.UnregisterButton(_menuButton);
            }
        }

        private void PresentFlowCoordinator()
        {
            _mainFlowCoordinator.PresentFlowCoordinator(_UIEflowCoordinator);
        }

    }
}
