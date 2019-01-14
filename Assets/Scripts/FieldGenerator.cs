using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Object
{
    public GameObject prefab;
    public double rate;
}

public class FieldGenerator : MonoBehaviour
{
    public List<Object> objects;
    public SnapGrid grid;
    public int seed;
    private GameData gameData;
    private GameController gameController;
    private int[] noiseValues;
    public List<GameObject> _objects;

    private void Awake() 
    {
        gameData = GetComponent<GameData>();
        gameController = GetComponent<GameController>();
    }

    void makeObjectList()
    {
        foreach (Object o in objects)
        {
            for (int i = 0; i<o.rate;i++)
            {
                _objects.Add(o.prefab);
            }
        }
    }

    public void Generate()
    {
        makeObjectList();
        Random.InitState(seed);
        noiseValues = new int[grid.allPointsOnMap.Count];
        
        for (int i = 0; i < noiseValues.Length; i++)
        {
            noiseValues[i] = Random.Range(0, _objects.Count);

            int id = noiseValues[i];

            if (_objects[id] != null)
            {
                gameController.PlaceObjectWithParams(_objects[id], grid.allPointsOnMap[i], Quaternion.identity);
            }
        }

        GetComponent<Notification>().SetNotification("Location Generated");
    }
}
