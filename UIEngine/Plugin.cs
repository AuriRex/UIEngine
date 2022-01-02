using IPA;
using IPA.Config;
using IPA.Config.Stores;
using SiraUtil.Zenject;
using UIEngine.Installers;
using IPALogger = IPA.Logging.Logger;

namespace UIEngine
{
    [Plugin(RuntimeOptions.DynamicInit)]
    public class Plugin
    {
        private Configuration.PluginConfig _config;

        [Init]
        public void Init(IPALogger logger, Config conf, Zenjector zenjector)
        {
            Logger.log = logger;
            
            _config = conf.Generated<Configuration.PluginConfig>();

            zenjector.Install<UIECoreInstaller>(Location.App, _config);
            zenjector.Install<UIEMenuInstaller>(Location.Menu);
        }
    }
}
