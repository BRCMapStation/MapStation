using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Reptile;
using CommonAPI;

namespace Winterland.Common {
    public class Sequence : MonoBehaviour {
        [Header("General")]
        public CameraRegisterer Camera;
        public bool ClearWantedLevel = true;
        public bool HidePlayer = false;
        public bool Skippable = true;
        [Header("On Begin")]
        public GameObject[] ActivateOnBegin;
        public GameObject[] DeactivateOnBegin;
        [Header("On End")]
        public GameObject[] ActivateOnEnd;
        public GameObject[] DeactivateOnEnd;
        public WinterObjective SetObjectiveOnEnd;

        private SequenceWrapper actualSequence;
        private bool initialized = false;

        private void Awake() {
            Initialize();
        }

        private void Initialize() {
            if (initialized)
                return;
            initialized = true;
            actualSequence = new SequenceWrapper(this);
        }

        public SequenceWrapper GetCustomSequence() {
            Initialize();
            return actualSequence;
        }
    }
}
