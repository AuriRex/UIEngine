using SiraUtil;
using UIEngine.Managers;
using UIEngine.UI;
using Zenject;

namespace UIEngine.Installers
{
    public class UIEMenuInstaller : Installer<UIEMenuInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<MenuButtonManager>().AsSingle();
            Container.Bind<UIEFlowCoordinator>().FromNewComponentOnNewGameObject().AsSingle();
            Container.Bind<UIEInitialSetupViewController>().FromNewComponentAsViewController().AsSingle();
            Container.Bind<UIEMainViewController>().FromNewComponentAsViewController().AsSingle();
            Container.Bind<UIEPreviewViewController>().FromNewComponentAsViewController().AsSingle();
        }
    }
}
