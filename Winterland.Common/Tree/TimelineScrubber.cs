using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// Manually moves the playhead of a PlayableDirector.
/// </summary>
class TimelineScrubber {
    PlayableDirector director;
    private bool started = false;
    private float percent = 0;

    public TimelineScrubber(PlayableDirector director) {
        this.director = director;
        if(director.timeUpdateMode != DirectorUpdateMode.Manual) {
            Debug.LogError($"TimelineScrubber instantiated for a PlayableDirector that's not in manual mode: {director.gameObject.name}");
        }
    }

    public void ResetTimeline() {
        director.Stop();
        director.time = 0;
        started = false;
    }

    public void SetPercentComplete(float newPercent) {
        var target = newPercent;
        if(!this.started || target < percent) {
            // Rewinding might break animations, so fully reset from the beginning.
            this.started = true;
            this.initializeTimelineToPosition(getTimeForPercent(target));
        } else {
            this.advanceTimelineToPosition(getTimeForPercent(target));
        }
        percent = target;
    }

    void initializeTimelineToPosition(float position) {
        director.Stop();
        director.Play();
        director.playableGraph.Evaluate();
        director.playableGraph.Evaluate(position);
    }

    void advanceTimelineToPosition(float position) {
        var deltaTime = position - (float)director.time;
        // Rounding errors might happen? I'm not sure
        if(deltaTime <= 0) return;
        director.playableGraph.Evaluate(deltaTime);
    }

    float getTimeForPercent(float percent) {
        return (float)director.duration * percent;
    }
}