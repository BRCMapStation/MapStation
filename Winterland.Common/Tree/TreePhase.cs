using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Winterland.Common;
public class TreePhase : MonoBehaviour {
    public ITreeState state;

    public float StartAt;

    public TreePart treePart;
    public List<PlayableDirector> progressTimelines;
    private List<TimelineScrubber> scrubbers;

    [HideInInspector]
    public bool IsActivePhase;

    void Awake() {
        scrubbers = new();
        foreach(var timeline in progressTimelines) {
            scrubbers.Add(new TimelineScrubber(timeline));
        }
    }

    private float progress;
    public float Progress {
        get => progress;
        set {
            progress = value;
            UpdateScrubbers();
        }
    }

    public void Enter() {
        IsActivePhase = true;
        UpdateScrubbers();
        treePart.Appear();
    }

    public void Exit() {
        IsActivePhase = false;
        UpdateScrubbers();
        treePart.Disappear();
    }

    void UpdateScrubbers() {
        if(IsActivePhase) {
            foreach(var s in scrubbers) {
                s.SetPercentComplete(Progress);
            }
        } else {
            foreach(var s in scrubbers) {
                s.ResetTimeline();
            }
        }
    }
}