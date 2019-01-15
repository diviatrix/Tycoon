using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public class SaveData
{
	public Resources resources;
	public string[] sceneObjects;
}

public class SaveSystem
{
	public void WriteData(SaveData saveData)
	{
		// load file if exist
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.OpenOrCreate);

		// save it
		bf.Serialize(file, saveData);
		file.Close();

		EventManager.Message = ("Gave Saved");
	}

	public Tuple<List<SceneObjectSaveData>, Resources> LoadGame()
	{
		Debug.Log("Start Loading");
		List<SceneObjectSaveData> loadedSO = new List<SceneObjectSaveData>();
		Resources resources = new Resources();

		if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
			SaveData saveData = (SaveData)bf.Deserialize(file);
			file.Close();

			resources = saveData.resources;
			foreach (string s in saveData.sceneObjects)
			{
				loadedSO.Add(JsonUtility.FromJson<SceneObjectSaveData>(s));
			}
		}

		Debug.Log ("Data loaded from disk");
		return Tuple.Create(loadedSO, resources);
	}

	public void SaveGame(Transform t, Resources r)
	{
		SaveData saveData = new SaveData();

		saveData.sceneObjects = new string[t.childCount];

		for (int i = 0; i < t.childCount; i++)
		{
			SceneObjectSaveData sosd = new SceneObjectSaveData();
			sosd.data = t.GetChild(i).GetComponent<SceneObjectBehavior>().data;
			sosd.position = t.GetChild(i).position;
			sosd.rotation = t.GetChild(i).rotation;

			saveData.sceneObjects[i] = JsonUtility.ToJson(sosd);
		}

		saveData.resources = r;

		WriteData(saveData);
	}
}
