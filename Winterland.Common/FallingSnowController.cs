using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Reptile;

namespace Winterland.Common {
    public class FallingSnowController : MonoBehaviour {
        [Tooltip("Size of the snow chunk grid. Make this match the size of the emitter.")]
        [SerializeField]
        private float gridSize = 50f;
        [SerializeField]
        private GameObject snowEmitter = null;

        // Amount of adjacent extra snow chunks to make around the camera. Should probably leave this at 1 as it will increase the number of snow particles exponentially.
        private const int AmountAroundCamera = 1;

        private Dictionary<Vector2, GameObject> particles = null;
        private Stack<GameObject> particlePool = null;

        private void Awake() {
            particles = new();
            particlePool = new();
            var rowsAndColumns = 1 + (AmountAroundCamera * 2);
            var poolAmount = rowsAndColumns * rowsAndColumns;
            AddToPool(snowEmitter);
            for (var i = 1; i < poolAmount; i++) {
                var instance = GameObject.Instantiate(snowEmitter);
                AddToPool(instance);
            }
        }

        private void AddToPool(GameObject instance) {
            instance.transform.parent = transform;
            instance.SetActive(false);
            particlePool.Push(instance);
        }

        private GameObject GetFromPool() {
            var instance = particlePool.Pop();
            instance.SetActive(true);
            return instance;
        }

        private Vector2 GridSnapPosition(Vector3 position) {
            var px = Mathf.Floor(position.x / gridSize) * gridSize;
            var py = Mathf.Floor(position.z / gridSize) * gridSize;
            return new Vector2(px, py);
        }

        private bool InRange(Vector2 position, Vector2 position2) {
            var xDist = Mathf.Abs(position.x - position2.x);
            var yDist = Mathf.Abs(position.y - position2.y);
            if (xDist > AmountAroundCamera * gridSize || yDist > AmountAroundCamera * gridSize)
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

            for (var i = -AmountAroundCamera;i <= AmountAroundCamera; i++) {
                for(var j = -AmountAroundCamera; j <= AmountAroundCamera; j++) {
                    var pos = new Vector2(gridPosCenter.x + (i * gridSize), gridPosCenter.y + (j * gridSize));
                    if (!particles.ContainsKey(pos)) {
                        var part = GetFromPool();
                        part.transform.position = new Vector3(pos.x + (gridSize * 0.5f), currentCamera.transform.position.y, pos.y + (gridSize * 0.5f));
                        particles[pos] = part;
                    }
                }
            }
        }
    }
}
