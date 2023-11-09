using UnityEngine;

class CE_HideInHierarchy : MonoBehaviour {
    void OnValidate() {
        gameObject.hideFlags |= HideFlags.HideInHierarchy;
        gameObject.hideFlags = HideFlags.None;
    }
}