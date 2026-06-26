using HarmonyLib;
using Godot;
using MegaCrit.Sts2.Core.Nodes.Screens;
using MegaCrit.Sts2.Core.Nodes.Cards;
using CardArtFullscreenViewer.Component;

namespace CardArtFullscreenViewer.Patch
{
    [HarmonyPatch(typeof(NInspectCardScreen), "Open")]
    public static class NInspectCardScreenOpenPatch
    {
        private static NInspectCardScreen _screen = null;
        private static NCard _card = null;

        private static bool _isInputEnabled = false;
        private static bool _hasListener = false;
        private static bool _isWaitingForRelease = false;

        public static void Postfix(NInspectCardScreen __instance, NCard ____card)
        {
            _screen = __instance;
            _card = ____card;
            _isInputEnabled = true;
            _isWaitingForRelease = false;

            SetProcessActive(true);
        }

        private static void OnProcess()
        {
            if (_screen == null || GodotObject.IsInstanceValid(_screen) == false || _screen.Visible == false)
            {
                _screen = null;
                _card = null;
                _isInputEnabled = false;
                _isWaitingForRelease = false;

                SetProcessActive(false);
                return;
            }

            bool isRightPressed = Input.IsMouseButtonPressed(MouseButton.Right);

            if (_isWaitingForRelease)
            {
                if (isRightPressed == false) //Wait until right pressed is false for prevent infinite loop
                {
                    _isWaitingForRelease = false;
                }

                return;
            }

            if (_isInputEnabled == false || isRightPressed == false)
            {
                return;
            }

            Texture2D cardTexture = _card.Model.Portrait;

            if (cardTexture != null)
            {
                FullscreenArtViewer.ShowArt(cardTexture, EnableInput); //Restore input
            }

            _isInputEnabled = false;
        }

        private static void EnableInput()
        {
            _isInputEnabled = true;
            _isWaitingForRelease = true;
        }

        private static void SetProcessActive(bool active)
        {
            if (Engine.GetMainLoop() is not SceneTree tree)
            {
                return;
            }

            if (active && _hasListener == false)
            {
                tree.ProcessFrame += OnProcess;
            }
            else if (active == false && _hasListener)
            {
                tree.ProcessFrame -= OnProcess;
            }
        }
    }
}
