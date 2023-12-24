using Reptile;
using UnityEngine;
using Winterland.Common;

/// <summary>
/// Every time this gameobject is activated it will spawn a one-off visual effect.
/// Can be used in animations w/activation track.
/// </summary>
class EffectSpawner : MonoBehaviour {
    public GameObject prefab;
    public string prefabAssetBundle;
    public string prefabAssetPath;
    public float playbackSpeed = 1f;
    public bool spawnParentedToMe = true;

    private bool spawnNextFrame;

    void Awake() {
        if(prefab == null) {
            prefab = Core.Instance.Assets.LoadAssetFromBundle<UnityEngine.GameObject>(prefabAssetBundle, prefabAssetPath);
        }
    }
    void OnEnable() {
        // Are we part of the tree? Then do not trigger if it's fast-forwarding
        var treeController = GetComponentInParent<TreeController>();
        if (treeController == null || !treeController.IsFastForwarding) {
            // Wait a frame in case we get rapidly enabled/disabled while things set up
            spawnNextFrame = true;
        }
    }
    void OnDisable() {
        spawnNextFrame = false;
    }
    void FixedUpdate() {
        if(spawnNextFrame) {
            spawnNextFrame = false;
            SpawnEffect();
        }
    }

    void SpawnEffect() {
        GameObject spawned;
        // Leave this enabled. unless there's some reason parenting the effect to the tree breaks stuff(??)
        if(spawnParentedToMe) {
            spawned = GameObject.Instantiate<GameObject>(prefab, transform);
        } else {
            spawned = GameObject.Instantiate<GameObject>(prefab);
            spawned.transform.position = transform.position;
            spawned.transform.rotation = transform.rotation;
            spawned.transform.localScale = transform.localScale;
        }
        var visFx = spawned.GetComponent<OneOffVisualEffect>();
        var animation = visFx.anim;
        // Not sure about this line of code, best way to get default animation's name to set the speed?
        animation[animation.clip.name].speed = playbackSpeed;
#if WINTER_DEBUG
        Debug.Log($"Spawned effect: {spawned}");
#endif
    }
}
