using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using CommonAPI;
using Reptile;

namespace Winterland.Common {
    [ExecuteAlways]
    public class CustomNPC : MonoBehaviour {
        [Header("General")]
        [SerializeField]
        private string guid = "";
        public Guid GUID {
            get {
                return Guid.Parse(guid);
            }
            set {
                guid = value.ToString();
            }
        }
        public string Name = "";
        public bool PlacePlayerAtSnapPosition = true;
        public bool LookAt = true;
        public bool ShowRep = false;
        public int MaxDialogueLevel = 1;
        [HideInInspector]
        public int CurrentDialogueLevel = 0;
        private EventDrivenInteractable interactable;
        private DialogueBranch[] dialogueBranches;
        private Transform head;

        private const float MaxHeadYaw = 75f;
        private const float LookAtDuration = 2f;
        private const float LookAtSpeed = 2.5f;
        private float currentLookAtAmount = 0f;
        private float lookAtTimer = 0f;
#if !UNITY_EDITOR
        private Player lookAtTarget = null;
#endif
        private bool isLookingAt = false;

        private void Reset() {
            if (!Application.isEditor)
                return;
            gameObject.layer = 19;
            GUID = Guid.NewGuid();
        }

        private void Awake() {
            if (Application.isEditor)
                return;
            ReptileAwake();

            void ReptileAwake() {
#if !UNITY_EDITOR
                CurrentDialogueLevel = 0;
                dialogueBranches = DialogueBranch.GetComponentsOrdered<DialogueBranch>(gameObject);
                interactable = gameObject.AddComponent<EventDrivenInteractable>();
                interactable.OnInteract = Interact;
                interactable.OnGetLookAtPos = GetLookAtPos;
                interactable.PlacePlayerAtSnapPosition = PlacePlayerAtSnapPosition;
                interactable.ShowRep = ShowRep;
                interactable.LookAt = LookAt;
                head = transform.FindRecursive("head");
                Core.OnUpdate += OnUpdate;
                Core.OnLateUpdate += OnLateUpdate;
                DeserializeNPC();
#endif
            }
        }
#if !UNITY_EDITOR
        private void OnUpdate() {
            if (isLookingAt) {
                currentLookAtAmount += LookAtSpeed * Core.dt;

                lookAtTimer -= Core.dt;
                if (lookAtTimer <= 0f) {
                    lookAtTimer = 0f;
                    isLookingAt = false;
                }
            }
            else {
                currentLookAtAmount -= LookAtSpeed * Core.dt;
            }
            currentLookAtAmount = Mathf.Clamp(currentLookAtAmount, 0f, 1f);
        }


        private void OnLateUpdate() {
            UpdateHeadTransform();
        }

#if !UNITY_EDITOR
        private void OnTriggerStay(Collider other) {
            var otherPlayer = other.GetComponentInChildren<Player>();
            if (otherPlayer == null)
                otherPlayer = other.GetComponentInParent<Player>();
            if (otherPlayer == null)
                return;
            if (otherPlayer.isAI)
                return;
            StartLookAt(otherPlayer);
        }
#endif

        private void UpdateHeadTransform() {
            if (lookAtTarget == null) return;
            if (head == null) return;
            var visual = lookAtTarget.characterVisual;
            if (visual == null) return;
            var targetHead = visual.head;
            if (head == null) return;

            var heading = (head.position - targetHead.position).normalized;
            var targetRotation = Quaternion.LookRotation(heading, Vector3.up).eulerAngles;
            var referenceRotation = Quaternion.LookRotation(-transform.forward, Vector3.up).eulerAngles;
            var forwardDifference = Mathf.DeltaAngle(targetRotation.y, referenceRotation.y);
            targetRotation.y = referenceRotation.y - Mathf.Clamp(forwardDifference, -MaxHeadYaw, MaxHeadYaw);
            head.transform.rotation = Quaternion.Lerp(head.transform.rotation, Quaternion.Euler(targetRotation), currentLookAtAmount);
        }

        private void StartLookAt(Player player) {
            var visual = player.characterVisual;
            if (visual == null) return;
            lookAtTarget = player;
            lookAtTimer = LookAtDuration;
            isLookingAt = true;
        }
#endif

        private void DeserializeNPC() {
            var progress = WinterProgress.Instance.LocalProgress.GetNPCProgress(this);
            if (progress == null)
                return;
            CurrentDialogueLevel = progress.DialogueLevel;
        }

        private void OnDestroy() {
            if (!Application.isEditor) {
                ReptileDestroy();
                return;
            }
            var branches = GetComponents<DialogueBranch>();
            foreach(var branch in branches) {
                DestroyImmediate(branch);
            }

            void ReptileDestroy() {
#if !UNITY_EDITOR
                Core.OnUpdate -= OnUpdate;
                Core.OnLateUpdate -= OnLateUpdate;
#endif
            }
        }

        public void AddDialogueLevel(int dialogueLevelToAdd) {
            CurrentDialogueLevel += dialogueLevelToAdd;
            CurrentDialogueLevel = Mathf.Clamp(CurrentDialogueLevel, 0, MaxDialogueLevel);
        }

        public Vector3 GetLookAtPos() {
            if (head)
                return head.position;
            return transform.position;
        }

        public void Interact(Player player) {
            Sequence sequence = null;
            foreach (var branch in dialogueBranches) {
                if (branch.Test(this)) {
                    sequence = branch.Sequence;
                    break;
                }
            }
            if (sequence == null)
                return;
            var sequenceWrapper = sequence.GetCustomSequence(this);
            if (sequenceWrapper == null)
                return;
            interactable.StartEnteringSequence(sequenceWrapper, sequence.HidePlayer, true, false, true, true, sequence.Skippable, true, false);
        }
    }
}
