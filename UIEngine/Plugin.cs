using HarmonyLib;
using IPA;
using IPA.Config;
using IPA.Config.Stores;
using UIEngine.Installers;
using UIEngine.Managers;
using SiraUtil.Zenject;
using System.Reflection;
using IPALogger = IPA.Logging.Logger;

namespace UIEngine
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        public const string HARMONYID = "com.aurirex.UIEngine";
        private Harmony _harmony;

        private Configuration.PluginConfig _config;
        private UIEColorManager _colorManager;
        private UIEButtonManager _elementManager;

        [Init]
        public void Init(IPALogger logger, Config conf, Zenjector zenjector)
        {
            Logger.log = logger;
            _harmony = new Harmony(HARMONYID);
            
            _config = conf.Generated<Configuration.PluginConfig>();
            _colorManager = new UIEColorManager(_config);
            _elementManager = new UIEButtonManager(_config, _colorManager);

            zenjector.OnApp<UIECoreInstaller>().WithParameters(_config, _elementManager, _colorManager);
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
            //_colorManager.Dispose();
        }
    }
}
