using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchSetActive : MonoBehaviour
{
    public GameObject go;

    public void Switch()
    {
        go.SetActive(!go.activeSelf);
    }
}
