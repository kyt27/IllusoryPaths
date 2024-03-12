using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReachGoal : BaseNodeInteractable
{

    ParticleSystem confetti;
    public GameObject confettiParticleSystem;
    public GameObject glowSection;
    public Color endGlowColor;
    public string nextLevel;
    Material[] materials;
    Material glowMaterial;
    GameObject xr_origin;

    float timeWait = 1;
    bool waiting = false;
    float waited = 0;

    void Awake()
        {
            xr_origin = GameObject.Find("XR Origin");
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
        waiting = true;

    }

    void Update() {
        if (waiting) {
            waited += Time.deltaTime;
        }
        if (waited > timeWait) {
            xr_origin.GetComponent<NewPlaceAndTrack>().ChangeScene(nextLevel);
        }
    }
}
