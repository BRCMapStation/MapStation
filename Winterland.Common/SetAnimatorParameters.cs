using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using UnityEngine;

class SetAnimatorParameters : MonoBehaviour {
    [SerializeField]
    private Animator animator = null;
    [SerializeField]
    private List<string> boolsToSet = [];

    void OnValidate() {
        if(animator == null) {
            animator = GetComponent<Animator>();
        }
    }

    void Awake() {
        setParams();
    }

    void OnEnable() {
        setParams();
    }

    void setParams() {
        foreach(var boolToSet in boolsToSet) {
            animator.SetBoolString(boolToSet, true);
        }
    }
}
