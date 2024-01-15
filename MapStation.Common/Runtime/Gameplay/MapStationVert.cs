using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MapStation.Common.Gameplay {
    // NOTES
    // CheckVert patch on player changes slope limit when on mapstation vert.
    // Need to patch GroundDetection.ComputeGroundHit when the player is on vert. There is a lot of raycasting that's not fit for steeper slopes in there. Raycast down relative to slope rather than player transform when on vert?.
    public class MapStationVert : MonoBehaviour {
    }
}
