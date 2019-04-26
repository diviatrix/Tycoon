using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class resourcePopupManager : MonoBehaviour
{
    public GameObject popupPrefab;

    public Sprite citizen,copper,food,gold,stone,wood;

    void Start()
    {
        EventManager.RPM = this;
    }

    public void ResBubble(Transform parent, ResourceType res, int amount)
    {
        Sprite tex = null;
        
        // set correct texture depend on resource
        switch(res) 
		{
			case ResourceType.citizen: tex = citizen; break;
			case ResourceType.copper: tex = copper; break;
			case ResourceType.food: tex = food; break;
			case ResourceType.gold: tex = gold; break;
			case ResourceType.stone: tex = stone; break;
			case ResourceType.wood: tex = wood; break;
		}

        // create new popup and set correct image and text
        GameObject popup = Instantiate(popupPrefab, parent.transform.position+Vector3.up, Quaternion.identity);
        popup.GetComponentInChildren<Image>().sprite = tex;

        string message = "";

        if (amount < 0)
        {
            popup.GetComponentInChildren<Text>().color = Color.red;
        }
        else message = "+";

        popup.GetComponentInChildren<Text>().text = message + amount;

        popup.transform.Rotate(45,-45,0, Space.World); // set rotation to camera
        popup.AddComponent<resPopupTween>(); // add animation with destroy timer
    }

}

public class resPopupTween : MonoBehaviour
{

    public Vector3 startPos;
    public float moveTime = 2;
    public float moveDistance = 1;

    void Start()
    {
        Destroy(gameObject, 2);
        AnimatePopup();
    }
    void AnimatePopup()
    {
        iTween.MoveBy(gameObject, Vector3.up*moveDistance, moveTime*2);
    }
}

