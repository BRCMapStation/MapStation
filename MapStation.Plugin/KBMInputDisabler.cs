using Reptile;

namespace Winterland.Plugin {
    class KBMInputDisabler {
        public static void Disable() {
            // Disable mouse and keyboard, only allow controller
            var gameInput = Core.Instance.GameInput;
            var player = gameInput.rewiredMappingHandler.GetRewiredPlayer();
            player.controllers.Mouse.enabled = false;
            player.controllers.Keyboard.enabled = false;
        }
    }
}
