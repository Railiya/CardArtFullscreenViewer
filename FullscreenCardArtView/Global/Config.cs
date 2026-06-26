using FullscreenCardArtView.External;

namespace FullscreenCardArtView
{
    public static class Config
    {
        public const string MOD_ID = "com.github.railiya.cardartfullscreenviewer";

        /// <summary>
        /// Enable on Decks, Rewards and Compendium
        /// </summary>
        public static bool EnableOnCardHolder => ModConfigBridge.GetValue("enable_on_card_holder", true);

        /// <summary>
        /// Enable On Card Inspection
        /// </summary>
        public static bool EnableOnCardInspection => ModConfigBridge.GetValue("enable_on_card_inspection", true);
    }
}
