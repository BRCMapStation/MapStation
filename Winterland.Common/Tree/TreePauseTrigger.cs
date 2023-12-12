using Reptile;
using UnityEngine;

namespace Winterland.Common;

/// <summary>
/// Tracks whenever player is touching this trigger and prevents the tree from growing.
/// Tree construction will resume when the player has left the trigger, preventing tree construction
/// from glitching player into the ground.
/// </summary>
class TreePauseTrigger : MonoBehaviour, ITreePauseReason {
    ITreeState state;

    bool touchedPlayerLastFrame = false;
    bool touchedPlayerThisFrame = false;

    void Update() {
        if(state == null) state = TreeController.Instance;
        // If tree exists
        if(state != null) {
            // If touch state changed
            if(!touchedPlayerLastFrame && touchedPlayerThisFrame) state.ReasonsToBePaused.Add(this);
            if(touchedPlayerLastFrame && !touchedPlayerThisFrame) state.ReasonsToBePaused.Remove(this);
        }
        touchedPlayerLastFrame = touchedPlayerThisFrame;
        touchedPlayerThisFrame = false;
    }

    void OnDisable() {
        if(state != null) {
            state.ReasonsToBePaused.Remove(this);
        }
        touchedPlayerLastFrame = false;
        touchedPlayerThisFrame = false;
    }

    void OnTriggerStay(Collider other) {
        // See also: player detection logic in CommonAPI.CustomInteractable.CheckForInteraction
        var player = other.GetComponentInChildren<Player>();
        if (!player)
                player = other.GetComponentInParent<Player>();
        if(player == null) return;
        if(player.isAI) return;
        touchedPlayerThisFrame = true;
    }
}