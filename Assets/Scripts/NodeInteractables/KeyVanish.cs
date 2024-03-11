using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyVanish : BaseNodeInteractable {
    public Node node;

    bool activated = false;
    public ParticleSystem particles;

    void Awake() {
        particles.Stop();
    }

    public override void Action(Vector3 myInput) {
        particles.Play();
        if(!activated && node != null) {
            node.EnableNode();
            Destroy(gameObject);
            activated = true;
        }
    }
}
