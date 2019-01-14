using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneObjectData",menuName = "Objects/SceneObjectData")]
[System.Serializable]
public class SceneObjectData : ScriptableObject
{
    [Header("Prefab settings")]
    public GameObject prefab;                   // object prefab
    public GameObject bgPrefab;                 // background prefab

    [Header("Effect settings")]
    public GameObject buildEffect;              // effect when you build object
    public GameObject destroyEffect;            // effect when you destroy object
    public Sprite sprite;                       // sprite for buttons and menus

    [Header("object settings")]
    new public string name;                     // name of object for ui and so
    public string description;                  // description for ui
                                   
    public bool canBuild;                       // can we build it
    public bool canSell;                        // can we sell it
    public bool canHarvest;                     // can we harvest it
    public bool canRotate;                      // is object player rotatable
    public Resources cost;                      // cost of this object  
    public ResourcePerTime rpmPlus;                    
    public ResourcePerTime rpmMinus;                    
}
