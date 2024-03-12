using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

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
    public int levelNum;
    GameData gameData;

    void Awake()
        {
            xr_origin = GameObject.Find("XR Origin");
            unlockNext();
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

    void unlockNext() {
        Debug.Log("unlocked");
        string saveFile = Application.persistentDataPath + "/gamedata.json";
        if (File.Exists(saveFile))
        {
            // Read the entire file and save its contents.
            string fileContents = File.ReadAllText(saveFile);

            // Deserialize the JSON data 
            //  into a pattern matching the GameData class.
            gameData = JsonUtility.FromJson<GameData>(fileContents);
            gameData.isUnlocked[levelNum+1] = true;
            string jsonString = JsonUtility.ToJson(gameData);
            File.WriteAllText(saveFile, jsonString);
        }
    }

    void Update() {
        if (waiting) {
            waited += Time.deltaTime;
        }
        if (waited > timeWait) {
            unlockNext();
            xr_origin.GetComponent<NewPlaceAndTrack>().ChangeScene(nextLevel);
        }
    }
}
