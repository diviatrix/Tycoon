using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SceneObjectBehavior : MonoBehaviour
{
    public SceneObjectData data;

	private ResourcePerTime resourcePerTime;

	public void Start()
    {
        AddCollider();            
        AddBg();
        PlayBuildEffect();

		resourcePerTime = data.rpm;

        
        EventManager.BuiltObject = data;         // send event it is built
    }    

    private void AddCollider()
    {
        BoxCollider col = gameObject.AddComponent<BoxCollider>();
        col.size = new Vector3(1,0.1f,1);
        col.center = new Vector3(0,0.05f,0);
    }
    private void AddBg()
    {
        if(!data.bgPrefab)
        {
            return;
        }
        GameObject bg = GameObject.Instantiate(data.bgPrefab,transform.position,Quaternion.identity);
        bg.transform.SetParent(transform);        
    }

    private void PlayBuildEffect()
    {
        if (data.buildEffect != null)
        {
            GameObject.Instantiate(data.buildEffect,transform);
        } 
    }
	void StartProduction()
	{
		Debug.Log("Start producing from: " + name);
		StartCoroutine(ResourceTimer());
		resourcePerTime.isGathering = true;
	}

	private void FixedUpdate()
	{
		if (!resourcePerTime.isGathering)
		{
			StartProduction();
		}
	}

	IEnumerator ResourceTimer()
	{
		yield return new WaitForSeconds(resourcePerTime.perSeconds);
		EventManager.Amount = resourcePerTime.resources;

		resourcePerTime.isGathering = false;
	}
}
