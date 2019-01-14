using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionActionPanelController : MonoBehaviour
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SelectObject(PlaceableObject bld)
    {
        harvestButton.SetActive(false);
        sellButton.SetActive(false);
        rotateButton.SetActive(false);

        if (bld.canHarvest){harvestButton.SetActive(true);}
        if (bld.canSell){sellButton.SetActive(true);}
        if (bld.canRotate){rotateButton.SetActive(true);}

        image.sprite = bld.sprite;
        name.text = bld.buildingName;
        description.text = bld.description;
        ShowActionPanel(true);
    }
}
