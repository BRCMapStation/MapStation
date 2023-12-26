using Reptile;
using UnityEngine;

namespace Winterland.Common;

[SelectionBase]
class WinterTrampoline : MonoBehaviour {

    public float Power;
    public AudioClip bounceAudioClip;

    void OnTriggerEnter(Collider other) {
        if(!isActiveAndEnabled) return;
        var player = PlayerCollisionUtility.GetPlayer(other);
        if(player != null) {
            // See also: Player.Jump implementation for BoostJump pads
            player.RegainAirMobility();
            player.ForceUnground();
            player.motor.SetVelocityYOneTime(Power);

            if (bounceAudioClip != null) {
                var audioManager = Core.Instance.AudioManager;
                audioManager.PlayNonloopingSfx(audioManager.audioSources[3], bounceAudioClip, audioManager.mixerGroups[3], 0f);
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
