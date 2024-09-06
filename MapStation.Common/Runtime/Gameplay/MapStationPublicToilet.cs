using Reptile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MapStation.Common.Runtime.Gameplay {
    public class MapStationPublicToilet : MonoBehaviour {
        public Transform Exit;
        public Animator DoorAnimator;
        public GameObject GreenLight;
        public GameObject RedLight;
        private void Awake() {
#if BEPINEX
            var myToilet = StagePrefabHijacker.Prefabs.GetToilet();
            var toiletScript = myToilet.GetComponent<PublicToilet>();
            Destroy(toiletScript.doorAnimator.gameObject);

            toiletScript.exit = Exit;
            toiletScript.doorAnimator = DoorAnimator;
            toiletScript.greenLight = GreenLight;
            toiletScript.redLight = RedLight;
            foreach(var output in toiletScript.sequence.playableAsset.outputs) {
                if (output.streamName == "Animation Track (3)") {
                    toiletScript.sequence.SetGenericBinding(output.sourceObject, DoorAnimator);
                    break;
                }
            }
            myToilet.transform.SetParent(transform, false);
            myToilet.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
#endif
        }
    }
}
