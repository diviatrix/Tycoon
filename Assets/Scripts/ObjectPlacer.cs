using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{

	public static GameObject PlaceObject(SceneObjectData data, Vector3 position, Quaternion rotation, Transform parent)
    {
        GameObject go = GameObject.Instantiate(data.prefab, position, rotation);
        go.name = data.objectName;
		go.transform.SetParent(parent);

        SceneObjectBehavior sob = go.AddComponent<SceneObjectBehavior>();        
        sob.data = data;

		return go;
    }

    public static GameObject PlaceTile(TileData data, Vector3 position, Quaternion rotation, Transform parent)
    {
        GameObject go = GameObject.Instantiate(data.prefab, position, rotation);
        go.name = "Tile";
		go.transform.SetParent(parent);

        TileBehavior tb = go.AddComponent<TileBehavior>();        
        tb.tileData = data;

		return go;
    }
}
