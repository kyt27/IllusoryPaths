using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class GameData
{
    // Public lives
    public bool[] isUnlocked;
}


public class Levels : MonoBehaviour
{
    public GameObject[] levelButtons;
    public float minOffset;
    public float maxOffset;

    // Start is called before the first frame update
    void Start()
    {
        /* Read from file */
        /* For each level check if its active or not */
        /* Set Active based on this */
    }

     // Create a field for the save file.
    string saveFile;

    // Create a GameData field.
    GameData gameData = new GameData();

    void OnEnable() {
        saveFile = Application.persistentDataPath + "/gamedata.json";
        if (!File.Exists(saveFile)) {
            gameData.isUnlocked = new bool[levelButtons.Length];
            gameData.isUnlocked[0] = true;
            for (int i = 1; i < levelButtons.Length; i++) {
                gameData.isUnlocked[1] = false;
            }
            string jsonString = JsonUtility.ToJson(gameData);
            writeFile();
        } else {
            readFile();
        }

        for (int i = 0; i < levelButtons.Length; i++) {
            levelButtons[i].GetComponent<NumberProjection>().isUnlocked = gameData.isUnlocked[i];
            levelButtons[i].GetComponent<NumberProjection>().offsetRange = Mathf.Lerp(minOffset, maxOffset, (i+1)/levelButtons.Length);
            levelButtons[i].GetComponent<NumberProjection>().InitTriangles();
        }
    }

    void Awake()
    {
        // Update the path once the persistent path exists.
        saveFile = Application.persistentDataPath + "/gamedata.json";
        if (!File.Exists(saveFile)) {
            gameData.isUnlocked = new bool[levelButtons.Length];
            gameData.isUnlocked[0] = true;
            for (int i = 1; i < levelButtons.Length; i++) {
                gameData.isUnlocked[1] = false;
            }
            string jsonString = JsonUtility.ToJson(gameData);
            writeFile();
        } else {
            readFile();
        }

        for (int i = 0; i < levelButtons.Length; i++) {
            levelButtons[i].GetComponent<NumberProjection>().isUnlocked = gameData.isUnlocked[i];
            levelButtons[i].GetComponent<NumberProjection>().offsetRange = Mathf.Lerp(minOffset, maxOffset, (i+1)/levelButtons.Length);
            levelButtons[i].GetComponent<NumberProjection>().InitTriangles();
        }
    }

    public void unlockLevel(int levelNum) {
        gameData.isUnlocked[levelNum] = true;
    }

    public void readFile()
    {
        // Does the file exist?
        if (File.Exists(saveFile))
        {
            // Read the entire file and save its contents.
            string fileContents = File.ReadAllText(saveFile);

            // Deserialize the JSON data 
            //  into a pattern matching the GameData class.
            gameData = JsonUtility.FromJson<GameData>(fileContents);
        }
    }

    public void writeFile()
    {
        // Serialize the object into JSON and save string.
        string jsonString = JsonUtility.ToJson(gameData);

        // Write JSON to file.
        File.WriteAllText(saveFile, jsonString);
    }
}
