using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

class TreeConveyorBelt : MonoBehaviour {
    public PlayableDirector[] directors;

    public float minInternal = 0.5f;
    public float maxInternal = 0.75f;

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
            yield return new WaitForSeconds(Random.Range(minInternal, maxInternal));

        }
    }
}