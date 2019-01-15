using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionPanelController : MonoBehaviour
{
	public GameObject panel; // GUI panel with selected object actions
	public Image image;
	public Text name;
	public Text description;

	[Header("Button bindings")]
	public GameObject harvestButton;
	public GameObject sellButton;
	public GameObject rotateButton;

	public void ShowActionPanel(bool show)
	{
		panel.SetActive(show);
	}

	void OnEnable()
	{
		EventManager.OnSelection += SelectObject;
		EventManager.OnExitSelectMode += DisablePanel;
	}

	void OnDisable()
	{
		EventManager.OnSelection -= SelectObject;
		EventManager.OnExitSelectMode -= DisablePanel;
	}

	public void SelectObject(SceneObjectBehavior sob)
	{
		harvestButton.SetActive(false);
		sellButton.SetActive(false);
		rotateButton.SetActive(false);

		if (sob.data.canHarvest) { harvestButton.SetActive(true); }
		if (sob.data.canSell) { sellButton.SetActive(true); }
		if (sob.data.canRotate) { rotateButton.SetActive(true); }

		image.sprite = sob.data.sprite;
		name.text = sob.data.objectName;
		description.text = sob.data.description;
		ShowActionPanel(true);
	}

	public void ExitSelection()
	{
		EventManager.ExitSelectMode();
		DisablePanel();
	}

	void DisablePanel()
	{
		panel.SetActive(false);
	}
}
