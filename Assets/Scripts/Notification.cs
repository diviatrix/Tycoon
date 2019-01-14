using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notification : MonoBehaviour
{
    public Text text;

    public void SetNotification(string s)
    {
        text.text = s;
    }
}
