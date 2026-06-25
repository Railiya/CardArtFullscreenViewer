using HarmonyLib;
using Godot;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;

namespace CardArtFullscreenViewer
{
    [HarmonyPatch(typeof(NCardHolder), "OnMouseReleased")]
    public static class NCardHolderOnMouseReleasedPatch
    {
        public static bool Prefix(NCardHolder __instance, InputEvent inputEvent,
            bool ____isHovered, ref InputEventMouseButton ____currentPressedAction)
        {
            if (__instance.CardNode == null || __instance.CardNode.Model == null || 
                !____isHovered || ____currentPressedAction == null)
            {
                return true;
            }

            if (inputEvent is InputEventMouseButton mouseEvent)
            {
                if (mouseEvent.ButtonIndex != ____currentPressedAction.ButtonIndex)
                {
                    return true;
                }

                if (mouseEvent.ButtonIndex == MouseButton.Right)
                {
                    Texture2D cardTexture = __instance.CardNode.Model.Portrait;

                    if (cardTexture != null)
                    {
                        FullscreenArtViewer.ShowArt(cardTexture);
                    }

                    ____currentPressedAction = null;
                    return false;
                }
            }

            return true;
        }
    }
}
