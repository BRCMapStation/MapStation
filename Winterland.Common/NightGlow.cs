using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Winterland.Common {
    [ExecuteAlways]
    public class NightGlow : MonoBehaviour {
        public float FadeInStartDistance = 0f;
        public float FadeInFalloff = 0f;
        public float FadeOutStartDistance = 500f;
        public float FadeOutFalloff = 0f;
        public float MaxSize = 25f;
        public float SizeByDistanceMultiplier = 0.002f;
        public Texture2D Texture = null;
        public Color Color = Color.white;
        private MaterialPropertyBlock propertyBlock = null;
        private MeshRenderer mesh = null;
        private int mainTexProperty = Shader.PropertyToID("_MainTex");
        private int colorProperty = Shader.PropertyToID("_Color");

        private void Awake() {
            Verify();
        }

        private void Update() {
#if UNITY_EDITOR
            Verify();
#endif
        }

        private float GetFadeMultiplier(float distance) {
            if (distance >= FadeInStartDistance && distance <= FadeInStartDistance + FadeInFalloff) {
                var distanceMinusStart = distance - FadeInStartDistance;
                return (distanceMinusStart / FadeInFalloff);
            }
            else if (distance >= FadeOutStartDistance && distance <= FadeOutStartDistance + FadeOutFalloff) {
                var distanceMinusStart = distance - FadeOutStartDistance;
                return (-(distanceMinusStart / FadeOutFalloff)) + 1f;
            }
            if (distance >= FadeOutStartDistance)
                return 0f;
            return 1f;
        }

        private float ApplyFadeFunction(float fade) {
            return fade * fade;
        }

        private void OnWillRenderObject() {
            var cam = Camera.current;

            if (cam == null) return;

            transform.rotation = cam.transform.rotation;
            var offset = cam.transform.position - transform.position;
            var sqrDist = offset.sqrMagnitude;
            var scale = 1f + (sqrDist * SizeByDistanceMultiplier);
            var normalizedScale = Mathf.Min(1f, scale / MaxSize);
            normalizedScale = (-normalizedScale) + 1f;
            normalizedScale *= normalizedScale;
            normalizedScale = (-normalizedScale) + 1f;
            scale = normalizedScale * MaxSize;
            transform.localScale = new Vector3(scale, scale, scale);

            if (propertyBlock == null) return;

            var fade = ApplyFadeFunction(GetFadeMultiplier(sqrDist));
            var color = Color;
            color.a *= fade;
            propertyBlock.SetColor(colorProperty, color);
        }

        private void Verify() {
            if (propertyBlock == null)
                propertyBlock = new MaterialPropertyBlock();
            if (Texture != null)
                propertyBlock.SetTexture(mainTexProperty, Texture);
            var meshTransform = transform.Find("Mesh");
            if (meshTransform == null) return;
            mesh = meshTransform.GetComponent<MeshRenderer>();
            if (mesh == null) return;
            mesh.SetPropertyBlock(propertyBlock);
        }

        private void OnDestroy() {
            propertyBlock?.Dispose();
        }
    }
}
