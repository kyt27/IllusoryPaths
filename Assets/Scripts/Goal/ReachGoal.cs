using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReachGoal : BaseNodeInteractable
{

    ParticleSystem confetti;
    public GameObject confettiParticleSystem;

    void Awake()
        {
            confetti = confettiParticleSystem.GetComponent<ParticleSystem>();
            confetti.Stop();
        }
    
    public override void Action(Vector3 position) {
        /* Do Confetti */
        confetti.Play();
    }
}
