using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Reptile;

namespace Winterland.Common {
    public class CameraSequenceAction : SequenceAction {
        public override bool SequenceOnly => true;
        [Header("Leave this on None to keep the current camera.")]
        public CameraRegisterer Camera;

        public override void Run() {
            base.Run();
            if (Camera != null)
                Sequence.SetCamera(Camera.gameObject);
        }
    }
}
