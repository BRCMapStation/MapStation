using Reptile;
using UnityEngine;

namespace Winterland.Common;

[SelectionBase]
class WinterTrampoline : MonoBehaviour {

    public float Power;
    public AudioSource audioSource;

    void OnTriggerEnter(Collider other) {
        if(!isActiveAndEnabled) return;
        var player = PlayerCollisionUtility.GetPlayer(other);
        if(player != null) {
            // See also: Player.Jump implementation for BoostJump pads
            player.motor.SetVelocityYOneTime(Power);
            if(audioSource != null) {
                audioSource.Play();
            }
            var winterPlayer = WinterPlayer.Get(player);
            if (winterPlayer != null) {
                winterPlayer.TimesTrampolined++;
                if (winterPlayer.TimesTrampolined > WinterPlayer.MaxTimesTrampolined) {
                    player.LandCombo();
                    winterPlayer.TimesTrampolined = 0;
                }
            }
        }
    }
}
