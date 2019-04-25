using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[System.Serializable]
public struct SaveData
{
	public Resources balance;
	public Resources maxBalance;
	public string[] sceneObjects;
	public string[] tileObjects;
	public string playerPosition;
}

public class SaveSystem
{

	public SaveData LoadGame() 
	{
		Debug.Log("Start Loading");
		Debug.Log(Application.persistentDataPath);
		SaveData saveData = new SaveData();

		if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
			saveData = (SaveData)bf.Deserialize(file);
			file.Close();
			Debug.Log ("Data loaded from disk");
		}
		else 
		{
			Debug.Log("Save data not found on disk");
		}
		
		return saveData;
	}

	public void SaveGame(Transform soTransform, Transform tileTransform, Resources balance, Resources maxBalance, Vector3 playerPosition)
	{
		SaveData saveData = new SaveData();

		saveData.sceneObjects = new string[soTransform.childCount];

		for (int i = 0; i < soTransform.childCount; i++)
		{
			SceneObjectSaveData sosd = new SceneObjectSaveData();
			if (soTransform.GetChild(i).GetComponent<SceneObjectBehavior>() != null)
			{
				sosd.data = soTransform.GetChild(i).GetComponent<SceneObjectBehavior>().data;
			}
			
			sosd.position = soTransform.GetChild(i).position;
			sosd.rotation = soTransform.GetChild(i).rotation;

			saveData.sceneObjects[i] = JsonUtility.ToJson(sosd);
		}

		saveData.tileObjects = new string[tileTransform.childCount];

		for (int i = 0; i < tileTransform.childCount; i++)
		{
			TileSaveData tsd = new TileSaveData();
			tsd.tileData = tileTransform.GetChild(i).GetComponent<TileBehavior>().tileData;			
			tsd.position = tileTransform.GetChild(i).position;
			tsd.rotation = tileTransform.GetChild(i).rotation;
			tsd.texture = tileTransform.GetChild(i).GetComponent<TileBehavior>().texture;

			saveData.tileObjects[i] = JsonUtility.ToJson(tsd);
		}

		saveData.balance = balance;
		saveData.maxBalance = maxBalance;
		saveData.playerPosition = JsonUtility.ToJson(playerPosition);

		WriteData(saveData);
	}

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
}
