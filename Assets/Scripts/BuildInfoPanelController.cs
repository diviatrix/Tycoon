using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildInfoPanelController : MonoBehaviour
{
	[Header("Main window bindings")]
	public GameObject panel;
	public Image image;
	public Text nameText;
	public Text descriptionText;
	

	[Header("Cost object bindings")]
	public GameObject gold;
	public GameObject wood;
	public GameObject stone;
	public GameObject copper;
	public GameObject food;

	// private crap
	private GameData gameData;

	void OnEnable()
    {
        EventManager.OnEnterBuildMode += UpdateDisplayUI;
		EventManager.OnExitBuildMode += ExitBuildMode;
		EventManager.OnRecieveGameData += SetGameData;
	}	

    void OnDisable()
    {
        EventManager.OnEnterBuildMode -= UpdateDisplayUI;
		EventManager.OnExitBuildMode -= ExitBuildMode;
		EventManager.OnRecieveGameData -= SetGameData;
	}

	void SetGameData(GameData data)
	{
		gameData = data;
	}

	public void UpdateDisplayUI(SceneObjectData data)
    {
		panel.SetActive(true);
		image.sprite = data.sprite;
		nameText.text = data.objectName;
		descriptionText.text = data.description;

		gold.SetActive(false);
		wood.SetActive(false);
		stone.SetActive(false);
		copper.SetActive(false);
		food.SetActive(false);

		if (data.cost.gold > 0)
		{
			string resText = data.cost.gold.ToString();
			if (data.cost.gold > gameData.GetBalance().gold) { resText = "<color=#ff0000ff>" + data.cost.gold.ToString() + "</color>"; }
			SetCostFor(gold, resText);
		}
		if (data.cost.wood > 0)
		{
			string resText = data.cost.wood.ToString();
			if (data.cost.wood > gameData.GetBalance().wood) { resText = "<color=#ff0000ff>" + data.cost.wood.ToString() + "</color>"; }
			SetCostFor(wood, resText);
		}
		if (data.cost.stone > 0)
		{
			string resText = data.cost.stone.ToString();
			if (data.cost.stone > gameData.GetBalance().stone) { resText = "<color=#ff0000ff>" + data.cost.stone.ToString() + "</color>"; }
			SetCostFor(stone, resText);
		}
		if (data.cost.copper > 0)
		{
			string resText = data.cost.copper.ToString();
			if (data.cost.copper > gameData.GetBalance().copper) { resText = "<color=#ff0000ff>" + data.cost.copper.ToString() + "</color>"; }
			SetCostFor(copper, resText);
		}
		if (data.cost.food > 0)
		{
			string resText = data.cost.food.ToString();
			if (data.cost.food > gameData.GetBalance().food) { resText = "<color=#ff0000ff>" + data.cost.food.ToString() + "</color>"; }
			SetCostFor(food, resText);
		}
	}

	void SetCostFor(GameObject go, string cost)
	{
		go.SetActive(true);
		go.GetComponentInChildren<Text>().text = cost;
	}

	public void ExitBuildMode()
	{
		DisablePanel();
	}

	public void DisablePanel()
	{
		panel.SetActive(false);
	}
}
