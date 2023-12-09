using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Reptile;

namespace Winterland.Common {
    public class FallingSnowController : MonoBehaviour {
        [Tooltip("How much to move the particles upwards when you look up. To make it look like the particles are coming from higher up.")]
        [SerializeField]
        private float heightOffsetWhenLookingUp = 10f;
        [Tooltip("Size of the snow chunk grid. Make this match the size of the emitter.")]
        [SerializeField]
        private float gridSize = 50f;
        [SerializeField]
        private GameObject snowEmitter = null;
        [Tooltip("Amount of adjacent snow chunks to create around the camera. Probably best left at 1 as it increases the number of chunks exponentially.")]
        [SerializeField]
        private int amountAroundCamera = 1;

        private Dictionary<Vector2, GameObject> particles = null;
        private Stack<GameObject> particlePool = null;
        private List<GameObject> allParticles = null;

        private void Awake() {
            particles = new();
            particlePool = new();
            allParticles = new();
            var rowsAndColumns = 1 + (amountAroundCamera * 2);
            var poolAmount = rowsAndColumns * rowsAndColumns;
            AddToPool(snowEmitter);
            allParticles.Add(snowEmitter);
            for (var i = 1; i < poolAmount; i++) {
                var instance = GameObject.Instantiate(snowEmitter);
                AddToPool(instance);
                allParticles.Add(instance);
            }
        }

        public void AddKillTrigger(Collider trigger) {
            foreach(var particle in allParticles) {
                var emitters = particle.GetComponentsInChildren<ParticleSystem>(true);
                if (emitters == null)
                    continue;
                foreach(var emitter in emitters)
                    emitter.trigger.AddCollider(trigger);
            }
        }

        private void AddToPool(GameObject instance) {
            instance.transform.parent = transform;
            particlePool.Push(instance);
        }

        private GameObject GetFromPool() {
            var instance = particlePool.Pop();
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
            if (xDist > amountAroundCamera * gridSize || yDist > amountAroundCamera * gridSize)
                return false;
            return true;
        }

        private void Update() {
            var currentCamera = WorldHandler.instance.CurrentCamera;
            if (currentCamera == null)
                return;
            // Add more snow towards where we're looking at.
            var forwardFlat = (currentCamera.transform.forward - Vector3.Project(currentCamera.transform.forward, Vector3.up)).normalized;
            var referencePosition = currentCamera.transform.position + (forwardFlat * gridSize);
            var gridPosCenter = GridSnapPosition(referencePosition);
            var targetHeight = currentCamera.transform.position.y;
            var lookingUp = Mathf.Max(0f, Vector3.Dot(currentCamera.transform.forward, Vector3.up));

            targetHeight += heightOffsetWhenLookingUp * lookingUp;

            var newParticles = new Dictionary<Vector2, GameObject>();
            foreach (var particle in particles) {
                if (!InRange(particle.Key, gridPosCenter))
                    AddToPool(particle.Value);
                else {
                    particle.Value.transform.position = new Vector3(particle.Value.transform.position.x, targetHeight, particle.Value.transform.position.z);
                    newParticles[particle.Key] = particle.Value;
                }
            }
            particles = newParticles;

            for (var i = -amountAroundCamera;i <= amountAroundCamera; i++) {
                for(var j = -amountAroundCamera; j <= amountAroundCamera; j++) {
                    var pos = new Vector2(gridPosCenter.x + (i * gridSize), gridPosCenter.y + (j * gridSize));
                    if (!particles.ContainsKey(pos)) {
                        var part = GetFromPool();
                        part.transform.position = new Vector3(pos.x + (gridSize * 0.5f), targetHeight, pos.y + (gridSize * 0.5f));
                        particles[pos] = part;
                    }
                }
            }
        }
    }
}
