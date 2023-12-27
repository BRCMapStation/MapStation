using System;
using System.Collections;
using SlopCrew.API;
using TMPro;
using UnityEngine;

namespace Winterland.Common;

class TreeProgressSign : MonoBehaviour {
    [Header("Whether to count an extra day if it's on a different day of the year.")]
    public bool compensateDays = true;
    [Header("Whether to count an extra hour if it's on a different hour in the same day.")]
    public bool compensateHours = true;
    public bool countSeconds = true;
    public bool countMinutes = true;
    public double unixTimeStampComingSoon;

    public TextMeshPro giftsCollectedText;
    public TextMeshPro giftsGoalText;
    public TextMeshPro totalGiftsCollectedText;
    public TextMeshPro playerCountText;
    public TextMeshPro normalProgressLabel;
    public TextMeshPro comingSoon999Label;
    public TextMeshPro comingSoonDateLabel;
    public GameObject showIfNoDateSet;
    public GameObject showIfDateSet;

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
        var goal = tree.TargetProgress.ActivePhaseGiftsGoal;
        giftsCollectedText.text = tree.TargetProgress.ActivePhaseGiftsCollected.ToString();
        giftsGoalText.text = "/" + goal.ToString();
        var goalTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        goalTime = goalTime.AddSeconds(unixTimeStampComingSoon).ToLocalTime();
        if (unixTimeStampComingSoon == 0) {
            showIfNoDateSet.SetActive(true);
            showIfDateSet.SetActive(false);
        } else {
            showIfNoDateSet.SetActive(false);
            showIfDateSet.SetActive(true);

            var now = DateTime.Now;
            var timeLeft = goalTime - now;
            var daysLeft = timeLeft.TotalDays;
            var hoursLeft = timeLeft.TotalHours;
            var minutesLeft = timeLeft.TotalMinutes;
            var secondsLeft = timeLeft.TotalSeconds;
            var label = "NOW!";

            if (compensateDays) {
                if (hoursLeft >= 24)
                    hoursLeft = Mathf.Ceil((float)daysLeft);
            }

            if (compensateHours && daysLeft <= 0) {
                if (minutesLeft >= 60)
                    hoursLeft = Mathf.Ceil((float)hoursLeft);
            }

            if (daysLeft >= 1) {
                var number = Mathf.Floor((float) daysLeft);
                var text = "DAYS";
                if (number <= 1)
                    text = "DAY";
                label = $"{number} {text}";
            }
            else if(hoursLeft >= 1) {
                var number = Mathf.Floor((float) hoursLeft);
                var text = "HOURS";
                if (number <= 1)
                    text = "HOUR";
                label = $"{number} {text}";
            }
            else if(minutesLeft >= 1) {
                if (countMinutes) {
                    var number = Mathf.Floor((float) minutesLeft);
                    var text = "MINUTES";
                    if (number <= 1)
                        text = "MINUTE";
                    label = $"{number} {text}";
                } else
                    label = "SOON!";
            } else if (secondsLeft >= 1) {
                if (countSeconds) {
                    var number = Mathf.Floor((float) secondsLeft);
                    var text = "SECONDS";
                    if (number <= 1)
                        text = "SECOND";
                    label = $"{number} {text}";
                } else
                    label = "SOON!";
            }
            comingSoonDateLabel.text = label;
        }
        if(goal >= 999) {
            // coming soon variant of the sign
            giftsGoalText.gameObject.SetActive(false);
            normalProgressLabel.gameObject.SetActive(false);
            comingSoon999Label.gameObject.SetActive(true);
        } else {
            giftsGoalText.gameObject.SetActive(!tree.TargetProgress.isLastPhase);
            normalProgressLabel.gameObject.SetActive(true);
            comingSoon999Label.gameObject.SetActive(false);
        }
        totalGiftsCollectedText.text = tree.TargetProgress.totalGiftsCollected.ToString();
        if(APIManager.API != null) {
            playerCountText.text = APIManager.API.PlayerCount.ToString();
        }
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
