using System.Collections;
using TMPro;
using UnityEngine;

namespace Winterland.Common;

class TreeProgressSign : MonoBehaviour {

    public float animationFramerate = 2;
    public int lvHammerHits = 2;
    public int wreathSegmentBlinks = 2;

    public TextMeshPro giftsCollectedText;
    public TextMeshPro giftsGoalText;

    private Coroutine coroutine;
    private float frameDuration;
    private TreeController tree;

    // void OnValidate() {
    //     if(tree == null) tree = FindObjectOfType<TreeController>();
    // }

    void Awake() {
        tree = FindObjectOfType<TreeController>();
        frameDuration = 1 / animationFramerate;
    }

    void OnEnable() {
        coroutine = StartCoroutine(AnimationCoroutine());
    }

    void OnDisable() {
        // I dunno if Unity safely does this automatically; no time to learn now.
        StopCoroutine(coroutine);
    }

    private IEnumerator AnimationCoroutine() {
        while(true) {

            yield return new WaitForSeconds(frameDuration);
        }
        
        // // Switch to Lv animation
        // for(var i = 0; i < lvHammerHits; i++) {
        //     // hammer up
        //     yield return new WaitForSeconds(frameDuration);

        //     // hammer down
        //     yield return new WaitForSeconds(frameDuration);
        // }

        // // Switch to wreath animation
        // for(var i = 0; i < wreathSegmentBlinks; i++) {
        //     // wreath segment empty
        //     yield return new WaitForSeconds(frameDuration);

        //     // wreath segment filled
        //     yield return new WaitForSeconds(frameDuration);
        // }
    }
}