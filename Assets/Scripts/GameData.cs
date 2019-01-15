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
	private Resources playerResources;

	[SerializeField]
	private List<SceneObjectData> buildObjects;

	[SerializeField]
	private List<Object> resourceObjects;

	public void ResetResources()
	{		
		playerResources = startResources;
		EventManager.PlayerBalance = playerResources;
		EventManager.Message = "New game started \nResources set to default";
	}

	public void AddResources(Resources res)
	{
		playerResources = playerResources + res;
		EventManager.PlayerBalance = playerResources;
	}

	public void ReduceResources(Resources res)
	{
		playerResources = playerResources - res;
		EventManager.PlayerBalance = playerResources;
	}

	public Resources GetBalance()
	{
		return playerResources;
	}

	public void SetBalance(Resources newBalance)
	{
		playerResources = newBalance;
		EventManager.PlayerBalance = playerResources;
	}

	public List<SceneObjectData> GetAvailableBuildings()
	{
		return buildObjects;
	}

	public List<Object> GetAvailableResources()
	{
		return resourceObjects;
	}	
}
