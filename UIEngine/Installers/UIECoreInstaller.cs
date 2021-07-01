using UIEngine.Configuration;
using UIEngine.Managers;
using Zenject;

namespace UIEngine.Installers
{
    internal class UIECoreInstaller : Installer<UIECoreInstaller>
    {
        private PluginConfig _pluginConfig;

        internal UIECoreInstaller(PluginConfig pluginConfig)
        {
            _pluginConfig = pluginConfig;
        }

        public override void InstallBindings()
        {
            Container.Bind<PluginConfig>().FromInstance(_pluginConfig).AsSingle();
            Container.BindInterfacesAndSelfTo<UIEButtonManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<UIEToggleManager>().AsSingle();
            Container.BindInterfacesAndSelfTo<UIESegmentManager>().AsSingle();
            // TODO I swear I'm gonna do something with this eventually xp
        }
    }
}
