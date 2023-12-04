using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Reptile;

namespace Winterland.Common {
    public class FallenSnowController : MonoBehaviour {
        public float MinimumSpeedForSnowParticles = 5f;
        public GameObject SnowFootstepParticlesPrefab = null;
        public Action OnUpdate = null;
        public Action OnUpdateOneShot = null;
        public static FallenSnowController Instance { get; private set; }
        private const string CameraPositionProp = "CameraPosition";
        private const string DepthHalfRadiusProp = "DepthHalfRadius";
        private const string DepthTextureProp = "DepthTexture";
        [SerializeField]
        private float clearRate = 0.033f;
        [SerializeField]
        private float clearStrength = 0.1f;
        [SerializeField]
        private float updateRate = 0.016f;
        [SerializeField]
        private float depthRadiusHalf = 100f;
        [SerializeField]
        private RenderTexture depthRenderTexture = null;
        [SerializeField]
        private Texture2D holeTexture = null;
        [SerializeField]
        private float gridSize = 10f;
        private RenderTexture backBufferRenderTexture = null;
        private Texture2D clearTexture = null;
        private Vector2 cameraPosition = Vector2.zero;
        private float currentUpdateTime = 0f;
        private float currentClearTime = 0f;

        private void ProcessRenderTextureDraws() {
            RenderTexture.active = depthRenderTexture;
            GL.PushMatrix();
            GL.LoadPixelMatrix(0, depthRadiusHalf * 2f, depthRadiusHalf * 2f, 0);
            OnUpdateOneShot?.Invoke();
            OnUpdate?.Invoke();
            GL.PopMatrix();
            RenderTexture.active = null;
            OnUpdateOneShot = null;
        }

        public void DrawHole(Vector2 worldPosition, float size, float strength) {
            var texturePosition = GetDepthPositionAt(worldPosition);
            texturePosition.y = -texturePosition.y + (depthRadiusHalf * 2f);
            texturePosition -= new Vector2(size * 0.5f, size * 0.5f);
            Graphics.DrawTexture(new Rect(texturePosition.x, texturePosition.y, size, size), holeTexture, new Rect(0f, 0f, 1f, 1f), 0, 0, 0, 0, new Color(1f, 1f, 1f, strength), null, -1);
        }

        private Vector2 GetDepthPositionAt(Vector2 position) {
            var worldCoordinateBegin = cameraPosition - new Vector2(depthRadiusHalf, depthRadiusHalf);
            var coords = position - worldCoordinateBegin;
            return coords;
        }

        private void Awake() {
            RenderTexture.active = depthRenderTexture;
            GL.Clear(true, true, Color.black);
            RenderTexture.active = null;
            backBufferRenderTexture = UnityEngine.Object.Instantiate(depthRenderTexture);
            clearTexture = new Texture2D(1, 1);
            clearTexture.SetPixel(0, 0, Color.black);
            clearTexture.Apply();
            Instance = this;
            Shader.SetGlobalFloat(DepthHalfRadiusProp, depthRadiusHalf);
            Shader.SetGlobalTexture(DepthTextureProp, depthRenderTexture);
        }

        private void TransformTexture(Vector2 previousCameraPosition, Vector2 currentCameraPosition) {
            Graphics.Blit(depthRenderTexture, backBufferRenderTexture);
            var offset = (currentCameraPosition - previousCameraPosition) / (depthRadiusHalf * 2f) * new Vector2(depthRenderTexture.width, depthRenderTexture.height);
            offset.x = -offset.x;
            RenderTexture.active = depthRenderTexture;
            GL.PushMatrix();
            GL.LoadPixelMatrix(0, depthRenderTexture.width, depthRenderTexture.height, 0);
            GL.Clear(true, true, Color.black);
            Graphics.DrawTexture(new Rect(offset.x, offset.y, depthRenderTexture.width, depthRenderTexture.height), backBufferRenderTexture);
            GL.PopMatrix();
            RenderTexture.active = null;
        }

        // Maybe conditionally build so we can track the editor camera in the unity project.
        private void Update() {
            var newCameraPosition = new Vector2(transform.position.x, transform.position.z);
            var currentCam = WorldHandler.instance.CurrentCamera;
            if (currentCam != null)
                newCameraPosition = new Vector2(currentCam.transform.position.x, currentCam.transform.position.z);

            var oldCameraPosition = cameraPosition;
            newCameraPosition.x = Mathf.Floor(newCameraPosition.x / gridSize) * gridSize;
            newCameraPosition.y = Mathf.Floor(newCameraPosition.y / gridSize) * gridSize;

            currentUpdateTime += Core.dt;
            currentClearTime += Core.dt;
            var clearAmount = Mathf.Floor(currentClearTime / clearRate);
            for (var i = 0; i < clearAmount; i++) {
                OnUpdateOneShot += () => { Graphics.DrawTexture(new Rect(0f, 0f, depthRadiusHalf * 2f, depthRadiusHalf * 2f), clearTexture, new Rect(0f, 0f, 1f, 1f), 0, 0, 0, 0, new Color(1f, 1f, 1f, clearStrength), null, -1); };
            }

            var updateAmount = Mathf.Floor(currentUpdateTime / updateRate);
            for(var i=0;i<updateAmount;i++) {
                ProcessRenderTextureDraws();
            }

            currentUpdateTime -= updateAmount * updateRate;
            currentClearTime -= clearAmount * clearRate;

            cameraPosition = newCameraPosition;

            if (newCameraPosition != oldCameraPosition)
                TransformTexture(oldCameraPosition, newCameraPosition);

            Shader.SetGlobalVector(CameraPositionProp, cameraPosition);
        }

        private void OnDestroy() {
            UnityEngine.Object.Destroy(clearTexture);
            UnityEngine.Object.Destroy(backBufferRenderTexture);
        }
    }
}
