using Reptile;
using UnityEngine;

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

    void Awake() {
        if(prefab == null) {
            prefab = Core.Instance.Assets.LoadAssetFromBundle<UnityEngine.GameObject>(prefabAssetBundle, prefabAssetPath);
        }
    }
    void OnEnable() {
        SpawnEffect();
    }
    void SpawnEffect() {
        GameObject instance;
        // Leave this enabled. unless there's some reason parenting the effect to the tree breaks stuff(??)
        if(spawnParentedToMe) {
            instance = GameObject.Instantiate<GameObject>(prefab, transform);
        } else {
            instance = GameObject.Instantiate<GameObject>(prefab);
            instance.transform.position = transform.position;
            instance.transform.rotation = transform.rotation;
            instance.transform.localScale = transform.localScale;
        }
        var effectC = instance.GetComponent<OneOffVisualEffect>();
        var animation = effectC.anim;
        // Not sure about this line of code, best way to get default animation's name to set the speed?
        animation[animation.clip.name].speed = playbackSpeed;
    }
}
