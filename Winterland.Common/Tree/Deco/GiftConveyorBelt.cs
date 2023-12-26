using System.Collections;
using Reptile;
using UnityEngine;

class GiftConveyorBelt : MonoBehaviour {
    public GameObject GiftLogicPrefab;

    public float spawnMinInterval = 0.5f;
    public float spawnMaxInterval = 0.75f;

    public float spawnMinYRot = -45f;
    public float spawnMaxYRot = 45f;

    public float spawnMinScale = 0.7f;
    public float spawnMaxScale = 1.3f;

    private Coroutine animCoro = null;

    public Transform waypointParent;
    public Transform[] waypoints;
    public float movementSpeed;

    public Transform giftVisualsPrefabParent;
    public Transform[] giftVisualsPrefabs;

    private float[] timeBetweenWaypoints;
    public float[] TimeBetweenWaypoints => timeBetweenWaypoints;

    void OnValidate() {
        waypoints = waypointParent.GetAllChildren();
        giftVisualsPrefabs = giftVisualsPrefabParent.GetAllChildren();
    }

    void Awake() {
        timeBetweenWaypoints = new float[waypoints.Length - 1];
        for(var i = 0; i < waypoints.Length - 1; i++) {
            timeBetweenWaypoints[i] = Vector3.Distance(waypoints[i].position, waypoints[i + 1].position) / movementSpeed;
        }
        foreach(var waypoint in waypoints) {
            Destroy(waypoint.GetChild(0));
        }
        GiftLogicPrefab.SetActive(false);
        waypointParent.gameObject.SetActive(false);
        giftVisualsPrefabParent.gameObject.SetActive(false);
    }

    void OnEnable() {
        animCoro = StartCoroutine(Animation());
    }

    void OnDisable() {
        if(animCoro != null) {
            StopCoroutine(animCoro);
            animCoro = null;
        }
    }

    IEnumerator Animation() {
        while(true) {
            yield return new WaitForSeconds(Random.Range(spawnMinInterval, spawnMaxInterval));

            // Spawn a new gift
            var newGiftGameObject = Instantiate(GiftLogicPrefab);
            var newGift = newGiftGameObject.GetComponent<GiftConveyorBeltGift>();
            newGiftGameObject.SetActive(true);

            // Give it a random visual
            newGift.Init(
                this,
                giftVisualsPrefabs[Random.Range(0, giftVisualsPrefabs.Length)].gameObject,
                Random.Range(spawnMinYRot, spawnMaxYRot),
                new Vector3(
                    Random.Range(spawnMinScale, spawnMaxScale),
                    Random.Range(spawnMinScale, spawnMaxScale),
                    Random.Range(spawnMinScale, spawnMaxScale)
                )
            );
        }
    }
}