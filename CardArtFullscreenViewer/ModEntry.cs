using HarmonyLib;
using Godot;
using MegaCrit.Sts2.Core.Modding;

namespace CardArtFullscreenViewer
{
    [ModInitializer("InitializeMod")]
    public static class ModEntry
    {
        private const string HARMONY_INSTANCE_ID = "com.github.railiya.cardartfullscreenviewer";

        public static void InitializeMod()
        {
            try
            {
                Harmony harmony = new Harmony(HARMONY_INSTANCE_ID);
                harmony.PatchAll();

                GD.Print($"Mod Initailized: {HARMONY_INSTANCE_ID}");
            }
            catch (Exception e)
            {
                GD.PrintErr($"Mod Initailized Failed: {e.Message}\n{e.StackTrace}");
            }
        }
    }
}
