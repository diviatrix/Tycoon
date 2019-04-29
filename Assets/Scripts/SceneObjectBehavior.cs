using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SceneObjectBehavior : MonoBehaviour
{
    public SceneObjectData data;

	private ResourcePerTime plusRpm;
	private Resources resCapacity;
	private GameData gameData;
    private resourcePopupManager rpm;

	public void Start()
    {
        AddCollider();            
        AddBg();

		plusRpm = data.plusRpm;
		gameData = EventManager.GameData;
        rpm = EventManager.RPM;
        if (data.objectName == "Town Hall")
        {
            EventManager.BuiltObject = this;         // send event it is built
            Debug.Log("Built city hall");
        }
    }    

    private void AddCollider()
    {
        BoxCollider col = gameObject.AddComponent<BoxCollider>();
        col.size = new Vector3(1,0.1f,1);
        col.center = new Vector3(0,0.05f,0);
    }
    private void AddBg()
    {
        if (data.bgPrefab == null) return;

        GameObject bg = GameObject.Instantiate(data.bgPrefab,transform.position,Quaternion.identity);
        bg.transform.SetParent(transform);        
    }	

	private void FixedUpdate()
	{
		if (!plusRpm.isGathering && gameData.GetResourceCapacity(plusRpm.resource.type) > gameData.GetResourceByType(plusRpm.resource.type))
		{
			StartProduction();
		}
	}

    void StartProduction()
	{
		Debug.Log("Start producing from: " + name);
		StartCoroutine(ResourceTimer());
		plusRpm.isGathering = true;
	}

	IEnumerator ResourceTimer()	{

		yield return new WaitForSeconds(plusRpm.perSeconds);
        gameData.AddBalanceByType(plusRpm.resource.type, plusRpm.resource.amount);
        rpm.ResBubble(transform, plusRpm.resource.type, plusRpm.resource.amount);
		plusRpm.isGathering = false;
        //EventManager.Message = ("Produced "+ plusRpm.resource.type + ": " + plusRpm.resource.amount);
	}
}
