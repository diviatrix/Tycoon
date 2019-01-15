using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Object
{
    public SceneObjectData sceneObjectData;
    public double rate;
}

public class FieldGenerator : MonoBehaviour
{
    public List<Object> objects;
   
    public List<Object> _objects = new List<Object>();

	private int[] noiseValues;
	private GameData gameData;



	void OnEnable()
	{
		EventManager.OnRecieveGameData += SetGameData;
	}

	void OnDisable()
	{
		EventManager.OnRecieveGameData -= SetGameData;
	}

	void SetGameData(GameData data)
	{
		gameData = data;
	}

	void makeObjectList()
    {
        foreach (Object o in objects)
        {
            for (int i = 0; i<o.rate;i++)
            {
                _objects.Add(o);
            }
        }
    }

    public void Generate(int seed, SnapGrid grid, Transform parent)
    {
		objects = gameData.GetAvailableResources();

		makeObjectList();

        Random.InitState(seed);
        noiseValues = new int[grid.allPointsOnMap.Count];
        
        for (int i = 0; i < noiseValues.Length; i++)
        {
            noiseValues[i] = Random.Range(0, _objects.Count);

            int id = noiseValues[i];

            if (_objects[id].sceneObjectData != null)
            {
                 ObjectPlacer.PlaceObject(_objects[id].sceneObjectData, grid.allPointsOnMap[i], Quaternion.identity, parent);
            }
        }
    }
}
