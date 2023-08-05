using BepInEx.Configuration;
using BepInEx.Unity.IL2CPP;
using BepInEx;
using HarmonyLib;
using Il2CppInterop.Runtime.Injection;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;
using Il2CppSystem.IO;

namespace Faithy_SOTF_Mod
{
    [BepInPlugin(GUID, MODNAME, VERSION)]
    public class Plugin : BasePlugin
    {
        public const string
            MODNAME = "Faithy_SOTF_Mod",
            AUTHOR = "Faithy",
            GUID = MODNAME,
            VERSION = "0.0.1";

        public static ConfigFile ConfigFile = new(Path.Combine(Paths.ConfigPath, "Faithy_SOTF_Mod.cfg"), true);
        public static ConfigEntry<KeyCode> ModMenuKeybind = ConfigFile.Bind("Hotkeys", "Toggle", KeyCode.BackQuote, "Enables or disables the Mod Menu");

        public Plugin()
        {
            //TODO
        }

        public override void Load()
        {
            try
            {

                ClassInjector.RegisterTypeInIl2Cpp<Main.MyMonoBehaviour>();
                GameObject gameObject = new GameObject("CoolObject");
                gameObject.AddComponent<Main.MyMonoBehaviour>();
                gameObject.hideFlags = HideFlags.HideAndDontSave;
                Object.DontDestroyOnLoad(gameObject);
            }
            catch
            {
                //log.LogError($"FAILED to Register Il2Cpp Type: MyMonoBehaviour!");
            }
            try
            {
                Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), null);
            }
            catch
            {
                //log.LogError($"FAILED to register patches!");
            }
            //log.LogInfo($"ModMenu toggle keybind set to: {ModMenuKeybind.Value}");
        }
    }
}