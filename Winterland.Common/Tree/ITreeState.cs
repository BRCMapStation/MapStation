using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Winterland.Common {

    // An object that wants the tree to be paused.
    // Objects can add themselves to ITreeState to signal that the tree cannot
    // progress until the player stops combo-ing, leaves a trigger volume, or
    // an animation finishes.
    public interface ITreePauseReason {}

    public interface ITreeState {
        /// <summary>
        /// When the tree is "fast-forwarding" animations are skipped.
        /// This avoids re-playing cutscenes when you load the game mid-event.
        /// </summary>        
        public bool IsFastForwarding { get; }

        public HashSet<ITreePauseReason> ReasonsToBePaused {get;}
    }
}
