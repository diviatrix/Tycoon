using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SceneObjectSaveData
{
	[SerializeField]
	public SceneObjectData data;
	[SerializeField]
	public Vector3 position;
	[SerializeField]
	public Quaternion rotation;
}
