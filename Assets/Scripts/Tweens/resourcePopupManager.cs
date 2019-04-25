using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class resourcePopupManager : MonoBehaviour
{
    public GameObject popupPrefab;

    public void PopResBubble(Transform parent, ResourceType res, int amount)
    {
        GameObject popup = Instantiate(popupPrefab, parent.transform.position, parent.transform.rotation);
    }
}

public class resPopupTween : MonoBehaviour
{

    public Vector3 startPos;
    public float moveTime;
    public float moveDistance;

    void Start()
    {
        startPos = transform.position;
        BackToStartPos();
    }
    void AnimatePopup(string s)
    {
        BackToStartPos();
        GetComponent<Text>().text = s;
        iTween.MoveBy(gameObject, Vector3.up*moveDistance, moveTime*2);
        iTween.MoveBy(gameObject, iTween.Hash("delay",moveTime,"oncomplete","BackToStartPos"));
    }
    void BackToStartPos()
    {
        GetComponent<Text>().text = "";
        transform.position = startPos;
    }
}

