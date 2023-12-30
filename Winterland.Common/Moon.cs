using Reptile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Winterland.Common {
    public class Moon : MonoBehaviour {
        private void Update() {
            var cam = WorldHandler.instance.currentCamera;
            if (cam == null) return;
            var ambient = AmbientOverride.Instance;
            if (ambient == null) return;
            transform.position = cam.transform.position;
            transform.rotation = Quaternion.LookRotation(-ambient.transform.forward);
        }
    }
}
