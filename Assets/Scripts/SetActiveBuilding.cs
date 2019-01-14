using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetActiveBuilding : MonoBehaviour
{
    public GameController gc;
    public GameData gameData;
    public Image image;
    public int id;

    public void Set()
    {
        if (gc != null)
        {
            gc.chosenPrefabToBuild = gameData.availableObjects[id];
            gc.EnterBuildMode();
        }
    }
}
