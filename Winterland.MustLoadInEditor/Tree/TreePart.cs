using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;

namespace Winterland.Common;
public class TreePart : MonoBehaviour {
    PlayableDirector playDirectorOnAppear;
    PlayableDirector playDirectorOnDisappear;

    public void TreePartEnable(ITreeState state) {
        if(!state.isFastForwarding) {
            playDirectorOnAppear.Play();
        }
    }

    public void TreePartDisable(ITreeState state) {
        if(!state.isFastForwarding) {
            playDirectorOnDisappear.Play();
        }
    }
}
