using Reptile;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Winterland.Common {
    public class FireworkHolder : MonoBehaviour {
        public float TimeForFirstLaunch = 0.4f;
        public float MinimumTimeBetweenLaunches = 1f;
        public float MaximumTimeBetweenLaunches = 2f;
        public AudioClip FireworkSFX = null;
        public int FireworkAmount = 10;
        private Firework[] fireworks = null;

        private void Awake() {
            fireworks = GetComponentsInChildren<Firework>(true);
        }

        public void Launch() {
            var audioManager = Core.Instance.AudioManager;
            audioManager.PlayNonloopingSfx(audioManager.audioSources[3], FireworkSFX, audioManager.mixerGroups[3], 0f);
            StartCoroutine(LaunchCoroutine());
        }

        private IEnumerator LaunchCoroutine() {
            var potentialFireworks = fireworks.ToList();
            yield return new WaitForSeconds(TimeForFirstLaunch);
            for(var i = 0; i < FireworkAmount; i++) {
                var interval = UnityEngine.Random.Range(MinimumTimeBetweenLaunches, MaximumTimeBetweenLaunches);
                var fireworkIndex = UnityEngine.Random.Range(0, potentialFireworks.Length);
                var firework = potentialFireworks[fireworkIndex];
                potentialFireworks.RemoveAt(fireworkIndex);
                firework.Launch();
                yield return new WaitForSeconds(interval);
            }
        }
    }
}
