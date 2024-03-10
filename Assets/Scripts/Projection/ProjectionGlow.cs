using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectionGlow : ProjectionInteractable
{
    Material glow;
    public float fadeSpeed;
    public Color color;
    public float intensity;
    private bool isMerged = false;
    float eps = 0.0001f;

    void Start() {
        glow = GetComponent<Renderer>().material;
        print(glow);
    }

    void diminishGlow() {
        intensity -= fadeSpeed * Time.deltaTime;
        glow.SetColor("_EmissionColor", color * intensity);
        if (intensity < eps)
            isMerged = false;
    }

    public override void Action() {
        glow.SetColor("_EmissionColor", color * intensity);
        isMerged = true;
    }

    void Update() {
        if (isMerged)
            diminishGlow();
    }

}
