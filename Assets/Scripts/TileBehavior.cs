using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileBehavior : MonoBehaviour
{
    public TileData tileData;
    public bool canBuild;
    public Texture texture;

    void Start()
    {
        AddCollider();
    }

    private void AddCollider()
    {
        BoxCollider col = gameObject.AddComponent<BoxCollider>();
        col.size = new Vector3(1,0.1f,1);
        col.center = new Vector3(0,0.01f,0);
    }
}

