using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct buildObject
{
    public SceneObjectData dataReference; 
    public bool enabled; 

    public buildObject(SceneObjectData DataReference, bool Enabled)
    {
        dataReference = DataReference;
        enabled = Enabled;
    }
}

public class BuildMenuController : MonoBehaviour
{
    //public SceneObjectsList objects;
    public GameData gameData; 
    public GameObject button; // original button to clone
    public GameObject buildPanel; // link for panel object

    public List<buildObject> buildingList = new List<buildObject>(); // list of currently active buildings

    public List<GameObject> uiButtonList = new List<GameObject>(); // cloned ui buttons

    public List<SceneObjectData> objects; // gamedata object reference


    // events
    void OnEnable()
    {
        EventManager.OnRecieveGameData += SetGameData;
        EventManager.OnBuild += CheckBuilding;
    }

    void OnDisable()
    {
        EventManager.OnRecieveGameData -= SetGameData;
        EventManager.OnBuild -= CheckBuilding;
    }

    // run stuff on recieve game data

    void SetGameData(GameData data)
    {
        Debug.Log("got gamedata");
        WipeButtons();

        gameData = data;

        objects = gameData.GetAvailableBuildings();

        buildingList = new List<buildObject>();

        FillBuildingList();

        FillButtons();

    }

    void FillBuildingList()
    {
        foreach (SceneObjectData sod in objects)
        {
            if (sod.objectName == "Town Hall") buildingList.Add(new buildObject(sod, true));
            else buildingList.Add(new buildObject(sod, false));
        }
    }

    void CheckBuilding(SceneObjectBehavior sob)
    {
        if (sob.data.objectName == "Town Hall")
        {
            for(int i = 0; i< buildingList.Count; i++)
            {
                buildObject newbo = buildingList[i];
                newbo.enabled = true;
                buildingList[i] = newbo;
            }
            FillButtons();
        }

    }

    void WipeButtons()
    {
        foreach (GameObject btn in uiButtonList)
        {
            Destroy(btn);
        }
        uiButtonList = new List<GameObject>();
        button.SetActive(true);
    }

	// fill ui build panel
	public void FillButtons()
	{
        WipeButtons();

        for (int i = 0; i < buildingList.Count; i++)
		{
			// dont add button if marked as cant build
			if (!buildingList[i].enabled) { continue; }
			GameObject newbtn = GameObject.Instantiate(button, buildPanel.transform);
            uiButtonList.Add(newbtn);

			ButtonProperty prop = newbtn.GetComponent<ButtonProperty>();

			prop.text.text = buildingList[i].dataReference.objectName;
			prop.data = buildingList[i].dataReference;

			if (buildingList[i].dataReference.sprite != null)
			{
				prop.image.sprite = buildingList[i].dataReference.sprite;
			}
		}

		button.SetActive(false);
		buildPanel.SetActive(false);
	}
}
