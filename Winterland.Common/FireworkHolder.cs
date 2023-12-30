using Reptile;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using SlopCrew.API;

namespace Winterland.Common {
    public class FireworkHolder : MonoBehaviour {
        public static FireworkHolder Instance = null;
        [HideInInspector]
        public DateTime LastTriggeredFireworks = default;
        public float CooldownSeconds = 30f;
        public float TimeForFirstLaunch = 0.4f;
        public float MinimumTimeBetweenLaunches = 1f;
        public float MaximumTimeBetweenLaunches = 2f;
        public AudioClip FireworkSFX = null;
        public int FireworkAmount = 10;
        private Firework[] fireworks = null;
        private ISlopCrewAPI slopCrewAPI = null;
        private const string PacketID = "Xmas-Client-Fireworks";

        private void Awake() {
            if (Instance != null) return;
            Instance = this;
            fireworks = GetComponentsInChildren<Firework>(true);
            slopCrewAPI = APIManager.API;
            slopCrewAPI.OnCustomPacketReceived += CustomPacketReceived;
        }

        private void CustomPacketReceived(uint playerid, string packetid, byte[] data) {
            if (packetid != PacketID) return;
            DispatchReceivedLaunch();
        }

        private void OnDestroy() {
            if (Instance == this)
                Instance = null;
            slopCrewAPI.OnCustomPacketReceived -= CustomPacketReceived;
        }

        public void Launch() {
            var audioManager = Core.Instance.AudioManager;
            audioManager.PlayNonloopingSfx(audioManager.audioSources[3], FireworkSFX, audioManager.mixerGroups[3], 0f);
            StartCoroutine(LaunchCoroutine(TimeForFirstLaunch));
            StartCoroutine(LaunchCoroutine(TimeForFirstLaunch + 0.5f));
        }

        public bool DispatchReceivedLaunch() {
            var now = DateTime.Now;
            var difference = now - LastTriggeredFireworks;
            if (difference.TotalSeconds <= CooldownSeconds)
                return false;
            LastTriggeredFireworks = now;
            Launch();
            return true;
        }

        public void BroadcastLaunch() {
            if (DispatchReceivedLaunch())
                slopCrewAPI.SendCustomPacket(PacketID, new byte[0]);
        }

        private IEnumerator LaunchCoroutine(float delay) {
            var potentialFireworks = fireworks.ToList();
            yield return new WaitForSeconds(delay);
            for(var i = 0; i < FireworkAmount; i++) {
                var interval = UnityEngine.Random.Range(MinimumTimeBetweenLaunches, MaximumTimeBetweenLaunches);
                var fireworkIndex = UnityEngine.Random.Range(0, potentialFireworks.Count);
                var firework = potentialFireworks[fireworkIndex];
                potentialFireworks.RemoveAt(fireworkIndex);
                firework.Launch();
                yield return new WaitForSeconds(interval);
            }
        }
    }
}
