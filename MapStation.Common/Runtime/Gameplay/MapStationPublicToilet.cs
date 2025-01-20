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
            var myTrigger = transform.Find("Trigger");
            var myToilet = StagePrefabHijacker.Prefabs.GetToilet();
            var toiletScript = myToilet.GetComponent<PublicToilet>();

            toiletScript.Awake();
            // Remove shell
            Destroy(toiletScript.transform.Find("OutHouse").Find("Cube.007").gameObject);
            // Remove collision
            Destroy(toiletScript.transform.Find("OutHouse").Find("Cube.001").gameObject);
            Destroy(toiletScript.transform.Find("Trigger").gameObject);
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
            myTrigger.transform.SetParent(myToilet.transform, true);
#endif
        }
    }
}
