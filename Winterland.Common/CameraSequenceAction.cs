using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Reptile;

namespace Winterland.Common {
    public class CameraSequenceAction : SequenceAction {
        [Header("Leave this on None to keep the current camera.")]
        public CameraRegisterer Camera;

        public override void Run(bool immediate) {
            base.Run(immediate);
            if (immediate)
                return;
            if (Camera != null)
                Sequence.SetCamera(Camera.gameObject);
        }
    }
}
