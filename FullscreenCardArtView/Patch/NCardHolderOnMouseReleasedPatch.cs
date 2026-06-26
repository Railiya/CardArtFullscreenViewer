using HarmonyLib;
using Godot;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using FullscreenCardArtView.Component;

namespace FullscreenCardArtView.Patch
{
    [HarmonyPatch(typeof(NCardHolder), "OnMouseReleased")]
    public static class NCardHolderOnMouseReleasedPatch
    {
        public static bool Prefix(NCardHolder __instance, InputEvent inputEvent,
            bool ____isHovered, ref InputEventMouseButton ____currentPressedAction)
        {
            if (Config.EnableOnCardHolder == false)
            {
                return true;
            }

            if (__instance.CardNode == null || !____isHovered || ____currentPressedAction == null)
            {
                return true;
            }

            if (inputEvent is not InputEventMouseButton mouseEvent)
            {
                return true;
            }

            if (mouseEvent.ButtonIndex != ____currentPressedAction.ButtonIndex)
            {
                return true;
            }

            if (mouseEvent.ButtonIndex == MouseButton.Right)
            {
                if (FullscreenArtViewer.ShowArt(__instance.CardNode))
                {
                    ____currentPressedAction = null;
                    return false;
                }
            }

            return true;
        }
    }
}
