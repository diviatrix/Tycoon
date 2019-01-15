using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    public SnapGrid grid;
	public Transform spawnedObjects;

	public static GameObject PlaceObject(SceneObjectData data, Vector3 position, Quaternion rotation, Transform parent)
    {
        GameObject go = GameObject.Instantiate(data.prefab, position, rotation);
        SceneObjectBehavior sob = go.AddComponent<SceneObjectBehavior>();        
        sob.data = data;
        go.name = data.objectName;
		go.transform.SetParent(parent);

		return go;
    }
}
