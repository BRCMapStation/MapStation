using System.Collections.Generic;

namespace Winterland.Common;

// An object that wants the tree to be paused.
// Objects can add themselves to ITreeState to signal that the tree cannot
// progress until the player stops combo-ing, leaves a trigger volume, or
// an animation finishes.
public interface ITreeConstructionBlocker {}

public interface ITreeState {
    /// <summary>
    /// When the tree is "fast-forwarding" animations are skipped.
    /// This avoids re-playing cutscenes when you load the game mid-event.
    /// </summary>        
    public bool IsFastForwarding { get; }

    /// <summary>
    /// Other behaviours can add themselves to this HashSet to prevent the tree
    /// from growing on the local client. They must remove themselves or else tree will never grow.
    /// </summary>
    public HashSet<ITreeConstructionBlocker> ConstructionBlockers {get;}

    /// <summary>
    /// Target tree progress set by debug UI or networking.  Tree will animate towards this point,
    /// pausing for cutscenes or because player is standing in the way, tec.
    /// </summary>
    public TreeProgress TargetProgress {get;set;}
}

public class TreeProgress {
    public int ActivePhaseIndex = -1;
    public float ActivePhaseProgress = 0;

    public TreeProgress Clone() {
        return (TreeProgress) this.MemberwiseClone();
    }
}
