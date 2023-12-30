using Reptile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Reptile;

namespace Winterland.Common {
    public class CameraBloomEffect : MonoBehaviour {
        private void OnRenderImage(RenderTexture src, RenderTexture dest) {
            var bloom = BloomManager.Instance;
            Graphics.Blit(src, bloom.BloomRT, bloom.BloomPassMaterial);
            Graphics.Blit(src, dest, bloom.ScreenPassMaterial);
            
        }
    }
}
