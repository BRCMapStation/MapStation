using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Winterland.Common {
    public class Firework : MonoBehaviour {
        public Color LightColor = Color.white;
        private ParticleSystem particleSystem = null;

        private void Awake() {
            particleSystem = GetComponent<ParticleSystem>();
            if (particleSystem != null)
                particleSystem.Stop();
        }

        public void Launch() {
            AmbientOverride.Instance.AddSkyLight(LightColor);
            if (particleSystem == null) return;
            particleSystem.Stop();
            particleSystem.Play();
        }
    }
}
