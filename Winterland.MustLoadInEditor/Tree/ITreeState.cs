using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winterland.Common {
    public interface ITreeState {
        /// <summary>
        /// When the tree is "fast-forwarding" animations are skipped.
        /// This avoids re-playing cutscenes when you load the game mid-event.
        /// </summary>        
        public bool isFastForwarding { get; }
    }
}
