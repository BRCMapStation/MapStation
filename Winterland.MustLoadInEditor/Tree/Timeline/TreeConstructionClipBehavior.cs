using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;

namespace Winterland.Common;

internal class TreeConstructionClipBehavior : IPlayableBehaviour {

    public TreePart treePart;

    public void OnBehaviourPause(Playable playable, FrameData info) {
        var duration = playable.GetDuration();
        var count = playable.GetTime() + info.deltaTime;

        if ((info.effectivePlayState == PlayState.Paused && count > duration) || playable.GetGraph().GetRootPlayable(0).IsDone()) {
            // Execute your finishing logic here:
            Debug.Log("Clip done!");
        }
    }

    public void OnBehaviourPlay(Playable playable, FrameData info) {
            
    }

    public void OnGraphStart(Playable playable) {
            
    }

    public void OnGraphStop(Playable playable) {
            
    }

    public void OnPlayableCreate(Playable playable) {
            
    }

    public void OnPlayableDestroy(Playable playable) {
            
    }

    public void PrepareFrame(Playable playable, FrameData info) {
            
    }

    public void ProcessFrame(Playable playable, FrameData info, object playerData) {
            
    }
}
