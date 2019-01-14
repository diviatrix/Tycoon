using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildPanelController : MonoBehaviour
{
    public GameObject button;
    public GameObject buildPanel;
    //public Text text;
    GameData gameData;

    void Awake()
    {
        gameData = GetComponent<GameData>();
    }

    // fill ui build panel
    public void FillBuildPanel()
    {
        for (int i = 0; i < gameData.availableObjects.Count; i++)
        {
            // dont add button if marked as cant build
            if (!gameData.availableObjects[i].GetComponent<PlaceableObject>().canBuild){continue;}

            GameObject newbtn = GameObject.Instantiate(button,buildPanel.transform);
            newbtn.GetComponentInChildren<Text>().text = gameData.availableObjects[i].GetComponent<PlaceableObject>().buildingName;
            
            SetActiveBuilding script = newbtn.GetComponent<SetActiveBuilding>();
            script.id = i;
            script.gc = GetComponent<GameController>();
            script.gameData = gameData;
            if (gameData.availableObjects[i].GetComponent<PlaceableObject>().sprite != null)
            {
               script.image.sprite = gameData.availableObjects[i].GetComponent<PlaceableObject>().sprite;
            }
        }
        button.SetActive(false);
        buildPanel.SetActive(false);
    }
}
