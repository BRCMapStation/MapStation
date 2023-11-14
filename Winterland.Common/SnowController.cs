using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Reptile;

namespace Winterland.Common {
    public class SnowController : MonoBehaviour {
        private const int PoolAmount = 9;
        private const int Distance = 1;
        private const float GridSize = 30f;
        private GameObject particlePrefab = null;
        private Dictionary<Vector2, GameObject> particles = null;
        private Stack<GameObject> particlePool = null;

        private void Awake() {
            particles = new();
            particlePrefab = WinterAssets.Instance.WinterBundle.LoadAsset<GameObject>("Snow Particles");
            particlePool = new();
            for (var i = 0; i < PoolAmount; i++) {
                var instance = GameObject.Instantiate(particlePrefab);
                AddToPool(instance);
            }
        }

        private void AddToPool(GameObject instance) {
            instance.SetActive(false);
            particlePool.Push(instance);
        }

        private GameObject GetFromPool() {
            var instance = particlePool.Pop();
            instance.SetActive(true);
            return instance;
        }

        private Vector2 GridSnapPosition(Vector3 position) {
            var px = Mathf.Floor(position.x / GridSize) * GridSize;
            var py = Mathf.Floor(position.z / GridSize) * GridSize;
            return new Vector2(px, py);
        }

        private bool InRange(Vector2 position, Vector2 position2) {
            var xDist = Mathf.Abs(position.x - position2.x);
            var yDist = Mathf.Abs(position.y - position2.y);
            if (xDist > Distance * GridSize || yDist > Distance * GridSize)
                return false;
            return true;
        }

        private void Update() {
            var currentCamera = WorldHandler.instance.CurrentCamera;
            if (currentCamera == null)
                return;
            var gridPosCenter = GridSnapPosition(currentCamera.transform.position);

            var newParticles = new Dictionary<Vector2, GameObject>();
            foreach (var particle in particles) {
                if (!InRange(particle.Key, gridPosCenter))
                    AddToPool(particle.Value);
                else {
                    particle.Value.transform.position = new Vector3(particle.Value.transform.position.x, currentCamera.transform.position.y, particle.Value.transform.position.z);
                    newParticles[particle.Key] = particle.Value;
                }
            }
            particles = newParticles;

            for (var i = -Distance;i <= Distance; i++) {
                for(var j = -Distance; j <= Distance; j++) {
                    var pos = new Vector2(gridPosCenter.x + (i * GridSize), gridPosCenter.y + (j * GridSize));
                    if (!particles.ContainsKey(pos)) {
                        var part = GetFromPool();
                        part.transform.position = new Vector3(pos.x + (GridSize * 0.5f), currentCamera.transform.position.y, pos.y + (GridSize * 0.5f));
                        particles[pos] = part;
                    }
                }
            }
        }
    }
}
