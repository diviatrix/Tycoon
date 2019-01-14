using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildActionPanelController : MonoBehaviour
{
    [Header("Main window bindings")]
    public GameObject panel;
    public Image image;
    public Text nameText;
    public Text descriptionText;
    
    //public Button closeButton;

    [Header("Cost object bindings")]
    public GameObject gold;
    public GameObject wood;
    public GameObject stone;
    public GameObject copper;
    public GameObject food;

    private GameData gameData;

    private void Awake() 
    {
        gameData = GetComponent<GameData>();        
    }

    public void Disable()
    {
        panel.SetActive(false);
    }

    public void Enable()
    {
        panel.SetActive(true);
    }

    void SetCostFor(GameObject go, string cost)
    {
        go.SetActive(true);
        go.GetComponentInChildren<Text>().text = cost;
    }

    public void EnterBuildMode(PlaceableObject obj)
    { 
        panel.SetActive(true);
        image.sprite = obj.sprite;
        nameText.text = obj.buildingName;
        descriptionText.text =  obj.description;
        
        gold.SetActive(false);
        wood.SetActive(false);
        stone.SetActive(false);
        copper.SetActive(false);
        food.SetActive(false);

        if (obj.cost.gold > 0) 
        {
            string resText = obj.cost.gold.ToString();
            if (obj.cost.gold > gameData.resources.gold){resText = "<color=#ff0000ff>"+ obj.cost.gold.ToString() +"</color>";}
            SetCostFor(gold, resText);
        }
        if (obj.cost.wood > 0) 
        {
            string resText = obj.cost.wood.ToString();
            if (obj.cost.wood > gameData.resources.wood){resText = "<color=#ff0000ff>"+ obj.cost.wood.ToString() +"</color>";}
            SetCostFor(wood, resText);
        }
        if (obj.cost.stone > 0) 
        {
            string resText = obj.cost.stone.ToString();
            if (obj.cost.stone > gameData.resources.stone){resText = "<color=#ff0000ff>"+ obj.cost.stone.ToString() +"</color>";}
            SetCostFor(stone, resText);
        }
        if (obj.cost.copper > 0) 
        {
            string resText = obj.cost.copper.ToString();
            if (obj.cost.copper > gameData.resources.copper){resText = "<color=#ff0000ff>"+ obj.cost.copper.ToString() +"</color>";}
            SetCostFor(copper, resText);
        }
        if (obj.cost.food > 0) 
        {
            string resText = obj.cost.food.ToString();
            if (obj.cost.food > gameData.resources.food){resText = "<color=#ff0000ff>"+ obj.cost.food.ToString() +"</color>";}
            SetCostFor(food, resText);
        }
    }
}
