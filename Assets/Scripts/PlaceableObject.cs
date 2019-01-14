using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct SerializableObject
{
    public ObjectType type;
    public string name;
    public string prefabName;
    public string position;
    public string rotation;
}

public enum ObjectType
{
    Building,
    Object
}

public class PlaceableObject : ClickableObject
{
    [Header("Prefab settings")]
    public ObjectType type;
    public GameObject prefab;
    public GameObject bgPrefab;

    [Header("Effect settings")]
    public GameObject destroyEffect;
    public GameObject buildEffect;
    public Sprite sprite;

    [Header("object settings")]
    public string buildingName;
    public string description;
    public bool canBuild;
    public bool canSell;
    public bool canHarvest;
    public bool canRotate;
    public Resources cost;
    public ResourcePerTime plusResource;
    public ResourcePerTime minusResource;

    
    [Header("Debug publics")]
    public GameData gameData; 
 
    // privates
    private GameObject building;
    private float timer;
    private Material normalMat;

    // handle when click on this object
    public override void OnClick()
    {
        gameData.GetComponent<SelectionActionPanelController>().SelectObject(this);
    }
    // Start is called before the first frame update
    public void Initialize()
    {
        AddCollider();            
        AddBg();

        // Create GameObject
        building = InstantiateObject(prefab);
        building.name = buildingName;    

        transform.SetParent(gameData.generatedObjectsGO.transform);
        gameData.generatedObjects.Add(this);
        
        if (buildEffect != null)
        {
            GameObject.Instantiate(buildEffect,transform);
        }        

        // push this building to GameData
        gameData.GetComponent<Notification>().text.text = "Placed "+ buildingName;

        StartProduction();
        StartReduction();
    }
    
    public void InitializeWithSO(SerializableObject so)
    {      
        Initialize();  
        transform.position = JsonUtility.FromJson<Vector3>(so.position);        
        building.transform.rotation = JsonUtility.FromJson<Quaternion>(so.rotation);
    }

    void AddCollider()
    {
        BoxCollider col = gameObject.AddComponent<BoxCollider>();
        col.size = new Vector3(1,0.1f,1);
        col.center = new Vector3(0,0.05f,0);
    }
    void AddBg()
    {
        if(!bgPrefab)
        {
            return;
        }
        GameObject bg = GameObject.Instantiate(bgPrefab,transform.position,Quaternion.identity);
        bg.transform.SetParent(transform);        
    }

    public void RotateMe(float degree)
    {
        building.transform.Rotate(Vector3.up,degree);
    }

    public void Sell()
    {
        if (!canSell) {return;}
        gameData.resources = gameData.resources + cost; 
        DestroyMe();
    }
    public void DestroyMe()
    {        
        gameData.generatedObjects.Remove(this);        
        gameData.GetComponent<Notification>().text.text = "Destroyed "+ buildingName;
        if (destroyEffect != null)
        {
            GameObject.Instantiate(destroyEffect,transform.position,Quaternion.identity);
        }
        Destroy(gameObject);
    }

    GameObject InstantiateObject(GameObject go)
    {
        GameObject newgo = Instantiate(go,transform.position,Quaternion.identity);
        newgo.transform.SetParent(transform);
        return newgo;
    }
    public SerializableObject ObjectSaveData()
    {
        SerializableObject saveData; 
        saveData.name = buildingName;
        saveData.position = JsonUtility.ToJson(transform.position, true);
        saveData.rotation = JsonUtility.ToJson(building.transform.rotation, true);
        saveData.prefabName = prefab.name;
        saveData.type = type;
        
        return saveData;        
    }    

    void StartProduction()
    {
        StartCoroutine(plusResourceTimer());
        plusResource.isGathering = true;
    }

    void StartReduction()
    {
        StartCoroutine(minusResourceTimer());
        minusResource.isGathering = true;
    }
    private void FixedUpdate() 
    {
        if (plusResource.amount != 0 && !plusResource.isGathering)
        {
            StartProduction();
        }
        if (minusResource.amount != 0 && !minusResource.isGathering)
        {
            StartReduction();
        }
    }

    IEnumerator plusResourceTimer()
    {   
        yield return new WaitForSeconds(plusResource.perSeconds);
        gameData.resources.AddResource(plusResource.resource, plusResource.amount); 
        plusResource.isGathering = false;
    }
    IEnumerator minusResourceTimer()
    {   
        yield return new WaitForSeconds(minusResource.perSeconds);
        gameData.resources.AddResource(minusResource.resource, minusResource.amount); 
        minusResource.isGathering = false;
    }
}
