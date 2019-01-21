using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct GenerationZone
{
    public Texture texture;
    public float minRate;
    public float maxRate;
    public List<ZoneObject> ZoneObjects;
}
[System.Serializable]
public struct ZoneObject
{
    public SceneObjectData sceneObjectData;
    public float minRate;
    public float maxRate;
}

public class MapGenerator : MonoBehaviour { 
     
    //public GameObject dirtPrefab;
    public TileData tile;
    private GameObject go;
    public List<GenerationZone> zones;
    public float sizeX = 100;
    public float sizeZ = 100;
    public float perlinModifierX;
    public float perlinModifierZ;
    public float perlin2ModifierX;
    public float perlin2ModifierZ;
    public int seed;
    
    void WipeScene()
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }
    }
 
    public void Regenerate(Transform tileParent, Transform soParent)
    {
        WipeScene();
        Random.InitState(seed);
        for (float  i = 0; i < sizeX; i++)
        {
            for (float j = 0; j < sizeZ; j++)
            {
                var perlin = Mathf.PerlinNoise(i/perlinModifierX, j/perlinModifierZ);
                GameObject go = ObjectPlacer.PlaceTile(tile, new Vector3(i, 0, j), Quaternion.identity, tileParent);
                //go = (GameObject)Instantiate(dirtPrefab, new Vector3(i, 0, j), Quaternion.identity);
                //go.transform.SetParent(transform);

                foreach (GenerationZone zone in zones)
                {
                    if (perlin > zone.minRate && perlin < zone.maxRate)
                    {                        
                        go.GetComponentInChildren<Renderer>().material.mainTexture = zone.texture;
                        
                        var perlin2 = Mathf.PerlinNoise(i/perlin2ModifierX, j/perlin2ModifierZ);
                        foreach (ZoneObject zo in zone.ZoneObjects)
                        {
                            if (perlin2 > zo.minRate && perlin2 < zo.maxRate)
                            {
                                if(zo.sceneObjectData != null)
                                {
                                    ObjectPlacer.PlaceObject(zo.sceneObjectData,go.transform.position, Quaternion.identity, soParent);
                                    //GameObject obj = Instantiate(zo.sceneObjectData.prefab, go.transform.position, Quaternion.identity);
                                    //obj.transform.SetParent(transform);
                                }                                
                            }                                 
                        }
                    }
                }
            }
        }
    }
 }