using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Reptile;

namespace Winterland.Common {
    public class AmbientOverrideTrigger : MonoBehaviour {
        public Color LightColor = Color.white;
        public Color ShadowColor = Color.black;
        public float TransitionDuration = 2f;
        [Header("If this is set to false, they Day and Night cycle mod won't take effect while you're in this ambient.")]
        public bool EnableDayLightCycleMod = true;

        private void OnTriggerEnter(Collider other) {
            var otherPlayer = other.GetComponent<Player>();
            if (otherPlayer == null)
                otherPlayer = other.GetComponentInParent<Player>();
            if (otherPlayer == null)
                otherPlayer = other.GetComponentInChildren<Player>();
            if (otherPlayer == null)
                return;
            if (otherPlayer.isAI)
                return;
            AmbientOverride.Instance.TransitionAmbient(this);
        }

        private void OnTriggerExit(Collider other) {
            var otherPlayer = other.GetComponent<Player>();
            if (otherPlayer == null)
                otherPlayer = other.GetComponentInParent<Player>();
            if (otherPlayer == null)
                otherPlayer = other.GetComponentInChildren<Player>();
            if (otherPlayer == null)
                return;
            if (otherPlayer.isAI)
                return;
            AmbientOverride.Instance.StopAmbient(this);
        }
    }
}
