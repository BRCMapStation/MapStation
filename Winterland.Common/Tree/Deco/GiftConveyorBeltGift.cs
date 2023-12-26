using System.Collections;
using Reptile;
using UnityEngine;
using UnityEngine.UIElements;

class GiftConveyorBeltGift : MonoBehaviour {

    public Transform GiftParent;

    public float simulatePhysicsXSecondsAfterLastWaypoint = 0;

    private GiftConveyorBelt belt;
    private uint lastWaypointIndex = 0;
    private float timeSinceLastWaypoint = 0;

    private bool finishedWaypoints = false;

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
        if(!finishedWaypoints) {
            moveAlongWaypoints();
        }
    }

    void moveAlongWaypoints() {
        timeSinceLastWaypoint += Time.deltaTime;
        while(timeSinceLastWaypoint > belt.TimeBetweenWaypoints[lastWaypointIndex]) {
            timeSinceLastWaypoint -= belt.TimeBetweenWaypoints[lastWaypointIndex];
            lastWaypointIndex++;
            if(lastWaypointIndex >= belt.waypoints.Length - 1) {
                finishWaypoints();
                return;
            }
        }
        var interpolant = timeSinceLastWaypoint / belt.TimeBetweenWaypoints[lastWaypointIndex];
        transform.position = Vector3.Lerp(belt.waypoints[lastWaypointIndex].position, belt.waypoints[lastWaypointIndex + 1].position, interpolant);
        transform.rotation = Quaternion.Lerp(belt.waypoints[lastWaypointIndex].rotation, belt.waypoints[lastWaypointIndex + 1].rotation, interpolant);
    }

    void finishWaypoints() {
        finishedWaypoints = true;
        if(simulatePhysicsXSecondsAfterLastWaypoint == 0) {
            Destroy(gameObject);
            return;
        }
        // Start simulating physics
        var rigidBody = GetComponent<Rigidbody>();
        rigidBody.isKinematic = false;
        // Give self momentum matching the direction of last waypoint
        var direction = belt.waypoints[belt.waypoints.Length - 1].position - belt.waypoints[belt.waypoints.Length - 2].position;
        rigidBody.velocity = direction.normalized * belt.movementSpeed;
        StartCoroutine(DestroySelfAfterDelay(simulatePhysicsXSecondsAfterLastWaypoint));
    }

    IEnumerator DestroySelfAfterDelay(float delay) {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}