using UnityEngine;
using SlopCrew.Server.XmasEvent;

namespace Winterland.Common {
    public class NetManagerDebugUI : MonoBehaviour {
        public static NetManagerDebugUI Instance {get; private set;}

        private uint modifyPhaseIndex = 0;
        private XmasPhaseModifications modifications = new XmasPhaseModifications();
        private bool ignoreCooldown = false;

        public static void Create() {
            if(Instance != null) return;
            var gameObject = new GameObject($"Winterland {nameof(NetManagerDebugUI)}");
            Instance = gameObject.AddComponent<NetManagerDebugUI>();
            DontDestroyOnLoad(gameObject);
        }

        void Awake() {
            DebugUI.Instance.RegisterMenu("Net Manager", OnDebugUI);
        }

        private void OnDebugUI() {
            var netManager = NetManager.Instance;
            GUILayout.Label($"Using {WinterProgress.Instance.GlobalProgress.GetType().Name}");
            var state = WinterProgress.Instance.GlobalProgress.State;
            if(state != null) {
                GUILayout.TextArea(state.DescribeWithoutPacketInfo());
            }
            ignoreCooldown = GUILayout.Toggle(this.ignoreCooldown, "Ignore cooldown");
            if (GUILayout.Button($"Send {nameof(XmasClientCollectGiftPacket)}")) {
                NetManager.Instance.SendPacket(new XmasClientCollectGiftPacket {
                    IgnoreCooldown = this.ignoreCooldown
                });
            }
            
            void modToggle(ref bool modify) {
                modify = GUILayout.Toggle(modify, "Write", GUILayout.Width(50));
            }
            void boolField(ref bool value, string name) {
                value = GUILayout.Toggle(value, name);
            }
            void uintField(ref uint value, string name) {
                GUILayout.Label(name);
                uint.TryParse(GUILayout.TextField(value.ToString()), out value);
            }
            
            GUILayout.Label("Choose phase to modify, check fields to modify, set values, and send");
            GUILayout.Label("Only fields w/Write selected will be modified");
            GUILayout.BeginHorizontal();
            uintField(ref this.modifyPhaseIndex, "Phase #");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            modToggle(ref this.modifications.ModifyActive);
            boolField(ref this.modifications.Phase.Active, nameof(XmasPhase.Active));
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            modToggle(ref this.modifications.ModifyGiftsCollected);
            uintField(ref this.modifications.Phase.GiftsCollected, nameof(XmasPhase.GiftsCollected));
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            modToggle(ref this.modifications.ModifyGiftsGoal);
            uintField(ref this.modifications.Phase.GiftsGoal, nameof(XmasPhase.GiftsGoal));
            GUILayout.EndHorizontal();
            
            GUILayout.BeginHorizontal();
            modToggle(ref this.modifications.ModifyActivatePhaseAutomatically);
            boolField(ref this.modifications.Phase.ActivateNextPhaseAutomatically, nameof(XmasPhase.ActivateNextPhaseAutomatically));
            GUILayout.EndHorizontal();
            
            if (GUILayout.Button("Write")) {
                sendModificationPacket(this.modifyPhaseIndex, this.modifications);
            }
        }

        private void sendModificationPacket(uint phase, XmasPhaseModifications modifications) {
            var packet = new XmasClientModifyEventStatePacket();
            for (var i = 0; i < phase; i++) {
                packet.PhaseModifications.Add(new XmasPhaseModifications());
            }
            packet.PhaseModifications.Add(modifications);
            NetManager.Instance.SendPacket(packet);
        }
    }
}
