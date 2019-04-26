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
        PlayBuildEffect();

		plusRpm = data.plusRpm;
		gameData = EventManager.GameData;
        rpm = EventManager.RPM;
		EventManager.BuiltObject = this;         // send event it is built
    }    

    private void AddCollider()
    {
        BoxCollider col = gameObject.AddComponent<BoxCollider>();
        col.size = new Vector3(1,0.1f,1);
        col.center = new Vector3(0,0.05f,0);
    }
    private void AddBg()
    {
        GameObject bg = GameObject.Instantiate(data.bgPrefab,transform.position,Quaternion.identity);
        bg.transform.SetParent(transform);        
    }

    private void PlayBuildEffect()
    {
        if (data.buildEffect != null)
        GameObject.Instantiate(data.buildEffect,transform);        
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
        EventManager.Message = ("Produced "+ plusRpm.resource.type + ": " + plusRpm.resource.amount);
	}
}
