using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BlinkManager : MonoBehaviour {
    Animator animator;

    bool animating = false;

    [SerializeField] private GameObject raycastBlocker;

    private Collider collider;

    void Awake() {
        animator = GetComponent<Animator>();
        collider = raycastBlocker.GetComponent<Collider>();
        if(raycastBlocker == null || collider == null) {
            Debug.Log("NO RAYCAST BLOCKER DETECTED");
        }
    }

    public void Blink() {
        if(!animating) {
            animating = true;
            collider.enabled = true;
            animator.SetTrigger("BlinkEnable");
        }
    }

    public void AnimationFinish() {
        animating = false;
        collider.enabled = false;
    }
}
