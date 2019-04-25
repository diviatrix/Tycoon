using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchSetActive : MonoBehaviour
{
    public List<GameObject> go;

    public void Switch()
    {
      foreach (GameObject g in go)
      {
        g.SetActive(!g.activeSelf);
      }        
    }
}
