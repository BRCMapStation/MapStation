using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// Manually moves the playhead of a PlayableDirector.
/// </summary>
class TimelineScrubber {
    PlayableDirector director;
    private bool started = false;
    private float position = 0;

    public float Position => position;

    public TimelineScrubber(PlayableDirector director) {
        this.director = director;
    }

    public void ResetTimeline() {
        director.Stop();
        director.time = 0;
        started = false;
    }

    public void SetPercentComplete(float percent) {
        var target = (float)director.duration * percent;
        if(!started || target < position) {
            // Rewinding might break animations, so fully reset from the beginning.
            started = true;
            initializeTimelineToPosition(target);
        } else {
            advanceTimelineToPosition(target);
        }
        position = target;
    }

    void initializeTimelineToPosition(float position) {
        director.Stop();
        director.Play();
        director.playableGraph.Evaluate();
        director.playableGraph.Evaluate(position);
    }

    void advanceTimelineToPosition(float position) {
        var deltaTime = position - (float)director.time;
        director.playableGraph.Evaluate(deltaTime);
        Debug.Log($"{nameof(TimelineScrubber)} director time to {director.time}");
    }
}