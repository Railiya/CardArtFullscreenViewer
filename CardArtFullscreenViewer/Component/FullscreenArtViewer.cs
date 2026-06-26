using System.Reflection;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace CardArtFullscreenViewer.Component
{
    public partial class FullscreenArtViewer : CanvasLayer
    {
        private static FullscreenArtViewer _instance = null;

        private ColorRect _background = null;
        private TextureRect _artRect = null;
        private FieldInfo _portraitField = null;

        private Action _closeCallback = null;
        private bool _isComponentsInitialized = false;

        public static bool ShowArt(NCard card, Action closeCallback = null)
        {
            if (card == null)
            {
                return false;
            }

            if (_instance == null)
            {
                SceneTree tree = Engine.GetMainLoop() as SceneTree;

                if (tree != null && tree.Root != null)
                {
                    _instance = new FullscreenArtViewer();
                    _instance.Layer = 500;

                    tree.Root.AddChild(_instance);
                }
            }

            if (_instance != null)
            {
                _instance._closeCallback = closeCallback;
                return _instance.Open(card);
            }
            else
            {
                return false;
            }
        }

        private void OnBackgroundGuiInput(InputEvent @event)
        {
            if (Visible == false)
            {
                return;
            }

            if (@event is InputEventMouseButton mouseEvent && mouseEvent.ButtonIndex == MouseButton.Right && mouseEvent.Pressed)
            {
                GetViewport().SetInputAsHandled();
                Close();
            }
        }

        private void InitiliazeComponents()
        {
            if (_isComponentsInitialized)
            {
                return;
            }

            _background = new ColorRect();
            _background.Color = new Color(0, 0, 0, 0.75f);
            _background.SetAnchorsPreset(Control.LayoutPreset.FullRect);
            _background.MouseFilter = Control.MouseFilterEnum.Stop;
            AddChild(_background);

            _artRect = new TextureRect();
            _artRect.ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize;
            _artRect.StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered;
            _artRect.SetAnchorsPreset(Control.LayoutPreset.FullRect);
            _artRect.MouseFilter = Control.MouseFilterEnum.Ignore;
            AddChild(_artRect);

            BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;

            _portraitField = typeof(NCard).GetField("_portrait", flags);
            _isComponentsInitialized = true;
        }

        private bool Open(NCard card)
        {
            InitiliazeComponents();

            Texture2D texture = GetPortrait(card);

            if (texture == null)
            {
                return false;
            }

            _background.GuiInput += OnBackgroundGuiInput;
            _artRect.Texture = texture;

            Visible = true;

            NHotkeyManager.Instance.AddBlockingScreen(this);
            NHotkeyManager.Instance.PushHotkeyPressedBinding(MegaInput.cancel, Close);
            NHotkeyManager.Instance.PushHotkeyPressedBinding(MegaInput.pauseAndBack, Close);
            return true;
        }

        private void Close()
        {
            if (_isComponentsInitialized == false)
            {
                return;
            }

            NHotkeyManager.Instance.RemoveHotkeyPressedBinding(MegaInput.cancel, Close);
            NHotkeyManager.Instance.RemoveHotkeyPressedBinding(MegaInput.pauseAndBack, Close);
            NHotkeyManager.Instance.RemoveBlockingScreen(this);

            Visible = false;

            _background.GuiInput -= OnBackgroundGuiInput;
            _artRect.Texture = null;

            _closeCallback?.Invoke();
            _closeCallback = null;

            SfxCmd.Play("event:/sfx/ui/map/map_close");
        }

        private Texture2D GetPortrait(NCard card)
        {
            object value = _portraitField.GetValue(card);

            if (value is TextureRect textureRect)
            {
                return textureRect.Texture;
            }

            return null;
        }
    }
}
