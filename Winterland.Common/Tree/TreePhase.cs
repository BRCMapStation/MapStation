using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Winterland.Common;
public class TreePhase : MonoBehaviour {
    public ITreeState state;

    public float StartAt;

    public TreePart[] showTreeParts;
    public TreePart[] hideTreeParts;
    public List<PlayableDirector> progressTimelines;
    private List<TimelineScrubber> scrubbers;

    [HideInInspector]
    public bool IsActivePhase;

    public void Init() {
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
        foreach(var part in showTreeParts) {
            part.Appear();
        }
    }

    public void Exit() {
        IsActivePhase = false;
        UpdateScrubbers();
        foreach(var part in hideTreeParts) {
            part.Disappear();
        }
    }

    public void ResetPhase() {
        IsActivePhase = false;
        Progress = 0;
        UpdateScrubbers();
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