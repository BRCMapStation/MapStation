using Reptile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Winterland.Common {
    public class BindGameObjectActive : MonoBehaviour {
        [Header("When the GameObject referenced here is disabled, I will get disabled as well, and viceversa.")]
        public GameObject BoundGameObject = null;

        private void Awake() {
            Core.OnAlwaysUpdate += CoreUpdate;
        }

        private void CoreUpdate() {
            if (BoundGameObject.activeInHierarchy && !gameObject.activeSelf)
                gameObject.SetActive(true);
            else if (!BoundGameObject.activeInHierarchy && gameObject.activeSelf)
                gameObject.SetActive(false);
        }

        private void OnDestroy() {
            Core.OnAlwaysUpdate -= CoreUpdate;
        }
    }
}
