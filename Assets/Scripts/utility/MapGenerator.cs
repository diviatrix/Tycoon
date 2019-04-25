using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

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
    public float size = 100;
    public float perlinModifier;
    public float perlin2Modifier;
    public int seed;


    public void SetSeed(InputField input)
    {
        seed = int.Parse(input.text);
    }
    public void SetSize(InputField input)
    {
        size = int.Parse(input.text);
    }
    
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
        for (float  i = 0; i < size; i++) // for each grid tile 
        {
            for (float j = 0; j < size; j++)
            {
                var perlin = Mathf.PerlinNoise(seed+i/perlinModifier, seed+j/perlinModifier); // get perlin random

                GameObject go = ObjectPlacer.PlaceTile(tile, new Vector3(i, 0, j), Quaternion.identity, null, tileParent); // place empty tile

                foreach (GenerationZone zone in zones) // for each generated tile and for each Zone defined in editor
                {
                    if (perlin > zone.minRate && perlin < zone.maxRate) // grab settings by perlin random
                    {                        
                        go.GetComponentInChildren<Renderer>().material.mainTexture = zone.texture; // change texture of tile by zone
                        go.GetComponent<TileBehavior>().texture = zone.texture; // and set it in tilescript, TODO: make it happen iside script
                        
                        var perlin2 = Mathf.PerlinNoise(seed+i/perlin2Modifier, seed+j/perlin2Modifier); //one more perlin random inside zone for objects on tiles

                        foreach (ZoneObject zo in zone.ZoneObjects)
                        {
                            if (perlin2 > zo.minRate && perlin2 < zo.maxRate) // randomize object
                            {
                                if(zo.sceneObjectData != null)
                                {
                                    ObjectPlacer.PlaceObject(zo.sceneObjectData,go.transform.position, Quaternion.identity, soParent); //
                                }                                
                            }                                 
                        }
                    }
                }
            }
        }
    }
 }