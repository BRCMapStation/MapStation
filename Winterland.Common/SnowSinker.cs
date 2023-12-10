using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Reptile;

namespace Winterland.Common {
    public class SnowSinker : MonoBehaviour {
        public bool Enabled = true;
        public float Size = 1f;
        public float Strength = 0.1f;
        private FallenSnowController snowController = null;

        private void Start() {
            snowController = FallenSnowController.Instance;
            if (snowController != null)
                snowController.OnUpdate += OnUpdate;
        }
        private void OnUpdate() {
            if (Enabled)
                snowController.DrawHole(new Vector2(transform.position.x, transform.position.z), Size, Strength);
        }

        private void OnDestroy() {
            if (snowController != null)
                snowController.OnUpdate -= OnUpdate;
        }
    }
}
