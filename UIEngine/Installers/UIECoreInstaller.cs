using UIEngine.Configuration;
using UIEngine.Managers;
using Zenject;

namespace UIEngine.Installers
{
    internal class UIECoreInstaller : Installer<UIECoreInstaller>
    {
        private PluginConfig _pluginConfig;
        private UIEElementManager _elementManager;
        private UIEColorManager _colorManager;

        internal UIECoreInstaller(PluginConfig pluginConfig, UIEElementManager elementManager, UIEColorManager colorManager)
        {
            _pluginConfig = pluginConfig;
            _elementManager = elementManager;
            _colorManager = colorManager;
        }

        public override void InstallBindings()
        {
            Container.Bind<PluginConfig>().FromInstance(_pluginConfig).AsSingle();
            Container.Bind<UIEElementManager>().FromInstance(_elementManager).AsSingle();
            Container.Bind<UIEColorManager>().FromInstance(_colorManager).AsSingle();
            // TODO I swear I'm gonna do something with this eventually xp
        }
    }
}
