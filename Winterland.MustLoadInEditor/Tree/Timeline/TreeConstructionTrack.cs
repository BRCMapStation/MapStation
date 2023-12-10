using UnityEngine;
using UnityEngine.Timeline;

namespace Winterland.Common;

[TrackColor(0, 1, 0)] // green
[TrackBindingType(typeof(TreePart))]
[TrackClipType(typeof(TreeConstructionClip))]
public class TreeConstructionTrack : TrackAsset {}
