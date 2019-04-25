using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TileData",menuName = "Objects/TileData")]
[System.Serializable]
public class TileData : ScriptableObject
{
    public GameObject prefab;
}

[System.Serializable]
public class TileSaveData
{
    [SerializeField]
    public TileData tileData;
    [SerializeField]
	public Vector3 position;
	[SerializeField]
	public Quaternion rotation;
    [SerializeField]
	public Texture texture;
}
