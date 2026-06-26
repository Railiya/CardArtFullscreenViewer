using System.Reflection;
using HarmonyLib;
using Godot;
using MegaCrit.Sts2.Core.Modding;
using FullscreenCardArtView.External;

namespace FullscreenCardArtView
{
    [ModInitializer("InitializeMod")]
    public static class ModEntry
    {
        public static void InitializeMod()
        {
            try
            {
                Harmony harmony = new Harmony(Config.MOD_ID);
                harmony.PatchAll(Assembly.GetExecutingAssembly());

                ModConfigBridge.DeferredRegister();

                GD.Print($"Mod Initailized: {Config.MOD_ID}");
            }
            catch (Exception e)
            {
                GD.PrintErr($"Mod Initailized Failed: {e.Message}\n{e.StackTrace}");
            }
        }
    }
}
