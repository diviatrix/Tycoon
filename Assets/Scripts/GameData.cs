using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[Serializable]
public class xmlData
{
    public Resources resources;
    public string[] sceneObjects;
}

[Serializable]
public struct ResourceSprite
{
    public Resource resource;
    public Sprite sprite;
}

public class GameData : MonoBehaviour
{
    public static GameData gameData;

    [Header("Scriptable obj")]
    
    
    // stats
    [Header("Game Settings")]
    public Resources startingResources;

    [Header("Game stats")]
    public Resources resources = new Resources();
    
    [Header("Game setup")]
    public List<GameObject> availableObjects = new List<GameObject>();
    public List<PlaceableObject> generatedObjects;
    public List<ResourceSprite> resourcesSprites;

    
    public GameObject generatedObjectsGO;

    private void Awake() 
    {
        ResetRes();
    }
    private void Start()
    {
        generatedObjectsGO = GeneratedObjectsGO();
    }
    public void ResetRes()
    {
        resources = startingResources;       
    }

    private GameObject GeneratedObjectsGO()
    {
        GameObject go = new GameObject();
        go.name = "generatedObjects";

        return go;
    }

    public void SaveData(xmlData data)
    {
        // load file if exist
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.OpenOrCreate);

        // save it
        bf.Serialize(file, data);
        file.Close();

        GetComponent<Notification>().SetNotification("Game Saved");
    }    

    public void LoadGame()
    {
        GameController gc = GetComponent<GameController>();
        gc.WipeScene();
        if(File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat",FileMode.Open);
            xmlData data = (xmlData)bf.Deserialize(file);
            file.Close();

            resources = data.resources;

            
            foreach (string s in data.sceneObjects)
            {
                SerializableObject so = JsonUtility.FromJson<SerializableObject>(s);
                gc.PlaceObjectfromSO(so);
            }
            
            GetComponent<Notification>().SetNotification("Game Loaded");
        }       
    }

    public void SaveGame()
    {   
        xmlData data = new xmlData();

        data.resources = resources;
        data.sceneObjects = new string[generatedObjects.Count];
        // itereate thru each one
        for (int i = 0; i < generatedObjects.Count; i++)
        {
            data.sceneObjects[i] = JsonUtility.ToJson(generatedObjects[i].ObjectSaveData(),true);
        }
        
        SaveData(data);
    }

    public void AddResources(Resources res)
    {
        resources = resources + res;
    }
}

