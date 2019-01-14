using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceBarController : MonoBehaviour
{
    public GameObject ResourceBar; // link for top ui bar 
    public Text gold,wood,stone,food,copper,citizen; // list with buttons, will fill in code

    private GameData gameData;
    

    private void Awake() 
    {
        gameData = GetComponent<GameData>();
    }

    void UpdateTopBar()
    {
        gold.text = "Gold\n" + gameData.resources.gold;
        wood.text = "Wood\n" + gameData.resources.wood;
        stone.text = "Stone\n" + gameData.resources.stone;
        food.text = "Food\n" + gameData.resources.food;
        copper.text = "Copper\n" + gameData.resources.copper;
        citizen.text = "Citizen\n" + gameData.resources.citizen;
    }

    private void FixedUpdate() 
    {
        UpdateTopBar();
    }    
}
