using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notification : MonoBehaviour
{
    public Text text;

		void Start()
		{
			text.text = "";
		}

    void OnEnable()
    {
        EventManager.OnSendNotification += SetNotification;
    }

    void OnDisable()
    {
        EventManager.OnSendNotification -= SetNotification;
    }

    public void SetNotification(string s)
    {
        GetComponent<ScrollRect>().verticalNormalizedPosition = 0;
        text.text += s + "\n";
    }
}
