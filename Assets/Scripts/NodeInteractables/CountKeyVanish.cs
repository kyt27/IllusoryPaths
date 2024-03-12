using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountKeyVanish : BaseNodeInteractable {
    public CountEnable obj;

    bool activated = false;
    public ParticleSystem particles;

    void Awake() {
        particles.Stop();
    }

    public override void Action(Vector3 myInput) {
        if(!activated) {
            activated = true;
            obj.increaseCount();
            particles.Play();
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
