using Reptile;
using System;
using System.Collections.Generic;
using UnityEngine;
using Winterland.MapStation.Common.Serialization;

namespace Winterland.MapStation.Common.VanillaAssets {
    public class MoveVanillaGameObjectV1 : MonoBehaviour {

        public string moveThis;
        public Transform targetLocation;

        private void Awake() {
            if(!Application.isEditor) {
                MoveGameObject();
            }
        }

        public void MoveGameObject() {
            var go = GameObject.Find(moveThis);
            go.transform.position = targetLocation.position;
            go.transform.rotation = targetLocation.rotation;
        }
    }
}
