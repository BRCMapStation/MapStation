using Reptile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Winterland.Common {
    public class CustomCharacterSelectSpot : MonoBehaviour {
        public string GUID = null;
        private static GameObject Source = null;

        private void Reset() {
            GUID = Guid.NewGuid().ToString();
        }

        private void Awake() {
            if (Source == null) {
                Source = FindObjectOfType<CharacterSelectSpot>().gameObject;
            }
            var mySpot = Instantiate(Source, transform);
            mySpot.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            mySpot.GetComponent<CharacterSelectSpot>().uid = GUID;
            mySpot.transform.Find("mesh").gameObject.SetActive(false);
        }
    }
}
