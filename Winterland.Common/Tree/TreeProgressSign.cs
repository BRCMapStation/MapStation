using System;
using System.Collections;
using SlopCrew.API;
using TMPro;
using UnityEngine;

namespace Winterland.Common;

class TreeProgressSign : MonoBehaviour {
    [Header("Whether to count an extra day if it's on a different day of the year.")]
    public bool compensateDays = true;
    [Header("Minimum amount of hours left to do day compensation.")]
    public int compensateDaysMinimumHours = 12;
    [Header("Whether to count an extra hour if it's on a different hour in the same day.")]
    public bool compensateHours = true;
    [Header("Minimum amount of minutes left to do hour compensation.")]
    public int compensateHoursMinimumMinutes = 30;
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
            var daysLeft = timeLeft.Days;
            var hoursLeft = timeLeft.Hours;
            var minutesLeft = timeLeft.Minutes;
            var secondsLeft = timeLeft.Seconds;
            var label = "NOW!";

            if (compensateDays) {
                if (hoursLeft > compensateDaysMinimumHours)
                    daysLeft = goalTime.DayOfYear - now.DayOfYear;
            }

            if (compensateHours && daysLeft <= 0) {
                if (minutesLeft > compensateHoursMinimumMinutes)
                    hoursLeft = goalTime.Hour - now.Hour;
            }

            if (daysLeft >= 1) {
                label = $"{daysLeft} DAYS";
            }
            else if(hoursLeft >= 1) {
                label = $"{hoursLeft} HOURS";
            }
            else if(minutesLeft >= 1) {
                if (countMinutes)
                    label = $"{minutesLeft} MINUTES";
                else
                    label = "SOON!";
            } else if (secondsLeft >= 1) {
                if (countSeconds)
                    label = $"{secondsLeft} SECONDS";
                else
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
