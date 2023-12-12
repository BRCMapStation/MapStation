using Reptile;
using UnityEngine;

namespace Winterland.Common;
public class PlayerCollisionUtility {
    public static Player GetPlayer(Collider other, bool includeAI = false) {
        // See also: player detection logic in CommonAPI.CustomInteractable.CheckForInteraction
        var otherPlayer = other.GetComponent<Player>();
        if (otherPlayer == null)
            otherPlayer = other.GetComponentInParent<Player>();
        if (otherPlayer == null)
            otherPlayer = other.GetComponentInChildren<Player>();
        if (!includeAI && otherPlayer.isAI)
            return null;
        return otherPlayer;
    }
}