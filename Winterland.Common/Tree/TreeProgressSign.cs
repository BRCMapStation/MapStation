using System.Collections;
using TMPro;
using UnityEngine;

namespace Winterland.Common;

class TreeProgressSign : MonoBehaviour {

    public TextMeshPro giftsCollectedText;
    public TextMeshPro giftsGoalText;

    // Unused
    [HideInInspector]
    public float animationFramerate = 2;
    [HideInInspector]
    public int lvHammerHits = 2;
    [HideInInspector]
    public int wreathSegmentBlinks = 2;

    private Coroutine coroutine = null;
    private float frameDuration;
    private TreeController tree;

    void Awake() {
        tree = FindObjectOfType<TreeController>();
        frameDuration = 1 / animationFramerate;
    }

    void OnEnable() {
        // I initially thought I'd drive the animation from a coroutine
        // coroutine = StartCoroutine(AnimationCoroutine());
    }

    void OnDisable() {
        // I dunno if Unity safely does this automatically; no time to learn now.
        if(coroutine != null) {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }

    void Update() {
        giftsCollectedText.text = tree.TargetProgress.ActivePhaseGiftsCollected.ToString();
        giftsGoalText.text = "/" + tree.TargetProgress.ActivePhaseGiftsGoal.ToString();
        giftsGoalText.gameObject.SetActive(!tree.TargetProgress.isLastPhase);
    }

    // Unused
    private IEnumerator AnimationCoroutine() {
        // Switch to Lv animation
        for(var i = 0; i < lvHammerHits; i++) {
            // hammer up
            yield return new WaitForSeconds(frameDuration);

            // hammer down
            yield return new WaitForSeconds(frameDuration);
        }

        // Switch to wreath animation
        for(var i = 0; i < wreathSegmentBlinks; i++) {
            // wreath segment empty
            yield return new WaitForSeconds(frameDuration);

            // wreath segment filled
            yield return new WaitForSeconds(frameDuration);
        }
    }
}