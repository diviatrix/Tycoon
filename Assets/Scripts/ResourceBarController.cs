using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceBarController : MonoBehaviour
{
	public GameObject ResourceBar; // link for top ui bar 
	public Text gold, wood, stone, food, copper, citizen; // list with buttons, will fill in code
	public Resources balance;
	public Resources maxBalance;

	void OnEnable()
	{
		EventManager.OnChangeBalance += UpdateCurrentBalance;
		EventManager.OnSetMaxResources += UpdateMaxBalance;
	}

	void OnDisable()
	{
		EventManager.OnChangeBalance -= UpdateCurrentBalance;
		EventManager.OnSetMaxResources -= UpdateMaxBalance;
	}

	void UpdateCurrentBalance(Resources res)
	{
		balance = res;
		UpdateTopBar();
	}

	void UpdateMaxBalance(Resources res)
	{
		maxBalance = res;
		UpdateTopBar();
	}

	void UpdateTopBar()
	{
		gold.text = "Gold\n" + balance.gold + "/" + maxBalance.gold;
		wood.text = "Wood\n" + balance.wood + "/" + maxBalance.wood;
		stone.text = "Stone\n" + balance.stone + "/" + maxBalance.stone;
		food.text = "Food\n" + balance.food + "/" + maxBalance.food;
		copper.text = "Copper\n" + balance.copper + "/" + maxBalance.copper;
		citizen.text = "Citizen\n" + balance.citizen + "/" + maxBalance.citizen;
	}
}
