using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class popupTween : MonoBehaviour
{
    public Vector3 startPos;
    public float moveTime;
    public float moveDistance;

    void OnEnable()
    {
		EventManager.OnSendNotification += AnimatePopup;
	}

    void OnDisable()
    {
		EventManager.OnSendNotification -= AnimatePopup;
	}

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
