using BepInEx.Logging;

namespace Faithy_SOTF_Mod
{
    public static class MyLogger
    {
        public static ManualLogSource Logger = new("MyLogger");

        public static void Dbg(string msg) =>
#if DEBUG
            Logger.LogInfo("[debug] " + msg);
#endif

        public static void Info(string msg) => Logger.LogInfo(msg);
        public static void Error(string msg) => Logger.LogError(msg);
    }
}