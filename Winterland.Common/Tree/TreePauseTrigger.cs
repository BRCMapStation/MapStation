using Reptile;
using UnityEngine;

namespace Winterland.Common;

/// <summary>
/// Tracks whenever player is touching this trigger and prevents the tree from growing.
/// Tree construction will resume when the player has left the trigger, preventing tree construction
/// from glitching player into the ground.
/// </summary>
[SelectionBase]
class TreePauseTrigger : MonoBehaviour, ITreeConstructionBlocker {
    ITreeState state;

    bool touchedPlayerLastTick = false;
    bool touchedPlayerThisTick = false;

    void FixedUpdate() {
        if(state == null) state = TreeController.Instance;
        // If tree exists
        if(state != null) {
            // If touch state changed
            if(!this.touchedPlayerLastTick && this.touchedPlayerThisTick) state.ConstructionBlockers.Add(this);
            if(this.touchedPlayerLastTick && !this.touchedPlayerThisTick) state.ConstructionBlockers.Remove(this);
        }
        this.touchedPlayerLastTick = this.touchedPlayerThisTick;
        this.touchedPlayerThisTick = false;
    }

    void OnDisable() {
        if(state != null) {
            state.ConstructionBlockers.Remove(this);
        }
        this.touchedPlayerLastTick = false;
        this.touchedPlayerThisTick = false;
    }

    void OnTriggerStay(Collider other) {
        var player = PlayerCollisionUtility.GetPlayer(other);
        if(player == null) return;
        this.touchedPlayerThisTick = true;
    }
}
