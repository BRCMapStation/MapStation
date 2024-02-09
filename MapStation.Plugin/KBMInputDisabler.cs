using Reptile;
using UnityEngine;
using Player = Rewired.Player;

namespace MapStation.Plugin {
    class KBMInputDisabler {
        private static bool KbmDisabled;
        private static KeyCode ToggleKey;

        public static void Init(bool kbmDisabled, KeyCode toggleKey, ref Plugin.UpdateDelegate updateEvent, ref Plugin.UpdateDelegate lateUpdateEvent) {
            KbmDisabled = kbmDisabled;
            ToggleKey = toggleKey;
            updateEvent += Update;
            lateUpdateEvent += LateUpdate;
        }
        
        private static void Update() {
            if (Input.GetKeyDown(ToggleKey)) {
                KbmDisabled = !KbmDisabled;
                Apply();
            }
        }
        private static void LateUpdate() {
            // Forcibly re-enable cursor as long as KBM is disabled.
            // Do *not* forcibly lock cursor when KBM is enabled; let the game toggle it on/off for pause menu.
            // Doing in LateUpdate conveniently overrides BRC's menu code, which hides cursor in menus when it
            // believes you're using controller.
            if (KbmDisabled) {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }

        private static Player TryGetRewiredPlayer() {
            var core = Core.Instance;
            if (core == null) return null;
            var gameInput = core.GameInput;
            var player = gameInput?.rewiredMappingHandler?.GetRewiredPlayer();
            return player;
        }
        
        /// <summary>
        /// Patches must call this when Player is created so disablement can be re-applied
        /// </summary>
        public static void Apply() {
            var player = TryGetRewiredPlayer();
            if (player == null) return;
            if (KbmDisabled) DisableKbm(player);
            else EnableKbm(player);
        }
        
        private static void DisableKbm(Player player) {
            // Disable mouse and keyboard, only allow controller
            player.controllers.Mouse.enabled = false;
            player.controllers.Keyboard.enabled = false;
            
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        private static void EnableKbm(Player player) {
            // Enable mouse and keyboard
            player.controllers.Mouse.enabled = true;
            player.controllers.Keyboard.enabled = true;
            
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
