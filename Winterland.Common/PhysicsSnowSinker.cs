using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Winterland.Common {
    [RequireComponent(typeof(SnowSinker))]
    [RequireComponent(typeof(Rigidbody))]
    public class PhysicsSnowSinker : MonoBehaviour {
        public LayerMask Mask;
        public float RayDistance = 1f;
        private SnowSinker snowSinker = null;
        private Rigidbody body = null;
        private void Awake() {
            snowSinker = GetComponent<SnowSinker>();
            body = GetComponent<Rigidbody>();
        }

        private void FixedUpdate() {
            if (body.isKinematic) {
                snowSinker.Enabled = false;
                return;
            }

            var ray = new Ray(transform.position, Vector3.down);
            if (Physics.Raycast(ray, out var hit, RayDistance, Mask, QueryTriggerInteraction.Ignore)) {
                var sink = true;
                var body = hit.collider.gameObject.GetComponent<Rigidbody>();
                if (body != null)
                    sink = false;
                if (sink)
                    snowSinker.Enabled = true;
                else
                    snowSinker.Enabled = false;
            } else
                snowSinker.Enabled = false;
        }
    }
}
