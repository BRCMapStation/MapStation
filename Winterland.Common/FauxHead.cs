using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Reptile;
using UnityEngine.UI;

namespace Winterland.Common {
    public class FauxHead : MonoBehaviour {
        [SerializeField]
        private float timeIdleToConsiderGrounded = 0.2f;
        [SerializeField]
        private float maximumDistanceTravelledToConsiderGrounded = 0.05f;
        private float lastMovedHeight = 0f;
        private float currentTimeIdle = 0f;
        [SerializeField]
        private LayerMask groundMask;
        [SerializeField]
        private float groundRayLength = 0.5f;
        [SerializeField]
        private float kickCooldownInSeconds = 0.5f;
        private float currentKickCooldown = 0f;
        private int currentJuggles = 0;
        private Rigidbody body;
        private bool onGround = false;

        private void Awake() {
            body = GetComponent<Rigidbody>();
            lastMovedHeight = transform.position.y;
        }

        private void OnTriggerStay(Collider other) {
            if (other.gameObject.layer != 18)
                return;
            if (currentKickCooldown > 0f)
                return;
            var otherPlayer = other.GetComponentInParent<Player>();
            if (otherPlayer == null)
                otherPlayer = other.GetComponentInChildren<Player>();
            if (otherPlayer == null)
                return;
            if (otherPlayer.isAI)
                return;
            var fauxJuggleUI = WinterUI.Instance?.FauxJuggleUI;
            if (fauxJuggleUI == null)
                return;
            currentKickCooldown = kickCooldownInSeconds;
            currentJuggles++;
            currentTimeIdle = 0f;
            if (currentJuggles >= 2)
                fauxJuggleUI.UpdateCounter(currentJuggles);
        }

        private void FixedUpdate() {
            currentKickCooldown -= Core.dt;
            if (currentKickCooldown < 0f)
                currentKickCooldown = 0f;

            if (currentKickCooldown <= 0f) {
                var ray = new Ray(transform.position, Vector3.down);
                if (Physics.Raycast(ray, out var hit, groundRayLength, groundMask, QueryTriggerInteraction.Ignore)) {
                    onGround = true;
                    if (hit.collider.GetComponent<Player>())
                        onGround = false;
                } else
                    onGround = false;
            } else
                onGround = false;

            if (onGround)
                currentTimeIdle = 0f;

            var positionDelta = transform.position.y - lastMovedHeight;
            if (positionDelta > maximumDistanceTravelledToConsiderGrounded) {
                currentTimeIdle = 0f;
                lastMovedHeight = transform.position.y;
            }
            else {
                currentTimeIdle += Core.dt;
                if (currentTimeIdle > timeIdleToConsiderGrounded)
                    onGround = true;
            }

            if (onGround) {
                currentJuggles = 0;
                var fauxJuggleUI = WinterUI.Instance?.FauxJuggleUI;
                if (fauxJuggleUI != null) {
                    if (onGround && fauxJuggleUI.CurrentJuggleAmount > 0) {
                        fauxJuggleUI.EndJuggle();
                    }
                }
            }
        }
    }
}
