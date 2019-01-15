using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceBarController : MonoBehaviour
{
	public GameObject ResourceBar; // link for top ui bar 
	public Text gold, wood, stone, food, copper, citizen; // list with buttons, will fill in code

	void OnEnable()
	{
		EventManager.OnChangeBalance += UpdateTopBar;
	}

	void OnDisable()
	{
		EventManager.OnChangeBalance -= UpdateTopBar;
	}

	void UpdateTopBar(Resources balance)
	{
		gold.text = "Gold\n" + balance.gold;
		wood.text = "Wood\n" + balance.wood;
		stone.text = "Stone\n" + balance.stone;
		food.text = "Food\n" + balance.food;
		copper.text = "Copper\n" + balance.copper;
		citizen.text = "Citizen\n" + balance.citizen;
	}
}
