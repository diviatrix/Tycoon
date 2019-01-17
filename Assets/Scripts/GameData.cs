using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Objects/GameData")]
[System.Serializable]
public class GameData : ScriptableObject
{
	[SerializeField]
	private Resources startResources;

	[SerializeField]
	private Resources resources;

	[SerializeField]
	private Resources startMaxResources;

	[SerializeField]
	private Resources maxResources;

	[SerializeField]
	private List<SceneObjectData> buildObjects;

	[SerializeField]
	private List<Object> resourceObjects;

	public void ResetResources()
	{
		maxResources = startMaxResources;
		resources = startResources;

		SendEvent();
	}

	void SendEvent()
	{
		EventManager.PlayerBalance = resources;
		EventManager.MaxResources = maxResources;
	}

	// current max game resources operators
	public void AddCapacity(Resources res)
	{
		maxResources = maxResources + res;
		SendEvent();
	}

	public void SetCapacity(Resources res)
	{
		maxResources = res;
		SendEvent();
	}

	public void ReduceCapacity(Resources res)
	{
		maxResources = maxResources - res;
		SendEvent();
	}

	public void AddCapacityByType(ResourceType rtype, int amount)
	{
		switch(rtype)
		{
			case ResourceType.citizen: maxResources.citizen += amount; break;
			case ResourceType.copper: maxResources.copper += amount; break;
			case ResourceType.food: maxResources.food += amount; break;
			case ResourceType.gold: maxResources.gold += amount; break;
			case ResourceType.stone: maxResources.stone += amount; break;
			case ResourceType.wood: maxResources.wood += amount; break;
		}
		SendEvent();
	}

	public void ReduceCapacityByType(ResourceType rtype, int amount)
	{
		switch(rtype)
		{
			case ResourceType.citizen: maxResources.citizen -= amount; break;
			case ResourceType.copper: maxResources.copper -= amount; break;
			case ResourceType.food: maxResources.food -= amount; break;
			case ResourceType.gold: maxResources.gold -= amount; break;
			case ResourceType.stone: maxResources.stone -= amount; break;
			case ResourceType.wood: maxResources.wood -= amount; break;
		}
		SendEvent();
	}


	// current game resources operators
	public void AddResources(Resources res)
	{
		resources = resources + res;
		SendEvent();
	}

	public void ReduceResources(Resources res)
	{
		resources = resources - res;
		SendEvent();
	}	

	public void SetBalance(Resources newBalance)
	{
		resources = newBalance;
		SendEvent();
	}

	public void AddBalanceByType(ResourceType rtype, int amount)
	{
		switch(rtype)
		{
			case ResourceType.citizen: resources.citizen += amount; break;
			case ResourceType.copper: resources.copper+= amount; break;
			case ResourceType.food: resources.food+= amount; break;
			case ResourceType.gold: resources.gold+= amount; break;
			case ResourceType.stone: resources.stone+= amount; break;
			case ResourceType.wood: resources.wood+= amount; break;
		}
		SendEvent();
	}

	public void ReduceBalanceByType(ResourceType rtype, int amount)
	{
		switch(rtype)
		{
			case ResourceType.citizen: resources.citizen -= amount; break;
			case ResourceType.copper: resources.copper -= amount; break;
			case ResourceType.food: resources.food -= amount; break;
			case ResourceType.gold: resources.gold -= amount; break;
			case ResourceType.stone: resources.stone -= amount; break;
			case ResourceType.wood: resources.wood -= amount; break;
		}
		SendEvent();
	}


	// not voids

	public List<SceneObjectData> GetAvailableBuildings()
	{
		return buildObjects;
	}

	public List<Object> GetAvailableResources()
	{
		return resourceObjects;
	}	

	public Resources GetBalance()
	{
		return resources;
	}

	public Resources GetResourcesCapacity()
	{
		return maxResources;
	}


	public bool CanBuild(List<Resource> res)
	{
		bool result = true;

		foreach (Resource r in res)
		{
			if (GetResourceByType(r.type) < r.amount)
			{
				result = false;
			}
		}
		return result;
	}
	public int GetResourceCapacity(ResourceType rtype)
	{
		int amount = new int();

		switch(rtype)
		{
			case ResourceType.citizen: amount = maxResources.citizen; break;
			case ResourceType.copper: amount = maxResources.copper; break;
			case ResourceType.food: amount = maxResources.food; break;
			case ResourceType.gold: amount = maxResources.gold; break;
			case ResourceType.stone: amount = maxResources.stone; break;
			case ResourceType.wood: amount = maxResources.wood; break;
		}

		return amount;
	}

	public int GetResourceByType(ResourceType rtype)
	{
		int amount = new int();

		switch(rtype)
		{
			case ResourceType.citizen: amount = resources.citizen; break;
			case ResourceType.copper: amount = resources.copper; break;
			case ResourceType.food: amount = resources.food; break;
			case ResourceType.gold: amount = resources.gold; break;
			case ResourceType.stone: amount = resources.stone; break;
			case ResourceType.wood: amount = resources.wood; break;
		}

		return amount;
	}
}
