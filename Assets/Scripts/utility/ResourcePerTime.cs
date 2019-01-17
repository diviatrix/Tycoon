using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ResourcePerTime
{
    public Resource resource;
    public float perSeconds;
    public bool isGathering;
}