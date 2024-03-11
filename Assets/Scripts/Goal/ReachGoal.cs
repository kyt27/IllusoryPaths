using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReachGoal : BaseNodeInteractable
{

    ParticleSystem confetti;
    public GameObject confettiParticleSystem;
    public GameObject glowSection;
    public Color endGlowColor;
    Material[] materials;
    Material glowMaterial;

    void Awake()
        {
            materials = glowSection.GetComponent<Renderer>().materials;
            confetti = confettiParticleSystem.GetComponent<ParticleSystem>();
            confetti.Stop();
            glowMaterial = materials[1];
        }
    
    public override void Action(Vector3 position) {
        /* Do Confetti */
        confetti.Play();

        /* Change Colour of Goal and Glow more intensely */
        glowMaterial.SetColor("_EmissionColor", endGlowColor);
    }
}
