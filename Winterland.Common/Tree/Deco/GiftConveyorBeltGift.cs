using Reptile;
using UnityEngine;
using UnityEngine.UIElements;

class GiftConveyorBeltGift : MonoBehaviour {

    public Transform GiftParent;

    private GiftConveyorBelt belt;
    private uint lastWaypointIndex = 0;
    private float timeSinceLastWaypoint = 0;

    public void Init(GiftConveyorBelt belt, GameObject giftPrefab, float yRot, Vector3 scale) {
        this.belt = belt;
        var visuals = Instantiate(giftPrefab, GiftParent);
        visuals.SetActive(true);
        var angles = GiftParent.transform.localEulerAngles;
        angles.y = yRot;
        GiftParent.transform.localEulerAngles = angles;
        GiftParent.transform.localScale = scale;
    }

    void Update() {
        moveAlongWaypoints();
    }

    void moveAlongWaypoints() {
        timeSinceLastWaypoint += Time.deltaTime;
        while(timeSinceLastWaypoint > belt.TimeBetweenWaypoints[lastWaypointIndex]) {
            timeSinceLastWaypoint -= belt.TimeBetweenWaypoints[lastWaypointIndex];
            lastWaypointIndex++;
            if(lastWaypointIndex >= belt.waypoints.Length - 1) {
                Destroy(gameObject);
                return;
            }
        }
        var interpolant = timeSinceLastWaypoint / belt.TimeBetweenWaypoints[lastWaypointIndex];
        transform.position = Vector3.Lerp(belt.waypoints[lastWaypointIndex].position, belt.waypoints[lastWaypointIndex + 1].position, interpolant);
        transform.rotation = Quaternion.Lerp(belt.waypoints[lastWaypointIndex].rotation, belt.waypoints[lastWaypointIndex + 1].rotation, interpolant);
    }
}