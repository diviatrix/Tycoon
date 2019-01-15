using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildMenuController : MonoBehaviour
{
	//public SceneObjectsList objects;
	public GameData gameData;
	public GameObject button;
	public GameObject buildPanel;

	void OnEnable()
	{
		EventManager.OnRecieveGameData += SetGameData;
	}

	void OnDisable()
	{
		EventManager.OnRecieveGameData -= SetGameData;
	}

	void SetGameData(GameData data)
	{
		gameData = data;
	}

	// fill ui build panel
	public void Start()
	{
		List<SceneObjectData> objects = gameData.GetAvailableBuildings();

		for (int i = 0; i < objects.Count; i++)
		{
			// dont add button if marked as cant build
			if (!objects[i].canBuild) { continue; }
			GameObject newbtn = GameObject.Instantiate(button, buildPanel.transform);

			ButtonProperty prop = newbtn.GetComponent<ButtonProperty>();

			prop.text.text = objects[i].objectName;
			prop.data = objects[i];

			if (objects[i].sprite != null)
			{
				prop.image.sprite =objects[i].sprite;
			}
		}

		button.SetActive(false);
		buildPanel.SetActive(false);
	}
}
