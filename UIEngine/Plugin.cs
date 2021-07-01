using HarmonyLib;
using IPA;
using IPA.Config;
using IPA.Config.Stores;
using SiraUtil.Zenject;
using System.Reflection;
using UIEngine.Installers;
using IPALogger = IPA.Logging.Logger;

namespace UIEngine
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        public const string HARMONYID = "dev.AuriRex.UIEngine";
        private Harmony _harmony;

        private Configuration.PluginConfig _config;

        [Init]
        public void Init(IPALogger logger, Config conf, Zenjector zenjector)
        {
            Logger.log = logger;
            _harmony = new Harmony(HARMONYID);
            
            _config = conf.Generated<Configuration.PluginConfig>();

            zenjector.OnApp<UIECoreInstaller>().WithParameters(_config);
        }

        [OnStart]
        public void OnStart()
        {
            _harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        [OnExit]
        public void OnExit()
        {
            _harmony.UnpatchAll(HARMONYID);
        }
    }
}
