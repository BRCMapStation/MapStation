using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Winterland.Common;

class TreeConstructionClip : PlayableAsset, ITimelineClipAsset {
    public ClipCaps clipCaps => ClipCaps.None;
    //public TreeConstructionClipBehavior template;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner) {
        //var playable = ScriptPlayable<TreeConstructionClipBehavior>.Create(graph, template);
        var playable = ScriptPlayable<TreeConstructionClipBehavior>.Create(graph);
        return playable;
    }
}
