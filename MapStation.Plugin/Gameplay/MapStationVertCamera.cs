using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reptile;
using UnityEngine;

namespace MapStation.Plugin.Gameplay {
    public class MapStationVertCamera : CameraMode {
        public MapStationPlayer MPPlayer => MapStationPlayer.Get(player);
        public override void UpdatePos(int fixedFrameCounter) {
            var lookAtOffset = MPPlayer.AirVertVector * 1.5f;
            var lookFromOffset = MPPlayer.AirVertVector * 0f;
            position = Vector3.Lerp(position, player.visualTf.position + lookFromOffset + (Vector3.up * 5f), 5f * Core.dt);
            var target = player.visualTf.position + lookAtOffset;
            var dist = Vector3.Distance(position, target);
            var minimumDistance = 5f;
            if (dist < minimumDistance) {
                var heading = (target - position).normalized;
                position = target - (heading * minimumDistance);
            }
            HandleLookAt(fixedFrameCounter, lookAtOffset);
            var vertRotation = Quaternion.LookRotation(Vector3.down, MPPlayer.AirVertVector);
            tilt = LerpTo(tilt, vertRotation.eulerAngles.z, 5f);
            positionFinal = HandleObstructions(position);
        }
    }
}
