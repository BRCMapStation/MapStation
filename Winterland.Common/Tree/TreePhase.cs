using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Playables;

namespace Winterland.Common;
public class TreePhase : MonoBehaviour {

    public enum TreePhaseState {
        Pending,
        Active,
        Finished
    }
    
    public ITreeState Tree;

    public TreePart[] showTreeParts;
    public TreePart[] hideTreeParts;
    public List<PlayableDirector> progressTimelines;
    private List<TimelineScrubber> scrubbers;

    public TreePhaseState State { get; private set; } = TreePhaseState.Pending;

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
        if (this.State != TreePhaseState.Pending) throw new Exception("Tree phase cannot enter; is not Pending");
        this.State = TreePhaseState.Active;
        UpdateScrubbers();
        foreach(var part in showTreeParts) {
            if(part != null) {
                part.Appear();
            }
        }
    }

    public void Exit() {
        if (this.State != TreePhaseState.Active) throw new Exception("Tree phase cannot exit; is not Active");
        this.State = TreePhaseState.Finished;
        UpdateScrubbers();
        foreach(var part in hideTreeParts) {
            if(part != null) {
                part.Disappear();
            }
        }
    }

    public void ResetPhase() {
        this.State = TreePhaseState.Pending;
        Progress = 0;
        UpdateScrubbers();
    }

    void UpdateScrubbers() {
        if(this.State == TreePhaseState.Active) {
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
