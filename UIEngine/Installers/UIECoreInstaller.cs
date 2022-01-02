using UIEngine.Configuration;
using UIEngine.HarmonyPatches;
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
            Container.BindInterfacesAndSelfTo<UIETitleBarManager>().AsSingle();

            Container.BindInterfacesTo<AnimatedSwitchViewPatch>().AsSingle();
            Container.BindInterfacesTo<ButtonStaticAnimationsPatch>().AsSingle();
            Container.BindInterfacesTo<LevelListTableCellPatch>().AsSingle();
            Container.BindInterfacesTo<SelectableCellStaticAnimationsPatch>().AsSingle();
            Container.BindInterfacesTo<TitleViewControllerPatch>().AsSingle();
        }
    }
}
