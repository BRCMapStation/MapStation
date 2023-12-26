using System.Collections;
using System.Management.Instrumentation;
using UnityEngine;
using UnityEngine.Playables;

class GiftConveyorBelt : MonoBehaviour {
    public GameObject GiftPrefab;

    public float minInterval = 0.5f;
    public float maxInterval = 0.75f;

    private Coroutine animCoro = null;

    void OnEnable() {
        animCoro = StartCoroutine(Animation());
    }

    void OnDisable() {
        if(animCoro != null) {
            StopCoroutine(animCoro);
            animCoro = null;
        }
    }

    IEnumerator Animation() {
        while(true) {
            yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));

            // Spawn a new gift
            var newInstance = Instantiate(GiftPrefab);
            // TODO randomize the gift mesh
            // TODO randomize y-axis rotation?
            // TODO despawn the gift once its timeline finishes.
        }
    }
}